using Microsoft.Msagl.Drawing;
using QuikGraph;
using QuikGraph.Algorithms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    internal class FiniteDirectedGraph <V> : AdjacencyGraph<V, Edge<V>> where V : IEquatable<V>
    {
        protected Microsoft.Msagl.Drawing.Graph? msaglGraph = new Microsoft.Msagl.Drawing.Graph("");
        protected Microsoft.Msagl.GraphViewerGdi.GraphRenderer? renderer;

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
                AddVertexToMsagl(vertex);
            }

            foreach (var edge in edges)
            {
                AddEdge(new Edge<V>(edge.Item1,edge.Item2));
                AddEdgeToMsagl(edge.Item1, edge.Item2);
            }
        }

        /// <summary>
        /// Konstruktor für andere Graphentypen
        /// </summary>
        public FiniteDirectedGraph()
        {

        }

        /// <summary>
        /// erstellt einen GameStateGraph (Spielverlauf) auf verschiedene Art und Weisen
        /// </summary>
        /// <param name="startState"></param>
        /// <param name="gameStateGraphTyp"></param>
        /// <returns></returns>
        public GameStateGraph<V> GetGameStateGraph(GameState<V> startState, GameStateGraphTyp gameStateGraphTyp)
        {
            var gameStateGraph = new GameStateGraph<V>(this, startState);
            if (gameStateGraphTyp == GameStateGraphTyp.Forward) //wenn ierativer Aufbau
            {
                gameStateGraph.BuildGameStateGraphForward(startState);
                return gameStateGraph;
            }
            else if (gameStateGraphTyp == GameStateGraphTyp.Backward) // wenn rekursiver Aufbau
            { 
                gameStateGraph.BuildGameStateGraphBackwards();
                return gameStateGraph;
            }
            else if (gameStateGraphTyp == GameStateGraphTyp.Fixpoint)
            {                
                gameStateGraph.BuildGameStateGraphFixpoint();
                return gameStateGraph;
            }
            return gameStateGraph;
        }

        /// <summary>
        /// gibt alle möglichen Zustände durch nächsten Move zurück
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public List<GameState<V>> GetNextPossibleStates(GameState<V> currentState)
        {
            List<GameState<V>> nextPossibleStates = [];

            if (currentState.detectivesTurn) // entscheidet ob Detectives oder Thief einen Zug spielen
            {
                nextPossibleStates.Add(currentState.Clone().ChangeTurn());
                if (currentState.detectiveMaxAmount > currentState.detectives.Count)
                {
                    nextPossibleStates.Add(currentState.Clone().MoveDetective(default).ChangeTurn());
                }
                foreach (var detective in currentState.detectives) // gehe jeden möglichen Move der Detectives durch
                {
                    nextPossibleStates.Add(currentState.Clone().MoveDetective(detective).ChangeTurn());
                }
            }
            else
            {
                List<V> possiblenextStates = GetOutgoingVertex(currentState.thiefPos).Except(currentState.detectives.ToList()).ToList(); // mögliche Züge des Diebes

                if (possiblenextStates.Count != 0) //prüft ob Züge möglich sind
                {
                    for (int i = 0; i < possiblenextStates.Count; i++) // geht jeden möglichen Zug
                    {
                        nextPossibleStates.Add(currentState.Clone().MoveThief(possiblenextStates[i]).ChangeTurn());
                    }
                }
            }
            return nextPossibleStates;
        }

        /// <summary>
        /// gibt die möglichen Züge des Diebes zurück als String
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public String GetNextPossibleStatesForThief(GameState<V> currentState)
        {
            string result = "";
            var nextPossibleStates = GetNextPossibleStates(currentState);
            if (nextPossibleStates.Count != 0)
            {
                result += nextPossibleStates[0].thiefPos;
                for (int i = 1; i < nextPossibleStates.Count; i++)
                {
                    result += ", ";
                    result += nextPossibleStates[i].thiefPos;
                }
            }
            return result;
        }

        /// <summary>
        /// gibt mögliche vorige Schritte zurück
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public List<GameState<V>> GetPreviousPossibleStates(GameState<V> currentState) 
        {
            List <GameState<V>> result = [];

            if (currentState.detectivesTurn) // wenn detektiv dran ist war davor der Dieb dran (currentState enthält den Zug des Dieb)
            {
                foreach (var edge in Edges)
                {
                    if (edge.Target.Equals(currentState.thiefPos)) // alle möglichen Knoten von den der Dieb kommen kann
                    {
                        var previousState = currentState.Clone().ChangeTurn();
                        previousState.thiefPos = edge.Source;
                        result.Add(previousState); // werden zum Result hinzugefügt
                    }
                }
            }
            else // wenn Dieb dran ist waren davor die Detektive dran (Detektive dran)
            {
                if (currentState.detectives.Contains(currentState.thiefPos)) // Wenn sich ein Detektiv auf der Position des Diebes befindet wurde dieser Detektiv bewegt
                {
                    var previousPos = currentState.Clone().ChangeTurn();
                    previousPos.detectives.Remove(currentState.thiefPos);                    
                    result.Add(previousPos);
                    foreach (var vertex in Vertices.Except(currentState.detectives.ToList())) //bewegt den Dieb zu jedem möglichen Knoten
                    {
                        previousPos = currentState.Clone().ChangeTurn();
                        previousPos.detectives.Remove(currentState.thiefPos);
                        previousPos.detectives.Add(vertex);
                        result.Add(previousPos);
                    }
                    
                }
                else // Diebe haben sich nicht bewegt
                {
                    result.Add(currentState.Clone().ChangeTurn()); // gleiche Position wird hinzugefügt, nur mit der Info das Dieb dran war
                }
            }
            return result;
        }

        /// <summary>
        /// nutzt den rekursiven Gamtree um Entanglement zu überprüfen
        /// </summary>
        /// <param name="startState"></param>
        /// <returns></returns>
        public bool IsEntanglement(GameState<V> startState, GameStateGraphTyp gameStateGraphTyp)
        {
            var gameStateGraph = GetGameStateGraph(startState, gameStateGraphTyp);
            return gameStateGraph.OutEdges(startState).Any();

        }

        /// <summary>
        /// berechnet minimalstest Entanglement
        /// </summary>
        /// <param name="thiefPos"></param>
        /// <returns></returns>
        public int? MinEntanglement(V thiefPos, GameStateGraphTyp gameStateGraphTyp)
        {
            int possibleMinEnt = VertexCount;
            foreach (var vertex in Vertices)
            {
                var temp = GetOutgoingVertex(vertex).Count;
                if (temp < possibleMinEnt)
                {
                    possibleMinEnt = temp;
                }
            }
            for (int i = possibleMinEnt; i <= VertexCount; i++) // geht von possibleMinEnt bis Anzahl an Knoten
            {
                if (IsEntanglement(new GameState<V>(i, thiefPos, true), gameStateGraphTyp)) //prüft Entanglment
                {
                    return i; //minimalstes Entanglement
                }
            }
            return null; // Fehler wenn kein Entanglement gefunden wurde
        }

        /// <summary>
        /// fügt Knoten zur Zeichnung hinzu
        /// </summary>
        /// <param name="vertex"></param>
        public void AddVertexToMsagl(V vertex)
        {
            msaglGraph.AddNode(vertex.ToString());
        }

        /// <summary>
        /// löscht Knoten von der Zeichnung
        /// </summary>
        /// <param name="vertex"></param>
        public void DeleteVertexToMsagl(V vertex)
        {
            msaglGraph.RemoveNode(msaglGraph.FindNode(vertex.ToString()));
        }

        /// <summary>
        /// fügt Kante zur Zeichnung hinzu
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void AddEdgeToMsagl(V source, V target)
        {
            msaglGraph.AddEdge(source.ToString(), target.ToString());
        }

        /// <summary>
        /// löscht eine Kante im Graphen
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void DeleteEdgeToMsagl(V source, V target)
        {
            var edge = msaglGraph.Edges.FirstOrDefault(e => e.Source == source.ToString() && e.Target == target.ToString());
            
            msaglGraph.RemoveEdge(edge);
        }

        /// <summary>
        /// Erstellt ein mögöiches Layout des Graphen bereit gezeichnet zu werden.
        /// </summary>
        /// <param name="pb"></param>
        public void CreateImage(PictureBox pb)
        {
            var layoutSettings = new Microsoft.Msagl.Layout.Layered.SugiyamaLayoutSettings();
            msaglGraph.LayoutAlgorithmSettings = layoutSettings;
            renderer = new Microsoft.Msagl.GraphViewerGdi.GraphRenderer(msaglGraph);            
            renderer.CalculateLayout();
        }

        /// <summary>
        /// Zeichnet den Graphen
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pb"></param>
        public void DrawImage(Graphics g, PictureBox pb)
        {
            renderer.Render(g, pb.ClientRectangle);
        }

        /// <summary>
        /// Färbt gegebenen Knoten ind gegebener Farbe ein
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="color"></param>
        public void ColorVertex(string vertex, Microsoft.Msagl.Drawing.Color color)
        {
            msaglGraph.FindNode(vertex).Attr.FillColor = color;
            renderer = new Microsoft.Msagl.GraphViewerGdi.GraphRenderer(msaglGraph);
            renderer.CalculateLayout();
        }

        /// <summary>
        /// formt den Knoten in gegebene Form um
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="shape"></param>
        public void ShapeVertex(string vertex, Microsoft.Msagl.Drawing.Shape shape)
        {
            msaglGraph.FindNode(vertex).Attr.Shape = shape;
            renderer = new Microsoft.Msagl.GraphViewerGdi.GraphRenderer(msaglGraph);
            renderer.CalculateLayout();
        }

        /// <summary>
        /// gibt die ereichbaren Knoten von einem anderen Knoten zurück
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public List<V> GetOutgoingVertex(V vertex)
        {
            List<V> result = [];
            foreach (var edge in this.OutEdges(vertex))
            {
                result.Add(edge.Target);
            }
            return result;
        }

        public Edge<V>? GetExistingEdge(V sourceState, V targetState)
        {
            foreach (var e in Edges)
            {
                if (e.Source.Equals(sourceState) && e.Target.Equals(targetState)) return e;
            }
            return null;
        }
    }
}
