using EntaglementOfGraphs;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntanglementOfGraphs
{
    internal class FullyConnectedGraph: FiniteDirectedGraph<int>
    {
        /// <summary>
        /// erstellt ein komplett verbundenen Graphen
        /// </summary>
        /// <param name="size"></param>
        public FullyConnectedGraph(int size) 
        {
            for (int i = 1; i <= size; i++)
            {
                AddVertex(i);
                AddVertexToMsagl(i);
            }

            for (int i = 1; i <= size; i++)
            {
                for (int j = 1; j <= size; j++)
                {
                    AddEdge(new Edge<int>(i, j));
                    AddEdgeToMsagl(i, j);
                }
            }
        }
    }
}
