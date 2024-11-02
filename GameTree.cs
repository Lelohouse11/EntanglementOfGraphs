using QuikGraph;
using QuikGraph.Graphviz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    internal class GameTree : AdjacencyGraph<Positions, Edge<Positions>>
    {
        public int VertexCounter = 1;
        public int EdgeCounter = 0;
        public readonly int detectiveCount;
        public readonly FiniteDirectedGraph graph;

        public GameTree(FiniteDirectedGraph _graph, Positions startPos) 
        {
            this.AddVertex(startPos);
            detectiveCount = startPos.detectives.Count;
            graph = _graph;
            Console.WriteLine($"Startknoten hinzugefügt: {startPos.thief} ({string.Join(",", startPos.detectives)})");
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
        /// <param name="detectivesTurn"></param>
        /// <returns></returns>
        public GameTree connectAllIterationToGraph(Positions pos, bool detectivesTurn)
        {
            var nextStep = graph.getNextPossibleSteps(pos, detectivesTurn);
            if (nextStep.Count() != 0)
            { //prüft ob keine weiteren Knoten gefunden wurden
                foreach (var nextPos in nextStep) // fügt gefunden Knoten hinzu und verbindet sie
                {
                    var tempPos = getExistingPosition(nextPos);
                    if (tempPos == null) // prüft ob nextPos neu ist
                    {
                        AddVertex(nextPos);
                        VertexCounter++;
                        Console.WriteLine($"Knoten hinzugefügt: {nextPos.thief} ({string.Join(",", nextPos.detectives)})");
                    }

                    var targetPos = tempPos ?? nextPos; // wenn nextPos nicht neu, alte vorhandene Pos benutzen
                    AddEdge(new Edge<Positions>(pos, targetPos));
                    EdgeCounter++;
                    Console.WriteLine($"Kante von {pos.thief} ({string.Join(",", pos.detectives)}) zu {targetPos.thief} ({string.Join(",", targetPos.detectives)}) hinzugefügt.");
                    if (tempPos == null || detectivesTurn) // Wenn NextPos neu oder Detektive sich nicht bewegen
                    {
                        connectAllIterationToGraph(targetPos, !detectivesTurn); //rekursiver Aufruf
                    }
                    
                }
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
    }
}
