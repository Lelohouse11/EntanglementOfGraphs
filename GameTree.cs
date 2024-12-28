using EntanglementOfGraphs;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    internal class GameTree<V> : AdjacencyGraph<Positions<V>, Edge<Positions<V>>> where V : IComparable<V>, IEquatable<V>
    {
        private readonly bool debug = false;

        public readonly int detectiveAmount;
        public readonly FiniteDirectedGraph<V> graph;
        public readonly Positions<V> startPosition;
        public readonly List<Move<V>> detectiveStrategy = [];
        public readonly List<Move<V>> thiefStrategy = [];

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
               
        public void CreateDetectiveStrategy ()
        {
            FlagIterativGameTree(GetPossibleFinalStates());
            var allVertices = Vertices.ToList();
            foreach (var vertex in allVertices)
            {
                if (vertex.detectivesTurn)
                {
                    nextDetectiveMove(vertex);
                }
            }
        }

        public void CreateThiefStrategy()
        {
            FlagIterativGameTree(GetPossibleFinalStates());
            var allVertices = Vertices.ToList();
            foreach (var vertex in allVertices)
            {
                if (!vertex.detectivesTurn)
                {
                    nextThiefMove(vertex);
                }
            }
        }

        private void nextDetectiveMove(Positions<V> currentPos)
        {
            List<Positions<V>> result = [];
            var outgoingVertex = GetOutgoingVertex(currentPos);
            int? minFlagOfTarget = VertexCount;
            foreach (var vertex in outgoingVertex)
            {
                if ((vertex.flag < minFlagOfTarget) && (vertex.flag != null))
                {
                    minFlagOfTarget = vertex.flag;
                }
            }
            foreach (var vertex in outgoingVertex)
            {
                if (minFlagOfTarget < currentPos.flag)
                {
                    if (vertex.flag == minFlagOfTarget)
                    {
                        result.Add(vertex);
                    }
                }
            }
            detectiveStrategy.Add(new Move<V>(currentPos,result));
        }

        private void nextThiefMove(Positions<V> currentPos)
        {
            List<Positions<V>> result = [];
            var nextPossibleSteps = graph.GetNextPossibleSteps(currentPos);
            int? maxFlagOfTarget = 0;
            foreach (var vertex in nextPossibleSteps)
            {
                if (vertex.flag == null) 
                {
                    maxFlagOfTarget = null;
                    break;
                }
                if (vertex.flag > maxFlagOfTarget)
                {
                    maxFlagOfTarget = vertex.flag;
                }
            }
            foreach (var vertex in nextPossibleSteps)
            {               
                if (vertex.flag == maxFlagOfTarget)
                {
                    result.Add(vertex);
                }                
            }
            thiefStrategy.Add(new Move<V>(currentPos, result));
        }

        public V BestDetectiveMove(Positions<V> currentPos)
        {
            foreach (var move in detectiveStrategy)
            {
                if(move.source.Equals(currentPos))
                {
                    return currentPos.getMovedDetective(move.target.First());
                }
            }
            return currentPos.detectives.First();
        }

        public V BestThiefMove(Positions<V> currentPos)
        {
            foreach (var move in thiefStrategy)
            {
                if (move.source.Equals(currentPos))
                {
                    return move.target.First().thief;
                }
            }
            return default;
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
                        if (debug)
                        {
                            Console.WriteLine($"Knoten hinzugefügt: {nextPos}");
                        }
                    }

                    var targetPos = isNewPos ?? nextPos; // wenn nextPos nicht neu, alte vorhandene Pos benutzen
                    AddEdge(new Edge<Positions<V>>(pos, targetPos));
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

        private void FlagIterativGameTree(List<Positions<V>> currentPos)
        {
            foreach (var pos in currentPos)
            {
                List<Positions<V>> exsistingPreviousPos = [];
                foreach (var previousPos in graph.GetPreviousPossibleSteps(pos))
                {
                    if (previousPos.flag == null)
                    {
                        var temp = GetExistingPosition(previousPos);
                        temp.flag = pos.flag + 1;
                        exsistingPreviousPos.Add(temp);
                    }
                }
                FlagIterativGameTree(exsistingPreviousPos);
            }
        }

        /// <summary>
        /// baut den GameTree rekursiv auf, allerdings nur bis zum Punkt das Detektiv einen Weg hat, 
        /// dass er sicher gewinnt
        /// </summary>
        /// <param name="currentPos"></param>
        public void BuildRecursiveGameTree(Positions<V> currentPos)
        {
            //Console.WriteLine($"{recLevel}, {currentPos}, {Vertices.Count()}");
            var previousPossibleSteps = graph.GetPreviousPossibleSteps(currentPos);
            foreach (var previousPos in previousPossibleSteps) // gehe die vorig möglichen Spielzustände durch
            {
                bool continueRecursion = false;
                if (!previousPos.detectivesTurn) //wenn der Deib dran ist muss jeder möglicher Zug schon im GameTree gespeichert sein
                {
                    continueRecursion = AddThiefVertex(previousPos);  // fügt Diebknoten hinzu                  
                }
                else
                {
                    var temp = AddDetectiveVertex(previousPos, currentPos); // fügt Detektivknoten hinzu
                    continueRecursion = temp.Item1 && !temp.Item2;
                }                
                if (continueRecursion) // Wenn neue Knoten hinzugekommen sind oder Startknoten gefunden wurde
                {
                    BuildRecursiveGameTree(previousPos); //rekursiver Aufruf
                }                
            }
        }

        /// <summary>
        /// baut den GameTree durch eine Fixpointiteration auf
        /// </summary>
        public void BuildFixpointGameTree()
        {
            bool startPosReached = false;
            bool continueFixpoint;
            do
            {
                continueFixpoint = false;
                var allVertices = Vertices.ToList();
                foreach (var currentPos in allVertices) // geht alle Knoten in bisheriger Menge durch
                {
                    if (!currentPos.detectivesTurn) //wenn detektiv vorher dran war wird alles hinzugefügt
                    {
                        foreach (var previousPos in graph.GetPreviousPossibleSteps(currentPos)) // alle Knoten mit den man jetzigen Knoten ereichen kann
                        {
                            var temp = AddDetectiveVertex(previousPos, currentPos); // fügt Detektivknoten hinzu
                            continueFixpoint = continueFixpoint || temp.Item1;
                            startPosReached = startPosReached || temp.Item2;
                        }
                    }
                    else // Dieb war davor dran
                    {
                        foreach (var previousPos in graph.GetPreviousPossibleSteps(currentPos)) // alle Knoten mit den man den jetzigen Knoten ereichen kann
                        {                            
                            continueFixpoint = continueFixpoint || AddThiefVertex(previousPos); // fügt Diebknoten hinzu
                        }
                    }
                }

            } while (continueFixpoint && !startPosReached); // macht ewweiter , wenn neuen Knoten oder och nicht Startknoten gefunden wurde
            if (debug)
            {
                Console.WriteLine("----------Keine neuen Knoten mehr gefunden oder Verbindung zu Startpos gefunden!----------");
            }
        }

        /// <summary>
        /// fügt Knoten wo Dieb dran ist zu GameTree hinzu
        /// </summary>
        /// <param name="previousPos"></param>
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

        /// <summary>
        /// fügt Knoten hinzu, wo der Detektiv dran ist
        /// </summary>
        /// <param name="previousPos"></param>
        /// <param name="currentPos"></param>
        /// <returns></returns>
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
            else if (isNewVertex.Equals(startPosition)) // prüft ob Startknoten gefunden
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
            foreach (var p in Vertices)
            {
                if (p.Equals(pos)) return true;
            }
            return false;            
        }

    /// <summary>
    /// gibt schon vorhandene Position zurück, wenn Position doppelt
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
        public Positions<V>? GetExistingPosition(Positions<V> pos)
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
                        tempState.flag = 0;
                    }
                                       
                    if (detectiveAmount == outgoingVerticesCount)
                    {
                        result.Add(tempState);
                    }
                    else
                    {
                        AddAllDetektivesToState(tempState, result);
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
        private void AddAllDetektivesToState(Positions<V> pos, List<Positions<V>> result)
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
                    AddAllDetektivesToState(finalState, result);
                }
            }
        }

        public List<Positions<V>> GetOutgoingVertex(Positions<V> vertex)
        {
            List<Positions<V>> result = [];
            foreach (var edge in this.OutEdges(vertex))
            {
                result.Add(edge.Target);
            }
            return result;
        }
    }
}
