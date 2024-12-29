using EntanglementOfGraphs;
using Microsoft.Msagl.GraphmapsWithMesh;
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
        
        /// <summary>
        /// erstellt Stategien für beide Spieler
        /// </summary>
        public void CreateStrategies ()
        {
            var allVertices = Vertices.ToList();
            foreach (var vertex in allVertices)
            {
                if (vertex.detectivesTurn)
                {
                    nextDetectiveMove(vertex);
                }
                else
                {
                    nextThiefMove(vertex);
                }
            }
        }

        /// <summary>
        /// erstellt eine Liste an besten Zügen zu den jeweiligem Status des Spiels
        /// </summary>
        /// <param name="currentPos"></param>
        private void nextDetectiveMove(Positions<V> currentPos)
        {
            List<Positions<V>> result = [];
            var outgoingVertex = GetOutgoingVertex(currentPos);
            double bestWinningChance = 0;
            double bestFlagOfTarget = 0;
            foreach (var vertex in outgoingVertex) // findet die beste Gewinnwahrscheinlichkeit
            {
                var vertexWinningChance = vertex.flag % 1;
                if (vertexWinningChance == 0) 
                {
                    break;
                }
                if (vertexWinningChance > bestWinningChance)
                {
                    bestWinningChance = vertexWinningChance;
                }
            }
            foreach (var vertex in outgoingVertex) // findet zur besten Gewinnwahrscheinlichkeit den kürzesten Weg
            {
                var vertexFlag = vertex.flag;
                if ((vertexFlag % 1 == bestFlagOfTarget) && (vertexFlag < bestFlagOfTarget))
                {
                    bestFlagOfTarget = vertexFlag;
                }
            }
            foreach (var vertex in outgoingVertex) // fügt beste und kürzeste Züge hinzu
            {
                if (vertex.flag == bestFlagOfTarget)
                {
                    result.Add(vertex);
                }
            }
            detectiveStrategy.Add(new Move<V>(currentPos,result));
        }


        /// <summary>
        /// erstellt eine Liste an besten Zügen zu den jeweiligem Status des Spiels
        /// </summary>
        /// <param name="currentPos"></param>
        private void nextThiefMove(Positions<V> currentPos)
        {
            List<Positions<V>> result = [];
            var outgoingVertex = GetOutgoingVertex(currentPos);
            var nextPossibleSteps = graph.GetNextPossibleSteps(currentPos);
            double worstWinningChance = 1;
            double worstFlagOfTarget = 0;
            bool saveWinFound = false;
            foreach (var possibleVertex in nextPossibleSteps) //prüft ob Dieb sicher gewinnen kann
            {
                bool saveVertex = true;
                foreach (var vertex in outgoingVertex)
                {
                    if (possibleVertex.Equals(vertex)) // ist im GameTree
                    {
                        saveVertex = false; // soll nicht jinzugefügt werden, da kein sicherer Gewinn
                        break;
                    }                    
                    

                }
                if (!saveVertex) // sicherer Spielzug gefunden und hinzufügen
                {
                    result.Add(possibleVertex);
                    saveWinFound = true;
                }
            }
            if (!saveWinFound) //sichere Spielzug nicht gefunden, finde den Sichersten
            {
                foreach (var vertex in outgoingVertex) // finde die beste Gewinnwahrscheinlichkeit
                {
                    var vertexWinningChance = vertex.flag % 1;
                    if ((vertexWinningChance < worstWinningChance))
                    {
                        worstWinningChance = vertexWinningChance;
                    }
                }
                foreach (var vertex in outgoingVertex) // finde zur besten Gewinnwahrscheinlichkeit den kürzesten Weg
                {
                    var vertexFlag = vertex.flag;
                    if ((vertexFlag % 1 == worstWinningChance) && (vertexFlag > worstFlagOfTarget))
                    {
                        worstFlagOfTarget = vertexFlag;
                    }
                }

                foreach (var vertex in outgoingVertex) // füge alle kürzesten und besten Möglichkeiten hinzu
                {
                    if (vertex.flag == worstFlagOfTarget)
                    {
                        result.Add(vertex);
                    }
                }
            }
            detectiveStrategy.Add(new Move<V>(currentPos, result));
        }


        /// <summary>
        /// sucht einen der besten Spielzüge zum aktuellen Spielstatus aus
        /// </summary>
        /// <param name="currentPos"></param>
        /// <returns></returns>
        public V BestDetectiveMove(Positions<V> currentPos)
        {
            foreach (var move in detectiveStrategy) // geht jeden Zug der Strategie durch
            {
                if(move.source.Equals(currentPos)) // findet den passenden zur currentPos
                {
                    return currentPos.getMovedDetective(move.target.First());
                }
            }
            return currentPos.detectives.First(); // wenn kein passender Gefunden, Gewinn nicht mehr möglch
        }

        /// <summary>
        /// sucht einen der besten Spielzüge zum aktuellen Spielstatus aus
        /// </summary>
        /// <param name="currentPos"></param>
        /// <returns></returns>
        public V BestThiefMove(Positions<V> currentPos)
        {
            foreach (var move in thiefStrategy) // geht jeden Zug der Strategie durch
            {
                if (move.source.Equals(currentPos)) // findet den passenden zur currentPos
                {
                    return move.target.First().thief;
                }
            }
            return graph.GetNextPossibleSteps(currentPos).First().thief; // wenn kein passender Gefunden, Gewinn sicher
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
                    var temp = AddDetectiveVertex(previousPos, currentPos, false); // fügt Detektivknoten hinzu
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
        public void BuildFixpointGameTree(bool flagged)
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
                            var temp = AddDetectiveVertex(previousPos, currentPos, flagged); // fügt Detektivknoten hinzu
                            continueFixpoint = continueFixpoint || temp.Item1;
                            startPosReached = startPosReached || temp.Item2;
                        }
                    }
                    else // Dieb war davor dran
                    {
                        foreach (var previousPos in graph.GetPreviousPossibleSteps(currentPos)) // alle Knoten mit den man den jetzigen Knoten ereichen kann
                        {
                            if (flagged)
                            {
                                continueFixpoint = continueFixpoint || AddFlaggedThiefVertex(previousPos);
                            }
                            else
                            {
                                continueFixpoint = continueFixpoint || AddThiefVertex(previousPos); // fügt Diebknoten hinzu
                            }
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
                        break;
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
        /// fügt Knoten zum GameTree mit Gewinnwahrschleinlichkeit für Detektiv bei zufälliger Zugwahl des Diebes
        /// </summary>
        /// <param name="previousPos"></param>
        /// <returns></returns>
        private bool AddFlaggedThiefVertex(Positions<V> previousPos)
        {
            bool goOn = false;
            var isNewVertex = GetExistingPosition(previousPos); //checken ob schon vorhandener Knoten
            if (isNewVertex == null)
            {
                bool detWin = true;
                var nextPossibleSteps = graph.GetNextPossibleSteps(previousPos);
                List<Positions<V>> exsistingNextPossibleSteps = [];
                double minFlagOfTarget = VertexCount;
                double sureDetWins = nextPossibleSteps.Count;
                foreach (var targetVertex in nextPossibleSteps) // checkt, ob alle ausgehenden Kanten wieder in den sicheren GameTree führen
                {
                    var isNewPos = GetExistingPosition(targetVertex);
                    if (isNewPos != null)
                    {
                        double isNewPosWinningChance = isNewPos.flag % 1;
                        if (isNewPosWinningChance != 0) //wenn nicht alle Kanten in den Baum führen gewinnt der Detektiv nicht sicher
                        {
                            detWin = false;
                            sureDetWins = sureDetWins - isNewPosWinningChance; // summiert die Gewinnchancen des Detektivs
                        }
                        else
                        {
                            if (targetVertex.flag < minFlagOfTarget) // sucht kürzesten Weg zum Gewinn
                            {
                                minFlagOfTarget = targetVertex.flag;
                            }
                        }
                        exsistingNextPossibleSteps.Add(isNewPos); // Knoten zu dennen Kanten gehen bei nicht sicheren Zuständen
                    }
                    else
                    {
                        detWin = false;
                        sureDetWins--; // neu gefundene Position bedeutet sicher Verloren für detektiv, wenn richtigen Weg gefählt (aber kann sein das Dieb flaschen Weg wählt)
                    }
                }
                if (detWin) // Detektiv gewinnt sicher bei Knoten
                {
                    previousPos.flag = minFlagOfTarget + 1.00;
                    exsistingNextPossibleSteps = nextPossibleSteps; // wenn sicherer Gewinn dann waren alle nextSteps im GameTree
                }
                else // Detektiv gewinnt mit einer Chance von sureDetWins / nextPossibleSteps.Count
                {
                    previousPos.flag = Math.Floor(minFlagOfTarget) + 1.00 + sureDetWins / nextPossibleSteps.Count; // Wahrscheinlichkeit des Gewinns bei Zufälliger Zugwahl
                }

                AddVertex(previousPos); //fügt alle Knoten hinzu
                goOn = true;
                if (debug)
                {
                    Console.WriteLine($"Knoten hinzugefügt: {previousPos}");
                }
                foreach (var targetVertex in exsistingNextPossibleSteps) // alle Kanten vom Knoten werden hinzugefügt
                {
                    if (debug)
                    {
                        Console.WriteLine($"Kante von {previousPos} zu {targetVertex} hinzugefügt.");
                    }
                    AddEdge(new Edge<Positions<V>>(previousPos, GetExistingPosition(targetVertex)));
                }                
            }
            return goOn;
        }

        /// <summary>
        /// fügt Knoten hinzu, wo der Detektiv dran ist (mit möglicher Gewinnwahrscheinlichkeit für Detektiv)
        /// </summary>
        /// <param name="previousPos"></param>
        /// <param name="currentPos"></param>
        /// <param name="flagged"></param>
        /// <returns></returns>
        private (bool,bool) AddDetectiveVertex (Positions<V> previousPos, Positions<V> currentPos, bool flagged)
        {
            bool goOn = false;
            bool startPosReached = false;
            var isNewVertex = GetExistingPosition(previousPos); //checken ob schon vorhandener Knoten
            if (isNewVertex == null)
            {
                if (flagged) // fügt sichere Wahrscheinlichkeit hinzu
                {
                    previousPos.flag = currentPos.flag + 1.00;
                }
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
                        tempState.flag = 0.00;
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

        /// <summary>
        /// gibt alle möglichen nächsten Spielzüge zurück
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
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
