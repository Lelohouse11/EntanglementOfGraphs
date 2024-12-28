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
        private readonly bool debug = false;

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
                    if (debug)
                    {
                        Console.WriteLine($"Knoten hinzugefügt: ({i},{j})");
                    }
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

                        if (debug)
                        {
                            Console.WriteLine($"Kante von {vertex} zu {nextVertex} hinzugefügt.");
                        }
                    }                    
                    else if (((vertex.ZweiterWert == nextVertex.ZweiterWert) && vertex.ErsterWert == (mTorus-1) && (nextVertex.ErsterWert == 0)) || //Endpunkt mit Anfangspunkt verbinden
                       ((vertex.ErsterWert == nextVertex.ErsterWert) && vertex.ZweiterWert == (nTorus - 1) && (nextVertex.ZweiterWert == 0)))
                    {
                        // Endender Knoten wird mit Anfangsknoten verbunden
                        AddEdge(new Edge<TorusVertex>(vertex, nextVertex));

                        if (debug)
                        {
                            Console.WriteLine($"Kante von {vertex} zu {nextVertex} hinzugefügt.");
                        }
                    }
                    
                }

            }
        }
    }
}
