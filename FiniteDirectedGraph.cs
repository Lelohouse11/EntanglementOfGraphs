using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Graphviz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    internal class FiniteDirectedGraph : AdjacencyGraph<int, Edge<int>>
    {
        /// <summary>
        /// Konstruktor für Grapherstellung
        /// </summary>
        /// <param name="vertexes"></param>
        /// <param name="edges"></param>
        public FiniteDirectedGraph(List<int> vertexes, List<(int,int)> edges) 
        { 
           foreach (var vertex in vertexes)
           {
                this.AddVertex(vertex);
           }

            foreach (var edge in edges)
            {
                this.AddEdge(new Edge<int>(edge.Item1,edge.Item2));
            }
        }

        /// <summary>
        /// erstellt String von .dot Datei für spätere Visualisierung
        /// </summary>
        /// <returns></returns>
        public String createDot()
        {
            var graphviz = new GraphvizAlgorithm<int, Edge<int>>(this);
            return graphviz.Generate();
        }

        /// <summary>
        /// gibt die ereichbaren Knoten von einem anderen Knoten zurück
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public List<int> getOutgoingVertex(int vertex)
        {
            List<int> result = new List<int>();
            foreach (var edge in this.OutEdges(vertex))
            {
                result.Add(edge.GetOtherVertex(vertex));
            }
            return result;
        }

        /// <summary>
        /// erstellt einen GameTree (kompleter Spielverlauf)
        /// </summary>
        /// <param name="startPos"></param>
        /// <returns></returns>
        public GameTree getGameTree(Positions startPos, bool gameTreeTyp)
        {
            var gameTree = new GameTree(this, startPos);
            if (gameTreeTyp) //wenn ierativer Aufbau
            {
                return gameTree.buildIterativGameTree(startPos);
            }
            else // wenn rekursiver Aufbau
            {
                var finalStates = gameTree.getPossibleFinalStates();
                foreach (var finalState in finalStates) //fügt alle möglichen Endzustände hinzu
                {
                    gameTree.AddVertex(finalState);
                    gameTree.vertexCounter++;
                    Console.WriteLine($"Endknoten hinzugefügt: {finalState.thief} ({string.Join(",", finalState.detectives)}) {finalState.detectivesTurn}");                    
                }

                foreach (var finalState in finalStates) // ruft rekursiven Aufruf auf alle Endzustände auf
                {
                    if (gameTree.OutEdges(startPos).Count() == 0)
                    {
                        gameTree.buildRecursiveGameTree(finalState);
                        Console.WriteLine(gameTree.OutEdges(startPos).Count());
                    }
                }
                return gameTree;
            }
        }

        /// <summary>
        /// gibt alle möglichen Zustände durch nächsten Move zurück
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public List<Positions> getNextPossibleSteps(Positions pos)
        {
            List<Positions> result = [];

            if (pos.detectivesTurn) // entscheidet ob Detectives oder Thief einen Zug spielen
            {
                result.Add(pos);
                for (int i = 0; i < pos.detectives.Count; i++) // gehe jeden möglichen Move der Detectives durch
                {
                    result.Add(pos.clone().moveDetective(i));
                }
            }
            else
            {
                List<int> possibleMoves = getOutgoingVertex(pos.thief).Except(pos.detectives).ToList(); // mögliche Züge des Diebes

                if (possibleMoves.Count != 0) //prüft ob Züge möglich sind
                {
                    for (int i = 0; i < possibleMoves.Count(); i++) // geht jeden möglichen Zug
                    {
                        result.Add(pos.clone().moveThief(possibleMoves[i]));
                    }
                }
            }
            return result;
        }

        public List<Positions> getPreviousPossibleSteps(Positions pos) 
        {
            List <Positions> result = [];

            if (pos.detectivesTurn) // wenn detektiv dran ist war davor der Dieb dran (pos enthält den Zug des Dieb)
            {
                foreach (var edge in Edges)
                {
                    if (edge.Target.Equals(pos.thief)) // alle möglichen Knoten von den der Dieb kommen kann
                    {
                        var previousPos = pos.clone().changeTurn();
                        previousPos.thief = edge.Source;
                        result.Add(previousPos); // werden zum Result hinzugefügt
                    }
                }
            }
            else // wenn Dieb dran ist waren davor die Detektive dran (Detektive dran)
            {
                if (pos.detectives.Contains(pos.thief)) // Wenn sich ein Detektiv auf der Position des Diebes befindet wurde dieser Detektiv bewegt
                {
                    var detectiveIndex = pos.detectives.IndexOf(pos.thief);
                    var previousPos = pos.clone().changeTurn();
                    previousPos.detectives[detectiveIndex] = -1;
                    result.Add(previousPos);
                    foreach (var vertex in Vertices.Except(pos.detectives)) //bewegt den Dieb zu jedem möglichen Knoten
                    {
                        previousPos = pos.clone().changeTurn();
                        previousPos.detectives[detectiveIndex] = vertex;
                        result.Add(previousPos);
                    }
                    
                }
                else // Diebe haben sich nicht bewegt
                {
                    result.Add(pos.clone().changeTurn()); // gleiche Position wird hinzugefügt, nur mit der Info das Dieb dran war
                }
            }
            return result;
        }

        /// <summary>
        /// nutzt den rekursiven Gamtree um Entanglement zu überprüfen
        /// </summary>
        /// <param name="initalPos"></param>
        /// <returns></returns>
        public bool IsEntanglement(Positions initalPos)
        {
            return getGameTree(initalPos,false).OutEdges(initalPos).Count() != 0;            
        }



    }
}
