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
                    AddVertex(new TorusVertex(i, j));
                    //Console.WriteLine($"Knoten hinzugefügt: ({i},{j})");
                }
            }
            
            foreach (var vertex in Vertices)
            {
                foreach (var nextVertex in Vertices)
                {
                    if (((vertex.ZweiterWert == nextVertex.ZweiterWert) && (vertex.ErsterWert + 1) == nextVertex.ErsterWert) || //bewegung auf ersten Graphen einen weiter auf Kreis
                       ((vertex.ErsterWert == nextVertex.ErsterWert) && (vertex.ZweiterWert + 1) == nextVertex.ZweiterWert))    //bewegung auf zweiten Graphen einen weiter auf Kreis
                    {
                        AddEdge(new Edge<TorusVertex>(vertex, nextVertex));

                        //Console.WriteLine($"Kante von {vertex.toString()} zu {nextVertex.toString()} hinzugefügt.");
                    }                    
                    else if (((vertex.ZweiterWert == nextVertex.ZweiterWert) && vertex.ErsterWert == (mTorus-1) && (nextVertex.ErsterWert == 0)) || //Endpunkt mit Anfangspunkt verbinden
                       ((vertex.ErsterWert == nextVertex.ErsterWert) && vertex.ZweiterWert == (nTorus - 1) && (nextVertex.ZweiterWert == 0)))
                    {
                        AddEdge(new Edge<TorusVertex>(vertex, nextVertex));
                        //Console.WriteLine($"Kante von {vertex.toString()} zu {nextVertex.toString()} hinzugefügt.");
                    }
                    
                }

            }
        }
    }
}
