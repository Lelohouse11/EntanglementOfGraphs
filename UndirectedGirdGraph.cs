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
        public UndirectedGirdGraph(int n, int m) 
        {
            int counter = 0;
            // Knoten hinzufügen
            for (int i = 1; i <= n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    //int vertex = i * m + j;
                    AddVertex(i+j+counter);
                    AddVertexToMsagl(i+j+counter);
                }
                counter += m -1;
            }
            counter = 0;
            for (int i = 1; i <= n; i++)
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


            /*
            // Kanten hinzufügen
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int vertex = i * m + j;

                    // Kante nach rechts hinzufügen
                    if (j < m - 1)
                    {
                        int rightVertex = vertex + 1;
                        AddEdge(new Edge<int>(vertex, rightVertex));
                        AddEdge(new Edge<int>(rightVertex, vertex));
                        AddEdgeToMsagl(vertex, rightVertex);
                        AddEdgeToMsagl(rightVertex, vertex);
                    }

                    // Kante nach unten hinzufügen
                    if (i < n - 1)
                    {
                        int bottomVertex = vertex + m;
                        AddEdge(new Edge<int>(vertex, bottomVertex));
                        AddEdge(new Edge<int>(bottomVertex, vertex));
                        AddEdgeToMsagl(vertex, bottomVertex);
                        AddEdgeToMsagl(bottomVertex, vertex);
                    }
                }
            }*/
        }
    }
}
