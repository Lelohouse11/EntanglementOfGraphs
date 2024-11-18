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
        public TorusGraph(int mTorus, int nTorus)
        {
            for (int i = 0; i < mTorus; i++)
            {
                for (int j = 0; j < nTorus; j++)
                {
                    var newVertex = new TorusVertex(i,j);
                    AddVertex(newVertex);                    
                }
            }

            foreach (var vertex in Vertices)
            {
                foreach (var nextVertex in Vertices.Except([vertex]))
                {
                    if (((vertex.ZweiterWert == nextVertex.ZweiterWert) && vertex.ErsterWert == (nextVertex.ErsterWert + 1)) || //bewegung auf ersten Graphen einen weiter auf Kreis
                       ((vertex.ErsterWert == nextVertex.ErsterWert) && vertex.ZweiterWert == (nextVertex.ZweiterWert + 1)))    //bewegung auf zweiten Graphen einen weiter auf Kreis
                    {
                        AddEdge(new Edge<TorusVertex>(vertex, nextVertex));
                        AddEdge(new Edge<TorusVertex>(nextVertex, vertex));
                    }
                    if (((vertex.ZweiterWert == nextVertex.ZweiterWert) && vertex.ErsterWert == (mTorus-1) && (nextVertex.ErsterWert == 0)) || //Endpunkt mit Anfangspunkt verbinden
                       ((vertex.ErsterWert == nextVertex.ErsterWert) && vertex.ZweiterWert == (nTorus - 1) && (nextVertex.ZweiterWert == 0)))
                    {
                        AddEdge(new Edge<TorusVertex>(nextVertex,vertex));
                        AddEdge(new Edge<TorusVertex>(vertex, nextVertex));
                    }
                }

            }
        }
    }
}
