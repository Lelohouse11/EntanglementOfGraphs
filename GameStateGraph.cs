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
    internal class GameStateGraph<V> : AdjacencyGraph<GameState<V>, Edge<GameState<V>>> where V : IEquatable<V>
    {
        public readonly int detectiveMaxAmount;
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
            //AddVertex(start);
            startState = start;
            detectiveMaxAmount = start.detectiveMaxAmount;
            graph = _graph;
            possibleFinalStates = GetPossibleFinalStates();
            foreach (var state in possibleFinalStates)
            {
                state.possiblePreviousStepsCount = graph.GetPreviousPossibleStates(state).Count;
                AddVertex(state);
                finalStates.Add(state);
            }
            
        }

        /// <summary>
        /// verbindet alle gefundenen Iterationen des Gametrees zu GameStateGraph
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public void BuildGameStateGraphForward(GameState<V> currentState)
        {
            var nextPossibleStates = graph.GetNextPossibleStates(currentState);
            foreach (var nextState in nextPossibleStates) // fügt gefunden Knoten hinzu und verbindet sie
            {
                var existingState = GetExistingState(nextState);
                if (existingState == null) AddVertex(nextState); // prüft ob nextState neu ist
                var targetState = existingState ?? nextState; // wenn nextState nicht neu, alte vorhandene Pos benutzen
                AddEdge(new Edge<GameState<V>>(currentState, targetState));
                if (existingState == null) BuildGameStateGraphForward(nextState);// Wenn NextPos neu oder Detektive sich nicht bewegen                
            }
        }

        /// <summary>
        /// verbindet alle gefundenen Iterationen des Gametrees zu GameStateGraph, sollte auch Entanglement berechen (funktioniert aktuell nicht)
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public bool BuildBetterGameStateGraphForward(GameState<V> currentState)
        {
            var nextPossibleStates = graph.GetNextPossibleStates(currentState);
            bool thiefStateSave = true;
            List<GameState<V>> existingNextStates = [];
            foreach (var nextState in nextPossibleStates) // fügt gefunden Knoten hinzu und verbindet sie
            {
                var existingState = GetExistingState(nextState);
                if (existingState == null) // prüft ob nextState neu ist
                {
                    AddVertex(nextState);
                }
                else
                {
                    existingNextStates.Add(existingState);
                }
                var targetState = existingState ?? nextState; // wenn nextState nicht neu, alte vorhandene Pos benutzen
                AddEdge(new Edge<GameState<V>>(currentState, targetState));

                //if (!currentState.detectivesTurn && finalStates.Contains(targetState))
                
                
                foreach ( var finalstate in finalStates)
                {
                    if (finalstate.Equals(targetState))
                    {
                        currentState.savePathFound = true;
                        targetState.savePathFound = true ;
                        return true;                        
                    }
                }
                
                

                if (finalStates.Contains(targetState))
                {
                    currentState.savePathFound = true;
                    return true;
                }
                
                if (existingState == null) // Wenn NextPos neu oder Detektive sich nicht bewegen || currentState.detectivesTurn
                {
                    var result = BuildBetterGameStateGraphForward(nextState);
                    if (currentState.detectivesTurn)
                    {
                        if (result)
                        {
                            currentState.savePathFound = true;
                            return true; // Was ist mit schon vorhanden Knoten?
                        }
                    }
                    else
                    {
                        thiefStateSave = thiefStateSave && result;
                    }
                }
            }
            if (currentState.detectivesTurn)
            {
                foreach (var existingNextState in existingNextStates)
                {
                    if(existingNextState.savePathFound)
                    {
                        currentState.savePathFound = true;
                        return true;
                    }
                }
            }
            else
            { // thiefTurn
                if (thiefStateSave) // D still win
                {                
                    foreach (var exsistingNextState in existingNextStates)
                    {
                        thiefStateSave = thiefStateSave && exsistingNextState.savePathFound;
                    }
                }
                currentState.savePathFound = thiefStateSave;
            }
            return currentState.savePathFound;
        }       

        public void BuildGameStateGraphBackwards()
        {
            AddVertex(startState);
            foreach (var finalState in finalStates) // ruft rekursiven Aufruf auf alle Endzustände auf
            {
                if (!OutEdges(startState).Any())
                {
                    GameStateGraphBackwardsRecursion(finalState);
                }
            }
        }

        /// <summary>
        /// baut den GameStateGraph rekursiv auf, allerdings nur bis zum Punkt das Detektiv einen Weg hat, 
        /// dass er sicher gewinnt
        /// </summary>
        /// <param name="currentState"></param>
        private void GameStateGraphBackwardsRecursion(GameState<V> currentState)
        {
            foreach (var previousState in graph.GetPreviousPossibleStates(currentState)) // gehe die vorig möglichen Spielzustände durch
            {
                if (previousState.detectivesTurn)
                {
                    if (AddDetectiveGameState(previousState, currentState) == true) // fügt Detektivknoten hinzu
                    {
                        GameStateGraphBackwardsRecursion(previousState);
                    }
                }
                else //wenn der Deib dran ist muss jeder möglicher Zug schon im GameStateGraph gespeichert sein
                {
                    if (AddThiefGameState(previousState)) // fügt Diebknoten hinzu
                    {
                        GameStateGraphBackwardsRecursion(previousState);
                    }
                }            
            }
        }

        public void BuildFlaggedGameStateGraphBackwards()
        {
            AddVertex(startState);
            foreach (var finalState in finalStates) // ruft rekursiven Aufruf auf alle Endzustände auf
            {
                FlaggedGameStateGraphBackwardsRecursion(finalState);
            }
        }

        /// <summary>
        /// baut den GameStateGraph rekursiv auf mit Gewinnwahrscheinlichkeit und Distanz
        /// </summary>
        /// <param name="currentState"></param>
        private void FlaggedGameStateGraphBackwardsRecursion(GameState<V> currentState)
        {
            foreach (var previousState in graph.GetPreviousPossibleStates(currentState)) // gehe die vorig möglichen Spielzustände durch
            {
                if (previousState.detectivesTurn)
                {
                    if (AddFlaggedDetectiveGameState(previousState, currentState)) // fügt Detektivknoten hinzu
                    {
                        FlaggedGameStateGraphBackwardsRecursion(previousState);
                    }
                }
                else //wenn der Deib dran ist muss jeder möglicher Zug schon im GameStateGraph gespeichert sein
                {
                    if (AddFlaggedThiefGameState(previousState)) //fügt Detektivknoten hinzu
                    {
                        FlaggedGameStateGraphBackwardsRecursion(previousState);
                    }
                }
            }
        }

        /// <summary>
        /// baut den GameStateGraph durch eine Fixpointiteration auf
        /// </summary>
        public void BuildGameStateGraphFixpoint()
        {
            bool continueFixpoint;
            do
            {
                continueFixpoint = false;
                var allVertices = Vertices.ToList();
                foreach (var currentState in allVertices) // geht alle Knoten in bisheriger Menge durch
                {
                    if (currentState.detectivesTurn && currentState.possiblePreviousStepsCount > 0) //wenn detektiv vorher dran war wird alles hinzugefügt
                    {
                        var previousPossibleStates = graph.GetPreviousPossibleStates(currentState);
                        currentState.possiblePreviousStepsCount = previousPossibleStates.Count;
                        foreach (var previousState in previousPossibleStates) // alle Knoten mit den man den jetzigen Knoten ereichen kann
                        {
                            continueFixpoint = continueFixpoint || AddThiefGameState(previousState); // fügt Diebknoten hinzu                                
                        }
                    }
                    else if (currentState.possiblePreviousStepsCount > 0)// Dieb war davor dran
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
                        
                    }
                }

            } while (continueFixpoint); // macht ewweiter , wenn neuen Knoten oder och nicht Startknoten gefunden wurde
        }

        /// <summary>
        /// baut den GameStateGraph durch eine Fixpointiteration auf mit Gewinnwahrscheinlichkeit und Distanz
        /// </summary>
        public void BuildFlaggedGameStateGraphFixpoint()
        {
            bool continueFixpoint;
            do
            {
                continueFixpoint = false;
                foreach (var currentState in Vertices) // geht alle Knoten in bisheriger Menge durch
                {
                    if (currentState.detectivesTurn && currentState.possiblePreviousStepsCount > 0) //wenn detektiv vorher dran war wird alles hinzugefügt
                    {
                        foreach (var previousState in graph.GetPreviousPossibleStates(currentState)) // alle Knoten mit den man den jetzigen Knoten ereichen kann
                        {
                            continueFixpoint = continueFixpoint || AddFlaggedThiefGameState(previousState);
                        }  
                    }
                    else if (currentState.possiblePreviousStepsCount > 0)// Dieb war davor dran
                    {
                        foreach (var previousState in graph.GetPreviousPossibleStates(currentState)) // alle Knoten mit den man jetzigen Knoten ereichen kann
                        {
                            continueFixpoint = continueFixpoint || AddFlaggedDetectiveGameState(previousState, currentState); // fügt Detektivknoten hinzu
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
                    if(GetExistingState(targetState) == null) return false; //wenn nicht alle Kanten in den Baum führen gewinnt der Detektiv nicht sicher
                }
                previousState.possiblePreviousStepsCount = graph.GetPreviousPossibleStates(previousState).Count;
                AddVertex(previousState);
                foreach (var targetState in nextPossibleStates) // alle Kanten vom Knoten werden hinzugefügt
                {
                    GetExistingState(targetState).possiblePreviousStepsCount--;
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
            int minDistanceToWin = VertexCount;
            double winningChanceSum = 0;
            var isNewPState = GetExistingState(previousState);

            var sourceState = isNewPState;
            if (isNewPState == null)
            {
                AddVertex(previousState); //fügt alle Knoten hinzu
                sourceState = previousState;
            }

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
                    if (!isExistingEdge(sourceState, isNewState))
                    {
                        AddEdge(new Edge<GameState<V>>(sourceState, isNewState));
                    }
                }
            }            

            previousState.winningChance = winningChanceSum / nextPossibleStates.Count; // Wahrscheinlichkeit des Gewinns bei Zufälliger Zugwahl
            previousState.distanceToWin += minDistanceToWin + 1;           
                
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
            var isNewState = GetExistingState(previousState); //checken ob schon vorhandener Knoten
            if (isNewState == null)
            {
                previousState.possiblePreviousStepsCount = graph.GetPreviousPossibleStates(previousState).Count;
                AddVertex(previousState); // Hinzufügen, wenn neu
                currentState.possiblePreviousStepsCount--;
                AddEdge(new Edge<GameState<V>>(previousState, currentState));
                return true;
            }            
            if (isNewState.Equals(startState)) // prüft ob Startknoten gefunden
            {
                currentState.possiblePreviousStepsCount--;
                AddVertex(startState);
                AddEdge(new Edge<GameState<V>>(startState, currentState));
                return null;
            }           
            return false;
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
            foreach (var state in Vertices) // geht alle Knoten im Spielbaum durch
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
            List<GameState<V>> bestDetectiveMoves = [];
            double bestWinningChance = 0;
            int bestDistanceToWin = VertexCount;
            foreach (var nextState in GetOutgoingStates(currentState)) // findet die beste Gewinnwahrscheinlichkeit
            {                
                if (nextState.winningChance > bestWinningChance)
                {
                    bestWinningChance = nextState.winningChance;
                    bestDistanceToWin = nextState.distanceToWin;
                    bestDetectiveMoves = [nextState];
                }
                else if ((nextState.winningChance == bestWinningChance) && (nextState.distanceToWin < bestDistanceToWin))
                {
                    bestDistanceToWin = nextState.distanceToWin;
                    bestDetectiveMoves = [nextState];
                }
                else if ((nextState.winningChance == bestWinningChance) && (nextState.distanceToWin == bestDistanceToWin))
                {
                    bestDetectiveMoves.Add(nextState);
                }
            }
            if (!(bestDetectiveMoves.Count == 0)) // es wurden keine guten Züge gefunden
            {
                detectiveStrategy.Add(new Move<V>(currentState, bestDetectiveMoves));
            }
        }

        /// <summary>
        /// erstellt eine Liste an besten Zügen zu den jeweiligem Status des Spiels
        /// </summary>
        /// <param name="currentState"></param>
        private void nextThiefMove(GameState<V> currentState)
        {            
            List<GameState<V>> bestThiefMoves = [];
            var outgoingVertex = GetOutgoingStates(currentState);
            double worstWinningChance = 1;
            int worstDistanceToWin = 0;
            foreach (var possibleNextState in graph.GetNextPossibleStates(currentState)) //prüft ob Dieb sicher gewinnen kann
            {
                foreach (var state in outgoingVertex)
                {
                    if (!possibleNextState.Equals(state)) // ist im GameStateGraph
                    {
                        bestThiefMoves.Add(possibleNextState);
                        return;
                    }
                }
            }
            foreach (var nextState in outgoingVertex)
            {
                if ((nextState.winningChance < worstWinningChance))
                {   // finde die schlechteste Gewinnwahrscheinlichkeit
                    worstWinningChance = nextState.winningChance;
                    worstDistanceToWin = nextState.distanceToWin;
                    bestThiefMoves = [nextState];
                }
                else if ((nextState.winningChance == worstWinningChance) && (nextState.distanceToWin > worstDistanceToWin))
                {   // finde zur schlechtesten Gewinnwahrscheinlichkeit den längsten Weg
                    worstDistanceToWin = nextState.distanceToWin;
                    bestThiefMoves = [nextState];
                }
                else if ((nextState.winningChance == worstWinningChance) && (nextState.distanceToWin == worstDistanceToWin))
                {   // füge alle längsten und schlechtesten Möglichkeiten hinzu
                    bestThiefMoves.Add(nextState);
                }
            }
            thiefStrategy.Add(new Move<V>(currentState, bestThiefMoves));
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
            foreach (var thiefPos in graph.Vertices) //geht jeden Knoten des Graphen durch
            {
                var outgoingStates = graph.GetOutgoingVertex(thiefPos).Distinct().ToList();
                var outgoingStateCount = outgoingStates.Count;
                if (outgoingStateCount <= detectiveMaxAmount) // prüft ob Fluchtmöglichkeiten von Detektiven blockiert werden können
                {
                    var tempState = new GameState<V>(detectiveMaxAmount, thiefPos, true);                    
                    for (var i = 0; i < outgoingStateCount; i++)
                    {
                        tempState.detectives.Add(outgoingStates[i]);// setzt Detektive auf die Fluchtmöglichkeit
                        tempState.winningChance = 1;
                        tempState.distanceToWin = 0;
                        tempState.savePathFound = true;
                    }
                                       
                    if (detectiveMaxAmount == outgoingStateCount)
                    {
                        result.Add(tempState);
                    }
                    else
                    {
                        AddAllDetektivesToState(tempState, result);
                    }
                }
            }
            var temp = result.ToArray().ToList();
            foreach (var finalState in temp)
            {
                var cloned = finalState.Clone().ChangeTurn();
                cloned.distanceToWin = finalState.distanceToWin;
                cloned.winningChance = finalState.winningChance;
                cloned.savePathFound = finalState.savePathFound;
                result.Add(cloned);
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
                finalState.winningChance = 1;
                finalState.distanceToWin = 0;
                finalState.savePathFound = true;
                finalState.detectives.Add(s);
                if (finalState.detectives.Count == detectiveMaxAmount)
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
