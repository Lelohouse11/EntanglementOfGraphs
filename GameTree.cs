using QuikGraph;
using QuikGraph.Graphviz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    // Testing durch Torus grapgen seite 2
    internal class GameTree : AdjacencyGraph<Positions, Edge<Positions>>
    {
        public int VertexCounter = 1;
        public int EdgeCounter = 0;
        public readonly int detectiveAmount;
        public readonly FiniteDirectedGraph graph;

        /// <summary>
        /// erstellt GameTree mit Startwerten
        /// </summary>
        /// <param name="_graph"></param>
        /// <param name="startPos"></param>
        public GameTree(FiniteDirectedGraph _graph, Positions startPos) 
        {
            this.AddVertex(startPos);
            detectiveAmount = startPos.detectives.Count;
            graph = _graph;
            Console.WriteLine($"Startknoten hinzugefügt: {startPos.thief} ({string.Join(",", startPos.detectives)}) {startPos.detectivesTurn}");
        }
        /// <summary>
        /// erstellt String von .dot Datei für spätere Visualisierung
        /// </summary>
        /// <returns></returns>
        public String createDot()
        {
            var graphviz = new GraphvizAlgorithm<Positions, Edge<Positions>>(this);
            return graphviz.Generate();
        }

        /// <summary>
        /// verbindet alle gefundenen Iterationen des Gametrees zu GameTree
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public GameTree buildIterativGameTree(Positions pos)
        {
            var nextStep = graph.getNextPossibleSteps(pos);
            if (nextStep.Count() != 0)
            { //prüft ob keine weiteren Knoten gefunden wurden
                foreach (var nextPos in nextStep) // fügt gefunden Knoten hinzu und verbindet sie
                {
                    var isNewPos = getExistingPosition(nextPos);
                    if (isNewPos == null) // prüft ob nextPos neu ist
                    {
                        AddVertex(nextPos);
                        VertexCounter++;
                        Console.WriteLine($"Knoten hinzugefügt: {nextPos.thief} ({string.Join(",", nextPos.detectives)}) {nextPos.detectivesTurn}");
                    }

                    var targetPos = isNewPos ?? nextPos; // wenn nextPos nicht neu, alte vorhandene Pos benutzen
                    AddEdge(new Edge<Positions>(pos, targetPos));
                    EdgeCounter++;
                    Console.WriteLine($"Kante von {pos.thief} ({string.Join(",", pos.detectives)}) {pos.detectivesTurn} zu {targetPos.thief} ({string.Join(",", targetPos.detectives)}) {targetPos.detectivesTurn} hinzugefügt.");
                    if (isNewPos == null || pos.detectivesTurn) // Wenn NextPos neu oder Detektive sich nicht bewegen
                    {
                        buildIterativGameTree(targetPos.changeTurn()); //rekursiver Aufruf
                    }
                    
                }
            }
            return this;
        }

        public GameTree buildRecusiveGameTree(List<Positions> currentStates) 
        { 
            foreach (var vertex in currentStates)
            {

            }
         return this;
        }



        public bool containsPosition(Positions pos)
        {
            foreach (var p in Vertices)
            {
                if (p.Equals(pos)) return true;
            }
            return false;
        }

        public Positions? getExistingPosition(Positions pos)
        {
            foreach (var p in Vertices)
            {
                if (p.Equals(pos)) return p;
            }
            return null;
        }
        
        /// <summary>
        /// Gibt die möglichen Endzustände des Gametrees zurück
        /// </summary>
        /// <returns></returns>
        public List<Positions> getPossibleFinalStates()
        {
            var result = new List<Positions>();
            foreach (var vertex in graph.Vertices)
            {
                var temp = graph.getOutgoingVertex(vertex);
                if (temp.Count <= detectiveAmount)
                {
                    var finalState = new Positions(detectiveAmount, vertex, false);
                    for (var i = 0; i < temp.Count; i++)
                    {
                        finalState.setDetectivePos(i, temp[i]);
                    }
                    result.Add(finalState);
                }
            }
            return result;
        }
    }
}
