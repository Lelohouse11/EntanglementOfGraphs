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

        public MainScreen()
        {
            InitializeComponent();
            graph.CreateImage(GraphPicture);
        }

        private void GraphPicture_Paint(object sender, PaintEventArgs e)
        {
            if (whichGraph)
            {
                graph.DrawImage(e.Graphics, GraphPicture);
            }
            else
            {
                tGraph.DrawImage(e.Graphics, GraphPicture);
            }
        }

        private void newVertex_Click(object sender, EventArgs e)
        {
            whichGraph = true;
            TextOutput.Clear();
            int vertex;
            bool isNumber = int.TryParse(vertexInput.Text, out vertex);
            if (isNumber)
            {
                graph.AddVertex(vertex);
                graph.AddVertexToMsagl(vertex);
                graph.CreateImage(GraphPicture);
                GraphPicture.Refresh();
                vertexInput.Clear();
            }
            else
            {
                TextOutput.Text = "Bitte eine Ganzzahl für den Knoten eingeben.";
                vertexInput.Clear();
            }
        }

        private void entanglement_Click(object sender, EventArgs e)
        {
            TextOutput.Clear();
            int startPos;
            bool isNumber = int.TryParse(startPosInput.Text, out startPos);
            if (isNumber)
            {
                if (startPos <= graph.VertexCount)
                {
                    TextOutput.Text = graph.MinEntanglement(startPos).ToString();
                    startPosInput.Clear();
                }
                else
                {
                    TextOutput.Text = "Bitte einen exsistierenden Knoten eingeben.";
                    startPosInput.Clear();
                }
            }
            else
            {
                TextOutput.Text = "Bitte eine Ganzzahl für die Startposition eingeben.";
                startPosInput.Clear();
            }
        }

        private void addEdge_Click(object sender, EventArgs e)
        {
            whichGraph = true;
            TextOutput.Clear();
            int source;
            int target;
            bool isSourceNumber = int.TryParse(edgeSource.Text, out source);
            bool isTargetNumber = int.TryParse(edgeTarget.Text, out target);

            if (isSourceNumber)
            {
                if (isTargetNumber)
                {
                    if (graph.ContainsVertex(source))
                    {
                        if (graph.ContainsVertex(target))
                        {
                            graph.AddEdge(new Edge<int>(source, target));
                            graph.AddEdgeToMsagl(source, target);
                            edgeSource.Clear();
                            edgeTarget.Clear();
                            graph.CreateImage(GraphPicture);
                            GraphPicture.Refresh();
                        }
                        else
                        {
                            TextOutput.Text = "Bitte einen exsistierenden Zielknoten eingeben.";
                            edgeTarget.Clear();
                        }
                    }
                    else
                    {
                        TextOutput.Text = "Bitte einen exsistierenden Ursprungsknoten eingeben.";
                        edgeSource.Clear();
                    }
                }
                else
                {
                    TextOutput.Text = "Bitte eine Ganzzahl für den Zielknoten eingeben.";
                    edgeTarget.Clear();
                }
            }
            else
            {
                TextOutput.Text = "Bitte eine Ganzzahl für den Urspungsknoten eingeben.";
                edgeSource.Clear();
            }
        }

        private void deleteGraph_Click(object sender, EventArgs e)
        {
            whichGraph = true;
            TextOutput.Clear();
            graph.Clear();
            graph.CreateImage(GraphPicture);
            GraphPicture.Refresh();
        }

        private void createTorusGraph_Click(Object sender, EventArgs e)
        {
            whichGraph = false;
            TextOutput.Clear();
            int nTorus;
            int mTorus;
            bool isNTorusNumber = int.TryParse(torusN.Text, out nTorus);
            bool isMTorusNumber = int.TryParse(torusM.Text, out mTorus);

            if (isNTorusNumber && isMTorusNumber)
            {
                tGraph = new TorusGraph(nTorus, mTorus);
                tGraph.CreateImage(GraphPicture);
                GraphPicture.Refresh();
                TextOutput.Text = tGraph.MinEntanglement(new TorusVertex(0, 0)).ToString();
                torusN.Clear();
                torusM.Clear();
            }
            else
            {
                TextOutput.Text = "Bitte Ganzzahlen für den Torusgraphen eingeben.";
                torusN.Clear();
                torusM.Clear();
            }
        }

        private void playGraph_Click(object sender, EventArgs e)
        {
            TextOutput.Clear();
            startPosInput.Clear();
            graphCreate.Hide();
            TorusCreate.Hide();
            computeEnt.Hide();
            playGraph.Hide();
            GameSettings.Show();
            editGraph.Show();
        }

        private void playThief_Click(object sender, EventArgs e)
        {
            if (checkGameSettings())
            {
                thiefMovement.Show();
                restartGame.Show();
                MoveDet();
            }
        }

        private void playDetective_Click(object sender, EventArgs e)
        {
            if (checkGameSettings())
            {
                detMovement.Show();
                restartGame.Show();
                TextOutput.Text = $"Du bist am Zug! Wähle einen Detektiv oder tue nichts. Noch nicht Plazierte Detektive: {gameTree.detectiveAmount}.";
            }
        }

        private bool checkGameSettings()
        {
            TextOutput.Clear();
            int startPos;
            int detectiveAmount;
            bool isStartPosNumber = int.TryParse(startPosInput2.Text, out startPos);
            bool isDetectiveAmountNumber = int.TryParse(detectiveAmountInput.Text, out detectiveAmount);

            if (isStartPosNumber)
            {
                if (isDetectiveAmountNumber)
                {
                    if (graph.ContainsVertex(startPos))
                    {
                        if (graph.VertexCount >= detectiveAmount)
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
                        else
                        {
                            TextOutput.Text = $"Anzahl der Detektive darf höchstens die Knotenzahl sein, also {graph.VertexCount}";
                            detectiveAmountInput.Clear();
                        }
                    }
                    else
                    {
                        TextOutput.Text = "Bitte einen exsistierenden Ursprungsknoten eingeben.";
                        startPosInput2.Clear();
                    }
                }
                else
                {
                    TextOutput.Text = "Bitte eine Ganzzahl für die Anzahl an Detektiven eingeben.";
                    detectiveAmountInput.Clear();
                }
            }
            else
            {
                TextOutput.Text = "Bitte eine Ganzzahl für den Urspungsknoten eingeben.";
                startPosInput2.Clear();
            }
            return false;
        }

        private void editGraph_Click(object sender, EventArgs e)
        {
            TextOutput.Clear();
            startPosInput2.Clear();
            detectiveAmountInput.Clear();
            graphCreate.Show();
            //TorusCreate.Show();
            computeEnt.Show();
            playGraph.Show();
            GameSettings.Hide();
            editGraph.Hide();
            detMovement.Hide();
            thiefMovement.Hide();
            restartGame.Hide();
            graph.ColorVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Color.White);
            foreach (var detective in currentPos.detectives)
            {
                graph.ShapeVertex(detective.ToString(), Microsoft.Msagl.Drawing.Shape.Box);
            }
            GraphPicture.Refresh();
        }

        private void moveDetective_Click(object sender, EventArgs e)
        {
            TextOutput.Clear();
            int movedDetective;
            bool isMovedDetectiveNumber = int.TryParse(movedDet.Text, out movedDetective);
            if (isMovedDetectiveNumber)
            {
                if ((0 < movedDetective) && (movedDetective <= gameTree.detectiveAmount))
                {
                    if (currentPos.detectives.Count >= movedDetective)
                    {
                        var detPos = currentPos.detectives.ElementAt(movedDetective - 1);
                        graph.ShapeVertex(detPos.ToString(), Microsoft.Msagl.Drawing.Shape.Box);
                        currentPos.MoveDetective(detPos);
                        currentPos.ChangeTurn();
                    }
                    else
                    {
                        currentPos.MoveDetective(movedDetective);
                        currentPos.ChangeTurn();
                    }
                    graph.ShapeVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Shape.Diamond);
                    GraphPicture.Refresh();
                    Thread.Sleep(1000);
                    MoveThief();
                    movedDet.Clear();
                }
                else
                {
                    TextOutput.Text = $"Bitte eine Ganzzahl zwischen 0 und {gameTree.detectiveAmount} für den zu bewegenden Detektiv eingeben.";
                    movedDet.Clear();
                }
            }
            else
            {
                TextOutput.Text = $"Bitte eine Ganzzahl zwischen 0 und {gameTree.detectiveAmount} für den zu bewegenden Detektiv eingeben.";
                movedDet.Clear();
            }
        }

        private void doNothingDet_Click(object sender, EventArgs e)
        {
            currentPos.ChangeTurn();
            MoveThief();
        }

        private void MoveThief()
        {
            TextOutput.Clear();
            if (graph.GetNextPossibleSteps(currentPos).Count == 0)
            {
                detMovement.Hide();
                TextOutput.Text = "Du hast Gewonnen!";

            }
            else
            {
                int nextThiefMove = gameTree.BestThiefMove(currentPos);
                graph.ColorVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Color.White);
                currentPos.MoveThief(nextThiefMove);
                currentPos.ChangeTurn();
                graph.ColorVertex(nextThiefMove.ToString(), Microsoft.Msagl.Drawing.Color.Red);
                GraphPicture.Refresh();
                TextOutput.Text = $"Du bist am Zug! Wähle einen Detektiv oder tue nichts. Noch nicht Plazierte Detektive: {gameTree.detectiveAmount - currentPos.detectives.Count}.";
            }
        }

        private void MoveDet()
        {
            TextOutput.Clear();
            if (gameTree.GetExistingPosition(currentPos) == null)
            {
                thiefMovement.Hide();
                TextOutput.Text = "Du hast Gewonnen!";
            }
            else
            {                
                (int, bool) nextDetectiveMove = gameTree.BestDetectiveMove(currentPos);
                if (nextDetectiveMove.Item2)
                {
                    if (nextDetectiveMove.Item1 != 0)
                    {
                        graph.ShapeVertex(nextDetectiveMove.Item1.ToString(), Microsoft.Msagl.Drawing.Shape.Box);
                        currentPos.MoveDetective(nextDetectiveMove.Item1);
                        graph.ShapeVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Shape.Diamond);
                        GraphPicture.Refresh();
                    }
                    else
                    {
                        currentPos.MoveDetective(nextDetectiveMove.Item1);
                        graph.ShapeVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Shape.Diamond);
                        GraphPicture.Refresh();
                    }
                }
                currentPos.ChangeTurn();
                if (graph.GetNextPossibleSteps(currentPos).Count == 0)
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
            graph.ColorVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Color.White);
            foreach (var detective in currentPos.detectives)
            {
                graph.ShapeVertex(detective.ToString(), Microsoft.Msagl.Drawing.Shape.Box);
            }
            GraphPicture.Refresh();
        }

        private void moveThiefToTarget_Click(object sender, EventArgs e)
        {
            TextOutput.Clear();
            int thiefTarget;
            bool isThiefTargetNumber = int.TryParse(targetThiefInput.Text, out thiefTarget);
            if (isThiefTargetNumber)
            {
                var possibleNextPos = currentPos.Clone() ;
                possibleNextPos.MoveThief(thiefTarget);
                possibleNextPos.ChangeTurn();
                bool temp = false;
                foreach (var possibleStep in graph.GetNextPossibleSteps(currentPos))
                {
                    if (possibleNextPos.Equals(possibleStep))
                    {
                        temp = true;
                        break;
                    }
                }
                if (temp)
                {
                    graph.ColorVertex(currentPos.thief.ToString(),Microsoft.Msagl.Drawing.Color.White);
                    currentPos = possibleNextPos;
                    graph.ColorVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Color.Red);                    
                    GraphPicture.Refresh();
                    Thread.Sleep(1000);
                    MoveDet();
                    targetThiefInput.Clear();
                }
                else
                {
                    TextOutput.Text = $"Bitte eine der folgenden Ganzzahlen eingeben: {graph.GetNextPossibleStepsForThief(currentPos)}.";
                    targetThiefInput.Clear();
                }
            }
            else
            {
                TextOutput.Text = $"Bitte eine der folgenden Ganzzahlen eingeben: {graph.GetNextPossibleStepsForThief(currentPos)}.";
                targetThiefInput.Clear();
            }        
        }
    }
}

