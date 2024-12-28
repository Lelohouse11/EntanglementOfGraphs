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
            int startPos;
            bool isNumber = int.TryParse(startPosInput.Text, out startPos);
            if (isNumber)
            {
                if (startPos <= graph.VertexCount)
                {                    
                    startPosInput.Clear();
                    graphCreate.Hide();
                    TorusCreate.Hide();
                    computeOrGame.Hide();
                   
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
    }
}
