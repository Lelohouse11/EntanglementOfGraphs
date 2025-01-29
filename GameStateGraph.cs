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
    internal class GameStateGraph<V> : AdjacencyGraph<GameState<V>, Edge<GameState<V>>> where V : IComparable<V>, IEquatable<V>
    {
        private readonly bool debug = false;

        public readonly int detectiveAmount;
        public readonly FiniteDirectedGraph<V> graph;
        public readonly List<GameState<V>> possibleFinalStates;
        public readonly List<Move<V>> detectiveStrategy = [];
        public readonly List<Move<V>> thiefStrategy = [];
        private readonly GameState<V> startState;

        /// <summary>
        /// erstellt GameStateGraph mit Startwerten
        /// </summary>
        /// <param name="_graph"></param>
        /// <param name="start"></param>
        public GameStateGraph(FiniteDirectedGraph<V> _graph, GameState<V> start)
        {
            AddVertex(start);
            startState = start;
            detectiveAmount = start.detectiveAmount;
            graph = _graph;
            possibleFinalStates = GetPossibleFinalStates();
            foreach (var state in possibleFinalStates)
            {
                state.flag = 0;
                AddVertex(state);
                if (debug)
                {
                    Console.WriteLine($"Endknoten hinzugefügt: {state}");
                }
            }
            if (debug)
            {
                Console.WriteLine($"Startknoten hinzugefügt: {start}");
            }
        }      

        /// <summary>
        /// verbindet alle gefundenen Iterationen des Gametrees zu GameStateGraph
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public GameStateGraph<V> BuildGameStateGraphForwards(GameState<V> currentState)
        {
            var nextPossibleStates = graph.GetNextPossibleStates(currentState);
            if (nextPossibleStates.Count != 0)
            { //prüft ob keine weiteren Knoten gefunden wurden
                foreach (var nextState in nextPossibleStates) // fügt gefunden Knoten hinzu und verbindet sie
                {
                    var isNewState = GetExistingState(nextState);
                    if (isNewState == null) // prüft ob nextState neu ist
                    {
                        AddVertex(nextState);
                        if (debug)
                        {
                            Console.WriteLine($"Knoten hinzugefügt: {nextState}");
                        }
                    }

                    var targetState = isNewState ?? nextState; // wenn nextState nicht neu, alte vorhandene Pos benutzen
                    AddEdge(new Edge<GameState<V>>(currentState, targetState));
                    if (debug)
                    {
                        Console.WriteLine($"Kante von {currentState} zu {targetState} hinzugefügt.");
                    }
                    if (isNewState == null || currentState.detectivesTurn) // Wenn NextPos neu oder Detektive sich nicht bewegen
                    {
                        BuildGameStateGraphForwards(targetState); //rekursiver Aufruf
                    }
                }
            }
            return this;
        }       

        /// <summary>
        /// baut den GameStateGraph rekursiv auf, allerdings nur bis zum Punkt das Detektiv einen Weg hat, 
        /// dass er sicher gewinnt
        /// </summary>
        /// <param name="currentState"></param>
        public void BuildGameStateGraphBackwards(GameState<V> currentState, bool flagged)
        {
            //Console.WriteLine($"{recLevel}, {currentState}, {Vertices.Count()}");
            var previousPossibleStates = graph.GetPreviousPossibleStates(currentState);
            bool startStateReached = false;
            foreach (var previousState in previousPossibleStates) // gehe die vorig möglichen Spielzustände durch
            {
                bool continueRecursion = false;
                if (!previousState.detectivesTurn) //wenn der Deib dran ist muss jeder möglicher Zug schon im GameStateGraph gespeichert sein
                {
                    if (flagged)
                    {
                        continueRecursion = continueRecursion || AddFlaggedThiefGameState(previousState);
                    }
                    else
                    {
                        continueRecursion = AddThiefGameState(previousState);  // fügt Diebknoten hinzu
                    }           
                }
                else
                {
                    var temp = AddDetectiveGameState(previousState, currentState, flagged); // fügt Detektivknoten hinzu
                    continueRecursion = temp.Item1;
                    startStateReached = startStateReached || temp.Item2;
                }                
                if (continueRecursion && !startStateReached) // Wenn neue Knoten hinzugekommen sind oder Startknoten gefunden wurde
                {
                    BuildGameStateGraphBackwards(previousState, continueRecursion && !startStateReached); //rekursiver Aufruf
                }                
            }
        }

        /// <summary>
        /// baut den GameStateGraph durch eine Fixpointiteration auf
        /// </summary>
        public void BuildGameStateGraphFixpoint(bool flagged)
        {
            bool startStateReached = false;
            bool continueFixpoint;
            do
            {
                continueFixpoint = false;
                var allStates = Vertices.ToList().Except([startState]);
                foreach (var currentState in allStates) // geht alle Knoten in bisheriger Menge durch
                {
                    if (!currentState.detectivesTurn) //wenn detektiv vorher dran war wird alles hinzugefügt
                    {
                        foreach (var previousState in graph.GetPreviousPossibleStates(currentState)) // alle Knoten mit den man jetzigen Knoten ereichen kann
                        {
                            var temp = AddDetectiveGameState(previousState, currentState, flagged); // fügt Detektivknoten hinzu
                            continueFixpoint = continueFixpoint || temp.Item1;
                            startStateReached = startStateReached || temp.Item2;
                        }
                    }
                    else // Dieb war davor dran
                    {
                        foreach (var previousState in graph.GetPreviousPossibleStates(currentState)) // alle Knoten mit den man den jetzigen Knoten ereichen kann
                        {
                            if (flagged)
                            {
                                continueFixpoint = continueFixpoint || AddFlaggedThiefGameState(previousState);
                            }
                            else
                            {
                                continueFixpoint = continueFixpoint || AddThiefGameState(previousState); // fügt Diebknoten hinzu
                            }
                        }
                    }
                }

            } while (continueFixpoint && !startStateReached); // macht ewweiter , wenn neuen Knoten oder och nicht Startknoten gefunden wurde
            if (debug)
            {
                Console.WriteLine("----------Keine neuen Knoten mehr gefunden oder Verbindung zu Startpos gefunden!----------");
            }
        }

        /// <summary>
        /// fügt Knoten wo Dieb dran ist zu GameStateGraph hinzu
        /// </summary>
        /// <param name="previousState"></param>
        private bool AddThiefGameState(GameState<V> previousState)
        {
            bool goOn = false;
            var isNewState = GetExistingState(previousState); //checken ob schon vorhandener Knoten
            if (isNewState == null)
            {
                bool detWin = true;
                var nextPossibleStates = graph.GetNextPossibleStates(previousState);
                foreach (var targetState in nextPossibleStates) // checkt, ob alle ausgehenden Kanten wieder in den GameStateGraph führen
                {                    
                    if (!ContainsState(targetState)) //wenn nicht alle Kanten in den Baum führen gewinnt der Detektiv nicht sicher
                    {
                        detWin = false;                        
                        break;
                    }
                }
                if (detWin) // Detektiv gewinnt sicher bei Knoten
                {
                    AddVertex(previousState);
                    goOn = true;
                    if (debug)
                    {
                        Console.WriteLine($"Knoten hinzugefügt: {previousState}");
                    }
                    foreach (var targetState in nextPossibleStates) // alle Kanten vom Knoten werden hinzugefügt
                    {                        
                        if (debug)
                        {
                            Console.WriteLine($"Kante von {previousState} zu {targetState} hinzugefügt.");
                        }
                        AddEdge(new Edge<GameState<V>>(previousState, GetExistingState(targetState)));                        
                    }
                }
            }
            return goOn;
        }

        /// <summary>
        /// fügt Knoten zum GameStateGraph mit Gewinnwahrschleinlichkeit für Detektiv bei zufälliger Zugwahl des Diebes
        /// </summary>
        /// <param name="previousState"></param>
        /// <returns></returns>
        private bool AddFlaggedThiefGameState(GameState<V> previousState)
        {
            bool goOn = false;
            var isNewPState = GetExistingState(previousState); //checken ob schon vorhandener Knoten, was wenn aber neuer State eine bessere Gewinnwahrscheinlichkeit hätte?
            if (isNewPState == null)
            {
                bool detWin = true;
                var nextPossibleStates = graph.GetNextPossibleStates(previousState);
                List<GameState<V>> exsistingNextPossibleStates = [];
                double minDistanceToWin = VertexCount;
                double winningChanceSum = nextPossibleStates.Count;
                foreach (var targetState in nextPossibleStates) // checkt, ob alle ausgehenden Kanten wieder in den sicheren GameStateGraph führen
                {
                    var isNewState = GetExistingState(targetState);
                    if (isNewState != null)
                    {
                        double isNewPosWinningChance = isNewState.flag % 1;
                        if (isNewPosWinningChance != 0) //wenn nicht alle Kanten in den Baum führen gewinnt der Detektiv nicht sicher
                        {
                            detWin = false;
                            winningChanceSum = winningChanceSum - isNewPosWinningChance; // summiert die Gewinnchancen des Detektivs
                        }
                        else
                        {
                            if (targetState.flag < minDistanceToWin) // sucht kürzesten Weg zum Gewinn
                            {
                                minDistanceToWin = targetState.flag;
                            }
                        }
                        exsistingNextPossibleStates.Add(isNewState); // Knoten zu dennen Kanten gehen bei nicht sicheren Zuständen
                    }
                    else
                    {
                        detWin = false;
                        winningChanceSum--; // neu gefundene Position bedeutet sicher Verloren für detektiv, wenn richtigen Weg gefählt (aber kann sein das Dieb flaschen Weg wählt)
                    }
                }
                if (detWin) // Detektiv gewinnt sicher bei Knoten
                {
                    previousState.flag = minDistanceToWin + 1.00;
                    exsistingNextPossibleStates = nextPossibleStates; // wenn sicherer Gewinn dann waren alle nextSteps im GameStateGraph
                }
                else // Detektiv gewinnt mit einer Chance von winningChanceSum / nextPossibleStates.Count
                {
                    previousState.flag = Math.Floor(minDistanceToWin) + 1.00 + winningChanceSum / nextPossibleStates.Count; // Wahrscheinlichkeit des Gewinns bei Zufälliger Zugwahl
                }

                AddVertex(previousState); //fügt alle Knoten hinzu
                goOn = true;
                if (debug)
                {
                    Console.WriteLine($"Knoten hinzugefügt: {previousState}");
                }
                foreach (var targetState in exsistingNextPossibleStates) // alle Kanten vom Knoten werden hinzugefügt
                {
                    if (debug)
                    {
                        Console.WriteLine($"Kante von {previousState} zu {targetState} hinzugefügt.");
                    }
                    AddEdge(new Edge<GameState<V>>(previousState, GetExistingState(targetState)));
                }                
            }
            return goOn;
        }

        /// <summary>
        /// fügt Knoten hinzu, wo der Detektiv dran ist (mit möglicher Gewinnwahrscheinlichkeit für Detektiv)
        /// </summary>
        /// <param name="previousState"></param>
        /// <param name="currentState"></param>
        /// <param name="flagged"></param>
        /// <returns></returns>
        private (bool,bool) AddDetectiveGameState (GameState<V> previousState, GameState<V> currentState, bool flagged)
        {
            bool goOn = false;
            bool startStateReached = false;
            var isNewState = GetExistingState(previousState); //checken ob schon vorhandener Knoten
            if (isNewState == null)
            {
                if (flagged) // fügt sichere Wahrscheinlichkeit hinzu
                {
                    previousState.flag = currentState.flag + 1.00;
                }
                AddVertex(previousState); // Hinzufügen, wenn neu
                goOn = true;                
                if (debug)
                {
                    Console.WriteLine($"Knoten hinzugefügt: {previousState}");
                }
            }
            else if (isNewState.Equals(startState)) // prüft ob Startknoten gefunden
            {
                startStateReached = true;
                if (debug)
                {
                    Console.WriteLine("-----Kante zum Startpunkt gefunden!-----");
                }
            }
            var sourceState = isNewState ?? previousState;
            AddEdge(new Edge<GameState<V>>(sourceState, currentState)); // Kante hinzufügen
            if (debug)
            {
                Console.WriteLine($"Kante von {sourceState} zu {currentState} hinzugefügt.");
            }
            return (goOn,startStateReached);
        }

        /// <summary>
        /// erstellt Stategien für beide Spieler
        /// </summary>
        public void CreateStrategies()
        {
            var allStates = Vertices.ToList();
            foreach (var state in allStates) // geht alle Knoten im Spielbaum durch
            {
                if (state.detectivesTurn)
                {
                    nextDetectiveMove(state); // berechnet besten Zug wenn Detektiv an der reihe ist
                }
                else
                {
                    nextThiefMove(state); // brechnet besten Zug wenn Dieb an der Reihe ist
                }
            }
        }

        /// <summary>
        /// erstellt eine Liste an besten Zügen zu den jeweiligem Status des Spiels
        /// </summary>
        /// <param name="currentState"></param>
        private void nextDetectiveMove(GameState<V> currentState)
        {
            List<GameState<V>> result = [];
            var outgoingStates = GetOutgoingStates(currentState);
            double bestWinningChance = 0;
            double bestDistanceToWin = VertexCount;
            foreach (var state in outgoingStates) // findet die beste Gewinnwahrscheinlichkeit
            {
                var vertexWinningChance = state.flag % 1;
                if (vertexWinningChance == 0)
                {
                    break;
                }
                if (vertexWinningChance > bestWinningChance)
                {
                    bestWinningChance = vertexWinningChance;
                }
            }
            foreach (var state in outgoingStates) // findet zur besten Gewinnwahrscheinlichkeit den kürzesten Weg
            {
                var vertexFlag = state.flag;
                if ((vertexFlag % 1 == bestWinningChance) && (vertexFlag < bestDistanceToWin))
                {
                    bestDistanceToWin = vertexFlag;
                }
            }
            foreach (var state in outgoingStates) // fügt beste und kürzeste Züge hinzu
            {
                if (state.flag == bestDistanceToWin)
                {
                    result.Add(state);
                }
            }
            if (!(result.Count == 0)) // es wurden keine guten Züge gefunden
            {
                detectiveStrategy.Add(new Move<V>(currentState, result));
            }
        }

        /// <summary>
        /// erstellt eine Liste an besten Zügen zu den jeweiligem Status des Spiels
        /// </summary>
        /// <param name="currentState"></param>
        private void nextThiefMove(GameState<V> currentState)
        {
            List<GameState<V>> result = [];
            var outgoingVertex = GetOutgoingStates(currentState);
            var nextPossibleStates = graph.GetNextPossibleStates(currentState);
            double worstWinningChance = 1;
            double worstDistanceToWin = 0;
            bool saveWinFound = false;
            foreach (var possibleState in nextPossibleStates) //prüft ob Dieb sicher gewinnen kann
            {
                bool saveVertex = true;
                foreach (var state in outgoingVertex)
                {
                    if (possibleState.Equals(state)) // ist im GameStateGraph
                    {
                        saveVertex = false; // soll nicht jinzugefügt werden, da kein sicherer Gewinn
                        break;
                    }


                }
                if (!saveVertex) // sicherer Spielzug gefunden und hinzufügen
                {
                    result.Add(possibleState);
                    saveWinFound = true;
                }
            }
            if (!saveWinFound) //sichere Spielzug nicht gefunden, finde den Sichersten
            {
                foreach (var state in outgoingVertex) // finde die beste Gewinnwahrscheinlichkeit
                {
                    var vertexWinningChance = state.flag % 1;
                    if ((vertexWinningChance < worstWinningChance))
                    {
                        worstWinningChance = vertexWinningChance;
                    }
                }
                foreach (var state in outgoingVertex) // finde zur besten Gewinnwahrscheinlichkeit den kürzesten Weg
                {
                    var vertexFlag = state.flag;
                    if ((vertexFlag % 1 == worstWinningChance) && (vertexFlag > worstDistanceToWin))
                    {
                        worstDistanceToWin = vertexFlag;
                    }
                }

                foreach (var state in outgoingVertex) // füge alle kürzesten und besten Möglichkeiten hinzu
                {
                    if (state.flag == worstDistanceToWin)
                    {
                        result.Add(state);
                    }
                }
            }
            thiefStrategy.Add(new Move<V>(currentState, result));
        }

        /// <summary>
        /// sucht einen der besten Spielzüge zum aktuellen Spielstatus aus und gibt in zurück mit der Angabe ob sich Detektiv bewegt hat 
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public (V, bool) BestDetectiveMove(GameState<V> currentState)
        {
            var cloned = currentState.Clone();
            cloned.ChangeTurn();
            foreach (var move in detectiveStrategy) // geht jeden Zug der Strategie durch
            {
                if (move.source.Equals(currentState)) // findet den passenden zur currentState
                {
                    if (cloned.Equals(move.target.First())) // der Detektiv bewegt sich nicht
                    {
                        return (default, false);
                    }
                    return (currentState.getMovedDetective(move.target.First()), true);
                }
            }
            return (default, false); // wenn kein passender Gefunden, Gewinn nicht mehr möglch
        }

        /// <summary>
        /// sucht einen der besten Spielzüge zum aktuellen Spielstatus aus
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public V BestThiefMove(GameState<V> currentState)
        {
            foreach (var move in thiefStrategy) // geht jeden Zug der Strategie durch
            {
                if (move.source.Equals(currentState)) // findet den passenden zur currentState
                {
                    return move.target.First().thiefPos;
                }
            }
            return graph.GetNextPossibleStates(currentState).First().thiefPos; // wenn kein passender Gefunden, Gewinn sicher
        }

        /// <summary>
        /// prüft ob gegebener Knoten schon im GameStateGraph vorhanden ist
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool ContainsState(GameState<V> state)
        {
            foreach (var s in Vertices)
            {
                if (s.Equals(state)) return true;
            }
            return false;            
        }

    /// <summary>
    /// gibt schon vorhandene Position zurück, wenn Position doppelt
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
        public GameState<V>? GetExistingState(GameState<V> state)
        {
            foreach (var s in Vertices)
            {
                if (s.Equals(state)) return s;
            }
            return null;            
        }
        
        /// <summary>
        /// Gibt die möglichen Endzustände des Gametrees zurück
        /// </summary>
        /// <returns></returns>
        public List<GameState<V>> GetPossibleFinalStates()
        {
            var result = new List<GameState<V>>();
            foreach (var state in graph.Vertices) //geht jeden Knoten des Graphen durch
            {
                var outgoingStates = graph.GetOutgoingVertex(state).Distinct().ToList();
                var outgoingStateCount = outgoingStates.Count;
                if (outgoingStateCount <= detectiveAmount) // prüft ob Fluchtmöglichkeiten von Detektiven blockiert werden können
                {
                    var tempState = new GameState<V>(detectiveAmount, state, false);
                    for (var i = 0; i < outgoingStateCount; i++)
                    {
                        tempState.detectives.Add(outgoingStates[i]);// setzt Detektive auf die Fluchtmöglichkeit
                        tempState.flag = 0.00;
                    }
                                       
                    if (detectiveAmount == outgoingStateCount)
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
        /// <param name="state"></param>
        /// <param name="blockedVertices"></param>
        /// <param name="result"></param>
        private void AddAllDetektivesToState(GameState<V> state, List<GameState<V>> result)
        { 
            foreach (var s in graph.Vertices.Except(state.detectives)) //Konten die noch frei sind
            {               
                var finalState = state.Clone();
                finalState.detectives.Add(s);
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
        /// <param name="state"></param>
        /// <returns></returns>
        public List<GameState<V>> GetOutgoingStates(GameState<V> state)
        {
            List<GameState<V>> result = [];
            foreach (var edge in this.OutEdges(state))
            {
                result.Add(edge.Target);
            }
            return result;
        }
    }
}
