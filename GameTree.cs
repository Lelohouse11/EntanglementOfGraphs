using QuikGraph;
using QuikGraph.Graphviz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    // Testing durch Torus grapgen seite 2
    internal class GameTree<V> : AdjacencyGraph<Positions<V>, Edge<Positions<V>>> where V : IComparable<V>, IEquatable<V>
    {
        private readonly bool debug = false;

        public int vertexCounter = 1;
        public int edgeCounter = 0;
        public readonly int detectiveAmount;
        public readonly FiniteDirectedGraph<V> graph;
        public readonly Positions<V> startPosition;

        /// <summary>
        /// erstellt GameTree mit Startwerten
        /// </summary>
        /// <param name="_graph"></param>
        /// <param name="startPos"></param>
        public GameTree(FiniteDirectedGraph<V> _graph, Positions<V> startPos)
        {
            AddVertex(startPos);
            startPosition = startPos;
            detectiveAmount = startPos.detectiveAmount;
            graph = _graph;
            if (debug)
            {
                Console.WriteLine($"Startknoten hinzugefügt: {startPos}");
            }
        }

        /// <summary>
        /// erstellt String von .dot Datei für spätere Visualisierung
        /// </summary>
        /// <returns></returns>
        public String CreateDot()
        {
            var graphviz = new GraphvizAlgorithm<Positions<V>, Edge<Positions<V>>>(this);
            return graphviz.Generate();
        }

        /// <summary>
        /// verbindet alle gefundenen Iterationen des Gametrees zu GameTree
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public GameTree<V> BuildIterativGameTree(Positions<V> pos)
        {
            var nextStep = graph.GetNextPossibleSteps(pos);
            if (nextStep.Count != 0)
            { //prüft ob keine weiteren Knoten gefunden wurden
                foreach (var nextPos in nextStep) // fügt gefunden Knoten hinzu und verbindet sie
                {
                    var isNewPos = GetExistingPosition(nextPos);
                    if (isNewPos == null) // prüft ob nextPos neu ist
                    {
                        AddVertex(nextPos);
                        vertexCounter++;
                        if (debug)
                        {
                            Console.WriteLine($"Knoten hinzugefügt: {nextPos}");
                        }
                    }

                    var targetPos = isNewPos ?? nextPos; // wenn nextPos nicht neu, alte vorhandene Pos benutzen
                    AddEdge(new Edge<Positions<V>>(pos, targetPos));
                    edgeCounter++;
                    if (debug)
                    {
                        Console.WriteLine($"Kante von {pos} zu {targetPos} hinzugefügt.");
                    }
                    if (isNewPos == null || pos.detectivesTurn) // Wenn NextPos neu oder Detektive sich nicht bewegen
                    {
                        BuildIterativGameTree(targetPos); //rekursiver Aufruf
                    }

                }
            }
            return this;
        }

        /// <summary>
        /// baut den GameTree rekursiv auf, allerdings nur bis zum Punkt das Detektiv einen Weg hat, 
        /// dass er sicher gewinnt
        /// </summary>
        /// <param name="currentPos"></param>
        public void BuildRecursiveGameTree(Positions<V> currentPos, int recLevel)
        {
            Console.WriteLine($"{recLevel}, {currentPos}, {Vertices.Count()}");
            var previousPossibleSteps = graph.GetPreviousPossibleSteps(currentPos);
            foreach (var previousPos in previousPossibleSteps) // gehe die vorig möglichen Spielzustände durch
            {
                bool continueRecursion = false;
                if (!previousPos.detectivesTurn) //wenn der Deib dran ist muss jeder möglicher Zug schon im GameTree gespeichert sein
                {
                    continueRecursion = AddThiefVertex(previousPos);                    
                }
                else
                {
                    var temp = AddDetectiveVertex(previousPos, currentPos);
                    continueRecursion = temp.Item1 && !temp.Item2;
                }                
                if (continueRecursion) // Wenn NextPos neu
                {
                    BuildRecursiveGameTree(previousPos, recLevel+1); //rekursiver Aufruf
                }                
            }
        }

        /// <summary>
        /// baut den GameTree durch eine Fixpointiteration auf
        /// </summary>
        public void BuildFixpointGameTree()
        {
            bool startPosReached = false;
            bool continueFixpoint = false;
            do
            {
                continueFixpoint = false;
                var allVertices = Vertices.ToList();
                foreach (var currentPos in allVertices)
                {
                    if (!currentPos.detectivesTurn) //wenn detektiv vorher dran war wird alles hinzugefügt
                    {
                        foreach (var previousPos in graph.GetPreviousPossibleSteps(currentPos)) // alle Knoten mit den man jetzigen Knoten ereichen kann
                        {
                            var temp = AddDetectiveVertex(previousPos, currentPos);
                            continueFixpoint = temp.Item1;
                            startPosReached = temp.Item2;
                        }
                    }
                    else // Dieb war davor dran
                    {
                        foreach (var previousPos in graph.GetPreviousPossibleSteps(currentPos)) // alle Knoten mit den man jetzigen Knoten ereichen kann
                        {                            
                            continueFixpoint = AddThiefVertex(previousPos);
                        }
                        //alle edges werden bei rekursiv nicht ergänzt (code von hier kopieren?)
                    }
                }

            } while (continueFixpoint && !startPosReached);
            if (debug)
            {
                Console.WriteLine("----------Keine neuen Knoten mehr gefunden oder Verbindung zu Startpos gefunden!----------");
            }
        }


        /// <summary>
        /// fügt Knoten wo Dieb dran ist zu GameTree hinzu
        /// </summary>
        /// <param name="previousPos"></param>
        /// <param name="goOn"></param>
        private bool AddThiefVertex(Positions<V> previousPos)
        {
            bool goOn = false;
            var isNewVertex = GetExistingPosition(previousPos); //checken ob schon vorhandener Knoten
            if (isNewVertex == null)
            {
                bool detWin = true;
                var nextPossibleSteps = graph.GetNextPossibleSteps(previousPos);
                foreach (var targetVertex in nextPossibleSteps) // checkt, ob alle ausgehenden Kanten wieder in den GameTree führen
                {
                    if (!ContainsPosition(targetVertex)) //wenn nicht alle Kanten in den Baum führen gewinnt der Detektiv nicht sicher
                    {
                        detWin = false;
                    }
                }
                if (detWin) // Detektiv gewinnt sicher bei Knoten
                {
                    AddVertex(previousPos);
                    goOn = true;
                    if (debug)
                    {
                        Console.WriteLine($"Knoten hinzugefügt: {previousPos}");
                    }
                    foreach (var targetVertex in nextPossibleSteps) // alle Kanten vom Knoten werden hinzugefügt
                    {                        
                        if (debug)
                        {
                            Console.WriteLine($"Kante von {previousPos} zu {targetVertex} hinzugefügt.");
                        }
                        AddEdge(new Edge<Positions<V>>(previousPos, GetExistingPosition(targetVertex)));                        
                    }
                }
            }
            return goOn;
        }

        private (bool,bool) AddDetectiveVertex (Positions<V> previousPos, Positions<V> currentPos)
        {
            bool goOn = false;
            bool startPosReached = false;
            var isNewVertex = GetExistingPosition(previousPos); //checken ob schon vorhandener Knoten
            if (isNewVertex == null)
            {
                AddVertex(previousPos); // Hinzufügen, wenn neu
                goOn = true;                
                if (debug)
                {
                    Console.WriteLine($"Knoten hinzugefügt: {previousPos}");
                }
            }
            else if (isNewVertex.Equals(startPosition))
            {
                startPosReached = true;
                if (debug)
                {
                    Console.WriteLine("-----Kante zum Startpunkt gefunden!-----");
                }
            }
            var sourcePos = isNewVertex ?? previousPos;
            AddEdge(new Edge<Positions<V>>(sourcePos, currentPos)); // Kante hinzufügen
            if (debug)
            {
                Console.WriteLine($"Kante von {sourcePos} zu {currentPos} hinzugefügt.");
            }
            return (goOn,startPosReached);
        }




        /// <summary>
        /// prüft ob gegebener Knoten schon im GameTree vorhanden ist
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool ContainsPosition(Positions<V> pos)
        {
            return Vertices.AsParallel().Any(p => p.Equals(pos));
            /*
            foreach (var p in Vertices)
            {
                if (p.Equals(pos)) return true;
            }
            return false;
            */
        }

    /// <summary>
    /// gibt schon vorhandene Position zurück, wenn Position doppelt
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
        public Positions<V>? GetExistingPosition(Positions<V> pos)
        {
            return Vertices.AsParallel().FirstOrDefault(p => p.Equals(pos));
            /*
            foreach (var p in Vertices)
            {
                if (p.Equals(pos)) return p;
            }
            return null;
            */
        }
        
        /// <summary>
        /// Gibt die möglichen Endzustände des Gametrees zurück
        /// </summary>
        /// <returns></returns>
        public List<Positions<V>> GetPossibleFinalStates()
        {
            var result = new List<Positions<V>>();
            foreach (var vertex in graph.Vertices) //geht jeden Knoten des Graphen durch
            {
                var outgoingVertices = graph.GetOutgoingVertex(vertex).Distinct().ToList();
                var outgoingVerticesCount = outgoingVertices.Count;
                if (outgoingVerticesCount <= detectiveAmount) // prüft ob Fluchtmöglichkeiten von Detektiven blockiert werden können
                {
                    var tempState = new Positions<V>(detectiveAmount, vertex, false);
                    for (var i = 0; i < outgoingVerticesCount; i++)
                    {
                        tempState.detectives.Add(outgoingVertices[i]);// setzt Detektive auf die Fluchtmöglichkeit
                    }
                                       
                    if (detectiveAmount == outgoingVerticesCount)
                    {
                        result.Add(tempState);
                    }
                    else
                    {
                        AddAllDetektives(tempState, result);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Hilfsfunktion für FinalStates um alle möglichen freien Detektive zu positioniernen
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="blockedVertices"></param>
        /// <param name="result"></param>
        private void AddAllDetektives(Positions<V> pos, List<Positions<V>> result)
        { 
            foreach (var vertex in graph.Vertices.Except(pos.detectives)) //Konten die noch frei sind
            {               
                var finalState = pos.Clone();
                finalState.detectives.Add(vertex);
                if (finalState.detectives.Count == detectiveAmount)
                {
                    result.Add(finalState);
                }
                else
                {
                    AddAllDetektives(finalState, result);
                }
            }
        }
    }
}
