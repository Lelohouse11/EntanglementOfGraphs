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
            detMovement.Show();
            checkGameSettings();
            detMovement.Show();
        }

        private void playDetective_Click(object sender, EventArgs e)
        {
            detMovement.Show();
            checkGameSettings();
            detMovement.Show();
            TextOutput.Text = "Du bist am Zug! Wähle einen Detektiv oder tue nichts.";
        }

        private void checkGameSettings()
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
        }

        private void editGraph_Click(object sender, EventArgs e)
        {
            TextOutput.Clear();
            startPosInput2.Clear();
            detectiveAmountInput.Clear();
            graphCreate.Show();
            TorusCreate.Show();
            computeEnt.Show();
            playGraph.Show();
            GameSettings.Hide();
            editGraph.Hide();
            detMovement.Hide();
        }

        private void moveDetective_Click(object sender, EventArgs e)
        {
            TextOutput.Clear();
            int movedDetective;
            bool isMovedDetectiveNumber = int.TryParse(movedDet.Text, out movedDetective);
            if (isMovedDetectiveNumber)
            {
                if (movedDetective <= gameTree.detectiveAmount)
                {
                    if(movedDetective >= 0)
                    {
                        graph.ShapeVertex(movedDetective.ToString(),Microsoft.Msagl.Drawing.Shape.Circle);
                    }
                    currentPos.MoveDetective(movedDetective);
                    currentPos.ChangeTurn();
                    graph.ShapeVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Shape.Diamond);
                    moveThief();
                }
            }
        }

        private void doNothingDet_Click(object sender, EventArgs e)
        {
            currentPos.ChangeTurn();
            moveThief();
        }

        private void moveThief()
        {
            int nextThiefMove = gameTree.BestThiefMove(currentPos);
            graph.ColorVertex(currentPos.thief.ToString(), Microsoft.Msagl.Drawing.Color.White);
            currentPos.MoveThief(nextThiefMove);
            currentPos.ChangeTurn();
            graph.ColorVertex(nextThiefMove.ToString(), Microsoft.Msagl.Drawing.Color.Red);
            TextOutput.Text = "Du bist am Zug! Wähle einen Detektiv oder tue nichts.";
        }

        private void moveDetective()
        {

        }
    }
}

