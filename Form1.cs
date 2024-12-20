using EntaglementOfGraphs;
using Microsoft.Msagl.GraphmapsWithMesh;

namespace EntanglementOfGraphs
{
    public partial class Form1
    {
        TorusGraph graph;

        public Form1()
        {
            InitializeComponent();
            graph = new TorusGraph(2, 2);
            graph.CreateImage();
            //GraphPicture.Image = Image.FromFile("test.png");

            graph.CreateImage(GraphPicture);
        }

        private void GraphPicture_Paint(object sender, PaintEventArgs e)
        {
            graph.DrawImage(e.Graphics,GraphPicture);
        }
    }
}
