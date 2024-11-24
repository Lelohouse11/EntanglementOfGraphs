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
    internal class FiniteDirectedGraph <V> : AdjacencyGraph<V, Edge<V>> where V : IComparable<V>, IEquatable<V>
    {
        bool debug = false;
        /// <summary>
        /// Konstruktor für Grapherstellung
        /// </summary>
        /// <param name="vertexes"></param>
        /// <param name="edges"></param>
        public FiniteDirectedGraph(List<V> vertexes, List<(V,V)> edges) 
        { 
           foreach (var vertex in vertexes)
           {
                AddVertex(vertex);
           }

            foreach (var edge in edges)
            {
                AddEdge(new Edge<V>(edge.Item1,edge.Item2));
            }
        }

        /// <summary>
        /// Konstruktor für andere Graphenklassen
        /// </summary>
        public FiniteDirectedGraph()
        {

        } 
        
        /// <summary>
        /// erstellt String von .dot Datei für spätere Visualisierung
        /// </summary>
        /// <returns></returns>
        public String createDot()
        {
            var graphviz = new GraphvizAlgorithm<V, Edge<V>>(this);
            return graphviz.Generate();
        }

        /// <summary>
        /// gibt die ereichbaren Knoten von einem anderen Knoten zurück
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public List<V> getOutgoingVertex(V vertex)
        {
            List<V> result = new List<V>();
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
        public GameTree<V>? getGameTree(Positions<V> startPos, GameTreeTyp gameTreeTyp)
        {
            var gameTree = new GameTree<V>(this, startPos);
            if (gameTreeTyp == GameTreeTyp.Iterativ) //wenn iterativer Aufbau
            {
                return gameTree.buildIterativGameTree(startPos);
            }
            else if (gameTreeTyp == GameTreeTyp.Recursiv) // wenn rekursiver Aufbau
            {
                var finalStates = gameTree.getPossibleFinalStates();
                foreach (var finalState in finalStates) //fügt alle möglichen Endzustände hinzu
                {
                    gameTree.AddVertex(finalState);
                    gameTree.vertexCounter++;
                    if (debug)
                    {
                        Console.WriteLine($"Endknoten hinzugefügt: {finalState.toString()}");
                    }
                }

                foreach (var finalState in finalStates) // ruft rekursiven Aufruf auf alle Endzustände auf
                {
                    if (gameTree.OutEdges(startPos).Count() == 0)
                    {
                        gameTree.buildRecursiveGameTree(finalState);
                        if (debug)
                        {
                            Console.WriteLine($"Gefundene wege von einem FinalState zur StartPos: {gameTree.OutEdges(startPos).Count()}");
                        }
                    }
                }
                return gameTree;
            }
            else if (gameTreeTyp == GameTreeTyp.Fixpoint)
            {
                //ToDo: fixpoint iteration
            }
            return null;
        }

        /// <summary>
        /// gibt alle möglichen Zustände durch nächsten Move zurück
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public List<Positions<V>> getNextPossibleSteps(Positions<V> pos)
        {
            List<Positions<V>> result = [];

            if (pos.detectivesTurn) // entscheidet ob Detectives oder Thief einen Zug spielen
            {
                result.Add(pos.changeTurn());
                if (pos.detectiveAmount > pos.detectives.Count)
                {
                    result.Add(pos.clone().moveDetective(default(V)).changeTurn());
                }
                foreach (var detective in pos.detectives) // gehe jeden möglichen Move der Detectives durch
                {
                    result.Add(pos.clone().moveDetective(detective).changeTurn());
                }
            }
            else
            {
                List<V> possibleMoves = getOutgoingVertex(pos.thief).Except(pos.detectives.ToList()).ToList(); // mögliche Züge des Diebes

                if (possibleMoves.Count != 0) //prüft ob Züge möglich sind
                {
                    for (int i = 0; i < possibleMoves.Count(); i++) // geht jeden möglichen Zug
                    {
                        result.Add(pos.clone().moveThief(possibleMoves[i]).changeTurn());
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// gibt alle möglichen vorigen Spielzüge zurück
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public List<Positions<V>> getPreviousPossibleSteps(Positions<V> pos) 
        {
            List <Positions<V>> result = [];

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
                    var previousPos = pos.clone().changeTurn();
                    previousPos.detectives.Remove(pos.thief);                    
                    result.Add(previousPos);
                    foreach (var vertex in Vertices.Except(pos.detectives.ToList())) //bewegt den Dieb zu jedem möglichen Knoten
                    {
                        previousPos = pos.clone().changeTurn();
                        previousPos.detectives.Remove(pos.thief);
                        previousPos.detectives.Add(vertex);
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
        /// <param name="startPos"></param>
        /// <returns></returns>
        public bool isEntanglement(Positions<V> startPos)
        {
            return getGameTree(startPos, GameTreeTyp.Recursiv).OutEdges(startPos).Any();  
            // Wenn Edge von Startknoten gefunden wurde gibt es einen sicheren Weg zu gewinnen         
        }

        /// <summary>
        /// Berechnet minimales Entanglemnet
        /// </summary>
        /// <param name="startPosOfThief"></param>
        /// <returns></returns>
        public int? minEntanglement(V startPosOfThief)
        {
            for (int i = 0; i <= VertexCount; i++) // geht von 0 bis Anzahl an Knoten
            {
                if (isEntanglement(new Positions<V>(i, startPosOfThief, true))) //prüft Entanglment
                {
                    return i; //minimalstes Entanglement
                }
            }
            return null; // Fehler wenn kein Entanglement gefunden wurde
        }
    }
}
