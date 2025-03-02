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
        private List<GameState<V>> allStartStates;
        public readonly List<Move<V>> detectiveStrategy = [];
        public readonly List<Move<V>> thiefStrategy = [];
        public readonly List<GameState<V>> finalStates = [];

        /// <summary>
        /// Erstellt einen Spielzustandsgraphen für die Berechnung des Entanglements.
        /// </summary>
        /// <param name="_graph"></param>
        /// <param name="detectiveAmount"></param>
        public GameStateGraph(FiniteDirectedGraph<V> _graph, int detectiveAmount)
        {
            detectiveMaxAmount = detectiveAmount;
            graph = _graph;
            possibleFinalStates = GetPossibleFinalStates();
            allStartStates = GetAllStartStates();
            foreach (var state in possibleFinalStates)
            {
                state.possiblePreviousStepsCount = graph.GetPreviousPossibleStates(state).Count;
                AddVertex(state);
                finalStates.Add(state);
            }
        }


        /// <summary>
        /// Erstellt einen Spielzustandsgraphen für die Berechnung der Strategien.
        /// </summary>
        /// <param name="_graph"></param>
        /// <param name="startState"></param>
        public GameStateGraph(FiniteDirectedGraph<V> _graph, GameState<V> startState)
        {
            detectiveMaxAmount = startState.detectiveMaxAmount;
            graph = _graph;
            possibleFinalStates = GetPossibleFinalStates();
            allStartStates = [startState];
            AddVertex(startState);
            foreach (var state in possibleFinalStates)
            {
                state.possiblePreviousStepsCount = graph.GetPreviousPossibleStates(state).Count;
                AddVertex(state);
                finalStates.Add(state);
            }
        }

        /// <summary>
        /// Startet die Rekursion der Tiefensuche. Der Wahrheitswert gibt an, ob der Detektiv sicher gewinnen kann.
        /// </summary>
        /// <returns></returns>
        public bool BuildGameStateGraphDFS()
        {
            foreach (var finalState in finalStates)
            {
                GameStateGraphDFSRecursion(finalState);
                if (allStartStates.Count == 0) return true;
            }
            return false;
        }

        /// <summary>
        /// Baut den GameStateGraph nach der Tiefensuche auf, allerdings nur bis zu dem Punkt, an dem der Detektiv sicher gewinnen kann.
        /// </summary>
        /// <param name="currentState"></param>
        private void GameStateGraphDFSRecursion(GameState<V> currentState)
        {
            foreach (var previousState in graph.GetPreviousPossibleStates(currentState))
            {
                if (previousState.detectivesTurn)
                {
                    var temp = AddDetectiveGameState(previousState, currentState);
                    if (temp == null)
                    {
                        return;
                    }
                    else if (temp == true)
                    {
                        GameStateGraphDFSRecursion(previousState);
                    }
                }
                else
                {
                    if (AddThiefGameState(previousState))
                    {
                        GameStateGraphDFSRecursion(previousState);
                    }
                }
            }
        }

        /// <summary>
        /// Baut den GameStateGraph nach der Tiefensuche auf mit bestimmten Wahrscheinlichkeiten (hier Flag genannt).
        /// </summary>
        public void BuildFlaggedGameStateGraphDFS()
        {
            foreach (var startState in allStartStates)
            {
                AddVertex(startState);
            }
            foreach (var finalState in finalStates)
            {
                FlaggedGameStateGraphDFSRecursion(finalState);
            }
        }

        /// <summary>
        /// Baut den GameStateGraph rekursiv auf mit Gewinnwahrscheinlichkeit und Distanz.
        /// </summary>
        /// <param name="currentState"></param>
        private void FlaggedGameStateGraphDFSRecursion(GameState<V> currentState)
        {
            foreach (var previousState in graph.GetPreviousPossibleStates(currentState))
            {
                if (previousState.detectivesTurn)
                {
                    if (AddFlaggedDetectiveGameState(previousState, currentState))
                    {
                        FlaggedGameStateGraphDFSRecursion(previousState);
                    }
                }
                else
                {
                    if (AddFlaggedThiefGameState(previousState))
                    {
                        FlaggedGameStateGraphDFSRecursion(previousState);
                    }
                }
            }
        }

        /// <summary>
        /// Baut den GameStateGraph durch die Breitensuche auf. Der Wahrheitswert gibt an, ob der Detektiv sicher gewinnen kann.
        /// </summary>
        public bool BuildGameStateGraphBFS()
        {
            bool continueFixpoint;
            do
            {
                continueFixpoint = false;
                var allVertices = Vertices.ToList();
                foreach (var currentState in allVertices)
                {
                    if (currentState.detectivesTurn && currentState.possiblePreviousStepsCount > 0)
                    {
                        var previousPossibleStates = graph.GetPreviousPossibleStates(currentState);
                        currentState.possiblePreviousStepsCount = previousPossibleStates.Count;
                        foreach (var previousState in previousPossibleStates)
                        {
                            continueFixpoint = continueFixpoint || AddThiefGameState(previousState);
                        }
                    }
                    else if (currentState.possiblePreviousStepsCount > 0)
                    {
                        var previousPossibleStates = graph.GetPreviousPossibleStates(currentState);
                        currentState.possiblePreviousStepsCount = previousPossibleStates.Count;
                        foreach (var previousState in previousPossibleStates)
                        {
                            bool? temp = AddDetectiveGameState(previousState, currentState);
                            if (temp == null)
                            {
                                continueFixpoint = false;
                                break;
                            }
                            continueFixpoint = continueFixpoint || (bool)temp;
                        }
                    }
                }
            } while (continueFixpoint);
            if (allStartStates.Count == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Baut den GameStateGraph durch die Breitensuche auf mit Gewinnwahrscheinlichkeit und Distanz.
        /// </summary>
        public void BuildFlaggedGameStateGraphBFS()
        {
            bool continueFixpoint;
            do
            {
                continueFixpoint = false;
                var allStates = Vertices.ToList();
                foreach (var currentState in allStates)
                {
                    if (currentState.detectivesTurn && currentState.possiblePreviousStepsCount > 0)
                    {
                        foreach (var previousState in graph.GetPreviousPossibleStates(currentState))
                        {
                            continueFixpoint = continueFixpoint || AddFlaggedThiefGameState(previousState);
                        }
                    }
                    else if (currentState.possiblePreviousStepsCount > 0)
                    {
                        foreach (var previousState in graph.GetPreviousPossibleStates(currentState))
                        {
                            continueFixpoint = continueFixpoint || AddFlaggedDetectiveGameState(previousState, currentState);
                        }
                    }
                }
            } while (continueFixpoint);
        }

        /// <summary>
        /// Fügt Knoten, wo der Dieb dran ist, zum GameStateGraph hinzu.
        /// </summary>
        /// <param name="previousState"></param>
        private bool AddThiefGameState(GameState<V> previousState)
        {
            if (GetExistingState(previousState) == null)
            {
                var nextPossibleStates = graph.GetNextPossibleStates(previousState);
                foreach (var targetState in nextPossibleStates)
                {
                    if (GetExistingState(targetState) == null) return false;
                }
                previousState.possiblePreviousStepsCount = graph.GetPreviousPossibleStates(previousState).Count;
                AddVertex(previousState);
                foreach (var targetState in nextPossibleStates)
                {
                    GetExistingState(targetState).possiblePreviousStepsCount--;
                    AddEdge(new Edge<GameState<V>>(previousState, GetExistingState(targetState)));
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Fügt Knoten zum GameStateGraph mit Gewinnwahrscheinlichkeit für den Detektiv bei zufälliger Zugwahl des Diebes hinzu.
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
                previousState.possiblePreviousStepsCount = graph.GetPreviousPossibleStates(previousState).Count;
                AddVertex(previousState);
                sourceState = previousState;
            }

            foreach (var nextState in nextPossibleStates)
            {
                var isNewState = GetExistingState(nextState);
                if (isNewState != null)
                {
                    winningChanceSum += isNewState.winningChance;
                    if (isNewState.distanceToWin < minDistanceToWin)
                    {
                        minDistanceToWin = isNewState.distanceToWin;
                    }
                    if (!isExistingEdge(sourceState, isNewState))
                    {
                        isNewState.possiblePreviousStepsCount--;
                        AddEdge(new Edge<GameState<V>>(sourceState, isNewState));
                    }
                }
            }

            previousState.winningChance = winningChanceSum / nextPossibleStates.Count;
            previousState.distanceToWin += minDistanceToWin + 1;

            if (isNewPState != null)
            {
                if (previousState.winningChance < isNewPState.winningChance)
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
        /// Ändert alle abhängigen Gewinnwahrscheinlichkeiten.
        /// </summary>
        /// <param name="state"></param>
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
        /// Fügt Knoten hinzu, wo der Detektiv dran ist.
        /// </summary>
        /// <param name="previousState"></param>
        /// <param name="currentState"></param>
        /// <returns></returns>
        private bool? AddDetectiveGameState(GameState<V> previousState, GameState<V> currentState)
        {
            var isNewState = GetExistingState(previousState);
            if (isNewState == null)
            {
                previousState.possiblePreviousStepsCount = graph.GetPreviousPossibleStates(previousState).Count;
                AddVertex(previousState);
                currentState.possiblePreviousStepsCount--;
                AddEdge(new Edge<GameState<V>>(previousState, currentState));
                return true;
            }
            if (AllStartStatesFoundAndAdded(isNewState, currentState)) return null;
            return false;
        }


        /// <summary>
        /// Fügt Knoten zum GameStateGraph mit Gewinnwahrscheinlichkeit für den Detektiv bei zufälliger Zugwahl des Diebes hinzu.
        /// </summary>
        /// <param name="previousState"></param>
        /// <param name="currentState"></param>
        /// <returns></returns>
        private bool AddFlaggedDetectiveGameState(GameState<V> previousState, GameState<V> currentState)
        {
            var isNewState = GetExistingState(previousState);
            if (isNewState == null)
            {
                previousState.distanceToWin = currentState.distanceToWin + 1;
                previousState.winningChance = currentState.winningChance;
                previousState.possiblePreviousStepsCount = graph.GetPreviousPossibleStates(previousState).Count;
                AddVertex(previousState);
                currentState.possiblePreviousStepsCount--;
                AddEdge(new Edge<GameState<V>>(previousState, currentState));
                return true;
            }
            else if (isNewState.winningChance < currentState.winningChance)
            {
                isNewState.distanceToWin = currentState.distanceToWin + 1;
                isNewState.winningChance = currentState.winningChance;
                changeAllPreviousWC(isNewState);
                if (!isExistingEdge(isNewState, currentState))
                {
                    AddEdge(new Edge<GameState<V>>(isNewState, currentState));
                }
                changeAllPreviousWC(isNewState);
            }
            return false;
        }

        /// <summary>
        /// Erstellt Strategien für beide Spieler.
        /// </summary>
        public void CreateStrategies()
        {
            foreach (var state in Vertices)
            {
                if (state.detectivesTurn)
                {
                    nextDetectiveMove(state);
                }
                else
                {
                    nextThiefMove(state);
                }
            }
        }

        /// <summary>
        /// Erstellt eine Liste an besten Zügen zu dem jeweiligen Status des Spiels.
        /// </summary>
        /// <param name="currentState"></param>
        private void nextDetectiveMove(GameState<V> currentState)
        {
            List<GameState<V>> bestDetectiveMoves = [];
            double bestWinningChance = 0;
            int bestDistanceToWin = VertexCount;
            foreach (var nextState in GetOutgoingStates(currentState))
            {
                if (nextState.winningChance > bestWinningChance)
                {
                    bestWinningChance = nextState.winningChance;
                    bestDistanceToWin = nextState.distanceToWin;
                    bestDetectiveMoves = [nextState];
                }
                else if (nextState.winningChance == bestWinningChance && nextState.distanceToWin < bestDistanceToWin)
                {
                    bestDistanceToWin = nextState.distanceToWin;
                    bestDetectiveMoves = [nextState];
                }
                else if (nextState.winningChance == bestWinningChance && nextState.distanceToWin == bestDistanceToWin)
                {
                    bestDetectiveMoves.Add(nextState);
                }
            }
            if (bestDetectiveMoves.Count != 0)
            {
                detectiveStrategy.Add(new Move<V>(currentState, bestDetectiveMoves));
            }
        }

        /// <summary>
        /// Erstellt eine Liste an besten Zügen zu dem jeweiligen Status des Spiels.
        /// </summary>
        /// <param name="currentState"></param>
        private void nextThiefMove(GameState<V> currentState)
        {
            List<GameState<V>> bestThiefMoves = [];
            var outgoingVertex = GetOutgoingStates(currentState);
            double worstWinningChance = 1;
            int worstDistanceToWin = 0;
            foreach (var possibleNextState in graph.GetNextPossibleStates(currentState))
            {
                bool saveWinFound = true;
                foreach (var state in outgoingVertex)
                {
                    if (possibleNextState.Equals(state))
                    {
                        saveWinFound = false;
                        break;
                    }
                }
                if (saveWinFound)
                {
                    bestThiefMoves.Add(possibleNextState);
                    return;
                }
            }
            foreach (var nextState in outgoingVertex)
            {
                if (nextState.winningChance < worstWinningChance)
                {
                    worstWinningChance = nextState.winningChance;
                    worstDistanceToWin = nextState.distanceToWin;
                    bestThiefMoves = [nextState];
                }
                else if (nextState.winningChance == worstWinningChance && nextState.distanceToWin > worstDistanceToWin)
                {
                    worstDistanceToWin = nextState.distanceToWin;
                    bestThiefMoves = [nextState];
                }
                else if (nextState.winningChance == worstWinningChance && nextState.distanceToWin == worstDistanceToWin)
                {
                    bestThiefMoves.Add(nextState);
                }
            }
            thiefStrategy.Add(new Move<V>(currentState, bestThiefMoves));
        }

        /// <summary>
        /// Sucht einen der besten Spielzüge zum aktuellen Spielstatus aus und gibt ihn zurück mit der Angabe, ob sich der Detektiv bewegt hat.
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public (V, bool) BestDetectiveMove(GameState<V> currentState)
        {
            var cloned = currentState.Clone();
            cloned.ChangeTurn();
            foreach (var move in detectiveStrategy)
            {
                if (move.source.Equals(currentState))
                {
                    if (cloned.Equals(move.target.First()))
                    {
                        return (default, false);
                    }
                    return (currentState.getMovedDetective(move.target.First()), true);
                }
            }
            return (default, false);
        }

        /// <summary>
        /// Sucht einen der besten Spielzüge zum aktuellen Spielstatus aus.
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public V BestThiefMove(GameState<V> currentState)
        {
            foreach (var move in thiefStrategy)
            {
                if (move.source.Equals(currentState))
                {
                    return move.target.First().thiefPos;
                }
            }
            return graph.GetNextPossibleStates(currentState).First().thiefPos;
        }

        /// <summary>
        /// Gibt schon vorhandene Position zurück, wenn Position doppelt ist.
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
        /// Prüft, ob Kante schon vorhanden ist.
        /// </summary>
        /// <param name="sourceState"></param>
        /// <param name="targetState"></param>
        /// <returns></returns>
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
        /// gibt alle möglichen Startzustände zurück
        /// </summary>
        /// <returns></returns>
        public List<GameState<V>> GetAllStartStates()
        {
            var result = new List<GameState<V>>();
            foreach (var thiefPos in graph.Vertices) //geht jeden Knoten des Graphen durch
            {
                result.Add(new GameState<V>(detectiveMaxAmount, thiefPos, true));
            }
            return result;
        }

        /// <summary>
        /// prüft ob alle Startzustände gefunden und hinzugefügt wurden
        /// </summary>
        /// <param name="possibleStartState"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool AllStartStatesFoundAndAdded(GameState<V> possibleStartState, GameState<V> state)
        {
            foreach (var startState in allStartStates)
            {
                if (startState.Equals(possibleStartState))
                {
                    allStartStates.Remove(startState);
                    state.possiblePreviousStepsCount--;
                    AddVertex(possibleStartState);
                    AddEdge(new Edge<GameState<V>>(possibleStartState, state));
                    break;
                }
            }
            if (allStartStates.Count == 0)
            {
                return true;
            }
            return false;
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
