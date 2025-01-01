using EntaglementOfGraphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikGraph;

namespace EntanglementOfGraphs
{
    internal class DirectedCircleGraph : FiniteDirectedGraph<int>
    {

        /// <summary>
        /// erstellt einen gerichteten Kreisgraphen passend zur eingegebenen Größe
        /// </summary>
        /// <param name="size"></param>
        public DirectedCircleGraph(int size) 
        {
            AddVertex(1);
            for (int i = 2; i <= size; i++)
            {
                AddVertex(i);
                AddVertexToMsagl(i);
                AddEdge(new Edge<int>(i-1,i));
                AddEdgeToMsagl(i-1,i);
            }
            AddEdge(new Edge<int>(size,1));
            AddEdgeToMsagl(size, 1);
            
        }
    }
}
