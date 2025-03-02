using EntaglementOfGraphs;
using Microsoft.Msagl.GraphmapsWithMesh;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntanglementOfGraphs
{
    class UndirectedGirdGraph : FiniteDirectedGraph<int>
    {
        /// <summary>
        /// Konstruktor für ein ungerichtetes Gittergraphen der Größe n x m
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        public UndirectedGirdGraph(int n, int m) 
        {
            int counter = 0;
            for (int i = 1; i <= n; i++) //Knoten hinzufügen
            {
                for (int j = 0; j < m; j++)
                {
                    AddVertex(i+j+counter);
                    AddVertexToMsagl(i+j+counter);
                }
                counter += m -1;
            }
            counter = 0;
            for (int i = 1; i <= n; i++) // Kanten hinzufügen
            {
                for (int j = 0; j < m; j++)
                {
                    if (j < m - 1)
                    {
                        AddEdge(new Edge<int>(i + j + counter, i + j + counter + 1));
                        AddEdgeToMsagl(i + j + counter, i + j + counter + 1);
                        AddEdge(new Edge<int>(i + j + counter + 1, i + j + counter));
                        AddEdgeToMsagl(i + j + counter + 1, i + j + counter);
                    }
                    if (i < n)
                    {
                        AddEdge(new Edge<int>(i + j + counter, i + j + counter + m));
                        AddEdgeToMsagl(i + j + counter, i + j + counter + m);
                        AddEdge(new Edge<int>(i + j + counter + m, i + j + counter));
                        AddEdgeToMsagl(i + j + counter + m, i + j + counter);
                    }
                }
                counter += m -1;
            }           
        }
    }
}
