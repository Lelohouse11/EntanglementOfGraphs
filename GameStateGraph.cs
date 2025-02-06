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
using static System.Windows.Forms.AxHost;

namespace EntaglementOfGraphs
{
    internal class GameStateGraph<V> : AdjacencyGraph<GameState<V>, Edge<GameState<V>>> where V : IComparable<V>, IEquatable<V>
    {
        public readonly int detectiveAmount;
        public readonly FiniteDirectedGraph<V> graph;
        public readonly List<GameState<V>> possibleFinalStates;
        public readonly List<Move<V>> detectiveStrategy = [];
        public readonly List<Move<V>> thiefStrategy = [];
        private readonly GameState<V> startState;
        public readonly List<GameState<V>> finalStates = [];

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
                state.winningChance = 1;
                state.distanceToWin = 0;
                AddVertex(state);
                finalStates.Add(state);
            }
            
        }            

        /// <summary>
        /// verbindet alle gefundenen Iterationen des Gametrees zu GameStateGraph
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public bool BuildGameStateGraphForwards(GameState<V> currentState)
        {
            var nextPossibleStates = graph.GetNextPossibleStates(currentState);
            bool thiefStateSave = true;
            bool entered = false;
            List<GameState<V>> exsistingnextStates = [];
            foreach (var nextState in nextPossibleStates) // fügt gefunden Knoten hinzu und verbindet sie
            {
                var isNewState = GetExistingState(nextState);
                if (isNewState == null) // prüft ob nextState neu ist
                {
                    AddVertex(nextState);
                }
                else
                {
                    exsistingnextStates.Add(isNewState);
                }
                var targetState = isNewState ?? nextState; // wenn nextState nicht neu, alte vorhandene Pos benutzen
                AddEdge(new Edge<GameState<V>>(currentState, targetState));

                if (!currentState.detectivesTurn && finalStates.Contains(targetState))
                {
                    currentState.savePathFound = true;
                    return true;
                }                        
                if (isNewState == null) // Wenn NextPos neu oder Detektive sich nicht bewegen || currentState.detectivesTurn
                {                    
                    if (currentState.detectivesTurn)
                    {
                        if (BuildGameStateGraphForwards(targetState))
                        {
                            currentState.savePathFound = true;
                            return true;
                        }
                    }
                    else
                    {
                        entered = true;
                        thiefStateSave = thiefStateSave && BuildGameStateGraphForwards(targetState);
                    }
                }
            }
            foreach (var exsistingNextState in exsistingnextStates)
            {
                thiefStateSave = thiefStateSave && exsistingNextState.savePathFound;
            }
            if (entered)
            {
                currentState.savePathFound = thiefStateSave;
                return thiefStateSave;
            }
            currentState.savePathFound = false;
            return false;
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
                    if (flagged)
                    {
                        continueRecursion = AddFlaggedDetectiveGameState(previousState, currentState); // fügt Detektivknoten hinzu
                    }
                    else
                    {
                        bool? temp = AddDetectiveGameState(previousState, currentState); // fügt Detektivknoten hinzu
                        if (temp == null)
                        {
                            continueRecursion = false;
                        }
                        else
                        {
                            continueRecursion = (bool)temp;
                        }
                    }
                }                
                if (continueRecursion) // Wenn neue Knoten hinzugekommen sind oder Startknoten gefunden wurde
                {
                    BuildGameStateGraphBackwards(previousState, flagged); //rekursiver Aufruf
                }                
            }
        }

        /// <summary>
        /// baut den GameStateGraph durch eine Fixpointiteration auf
        /// </summary>
        public void BuildGameStateGraphFixpoint(bool flagged)
        {
            bool continueFixpoint;
            List<GameState<V>> excludingStates = [startState];
            do
            {
                continueFixpoint = false;
                var allStates = Vertices.ToList().Except(excludingStates);
                foreach (var currentState in allStates) // geht alle Knoten in bisheriger Menge durch
                {
                    if (!currentState.detectivesTurn) //wenn detektiv vorher dran war wird alles hinzugefügt
                    {
                        if (flagged)
                        {
                            //var temp = graph.GetPreviousPossibleStates(currentState);
                            foreach (var previousState in graph.GetPreviousPossibleStates(currentState)) // alle Knoten mit den man jetzigen Knoten ereichen kann
                            {
                                //AddFlaggedDetectiveGameState(previousState, currentState);
                                //continueFixpoint = true;                                          // stimmt noch was nicht
                                continueFixpoint = continueFixpoint || AddFlaggedDetectiveGameState(previousState, currentState); // fügt Detektivknoten hinzu
                            }
                        }
                        else
                        {
                            var previousPossibleStates = graph.GetPreviousPossibleStates(currentState);
                            currentState.possiblePreviousStepsCount = previousPossibleStates.Count;
                            foreach (var previousState in previousPossibleStates) // alle Knoten mit den man jetzigen Knoten ereichen kann
                            {
                                bool? temp = AddDetectiveGameState(previousState, currentState); // fügt Detektivknoten hinzu
                                if (temp == null)
                                {
                                    continueFixpoint = false; 
                                    break;
                                }
                                continueFixpoint = continueFixpoint || (bool)temp;                                
                            }
                            if (currentState.possiblePreviousStepsCount <= 0)
                            {
                                excludingStates.Add(currentState);
                            }
                        }
                    }
                    else // Dieb war davor dran
                    {
                        if (flagged)
                        {
                            foreach (var previousState in graph.GetPreviousPossibleStates(currentState)) // alle Knoten mit den man den jetzigen Knoten ereichen kann
                            {
                                continueFixpoint = continueFixpoint || AddFlaggedThiefGameState(previousState);
                            }
                        }
                        else
                        {
                            var previousPossibleStates = graph.GetPreviousPossibleStates(currentState);
                            currentState.possiblePreviousStepsCount = previousPossibleStates.Count;
                            foreach (var previousState in previousPossibleStates) // alle Knoten mit den man den jetzigen Knoten ereichen kann
                            {                                
                                continueFixpoint = continueFixpoint || AddThiefGameState(previousState); // fügt Diebknoten hinzu                                
                            }
                            if (currentState.possiblePreviousStepsCount <= 0)
                            {
                                excludingStates.Add(currentState);
                            }
                        }
                    }
                }

            } while (continueFixpoint); // macht ewweiter , wenn neuen Knoten oder och nicht Startknoten gefunden wurde
        }

        /// <summary>
        /// fügt Knoten wo Dieb dran ist zu GameStateGraph hinzu
        /// </summary>
        /// <param name="previousState"></param>
        private bool AddThiefGameState(GameState<V> previousState)
        {
            if (GetExistingState(previousState) == null) //checken ob schon vorhandener Knoten
            {
                var nextPossibleStates = graph.GetNextPossibleStates(previousState);
                foreach (var targetState in nextPossibleStates) // checkt, ob alle ausgehenden Kanten wieder in den GameStateGraph führen
                {
                    if (!ContainsState(targetState)) return false; //wenn nicht alle Kanten in den Baum führen gewinnt der Detektiv nicht sicher
                }
                foreach (var targetState in nextPossibleStates)
                {
                    GetExistingState(targetState).possiblePreviousStepsCount--;
                }
                AddVertex(previousState);
                foreach (var targetState in nextPossibleStates) // alle Kanten vom Knoten werden hinzugefügt
                {       
                    AddEdge(new Edge<GameState<V>>(previousState, GetExistingState(targetState)));                        
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// fügt Knoten zum GameStateGraph mit Gewinnwahrschleinlichkeit für Detektiv bei zufälliger Zugwahl des Diebes
        /// </summary>
        /// <param name="previousState"></param>
        /// <returns></returns>
        private bool AddFlaggedThiefGameState(GameState<V> previousState)
        {
            var nextPossibleStates = graph.GetNextPossibleStates(previousState);
            List<GameState<V>> exsistingNextPossibleStates = [];
            int minDistanceToWin = VertexCount;
            double winningChanceSum = 0;
            var isNewPState = GetExistingState(previousState);
            foreach (var nextState in nextPossibleStates) // checkt, ob alle ausgehenden Kanten wieder in den sicheren GameStateGraph führen
            {
                var isNewState = GetExistingState(nextState);
                if (isNewState != null)
                {
                    winningChanceSum += isNewState.winningChance; // summiert die Gewinnchancen des Detektivs
                    if (isNewState.distanceToWin < minDistanceToWin) // sucht kürzesten Weg zum Gewinn
                    {
                        minDistanceToWin = isNewState.distanceToWin;
                    }
                    exsistingNextPossibleStates.Add(isNewState); // Knoten zu dennen Kanten gehen bei nicht sicheren Zuständen
                }
            }            

            previousState.winningChance = winningChanceSum / nextPossibleStates.Count; // Wahrscheinlichkeit des Gewinns bei Zufälliger Zugwahl
            previousState.distanceToWin += minDistanceToWin + 1;

            var sourceState = isNewPState;
            if (isNewPState == null)
            {
                AddVertex(previousState); //fügt alle Knoten hinzu
                sourceState = previousState;
            }         

            foreach (var targetState in exsistingNextPossibleStates) // alle Kanten vom Knoten werden hinzugefügt
            {
                if (!isExistingEdge(sourceState, targetState))
                {
                    AddEdge(new Edge<GameState<V>>(sourceState, targetState));
                }
            }
                
            if(isNewPState != null)
            {
                if ((previousState.winningChance) < (isNewPState.winningChance))
                {
                    isNewPState.distanceToWin = previousState.distanceToWin + 1;
                    isNewPState.winningChance = previousState.winningChance;
                    changeAllPreviousWC(isNewPState);
                }
                return false;
            }
           return true;
        }

            /// <summary>
            /// ändert alle abhängigen Gewinnwahrscheinlichkeiten
            /// </summary>
            /// <param name="state"></param>
            /// <param name="newWC"></param>
        private void changeAllPreviousWC(GameState<V> state)
        {            
            foreach (var previousState in GetIncomingStates(state))
            {
                if (previousState.winningChance < state.winningChance)
                {
                    previousState.distanceToWin = state.distanceToWin + 1;
                    if (state.detectivesTurn)
                    {
                        previousState.winningChance += (state.winningChance - previousState.winningChance) / OutDegree(previousState);
                    }
                    else
                    {
                        previousState.winningChance = state.winningChance;
                    }
                    changeAllPreviousWC(previousState);
                }                
            }
            
        }

        /// <summary>
        /// fügt Knoten hinzu, wo der Detektiv dran ist (mit möglicher Gewinnwahrscheinlichkeit für Detektiv)
        /// </summary>
        /// <param name="previousState"></param>
        /// <param name="currentState"></param>
        /// <param name="flagged"></param>
        /// <returns></returns>
        private bool? AddDetectiveGameState (GameState<V> previousState, GameState<V> currentState)
        {
            bool? goOn = false;
            var isNewState = GetExistingState(previousState); //checken ob schon vorhandener Knoten
            if (isNewState == null)
            {
                AddVertex(previousState); // Hinzufügen, wenn neu
                currentState.possiblePreviousStepsCount--;
                goOn = true;
            }
            
            else if (isNewState.Equals(startState)) // prüft ob Startknoten gefunden
            {
                goOn = null;
            }
            var sourceState = isNewState ?? previousState;
            AddEdge(new Edge<GameState<V>>(sourceState, currentState)); // Kante hinzufügen            
            return goOn;
        }

        private bool AddFlaggedDetectiveGameState(GameState<V> previousState, GameState<V> currentState)
        {
            var isNewState = GetExistingState(previousState); //checken ob schon vorhandener Knoten
            GameState<V> cloned = currentState.Clone().ChangeTurn();
            if (isNewState == null) // hier auch falsch
            {        
                previousState.distanceToWin = currentState.distanceToWin + 1;
                previousState.winningChance = currentState.winningChance;
                AddVertex(previousState); // Hinzufügen, wenn neu und auch möglich
                AddEdge(new Edge<GameState<V>>(previousState, currentState)); // Kante hinzufügen
                return true;
            }
            else if (isNewState.winningChance < currentState.winningChance)
            {
                isNewState.distanceToWin = currentState.distanceToWin + 1;
                isNewState.winningChance = currentState.winningChance;
                changeAllPreviousWC(isNewState);
                if (!isExistingEdge(isNewState, currentState))
                {
                    AddEdge(new Edge<GameState<V>>(isNewState, currentState)); // Kante hinzufügen
                }
                changeAllPreviousWC(isNewState);
            }
            return false;
        }

        /// <summary>
        /// erstellt Stategien für beide Spieler
        /// </summary>
        public void CreateStrategies()
        {
            var vertexCount = VertexCount;
            var edgeCount = Edges;
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
            int bestDistanceToWin = VertexCount;
            foreach (var state in outgoingStates) // findet die beste Gewinnwahrscheinlichkeit
            {
                if (state.winningChance == 1)
                {
                    bestWinningChance = 1;
                    break;
                }
                if (state.winningChance > bestWinningChance)
                {
                    bestWinningChance = state.winningChance;
                }
            }
            foreach (var state in outgoingStates) // findet zur besten Gewinnwahrscheinlichkeit den kürzesten Weg
            {  
                if ((state.winningChance == bestWinningChance) && (state.distanceToWin < bestDistanceToWin))
                {
                    bestDistanceToWin = state.distanceToWin;
                }
            }
            foreach (var state in outgoingStates) // fügt beste und kürzeste Züge hinzu
            {
                if ((state.winningChance == bestWinningChance) && (state.distanceToWin == bestDistanceToWin))
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
            int worstDistanceToWin = 0;
            bool saveWinFound = true;
            foreach (var possibleState in nextPossibleStates) //prüft ob Dieb sicher gewinnen kann
            {
                saveWinFound = true;
                foreach (var state in outgoingVertex)
                {
                    if (possibleState.Equals(state)) // ist im GameStateGraph
                    {
                        saveWinFound = false; // soll nicht jinzugefügt werden, da kein sicherer Gewinn
                        break;
                    }
                }
                if (saveWinFound) // sicherer Spielzug gefunden und hinzufügen
                {
                    result.Add(possibleState);
                    break;
                }
            }
            if (!saveWinFound) //sichere Spielzug nicht gefunden, finde den Sichersten
            {
                foreach (var state in outgoingVertex) // finde die schlechteste Gewinnwahrscheinlichkeit
                {
                    if ((state.winningChance < worstWinningChance))
                    {
                        worstWinningChance = state.winningChance;
                    }
                }
                foreach (var state in outgoingVertex) // finde zur schlechtesten Gewinnwahrscheinlichkeit den längsten Weg
                {
                    if ((state.winningChance == worstWinningChance) && (state.distanceToWin > worstDistanceToWin))
                    {
                        worstDistanceToWin = state.distanceToWin;
                    }
                }

                foreach (var state in outgoingVertex) // füge alle längsten und schlechtesten Möglichkeiten hinzu
                {
                    if ((state.winningChance == worstWinningChance) && (state.distanceToWin == worstDistanceToWin))
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

        public bool isExistingEdge(GameState<V> sourceState, GameState<V> targetState)
        {
            foreach (var e in Edges)
            {
                if (e.Source.Equals(sourceState) && e.Target.Equals(targetState)) return true;
            }
            return false;
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
                    var tempState = new GameState<V>(detectiveAmount, state, true);
                    for (var i = 0; i < outgoingStateCount; i++)
                    {
                        tempState.detectives.Add(outgoingStates[i]);// setzt Detektive auf die Fluchtmöglichkeit
                        tempState.winningChance = 1;
                        tempState.distanceToWin = 0;
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
            foreach (var edge in Edges)
            {
                if (edge.Source.Equals(state))
                {
                    result.Add(edge.Target);
                }
            }
            return result;
        }

        /// <summary>
        /// gitb die States zurück, die zum gegeben State führen
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<GameState<V>> GetIncomingStates(GameState<V> state)
        {
            List<GameState<V>> result = [];
            foreach (var edge in Edges)
            {
                if (edge.Target == state)
                {
                    result.Add(edge.Source);
                }
            }
            return result;
        }
    }
}
