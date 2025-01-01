using EntaglementOfGraphs;
using QuikGraph;
//using Microsoft.Msagl.GraphmapsWithMesh;

namespace EntanglementOfGraphs
{
    public partial class MainScreen
    {
        bool whichGraph = true;
        FiniteDirectedGraph<int> graph = new FiniteDirectedGraph<int>();
        TorusGraph tGraph;
        GameTree<int> gameTree;
        GameTree<TorusVertex> gameTreeOfTorus;
        Positions<int> currentPos;
        bool gamestarted;

        public MainScreen()
        {
            InitializeComponent();
            graph.CreateImage(GraphPicture);
        }


        /// <summary>
        /// Stellt den Graphen da
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphPicture_Paint(object sender, PaintEventArgs e)
        {
            if (whichGraph) // entscheidet welcher Graphtyp (int oder Torusvertex)
            {
                graph.DrawImage(e.Graphics, GraphPicture);
            }
            else
            {
                tGraph.DrawImage(e.Graphics, GraphPicture);
            }
        }

        /// <summary>
        /// fügt einen Knoten zum Graphen hinzu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newVertex_Click(object sender, EventArgs e)
        {
            whichGraph = true; // Graphen vom Txpen int
            TextOutput.Clear();
            int vertex;
            bool isNumber = int.TryParse(vertexInput.Text, out vertex);
            if (isNumber) // prüft ob Eingabe Zahl und fügt ihn dann hinzu
            {
                graph.AddVertex(vertex);
                graph.AddVertexToMsagl(vertex);
                graph.CreateImage(GraphPicture);
                GraphPicture.Refresh();
                vertexInput.Clear();
            }
            else // Eingabe unpassend
            {
                TextOutput.Text = "Bitte eine Ganzzahl für den Knoten eingeben.";
                vertexInput.Clear();
            }
            vertexInput.Focus();
        }

        /// <summary>
        /// berechnet das Entanglement vom aktuell angezeigten Graph Graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void entanglement_Click(object sender, EventArgs e)
        {
            TextOutput.Clear();
            int startPos;
            bool isNumber = int.TryParse(startPosInput.Text, out startPos);
            if (isNumber) // prüft ob Startposition gültige Zahl ist
            {
                if (startPos <= graph.VertexCount) // muss gültiger Knoten sein
                {
                    TextOutput.Text = graph.MinEntanglement(startPos).ToString();
                    startPosInput.Clear();
                }
                else // es war kein gültiger Knoten
                {
                    TextOutput.Text = "Bitte einen exsistierenden Knoten eingeben.";
                    startPosInput.Clear();
                    startPosInput.Focus();
                }
            }
            else // es war keine gültige Zahl
            {
                TextOutput.Text = "Bitte eine Ganzzahl für die Startposition eingeben.";
                startPosInput.Clear();
                startPosInput.Focus();
            }
        }

        /// <summary>
        /// fügt eine Kante zum Graphen hinzu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addEdge_Click(object sender, EventArgs e)
        {
            whichGraph = true;
            TextOutput.Clear();
            int source;
            int target;
            bool isSourceNumber = int.TryParse(edgeSource.Text, out source);
            bool isTargetNumber = int.TryParse(edgeTarget.Text, out target);

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
                            edgeSource.Clear();
                            edgeTarget.Clear();
                            graph.CreateImage(GraphPicture);
                            GraphPicture.Refresh();
                            edgeSource.Focus();
                        }
                        else // kein gültiger targetknoten
                        {
                            TextOutput.Text = "Bitte einen exsistierenden Zielknoten eingeben.";
                            edgeTarget.Clear();
                            edgeTarget.Focus();
                        }
                    }
                    else // kein gültiger sourceknoten
                    {
                        TextOutput.Text = "Bitte einen exsistierenden Ursprungsknoten eingeben.";
                        edgeSource.Clear();
                        edgeSource.Focus();
                    }
                }
                else // keine gültige Zahl für targetknoten
                {
                    TextOutput.Text = "Bitte eine Ganzzahl für den Zielknoten eingeben.";
                    edgeTarget.Clear();
                    edgeTarget.Focus();
                }
            }
            else // keine gültige Zahl für sourceknoten
            {
                TextOutput.Text = "Bitte eine Ganzzahl für den Urspungsknoten eingeben.";
                edgeSource.Clear();
                edgeSource.Focus();
            }
        }

        /// <summary>
        /// löscht den eingegebenen Graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteGraph_Click(object sender, EventArgs e)
        {
            whichGraph = true;
            TextOutput.Clear();
            graph = new FiniteDirectedGraph<int>();
            graph.CreateImage(GraphPicture);
            GraphPicture.Refresh();
        }

        /// <summary>
        /// erstellt einen Torusgraph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createTorusGraph_Click(Object sender, EventArgs e)
        {
            whichGraph = false;
            TextOutput.Clear();
            int nTorus;
            int mTorus;
            bool isNTorusNumber = int.TryParse(torusN.Text, out nTorus);
            bool isMTorusNumber = int.TryParse(torusM.Text, out mTorus);

            if (isNTorusNumber && isMTorusNumber) // prüft ob Eingaben gültige Zahl und erstellt dann Graphen
            {
                tGraph = new TorusGraph(nTorus, mTorus);
                tGraph.CreateImage(GraphPicture);
                GraphPicture.Refresh();
                TextOutput.Text = tGraph.MinEntanglement(new TorusVertex(0, 0)).ToString();
                torusN.Clear();
                torusM.Clear();
            }
            else // kein Gültige Zahl eingegeben
            {
                TextOutput.Text = "Bitte Ganzzahlen für den Torusgraphen eingeben.";
                torusN.Clear();
                torusM.Clear();
                torusN.Focus();
            }
        }

        /// <summary>
        /// wechselt in den Spielemodus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playGraph_Click(object sender, EventArgs e)
        {
            TextOutput.Clear();
            startPosInput.Clear();

            //graphCreate.Hide();
            chooseGraphTyp.Hide();
            TorusCreate.Hide();
            createUnCircleGraph.Hide();

            //generateGraph.Hide();
            computeEnt.Hide();
            playGraph.Hide();
            GameSettings.Show();
            editGraph.Show();
            gamestarted = false;
        }

        /// <summary>
        /// wechsel zur Eingabe von Graphen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editGraph_Click(object sender, EventArgs e)
        {
            TextOutput.Clear();
            startPosInput2.Clear();
            detectiveAmountInput.Clear();
            graphCreate.Show();

            //generateGraph.Show();
            chooseGraphTyp.Show();
            TorusCreate.Hide();
            createUnCircleGraph.Hide();

            computeEnt.Show();
            playGraph.Show();
            GameSettings.Hide();
            editGraph.Hide();
            detMovement.Hide();
            thiefMovement.Hide();
            restartGame.Hide();
            if (gamestarted) // löscht die Einfärbung des Graphen
            {
                graph.ColorVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Color.White);
                foreach (var detective in currentPos.detectives)
                {
                    graph.ShapeVertex(detective.ToString(), Microsoft.Msagl.Drawing.Shape.Box);
                }
                GraphPicture.Refresh();
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
                thiefMovement.Show();
                restartGame.Show();
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
                detMovement.Show();
                restartGame.Show();
                TextOutput.Text = $"Du bist am Zug! Wähle einen Detektiv oder tue nichts. Noch nicht Plazierte Detektive: {gameTree.detectiveAmount}.";
                gamestarted = true;
            }
        }

        /// <summary>
        /// prüft eingabe und erstellt das Spiel und passende Strategien
        /// </summary>
        /// <returns></returns>
        private bool checkGameSettings()
        {
            TextOutput.Clear();
            int startPos;
            int detectiveAmount;
            bool isStartPosNumber = int.TryParse(startPosInput2.Text, out startPos);
            bool isDetectiveAmountNumber = int.TryParse(detectiveAmountInput.Text, out detectiveAmount);

            if (isStartPosNumber) // gültige Zahl für startPosition
            {
                if (isDetectiveAmountNumber) // gültige Zahl an Detektiven
                {
                    if (graph.ContainsVertex(startPos)) // gültige Startposition
                    {
                        if (graph.VertexCount >= detectiveAmount) // gültige Anzahl an Detektiven und dann erstellung des Spiels
                        {
                            currentPos = new Positions<int>(detectiveAmount, startPos, true);
                            gameTree = new GameTree<int>(graph, currentPos);
                            gameTree.BuildFixpointGameTree(true);
                            gameTree.CreateStrategies();
                            graph.ColorVertex(startPos.ToString(), Microsoft.Msagl.Drawing.Color.Red);
                            GraphPicture.Refresh();
                            startPosInput2.Clear();
                            detectiveAmountInput.Clear();
                            GameSettings.Hide();
                            return true;
                        }
                        else // kein gültige Anzahl an Detektiven
                        {
                            TextOutput.Text = $"Anzahl der Detektive darf höchstens die Knotenzahl sein, also {graph.VertexCount}";
                            detectiveAmountInput.Clear();
                            detectiveAmountInput.Focus();
                        }
                    }
                    else // keine gültige Startposition
                    {
                        TextOutput.Text = "Bitte einen exsistierenden Ursprungsknoten eingeben.";
                        startPosInput2.Clear();
                        startPosInput2.Focus();
                    }
                }
                else // keine gültige Zahl für Detektive
                {
                    TextOutput.Text = "Bitte eine Ganzzahl für die Anzahl an Detektiven eingeben.";
                    detectiveAmountInput.Clear();
                    detectiveAmountInput.Focus();
                }
            }
            else // keine gültige Zahl für startposition
            {
                TextOutput.Text = "Bitte eine Ganzzahl für den Urspungsknoten eingeben.";
                startPosInput2.Clear();
                startPosInput2.Focus();
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
            TextOutput.Clear();
            int movedDetective;
            bool isMovedDetectiveNumber = int.TryParse(movedDet.Text, out movedDetective);
            if (isMovedDetectiveNumber) // gültige Zahl für gewählten Detektiv
            {
                if ((0 < movedDetective) && (movedDetective <= gameTree.detectiveAmount)) // gültige Wahl für Detektiv
                {
                    if (currentPos.detectives.Count >= movedDetective) // Wenn Detektiv schon auf Spielfeld war
                    {
                        var detPos = currentPos.detectives.ElementAt(movedDetective - 1);
                        graph.ShapeVertex(detPos.ToString(), Microsoft.Msagl.Drawing.Shape.Box);
                        currentPos.MoveDetective(detPos);
                        currentPos.ChangeTurn();
                    }
                    else // Wenn Detektive noch nicht auf Spielfeld war
                    {
                        currentPos.MoveDetective(movedDetective);
                        currentPos.ChangeTurn();
                    }
                    graph.ShapeVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Shape.Diamond);
                    GraphPicture.Refresh();
                    Thread.Sleep(1000);
                    MoveThief(); // simuliert zug des Diebes
                    movedDet.Clear();
                }
                else // ungültige Detektivewahl
                {
                    TextOutput.Text = $"Bitte eine Ganzzahl zwischen 0 und {gameTree.detectiveAmount} für den zu bewegenden Detektiv eingeben.";
                    movedDet.Clear();
                    movedDet.Focus();
                }
            }
            else // ungültige Detektivewahl
            {
                TextOutput.Text = $"Bitte eine Ganzzahl zwischen 0 und {gameTree.detectiveAmount} für den zu bewegenden Detektiv eingeben.";
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
            currentPos.ChangeTurn();
            MoveThief();
        }


        /// <summary>
        /// bewegt den Dieb, wenn man Dieb spielt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void moveThiefToTarget_Click(object sender, EventArgs e)
        {
            TextOutput.Clear();
            int thiefTarget;
            bool isThiefTargetNumber = int.TryParse(targetThiefInput.Text, out thiefTarget);
            if (isThiefTargetNumber) // prüft ob gültige Zahl
            {
                var possibleNextPos = currentPos.Clone();
                possibleNextPos.MoveThief(thiefTarget);
                possibleNextPos.ChangeTurn();
                bool moveAllowed = false;
                foreach (var possibleStep in graph.GetNextPossibleSteps(currentPos)) // prüft ob eingegebener Zug gültig ist
                {
                    if (possibleNextPos.Equals(possibleStep))
                    {
                        moveAllowed = true;
                        break;
                    }
                }
                if (moveAllowed) // wenn gültig, wird Zug durchgeführt
                {
                    graph.ColorVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Color.White);
                    currentPos = possibleNextPos;
                    graph.ColorVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Color.Red);
                    GraphPicture.Refresh();
                    Thread.Sleep(1000);
                    MoveDet(); // simuliert zu des Detektiv
                    targetThiefInput.Clear();
                }
                else // ungültige Zugeingabe
                {
                    TextOutput.Text = $"Bitte eine der folgenden Ganzzahlen eingeben: {graph.GetNextPossibleStepsForThief(currentPos)}.";
                    targetThiefInput.Clear();
                    targetThiefInput.Focus();
                }
            }
            else // ungültige Zugeingabe
            {
                TextOutput.Text = $"Bitte eine der folgenden Ganzzahlen eingeben: {graph.GetNextPossibleStepsForThief(currentPos)}.";
                targetThiefInput.Clear();
                targetThiefInput.Focus();
            }
        }


        /// <summary>
        /// simuliert einen Zug des Diebes, nach berechneter Strategie (Spieler ist Detektiv)
        /// </summary>
        private void MoveThief()
        {
            TextOutput.Clear();
            bool detCanMove = false;
            foreach (var item in gameTree.thiefStrategy) // prüft ob Detetkiv noch einen guten Zug machen kann
            {
                if (item.source.Equals(currentPos))
                {
                    detCanMove = true;
                    break;
                }
            }
            if (!detCanMove) // wenn er Detektiv keinen guten Zug mehr hat, hat der Spieler verloren
            {
                detMovement.Hide();
                TextOutput.Text = "Du hast Verloren!";
            }
            else
            {
                if (graph.GetNextPossibleSteps(currentPos).Count == 0) // Der Detektiv kann dann nicht mehr gewinnen
                {
                    detMovement.Hide();
                    TextOutput.Text = "Du hast Gewonnen!";

                }
                else // führt den Zug des Diebes durch
                {
                    int nextThiefMove = gameTree.BestThiefMove(currentPos); // bester Zug
                    graph.ColorVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Color.White);
                    currentPos.MoveThief(nextThiefMove);
                    currentPos.ChangeTurn();
                    graph.ColorVertex(nextThiefMove.ToString(), Microsoft.Msagl.Drawing.Color.Red);
                    GraphPicture.Refresh();
                    TextOutput.Text = $"Du bist am Zug! Wähle einen Detektiv oder tue nichts. Noch nicht Plazierte Detektive: {gameTree.detectiveAmount - currentPos.detectives.Count}.";
                }
            }
        }

        /// <summary>
        /// simuliert ein Zug des Detektives, nach berechneter Strategie (Spieler ist Dieb)
        /// </summary>
        private void MoveDet()
        {
            TextOutput.Clear();
            if (gameTree.GetExistingPosition(currentPos) == null) // Detektiv hat auf jeden Fall verloren
            {
                thiefMovement.Hide();
                TextOutput.Text = "Du hast Gewonnen!";
            }
            else
            {
                (int, bool) nextDetectiveMove = gameTree.BestDetectiveMove(currentPos); // bester Zug
                if (nextDetectiveMove.Item2)
                {
                    if (nextDetectiveMove.Item1 != 0) // Detektiv ist schon auf dem Graph
                    {
                        graph.ShapeVertex(nextDetectiveMove.Item1.ToString(), Microsoft.Msagl.Drawing.Shape.Box);
                        currentPos.MoveDetective(nextDetectiveMove.Item1);
                        graph.ShapeVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Shape.Diamond);
                        GraphPicture.Refresh();
                    }
                    else // Detektiv ist noch nicht auf dem Graph
                    {
                        currentPos.MoveDetective(nextDetectiveMove.Item1);
                        graph.ShapeVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Shape.Diamond);
                        GraphPicture.Refresh();
                    }
                }
                currentPos.ChangeTurn();
                if (graph.GetNextPossibleSteps(currentPos).Count == 0) // Dieb hat auf jeden Fall verloren
                {
                    thiefMovement.Hide();
                    TextOutput.Text = "Du hast Verloren!!";
                }
                else
                {
                    TextOutput.Text = $"Du bist am Zug! Wähle ein der folgenden Knoten aus: {graph.GetNextPossibleStepsForThief(currentPos)}.";
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
            TextOutput.Clear();
            startPosInput.Clear();
            graphCreate.Hide();
            TorusCreate.Hide();
            computeEnt.Hide();
            playGraph.Hide();
            GameSettings.Show();
            editGraph.Show();
            restartGame.Hide();
            graph.ColorVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Color.White); // entfärbt Graph
            foreach (var detective in currentPos.detectives)
            {
                graph.ShapeVertex(detective.ToString(), Microsoft.Msagl.Drawing.Shape.Box);
            }
            GraphPicture.Refresh();
        }

        private void chooseGraphTyp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedGraph = (string)chooseGraphTyp.SelectedItem;
            if (selectedGraph.Equals("Torusgraph"))
            {
                createUnCircleGraph.Hide();
                createDiCircleGraph.Hide();
                TorusCreate.Show();
            }
            else if (selectedGraph.Equals("Ungerichteter Kreisgraph"))
            {
                TorusCreate.Hide();
                createDiCircleGraph.Hide();
                createUnCircleGraph.Show();
            }
            else if (selectedGraph.Equals("Gerichteter Kreisgraph"))
            {
                TorusCreate.Hide();
                createUnCircleGraph.Hide();
                createDiCircleGraph.Show();
            }
            else if (selectedGraph.Equals("Graph von unärer Funktion"))
            {
                TorusCreate.Hide();
                createUnCircleGraph.Hide();
                createUnCircleGraph.Hide();
            }
            else if (selectedGraph.Equals("Komplett verbundener Graph"))
            {
                TorusCreate.Hide();
                createUnCircleGraph.Hide();
                createUnCircleGraph.Hide();
            }
        }

        private void createUndirectedCircleGraph_Click(object sender, EventArgs e)
        {
            whichGraph = true; // Graphen vom Typen int
            TextOutput.Clear();
            int size;
            bool isNumber = int.TryParse(unCircleSizeInput.Text, out size);
            if (isNumber) // prüft ob Eingabe Zahl und fügt ihn dann hinzu
            {
                graph = new UndirectedCircleGraph(size);
                graph.CreateImage(GraphPicture);
                GraphPicture.Refresh();
                unCircleSizeInput.Clear();
            }
            else // Eingabe unpassend
            {
                TextOutput.Text = "Bitte eine Ganzzahl für die Größe eingeben.";
                unCircleSizeInput.Clear();
            }
            unCircleSizeInput.Focus();
        }

        private void createDirectedCircleGraph_Click(object sender, EventArgs e)
        {
            whichGraph = true; // Graphen vom Typen int
            TextOutput.Clear();
            int size;
            bool isNumber = int.TryParse(diCircleSizeInput.Text, out size);
            if (isNumber) // prüft ob Eingabe Zahl und fügt ihn dann hinzu
            {
                graph = new DirectedCircleGraph(size+1);
                graph.CreateImage(GraphPicture);
                GraphPicture.Refresh();
                diCircleSizeInput.Clear();
            }
            else // Eingabe unpassend
            {
                TextOutput.Text = "Bitte eine Ganzzahl für die Größe eingeben.";
                diCircleSizeInput.Clear();
            }
            diCircleSizeInput.Focus();
        }
    }
}

