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
    internal class FiniteDirectedGraph<V> : AdjacencyGraph<V, Edge<V>> where V : IEquatable<V>
    {
        protected Microsoft.Msagl.Drawing.Graph? msaglGraph = new Microsoft.Msagl.Drawing.Graph("");
        protected Microsoft.Msagl.GraphViewerGdi.GraphRenderer? renderer;

        /// <summary>
        /// Konstruktor für die Erstellung des Graphen
        /// </summary>
        /// <param name="vertexes"></param>
        /// <param name="edges"></param>
        public FiniteDirectedGraph(List<V> vertexes, List<(V, V)> edges)
        {
            foreach (var vertex in vertexes)
            {
                AddVertex(vertex);
                AddVertexToMsagl(vertex);
            }

            foreach (var edge in edges)
            {
                AddEdge(new Edge<V>(edge.Item1, edge.Item2));
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
        /// Gibt alle möglichen Zustände zurück, die durch den nächsten Zug möglich wären
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public List<GameState<V>> GetNextPossibleStates(GameState<V> currentState)
        {
            List<GameState<V>> nextPossibleStates = [];

            if (currentState.detectivesTurn) // Entscheidet, ob Detectives oder Thief einen Zug spielen
            {
                nextPossibleStates.Add(currentState.Clone().ChangeTurn());
                if (currentState.detectiveMaxAmount > currentState.detectives.Count)
                {
                    nextPossibleStates.Add(currentState.Clone().MoveDetective(default).ChangeTurn());
                }
                foreach (var detective in currentState.detectives) // Gehe jeden möglichen Zug der Detectives durch
                {
                    nextPossibleStates.Add(currentState.Clone().MoveDetective(detective).ChangeTurn());
                }
            }
            else
            {
                List<V> possibleNextStates = GetOutgoingVertex(currentState.thiefPos).Except(currentState.detectives.ToList()).ToList(); // Mögliche Züge des Diebes

                if (possibleNextStates.Count != 0) // Prüft, ob Züge möglich sind
                {
                    for (int i = 0; i < possibleNextStates.Count; i++) // Geht jeden möglichen Zug durch
                    {
                        nextPossibleStates.Add(currentState.Clone().MoveThief(possibleNextStates[i]).ChangeTurn());
                    }
                }
            }
            return nextPossibleStates;
        }

        /// <summary>
        /// Gibt die möglichen Züge des Diebes als String für die Ausgabe zurück
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
        /// Gibt mögliche vorige Schritte zurück
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public List<GameState<V>> GetPreviousPossibleStates(GameState<V> currentState)
        {
            List<GameState<V>> result = [];

            if (currentState.detectivesTurn) // Wenn der Detektiv dran ist, war davor der Dieb dran (currentState enthält den Zug des Diebes)
            {
                foreach (var edge in Edges)
                {
                    if (edge.Target.Equals(currentState.thiefPos)) // Alle möglichen Knoten, von denen der Dieb kommen kann
                    {
                        var previousState = currentState.Clone().ChangeTurn();
                        previousState.thiefPos = edge.Source;
                        result.Add(previousState); // Werden zum Result hinzugefügt
                    }
                }
            }
            else // Wenn der Dieb dran ist, waren davor die Detektive dran
            {
                if (currentState.detectives.Contains(currentState.thiefPos)) // Wenn sich ein Detektiv auf der Position des Diebes befindet, wurde dieser Detektiv bewegt
                {
                    var previousPos = currentState.Clone().ChangeTurn();
                    previousPos.detectives.Remove(currentState.thiefPos);
                    result.Add(previousPos);
                    foreach (var vertex in Vertices.Except(currentState.detectives.ToList())) // Bewegt den Dieb zu jedem möglichen Knoten
                    {
                        previousPos = currentState.Clone().ChangeTurn();
                        previousPos.detectives.Remove(currentState.thiefPos);
                        previousPos.detectives.Add(vertex);
                        result.Add(previousPos);
                    }

                }
                else // Diebe haben sich nicht bewegt
                {
                    result.Add(currentState.Clone().ChangeTurn()); // Gleiche Position wird hinzugefügt, nur mit der Info, dass der Dieb dran war
                }
            }
            return result;
        }

        /// <summary>
        /// Berechnet das Entanglement mit BFS oder DFS
        /// </summary>
        /// <param name="gameStateGraphTyp"></param>
        /// <returns></returns>
        public int? MinEntanglement(GameStateGraphTyp gameStateGraphTyp)
        {
            int possibleMinEnt = VertexCount;
            foreach (var vertex in Vertices) // Berechnet minimal mögliches Entanglement
            {
                var temp = GetOutgoingVertex(vertex).Count;
                if (temp < possibleMinEnt)
                {
                    possibleMinEnt = temp;
                }
            }
            if (gameStateGraphTyp == GameStateGraphTyp.BFS)
            {
                for (int i = possibleMinEnt; i <= VertexCount; i++) // Geht von possibleMinEnt bis Anzahl an Knoten
                {
                    if (new GameStateGraph<V>(this, i).BuildGameStateGraphBFS()) // Prüft Entanglement
                    {
                        return i; // Minimalstes Entanglement
                    }
                }
            }
            else
            {
                for (int i = possibleMinEnt; i <= VertexCount; i++) // Geht von possibleMinEnt bis Anzahl an Knoten
                {
                    if (new GameStateGraph<V>(this, i).BuildGameStateGraphDFS()) // Prüft Entanglement
                    {
                        return i; // Minimalstes Entanglement
                    }
                }
            }
            return null; // Fehler, wenn kein Entanglement gefunden wurde
        }

        /// <summary>
        /// Fügt Knoten zur Zeichnung hinzu
        /// </summary>
        /// <param name="vertex"></param>
        public void AddVertexToMsagl(V vertex)
        {
            msaglGraph.AddNode(vertex.ToString());
        }

        /// <summary>
        /// Löscht Knoten von der Zeichnung
        /// </summary>
        /// <param name="vertex"></param>
        public void DeleteVertexToMsagl(V vertex)
        {
            msaglGraph.RemoveNode(msaglGraph.FindNode(vertex.ToString()));
        }

        /// <summary>
        /// Fügt Kante zur Zeichnung hinzu
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void AddEdgeToMsagl(V source, V target)
        {
            msaglGraph.AddEdge(source.ToString(), target.ToString());
        }

        /// <summary>
        /// Löscht eine Kante im Graphen
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public void DeleteEdgeToMsagl(V source, V target)
        {
            var edge = msaglGraph.Edges.FirstOrDefault(e => e.Source == source.ToString() && e.Target == target.ToString());
            msaglGraph.RemoveEdge(edge);
        }

        /// <summary>
        /// Erstellt ein mögliches Layout des Graphen, bereit gezeichnet zu werden
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
        /// Färbt gegebenen Knoten in der gegebenen Farbe ein
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
        /// Formt den Knoten in die gegebene Form um
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
        /// Gibt die erreichbaren Knoten von einem anderen Knoten zurück
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
