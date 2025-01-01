using EntaglementOfGraphs;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntanglementOfGraphs
{
    internal class UndirectedCircleGraph: FiniteDirectedGraph<int>
    {
        /// <summary>
        /// erstellt einen gerichteten Kreisgraphen passend zur eingegebenen Größe
        /// </summary>
        /// <param name="size"></param>
        public UndirectedCircleGraph(int size)
        {
            AddVertex(1);
            for (int i = 2; i <= size; i++)
            {
                AddVertex(i);
                AddVertexToMsagl(i);
                AddEdge(new Edge<int>(i - 1, i));
                AddEdgeToMsagl(i - 1, i);
                AddEdge(new Edge<int>(i, i-1));
                AddEdgeToMsagl(i, i-1);
            }
            if (size != 2)
            {
                AddEdge(new Edge<int>(size, 1));
                AddEdgeToMsagl(size, 1);
                if (size > 1)
                {
                    AddEdge(new Edge<int>(1, size));
                    AddEdgeToMsagl(1, size);
                }
            }
        }
    }
}
