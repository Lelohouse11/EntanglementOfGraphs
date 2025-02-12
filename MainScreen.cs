using EntaglementOfGraphs;
using QuikGraph;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Data;
using System.Windows;
using System.Windows.Navigation;

namespace EntanglementOfGraphs
{
    public partial class MainScreen
    {
        FiniteDirectedGraph<int> graph = new FiniteDirectedGraph<int>();
        GameStateGraph<int>? gameStateGraph;
        GameState<int> currentState;
        bool gamestarted;
        GameStateGraphTyp gameStateGraphTyp;
        private int startThiefPos = 1;
        private long maxMemoryUsed = 0;
        IProgress<string>? progress = null;
        IProgress<string>? progress1 = null;

        string filepath = "output.txt";
        int count = 1;
        int count2 = 1;
        Stopwatch globalStopwatch = new Stopwatch();

        public MainScreen()
        {
            InitializeComponent();
            graph.CreateImage(graph_PictureBox);
            chooseFixOrBack_ComboBox.SelectedIndex = 0;
            chooseGraphTyp_ComboBox.SelectedIndex = 0;
            progress = new Progress<string>(s =>
            {
                AppendToFile(filepath, s);
                if (count % 4 == 0)
                {
                    count = 1;
                    count2 = count2 + 1;
                }
                else
                {
                    count = count + 1;
                }
                TestingGraphClasses(count2, count);
                //entanglement_Button.Enabled = true;
            });
            progress1 = new Progress<string>(s =>
            {
                entanglement_Button.Enabled = true;
                TextOutput_TextBox.Text = s;
            });
        }


        /// <summary>
        /// Stellt den Graphen da
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphPicture_Paint(object sender, PaintEventArgs e)
        {
            graph.DrawImage(e.Graphics, graph_PictureBox);
        }

        /// <summary>
        /// fügt einen Knoten zum Graphen hinzu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newVertex_Click(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            int vertex;
            bool isNumber = int.TryParse(vertexInput_TextBox.Text, out vertex);
            if (isNumber) // prüft ob Eingabe Zahl und fügt ihn dann hinzu
            {
                graph.AddVertex(vertex);
                graph.AddVertexToMsagl(vertex);
                graph.CreateImage(graph_PictureBox);
                graph_PictureBox.Refresh();
                vertexInput_TextBox.Clear();
            }
            else // Eingabe unpassend
            {
                TextOutput_TextBox.Text = "Bitte eine Ganzzahl für den Knoten eingeben.";
                vertexInput_TextBox.Clear();
            }
            vertexInput_TextBox.Focus();
        }

        private void MonitorMemoryUsage(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                long currentMemoryUsed = Process.GetCurrentProcess().WorkingSet64;

                if (currentMemoryUsed > maxMemoryUsed)
                {
                    maxMemoryUsed = currentMemoryUsed;
                }
                if (globalStopwatch.ElapsedMilliseconds > 600000)
                {
                    Environment.Exit(0);
                }
                Thread.Sleep(10);
            }
        }

        private void CalculateEntanglementOfThread()
        {
            CancellationTokenSource cancellationTokenForInnerStopwatch = new CancellationTokenSource();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Task memoryMonitorTask = Task.Run(() => MonitorMemoryUsage(cancellationTokenForInnerStopwatch.Token));
            int? entanglement = graph.MinEntanglement(startThiefPos, GameStateGraphTyp.Backward);
            stopwatch.Stop();
            cancellationTokenForInnerStopwatch.Cancel();
            memoryMonitorTask.Wait();
            string result = $"{graph.VertexCount}, ({count2}x{count})-viele Knoten: Das Entanglement ist {entanglement}. " +
                  $"Die benötigte Zeit ist {stopwatch.ElapsedMilliseconds} ms und der maximal verbrauchte Speicher war {maxMemoryUsed} Bytes.";
            progress1?.Report(result);
        }

        private void AppendToFile(string filepath, string content)
        {
            using (StreamWriter writer = new StreamWriter(filepath, true))
            {
                writer.WriteLine(content);
            }
        }

        /// <summary>
        /// berechnet das Entanglement vom aktuell angezeigten Graph Graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void entanglement_Click(object sender, EventArgs e)
        {

            TextOutput_TextBox.Clear();

            bool isNumber = int.TryParse(startPosInputEnt_TextBox.Text, out startThiefPos);
            if (isNumber) // prüft ob Startposition gültige Zahl ist
            {
                if (graph.ContainsVertex(startThiefPos)) // muss gültiger Knoten sein nein überarbeiten
                {
                    string selectedCalcultaion = (string)chooseFixOrBack_ComboBox.SelectedItem;
                    if (selectedCalcultaion.Equals("Fixpoint"))
                    {
                        gameStateGraphTyp = GameStateGraphTyp.Fixpoint;
                    }
                    else if (selectedCalcultaion.Equals("Rückwärts"))
                    {
                        gameStateGraphTyp = GameStateGraphTyp.Backward;
                    }
                    else
                    {
                        TextOutput_TextBox.Text = "Bitte eine Berechnungsart auswählen.";
                        return;
                    }
                    entanglement_Button.Enabled = false;
                    GC.Collect();
                    var t = new Thread(new ThreadStart(CalculateEntanglementOfThread), 1000000000);
                    t.Start();
                    maxMemoryUsed = 0;
                    startPosInputEnt_TextBox.Clear();
                }
                else // es war kein gültiger Knoten
                {
                    TextOutput_TextBox.Text = "Bitte einen exsistierenden Knoten eingeben.";
                    startPosInputEnt_TextBox.Clear();
                    startPosInputEnt_TextBox.Focus();
                }
            }
            else // es war keine gültige Zahl
            {
                TextOutput_TextBox.Text = "Bitte eine Ganzzahl für die Startposition eingeben.";
                startPosInputEnt_TextBox.Clear();
                startPosInputEnt_TextBox.Focus();
            }
        }

        private void TestingGraphClasses(int i, int k)
        {
            graph = new TorusGraph(i, k).TranslateToInt();
            GC.Collect();
            var t = new Thread(new ThreadStart(CalculateEntanglementOfThread), 1000000000);
            t.Start();
            maxMemoryUsed = 0;
        }

        /// <summary>
        /// fügt eine Kante zum Graphen hinzu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addEdge_Click(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            int source;
            int target;
            bool isSourceNumber = int.TryParse(edgeSource_TextBox.Text, out source);
            bool isTargetNumber = int.TryParse(edgeTarget_TextBox.Text, out target);

            if (isSourceNumber) //prüft ob sourceknoten Zahl ist
            {
                if (isTargetNumber) // prüft ob targetknoten Zahl ist
                {
                    if (graph.ContainsVertex(source)) // gültiger sourceknoten?
                    {
                        if (graph.ContainsVertex(target)) // wenn gültiger targetknoten, dann wird die Kante hinzugefügt
                        {
                            graph.AddEdge(new Edge<int>(source, target));
                            graph.AddEdgeToMsagl(source, target);
                            edgeSource_TextBox.Clear();
                            edgeTarget_TextBox.Clear();
                            graph.CreateImage(graph_PictureBox);
                            graph_PictureBox.Refresh();
                            edgeSource_TextBox.Focus();
                        }
                        else // kein gültiger targetknoten
                        {
                            TextOutput_TextBox.Text = "Bitte einen exsistierenden Zielknoten eingeben.";
                            edgeTarget_TextBox.Clear();
                            edgeTarget_TextBox.Focus();
                        }
                    }
                    else // kein gültiger sourceknoten
                    {
                        TextOutput_TextBox.Text = "Bitte einen exsistierenden Ursprungsknoten eingeben.";
                        edgeSource_TextBox.Clear();
                        edgeSource_TextBox.Focus();
                    }
                }
                else // keine gültige Zahl für targetknoten
                {
                    TextOutput_TextBox.Text = "Bitte eine Ganzzahl für den Zielknoten eingeben.";
                    edgeTarget_TextBox.Clear();
                    edgeTarget_TextBox.Focus();
                }
            }
            else // keine gültige Zahl für sourceknoten
            {
                TextOutput_TextBox.Text = "Bitte eine Ganzzahl für den Urspungsknoten eingeben.";
                edgeSource_TextBox.Clear();
                edgeSource_TextBox.Focus();
            }
        }

        /// <summary>
        /// löscht den eingegebenen Graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteGraph_Click(object sender, EventArgs e)
        {
            if (false)
            {
                AppendToFile(filepath, "");
                AppendToFile(filepath, "Torus-Graph mit Rückwärts Aufbau:");

                globalStopwatch.Start();
                TestingGraphClasses(count2, count);
            }
            else
            {
                TextOutput_TextBox.Clear();
                graph = new FiniteDirectedGraph<int>();
                graph.CreateImage(graph_PictureBox);
                graph_PictureBox.Refresh();
            }
        }

        /// <summary>
        /// wechselt in den Spielemodus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playGraph_Click(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            startPosInputEnt_TextBox.Clear();
            graphCreate_Panel.Hide();
            chooseGraphTyp_ComboBox.Hide();
            TorusCreate_Panel.Hide();
            createDiCircleGraph_Panel.Hide();
            createUnCircleGraph_Panel.Hide();
            createFullyConGraph_Panel.Hide();
            createUnaryFunc_Panel.Hide();
            computeEnt_Panel.Hide();
            playGraph_Button.Hide();
            GameSettings_Panel.Show();
            editGraph_Button.Show();
            gamestarted = false;
        }

        /// <summary>
        /// wechsel zur Eingabe von Graphen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editGraph_Click(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            startPosInputPlay_TextBox.Clear();
            detectiveAmountInput_TextBox.Clear();
            graphCreate_Panel.Show();
            chooseGraphTyp_ComboBox.Show();
            chooseGraphTyp_ComboBox.SelectedIndex = 0;
            TorusCreate_Panel.Hide();
            createDiCircleGraph_Panel.Hide();
            createUnCircleGraph_Panel.Hide();
            createFullyConGraph_Panel.Hide();
            createUnaryFunc_Panel.Hide();
            computeEnt_Panel.Show();
            playGraph_Button.Show();
            GameSettings_Panel.Hide();
            editGraph_Button.Hide();
            detMovement_Panel.Hide();
            thiefMovement_Panel.Hide();
            restartGame_Button.Hide();
            if (gamestarted) // löscht die Einfärbung des Graphen
            {
                graph.ColorVertex(currentState.thiefPos.ToString(), Microsoft.Msagl.Drawing.Color.White);
                foreach (var detective in currentState.detectives)
                {
                    graph.ShapeVertex(detective.ToString(), Microsoft.Msagl.Drawing.Shape.Box);
                }
                graph_PictureBox.Refresh();
            }
        }

        /// <summary>
        /// startet das Spiel als Dieb
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playThief_Click(object sender, EventArgs e)
        {
            if (checkGameSettings()) //checkt alle Eingaben und erstellt Spiel
            {
                thiefMovement_Panel.Show();
                restartGame_Button.Show();
                MoveDet();
                gamestarted = true;
            }
        }

        /// <summary>
        /// startet das Spiel als Detektiv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playDetective_Click(object sender, EventArgs e)
        {
            if (checkGameSettings()) //checkt alle Eingaben und erstellt Spiel
            {
                detMovement_Panel.Show();
                restartGame_Button.Show();
                TextOutput_TextBox.Text = $"Du bist am Zug! Wähle einen Detektiv oder tue nichts. Noch nicht Plazierte Detektive: {gameStateGraph.detectiveMaxAmount}.";
                gamestarted = true;
            }
        }

        /// <summary>
        /// prüft eingabe und erstellt das Spiel und passende Strategien
        /// </summary>
        /// <returns></returns>
        private bool checkGameSettings()
        {
            TextOutput_TextBox.Clear();
            int startPos;
            int detectiveAmount;
            bool isStartPosNumber = int.TryParse(startPosInputPlay_TextBox.Text, out startPos);
            bool isDetectiveAmountNumber = int.TryParse(detectiveAmountInput_TextBox.Text, out detectiveAmount);

            if (isStartPosNumber) // gültige Zahl für startPosition
            {
                if (isDetectiveAmountNumber) // gültige Zahl an Detektiven
                {
                    if (graph.ContainsVertex(startPos)) // gültige Startposition
                    {
                        if (graph.VertexCount >= detectiveAmount) // gültige Anzahl an Detektiven und dann erstellung des Spiels
                        {
                            currentState = new GameState<int>(detectiveAmount, startPos, true);
                            gameStateGraph = new GameStateGraph<int>(graph, currentState);
                            gameStateGraph.BuildFlaggedGameStateGraphFixpoint();
                            gameStateGraph.CreateStrategies();
                            graph.ColorVertex(startPos.ToString(), Microsoft.Msagl.Drawing.Color.Red);
                            graph_PictureBox.Refresh();
                            startPosInputPlay_TextBox.Clear();
                            detectiveAmountInput_TextBox.Clear();
                            GameSettings_Panel.Hide();
                            return true;
                        }
                        else // kein gültige Anzahl an Detektiven
                        {
                            TextOutput_TextBox.Text = $"Anzahl der Detektive darf höchstens die Knotenzahl sein, also {graph.VertexCount}";
                            detectiveAmountInput_TextBox.Clear();
                            detectiveAmountInput_TextBox.Focus();
                        }
                    }
                    else // keine gültige Startposition
                    {
                        TextOutput_TextBox.Text = "Bitte einen exsistierenden Ursprungsknoten eingeben.";
                        startPosInputPlay_TextBox.Clear();
                        startPosInputPlay_TextBox.Focus();
                    }
                }
                else // keine gültige Zahl für Detektive
                {
                    TextOutput_TextBox.Text = "Bitte eine Ganzzahl für die Anzahl an Detektiven eingeben.";
                    detectiveAmountInput_TextBox.Clear();
                    detectiveAmountInput_TextBox.Focus();
                }
            }
            else // keine gültige Zahl für startposition
            {
                TextOutput_TextBox.Text = "Bitte eine Ganzzahl für den Urspungsknoten eingeben.";
                startPosInputPlay_TextBox.Clear();
                startPosInputPlay_TextBox.Focus();
            }
            return false;
        }

        /// <summary>
        /// bewegt den Detektiv, wenn man Detektiv spielt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moveDetective_Click(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            int movedDetective;
            bool isMovedDetectiveNumber = int.TryParse(movedDet.Text, out movedDetective);
            if (isMovedDetectiveNumber) // gültige Zahl für gewählten Detektiv
            {

                if (currentState.detectives.Contains(movedDetective)) // gültige Wahl für Detektiv
                {
                    graph.ShapeVertex(movedDetective.ToString(), Microsoft.Msagl.Drawing.Shape.Box);
                    currentState.MoveDetective(movedDetective);
                    currentState.ChangeTurn();
                }
                else if (movedDetective == -1) // Wenn Detektive noch nicht auf Spielfeld war
                {
                    currentState.MoveDetective(0);
                    currentState.ChangeTurn();
                }
                else // ungültige Detektivewahl
                {
                    if (currentState.detectives.Count == currentState.detectiveMaxAmount)
                    {
                        TextOutput_TextBox.Text = $"Bitte geben Sie {string.Join(", ", currentState.detectives)} für den zu bewegenden Detektiv ein.";
                    }
                    else if (currentState.detectives.Count == 0)
                    {
                        TextOutput_TextBox.Text = $"Bitte geben Sie 0 für den zu bewegenden Detektiv ein.";
                    }
                    else
                    {
                        TextOutput_TextBox.Text = $"Bitte geben Sie {string.Join(", ", currentState.detectives)}, -1 für den zu bewegenden Detektiv ein.";
                    }
                    movedDet.Clear();
                    movedDet.Focus();
                    return;
                }
                graph.ShapeVertex(currentState.thiefPos.ToString(), Microsoft.Msagl.Drawing.Shape.Diamond);
                graph_PictureBox.Refresh();
                Thread.Sleep(1000);
                MoveThief(); // simuliert zug des Diebes
                movedDet.Clear();
            }
            else // ungültige Detektivewahl
            {
                TextOutput_TextBox.Text = $"Bitte geben Sie {string.Join(", ", currentState.detectives)}, -1 für den zu bewegenden Detektiv ein.";
                movedDet.Clear();
                movedDet.Focus();
            }
        }

        /// <summary>
        /// bewegt Detektiv nicht, wenn man Detektiv spielt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void doNothingDet_Click(object sender, EventArgs e)
        {
            currentState.ChangeTurn();
            MoveThief();
        }


        /// <summary>
        /// bewegt den Dieb, wenn man Dieb spielt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moveThiefToTarget_Click(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            int thiefTarget;
            bool isThiefTargetNumber = int.TryParse(targetThiefInput_TextBox.Text, out thiefTarget);
            if (isThiefTargetNumber) // prüft ob gültige Zahl
            {
                var possibleNextPos = currentState.Clone();
                possibleNextPos.MoveThief(thiefTarget);
                possibleNextPos.ChangeTurn();
                bool moveAllowed = false;
                foreach (var possibleStep in graph.GetNextPossibleStates(currentState)) // prüft ob eingegebener Zug gültig ist
                {
                    if (possibleNextPos.Equals(possibleStep))
                    {
                        moveAllowed = true;
                        break;
                    }
                }
                if (moveAllowed) // wenn gültig, wird Zug durchgeführt
                {
                    graph.ColorVertex(currentState.thiefPos.ToString(), Microsoft.Msagl.Drawing.Color.White);
                    currentState = possibleNextPos;
                    graph.ColorVertex(currentState.thiefPos.ToString(), Microsoft.Msagl.Drawing.Color.Red);
                    graph_PictureBox.Refresh();
                    Thread.Sleep(1000);
                    MoveDet(); // simuliert zu des Detektiv
                    targetThiefInput_TextBox.Clear();
                }
                else // ungültige Zugeingabe
                {
                    TextOutput_TextBox.Text = $"Bitte eine der folgenden Ganzzahlen eingeben: {graph.GetNextPossibleStatesForThief(currentState)}.";
                    targetThiefInput_TextBox.Clear();
                    targetThiefInput_TextBox.Focus();
                }
            }
            else // ungültige Zugeingabe
            {
                TextOutput_TextBox.Text = $"Bitte eine der folgenden Ganzzahlen eingeben: {graph.GetNextPossibleStatesForThief(currentState)}.";
                targetThiefInput_TextBox.Clear();
                targetThiefInput_TextBox.Focus();
            }
        }


        /// <summary>
        /// simuliert einen Zug des Diebes, nach berechneter Strategie (Spieler ist Detektiv)
        /// </summary>
        private void MoveThief()
        {
            TextOutput_TextBox.Clear();
            bool thiefCanMove = false;
            foreach (var item in gameStateGraph.thiefStrategy) // prüft ob Detetkiv noch einen guten Zug machen kann
            {
                if (item.source.Equals(currentState))
                {
                    thiefCanMove = true;
                    break;
                }
            }
            if (!thiefCanMove) // wenn er Detektiv keinen guten Zug mehr hat, hat der Spieler verloren
            {
                detMovement_Panel.Hide();
                TextOutput_TextBox.Text = "Du hast Gewonnen!";
            }
            else
            {
                int nextThiefMove = gameStateGraph.BestThiefMove(currentState); // bester Zug
                graph.ColorVertex(currentState.thiefPos.ToString(), Microsoft.Msagl.Drawing.Color.White);
                currentState.MoveThief(nextThiefMove);
                currentState.ChangeTurn();
                graph.ColorVertex(nextThiefMove.ToString(), Microsoft.Msagl.Drawing.Color.Red);
                graph_PictureBox.Refresh();
                if (!gameStateGraph.OutEdges(currentState).Any()) // Der Detektiv kann dann nicht mehr gewinnen
                {
                    detMovement_Panel.Hide();
                    TextOutput_TextBox.Text = "Du hast Verloren!";

                }
                else
                {
                    TextOutput_TextBox.Text = $"Du bist am Zug! Wähle einen Detektiv (Knotenummer) oder tue nichts. Noch nicht Plazierte Detektive (-1): {gameStateGraph.detectiveMaxAmount - currentState.detectives.Count}.";
                }
            }
        }

        /// <summary>
        /// simuliert ein Zug des Detektives, nach berechneter Strategie (Spieler ist Dieb)
        /// </summary>
        private void MoveDet()
        {
            TextOutput_TextBox.Clear();
            if (gameStateGraph.GetExistingState(currentState) == null) // Detektiv hat auf jeden Fall verloren
            {
                thiefMovement_Panel.Hide();
                TextOutput_TextBox.Text = "Du hast Gewonnen!";
            }
            else
            {
                (int, bool) nextDetectiveMove = gameStateGraph.BestDetectiveMove(currentState); // bester Zug
                if (nextDetectiveMove.Item2)
                {
                    if (nextDetectiveMove.Item1 != 0) // Detektiv ist schon auf dem Graph
                    {
                        graph.ShapeVertex(nextDetectiveMove.Item1.ToString(), Microsoft.Msagl.Drawing.Shape.Box);
                        currentState.MoveDetective(nextDetectiveMove.Item1);
                        graph.ShapeVertex(currentState.thiefPos.ToString(), Microsoft.Msagl.Drawing.Shape.Diamond);
                        graph_PictureBox.Refresh();
                    }
                    else // Detektiv ist noch nicht auf dem Graph
                    {
                        currentState.MoveDetective(nextDetectiveMove.Item1);
                        graph.ShapeVertex(currentState.thiefPos.ToString(), Microsoft.Msagl.Drawing.Shape.Diamond);
                        graph_PictureBox.Refresh();
                    }
                }
                currentState.ChangeTurn();
                if (graph.GetNextPossibleStates(currentState).Count == 0) // Dieb hat auf jeden Fall verloren
                {
                    thiefMovement_Panel.Hide();
                    TextOutput_TextBox.Text = "Du hast Verloren!!";
                }
                else
                {
                    TextOutput_TextBox.Text = $"Du bist am Zug! Wähle ein der folgenden Knoten aus: {graph.GetNextPossibleStatesForThief(currentState)}.";
                }
            }
        }

        /// <summary>
        /// setzt Spiel zurück und startet es neu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restartGame_Click_1(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            startPosInputEnt_TextBox.Clear();
            graphCreate_Panel.Hide();
            TorusCreate_Panel.Hide();
            computeEnt_Panel.Hide();
            playGraph_Button.Hide();
            GameSettings_Panel.Show();
            editGraph_Button.Show();
            restartGame_Button.Hide();
            graph.ColorVertex(currentState.thiefPos.ToString(), Microsoft.Msagl.Drawing.Color.White); // entfärbt Graph
            foreach (var detective in currentState.detectives)
            {
                graph.ShapeVertex(detective.ToString(), Microsoft.Msagl.Drawing.Shape.Box);
            }
            graph_PictureBox.Refresh();
        }

        private void chooseGraphTyp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedGraph = (string)chooseGraphTyp_ComboBox.SelectedItem;
            TorusCreate_Panel.Visible = selectedGraph.Equals("Torusgraph");
            createDiCircleGraph_Panel.Visible = selectedGraph.Equals("Gerichteter Kreisgraph");
            createUnCircleGraph_Panel.Visible = selectedGraph.Equals("Ungerichteter Kreisgraph");
            createFullyConGraph_Panel.Visible = selectedGraph.Equals("Komplett verbundener Graph");
            createUnaryFunc_Panel.Visible = selectedGraph.Equals("Graph von unärer Funktion");

        }

        /// <summary>
        /// erstellt einen Torusgraph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createTorusGraph_Click(Object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            int nTorus;
            int mTorus;
            bool isNTorusNumber = int.TryParse(torusN_TextBox.Text, out nTorus);
            bool isMTorusNumber = int.TryParse(torusM_TextBox.Text, out mTorus);

            if (isNTorusNumber && isMTorusNumber) // prüft ob Eingaben gültige Zahl und erstellt dann Graphen
            {
                graph = new TorusGraph(nTorus, mTorus).TranslateToInt();
                graph.CreateImage(graph_PictureBox);
                graph_PictureBox.Refresh();
                torusN_TextBox.Clear();
                torusM_TextBox.Clear();
            }
            else // kein Gültige Zahl eingegeben
            {
                TextOutput_TextBox.Text = "Bitte Ganzzahlen für den Torusgraphen eingeben.";
                torusN_TextBox.Clear();
                torusM_TextBox.Clear();
                torusN_TextBox.Focus();
            }
        }

        private void createUndirectedCircleGraph_Click(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            int size;
            bool isNumber = int.TryParse(unCircleSizeInput_TextBox.Text, out size);
            if (isNumber) // prüft ob Eingabe Zahl und fügt ihn dann hinzu
            {
                graph = new UndirectedCircleGraph(size);
                graph.CreateImage(graph_PictureBox);
                graph_PictureBox.Refresh();
                unCircleSizeInput_TextBox.Clear();
            }
            else // Eingabe unpassend
            {
                TextOutput_TextBox.Text = "Bitte eine Ganzzahl für die Größe eingeben.";
                unCircleSizeInput_TextBox.Clear();
            }
            unCircleSizeInput_TextBox.Focus();
        }

        private void createDirectedCircleGraph_Click(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            int size;
            bool isNumber = int.TryParse(diCircleSizeInput_TextBox.Text, out size);
            if (isNumber) // prüft ob Eingabe Zahl und fügt ihn dann hinzu
            {
                graph = new DirectedCircleGraph(size);
                graph.CreateImage(graph_PictureBox);
                graph_PictureBox.Refresh();
                diCircleSizeInput_TextBox.Clear();
            }
            else // Eingabe unpassend
            {
                TextOutput_TextBox.Text = "Bitte eine Ganzzahl für die Größe eingeben.";
                diCircleSizeInput_TextBox.Clear();
            }
            diCircleSizeInput_TextBox.Focus();
        }

        private void fullyConCreate_Click(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            int size;
            bool isNumber = int.TryParse(fullyConSizeInput_TextBox.Text, out size);
            if (isNumber) // prüft ob Eingabe Zahl und fügt ihn dann hinzu
            {
                graph = new FullyConnectedGraph(size);
                graph.CreateImage(graph_PictureBox);
                graph_PictureBox.Refresh();
                fullyConSizeInput_TextBox.Clear();
            }
            else // Eingabe unpassend
            {
                TextOutput_TextBox.Text = "Bitte eine Ganzzahl für die Größe eingeben.";
                fullyConSizeInput_TextBox.Clear();
            }
            fullyConSizeInput_TextBox.Focus();
        }

        private void unaryFuncCreate_Click(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            int startDomain;
            int endDomain;
            bool isStartDomainNumber = int.TryParse(unaryFuncStartDomImput_TextBox.Text, out startDomain);
            bool istEndDomainNumber = int.TryParse(unaryFuncEndDomImput_TextBox.Text, out endDomain);
            if (isStartDomainNumber && istEndDomainNumber)
            {
                Func<int, int> func = CreateFunction(unaryFuncInput_TextBox.Text);

                if (func != null)
                {

                    graph = new UnaryFunctionGraph(func, startDomain, endDomain);
                    graph.CreateImage(graph_PictureBox);
                    graph_PictureBox.Refresh();
                    unaryFuncInput_TextBox.Clear();
                    unaryFuncStartDomImput_TextBox.Clear();
                    unaryFuncEndDomImput_TextBox.Clear();
                }
                else
                {
                    TextOutput_TextBox.Text = "Ungültige Funktion eingegeben. Bitte nur x als Variable benutzen und nur gültige mathematische Operationen (+, -, *, /, ^).";
                    unaryFuncInput_TextBox.Clear();
                }
                unaryFuncInput_TextBox.Focus();
            }
            else
            {
                TextOutput_TextBox.Text = "Bitte eine Ganzzahl für den Definitionsbereich eingeben.";
                unaryFuncStartDomImput_TextBox.Clear();
                unaryFuncEndDomImput_TextBox.Clear();
                unaryFuncStartDomImput_TextBox.Focus();
            }
        }

        private Func<int, int> CreateFunction(string funktionString)
        {

            return x =>
            {
                try
                {
                    var dataTable = new DataTable();
                    var temp = dataTable.Compute(funktionString.Replace("x", x.ToString()), string.Empty);
                    return Convert.ToInt32(temp);
                }
                catch
                {
                    return 0;
                };
            };
        }

        private void deleteVertex_Button_Click(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            int vertex;
            bool isNumber = int.TryParse(vertexInput_TextBox.Text, out vertex);
            if (isNumber) // prüft ob Eingabe Zahl und fügt ihn dann hinzu
            {
                if (graph.RemoveVertex(vertex))
                {
                    graph.DeleteVertexToMsagl(vertex);
                    graph.CreateImage(graph_PictureBox);
                    graph_PictureBox.Refresh();
                    vertexInput_TextBox.Clear();
                }
                else
                {
                    TextOutput_TextBox.Text = "Angegebener Knoten nicht im Graph vorhanden.";
                }
            }
            else // Eingabe unpassend
            {
                TextOutput_TextBox.Text = "Bitte eine Ganzzahl für den Knoten eingeben.";
                vertexInput_TextBox.Clear();
            }
            vertexInput_TextBox.Focus();
        }

        private void deleteEdge_Button_Click(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            int source;
            int target;
            bool isSourceNumber = int.TryParse(edgeSource_TextBox.Text, out source);
            bool isTargetNumber = int.TryParse(edgeTarget_TextBox.Text, out target);

            if (isSourceNumber) //prüft ob sourceknoten Zahl ist
            {
                if (isTargetNumber) // prüft ob targetknoten Zahl ist
                {
                    var possibleEdge = graph.GetExistingEdge(source, target);

                    if(possibleEdge != null)
                    {
                        graph.RemoveEdge(possibleEdge);
                        graph.DeleteEdgeToMsagl(source, target);
                        edgeSource_TextBox.Clear();
                        edgeTarget_TextBox.Clear();
                        graph.CreateImage(graph_PictureBox);
                        graph_PictureBox.Refresh();
                        edgeSource_TextBox.Focus();
                    }
                    else // kein gültiger targetknoten
                    {
                        TextOutput_TextBox.Text = "Bitte einen exsistierenden Zielknoten eingeben.";
                        edgeTarget_TextBox.Clear();
                        edgeSource_TextBox.Clear();
                        edgeSource_TextBox.Focus();
                    }
                }
                else // keine gültige Zahl für targetknoten
                {
                    TextOutput_TextBox.Text = "Bitte eine Ganzzahl für den Zielknoten eingeben.";
                    edgeTarget_TextBox.Clear();
                    edgeTarget_TextBox.Focus();
                }
            }
            else // keine gültige Zahl für sourceknoten
            {
                TextOutput_TextBox.Text = "Bitte eine Ganzzahl für den Urspungsknoten eingeben.";
                edgeSource_TextBox.Clear();
                edgeSource_TextBox.Focus();
            }
        }
    }
}