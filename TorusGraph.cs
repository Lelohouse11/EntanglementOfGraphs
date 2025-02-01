using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    internal class TorusGraph : FiniteDirectedGraph<TorusVertex>
    {
        public TorusGraph() { }

        /// <summary>
        /// erstellt mxn Torus Graph
        /// </summary>
        /// <param name="mTorus"></param>
        /// <param name="nTorus"></param>
        public TorusGraph(int mTorus, int nTorus)
        {
            //AddVertex(new TorusVertex(-1, -1));
            for (int i = 0; i < mTorus; i++) // m Knoten
            {
                for (int j = 0; j < nTorus; j++) // n Knoten
                {
                    AddVertex(new TorusVertex(i, j));
                }
            }
            
            foreach (var vertex in Vertices) // fügt Edges Hinzu
            {
                foreach (var nextVertex in Vertices)
                {
                    if (((vertex.ZweiterWert == nextVertex.ZweiterWert) && (vertex.ErsterWert + 1) == nextVertex.ErsterWert) || //bewegung auf ersten Graphen einen weiter auf Kreis
                       ((vertex.ErsterWert == nextVertex.ErsterWert) && (vertex.ZweiterWert + 1) == nextVertex.ZweiterWert))    //bewegung auf zweiten Graphen einen weiter auf Kreis
                    {
                        // Folgender Knoten im Kreis
                        AddEdge(new Edge<TorusVertex>(vertex, nextVertex));
                    }                    
                    else if (((vertex.ZweiterWert == nextVertex.ZweiterWert) && vertex.ErsterWert == (mTorus-1) && (nextVertex.ErsterWert == 0)) || //Endpunkt mit Anfangspunkt verbinden
                       ((vertex.ErsterWert == nextVertex.ErsterWert) && vertex.ZweiterWert == (nTorus - 1) && (nextVertex.ZweiterWert == 0)))
                    {
                        // Endender Knoten wird mit Anfangsknoten verbunden
                        AddEdge(new Edge<TorusVertex>(vertex, nextVertex));
                    }
                    
                }

            }
        }

        public FiniteDirectedGraph<int> TranslateToInt()
        {
            var result = new FiniteDirectedGraph<int>();
            var allVertices = Vertices.ToList();
            var allEdges = Edges.ToList();
            List<(TorusVertex, int)> vertices = [];
            int vertexCounter = 1;

            foreach (var vertex in allVertices)
            {
                vertices.Add((vertex,vertexCounter));
                result.AddVertex(vertexCounter);
                result.AddVertexToMsagl(vertexCounter);
                vertexCounter++;                
            }
            foreach (var sourceVertex in vertices)
            {
                foreach (var edge in allEdges)
                {
                    if (edge.Source.Equals(sourceVertex.Item1))
                    {
                        foreach (var targetVertex in vertices)
                        {
                            if (edge.Target.Equals(targetVertex.Item1))
                            {
                                result.AddEdge(new Edge<int>(sourceVertex.Item2,targetVertex.Item2));
                                result.AddEdgeToMsagl(sourceVertex.Item2, targetVertex.Item2);
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
