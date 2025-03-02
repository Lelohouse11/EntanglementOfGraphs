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
using EntanglementOfGraphs;
using Microsoft.Msagl.Drawing;

namespace EntanglementOfGraphs
{
    public partial class MainScreen
    {
        FiniteDirectedGraph<int> graph = new FiniteDirectedGraph<int>();
        GameStateGraph<int>? gameStateGraph;
        GameState<int> currentState;
        private bool gamestarted;
        private bool choosedPlayer;
        GameStateGraphTyp gameStateGraphTyp;
        private long maxMemoryUsed = 0;
        IProgress<string>? progressforEnt = null;
        IProgress<string>? progressforDetective = null;
        IProgress<string>? progressforThief = null;

        public MainScreen()
        {
            InitializeComponent();
            graph.CreateImage(graph_PictureBox);
            chooseDFSOrBFS_ComboBox.SelectedIndex = 0;
            chooseGraphTyp_ComboBox.SelectedIndex = 0;            
            progressforEnt = new Progress<string>(s =>
            {
                ActivateAllButtons();
                TextOutput_TextBox.Text = s;
            });
            progressforDetective = new Progress<string>(s =>
            {
                ActivateAllButtons();
                graph_PictureBox.Refresh();
                thiefMovement_Panel.Hide();
                detMovement_Panel.Show();
                restartGame_Button.Show();
                gamestarted = true;
                TextOutput_TextBox.Text = s;
            });
            progressforThief = new Progress<string>(s =>
            {
                detMovement_Panel.Hide();
                thiefMovement_Panel.Show();
                restartGame_Button.Show();
                MoveDet();
                ActivateAllButtons();
                gamestarted = true;
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

        /// <summary>
        /// überwacht den Speicherverbrauch
        /// </summary>
        /// <param name="token"></param>
        private void MonitorMemoryUsage(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                long currentMemoryUsed = Process.GetCurrentProcess().WorkingSet64;

                if (currentMemoryUsed > maxMemoryUsed)
                {
                    maxMemoryUsed = currentMemoryUsed;
                }
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// berechnet das Entanglement des Graphen
        /// </summary>
        private void CalculateEntanglementOfThread()
        {
            CancellationTokenSource cancellationTokenForInnerStopwatch = new CancellationTokenSource();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Task memoryMonitorTask = Task.Run(() => MonitorMemoryUsage(cancellationTokenForInnerStopwatch.Token));
            int? entanglement = graph.MinEntanglement(gameStateGraphTyp);
            stopwatch.Stop();
            cancellationTokenForInnerStopwatch.Cancel();
            memoryMonitorTask.Wait();
            string result = $"Das Entanglement ist {entanglement}. " +
                  $"Die benötigte Zeit ist {stopwatch.ElapsedMilliseconds} ms und der maximal verbrauchte Speicher war {maxMemoryUsed/1000} KiloBytes.";
            progressforEnt?.Report(result);
        }

        /// <summary>
        /// berechnet das Entanglement vom aktuell angezeigten Graph Graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void entanglement_Click(object sender, EventArgs e)
        {

            TextOutput_TextBox.Clear();


            string selectedCalcultaion = (string)chooseDFSOrBFS_ComboBox.SelectedItem;
            if (selectedCalcultaion.Equals("Breitensuche"))
            {
                gameStateGraphTyp = GameStateGraphTyp.BFS;
            }
            else if (selectedCalcultaion.Equals("Tiefensuche"))
            {
                gameStateGraphTyp = GameStateGraphTyp.DFS;
            }
            else
            {
                TextOutput_TextBox.Text = "Bitte eine Berechnungsart auswählen.";
                return;
            }
            DeactivateAllButtons();
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
            TextOutput_TextBox.Clear();
            graph = new FiniteDirectedGraph<int>();
            graph.CreateImage(graph_PictureBox);
            graph_PictureBox.Refresh();
            
        }

        /// <summary>
        /// wechselt in den Spielemodus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playGraph_Click(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            graphCreate_Panel.Hide();
            chooseGraphTyp_ComboBox.Hide();
            TorusCreate_Panel.Hide();
            createDiCircleGraph_Panel.Hide();
            createUnCircleGraph_Panel.Hide();
            createFullyConGraph_Panel.Hide();
            createGirdGraph_Panel.Hide();
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
            createGirdGraph_Panel.Hide();
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
            checkGameSettings(false);
        }

        /// <summary>
        /// startet das Spiel als Detektiv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playDetective_Click(object sender, EventArgs e)
        {
            checkGameSettings(true);
        }

        /// <summary>
        /// prüft eingabe und erstellt das Spiel und passende Strategien
        /// </summary>
        /// <returns></returns>
        private void checkGameSettings(bool choosedP)
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
                            choosedPlayer = choosedP;
                            startPosInputPlay_TextBox.Clear();
                            detectiveAmountInput_TextBox.Clear();
                            GameSettings_Panel.Hide();
                            DeactivateAllButtons();
                            var t = new Thread(new ThreadStart(CalculateStrategiesOfThread), 1000000000);
                            t.Start();
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
        }

        /// <summary>
        /// berechnet das Entanglement des Graphen
        /// </summary>
        private void CalculateStrategiesOfThread()
        {            
            gameStateGraph = new GameStateGraph<int>(graph, currentState);
            gameStateGraph.BuildFlaggedGameStateGraphBFS();
            gameStateGraph.CreateStrategies();
            graph.ColorVertex(currentState.thiefPos.ToString(), Microsoft.Msagl.Drawing.Color.Red);
            if (choosedPlayer) // Spieler ist Detektiv
            {              
                string result = $"Du bist am Zug! Wähle einen Detektiv (Knotenummer) oder tue nichts. Noch nicht Plazierte Detektive (-1): {gameStateGraph.detectiveMaxAmount - currentState.detectives.Count}.";
                progressforDetective?.Report(result);
            }
            else // Spieler ist Dieb
            {
                progressforThief?.Report("");
            }
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
                if (item.source.Equals(currentState) && item.target.Count != 0)
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
            graphCreate_Panel.Hide();
            TorusCreate_Panel.Hide();
            computeEnt_Panel.Hide();
            playGraph_Button.Hide();
            GameSettings_Panel.Show();
            editGraph_Button.Show();
            restartGame_Button.Hide();
            detMovement_Panel.Hide();
            thiefMovement_Panel.Hide();
            graph.ColorVertex(currentState.thiefPos.ToString(), Microsoft.Msagl.Drawing.Color.White); // entfärbt Graph
            foreach (var detective in currentState.detectives)
            {
                graph.ShapeVertex(detective.ToString(), Microsoft.Msagl.Drawing.Shape.Box);
            }
            graph_PictureBox.Refresh();
        }

        /// <summary>
        /// ändert die Anzeige, je nach ausgewähltem Graphen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chooseGraphTyp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedGraph = (string)chooseGraphTyp_ComboBox.SelectedItem;
            TorusCreate_Panel.Visible = selectedGraph.Equals("Torusgraph");
            createDiCircleGraph_Panel.Visible = selectedGraph.Equals("Gerichteter Kreisgraph");
            createUnCircleGraph_Panel.Visible = selectedGraph.Equals("Ungerichteter Kreisgraph");
            createFullyConGraph_Panel.Visible = selectedGraph.Equals("Komplett verbundener Graph");
            createGirdGraph_Panel.Visible = selectedGraph.Equals("Ungerichteter Gittergraph");

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

        /// <summary>
        /// erstellt einen ungerichteten Kreisgraphen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// erstellt einen gerichteten Kreisgraphen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// erstellt einen vollständig verbundenen Graphen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// lösch einen Knoten und dazugehörige Kanten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// löscht eine Kante
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                    if (possibleEdge != null)
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

        /// <summary>
        /// erstellt einen Gittergraphen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridGraphCreate_Button_Click(object sender, EventArgs e)
        {
            TextOutput_TextBox.Clear();
            int n;
            int m;
            bool isNNumber = int.TryParse(GirdN_TextBox.Text, out n);
            bool istMNumber = int.TryParse(GridM_TextBox.Text, out m);
            if (isNNumber && istMNumber)
            {
                graph = new UndirectedGirdGraph(n, m);
                graph.CreateImage(graph_PictureBox);
                graph_PictureBox.Refresh();
                GirdN_TextBox.Clear();
                GridM_TextBox.Clear();
                GirdN_TextBox.Focus();
            }
            else
            {
                TextOutput_TextBox.Text = "Bitte Ganzzahlen für die Größe eingeben.";
                GirdN_TextBox.Clear();
                GridM_TextBox.Clear();
                GirdN_TextBox.Focus();
            }
        }

        /// <summary>
        /// deaktiviert alle Buttons
        /// </summary>
        private void DeactivateAllButtons()
        {
            deleteGraph_Button.Enabled = false;
            newVertex_Button.Enabled = false;
            addEdge_Button.Enabled = false;
            deleteVertex_Button.Enabled = false;
            deleteEdge_Button.Enabled = false;
            entanglement_Button.Enabled = false;
            playGraph_Button.Enabled = false;
            editGraph_Button.Enabled = false;
            playThief_Button.Enabled = false;
            playDetective_Button.Enabled = false;
            moveDetective_Button.Enabled = false;
            doNothingDet_Button.Enabled = false;
            moveThiefToTarget_Button.Enabled = false;
            restartGame_Button.Enabled = false;
            createTorusGraph_Button.Enabled = false;
            createUndirectedCircleGraph_Button.Enabled = false;
            createDirectedCircleGraph_Button.Enabled = false;
            fullyConCreate_Button.Enabled = false;
            GridGraphCreate_Button.Enabled = false;
        }

        /// <summary>
        /// aktiviert alle Buttons
        /// </summary>
        private void ActivateAllButtons()
        {
            deleteGraph_Button.Enabled = true;
            newVertex_Button.Enabled = true;
            addEdge_Button.Enabled = true;
            deleteVertex_Button.Enabled = true;
            deleteEdge_Button.Enabled = true;
            entanglement_Button.Enabled = true;
            playGraph_Button.Enabled = true;
            editGraph_Button.Enabled = true;
            playThief_Button.Enabled = true;
            playDetective_Button.Enabled = true;
            moveDetective_Button.Enabled = true;
            doNothingDet_Button.Enabled = true;
            moveThiefToTarget_Button.Enabled = true;
            restartGame_Button.Enabled = true;
            createTorusGraph_Button.Enabled = true;
            createUndirectedCircleGraph_Button.Enabled = true;
            createDirectedCircleGraph_Button.Enabled = true;
            fullyConCreate_Button.Enabled = true;
            GridGraphCreate_Button.Enabled = true;
        }
    }
}