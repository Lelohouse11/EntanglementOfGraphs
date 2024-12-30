using Microsoft.Msagl.Drawing;
using QuikGraph;
using QuikGraph.Algorithms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    internal class FiniteDirectedGraph <V> : AdjacencyGraph<V, Edge<V>> where V : IComparable<V>, IEquatable<V>
    {
        private readonly bool debug = false;
        protected Microsoft.Msagl.Drawing.Graph? msaglGraph;
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
            }

            foreach (var edge in edges)
            {
                AddEdge(new Edge<V>(edge.Item1,edge.Item2));
            }
        }

        /// <summary>
        /// Konstruktor für andere Graphentypen
        /// </summary>
        public FiniteDirectedGraph()
        {

        } 
        
        /// <summary>
        /// Erstellt Graph den man Zeichnen kann
        /// </summary>
        /// <returns></returns>
        public void CreateMsagl()
        {
            msaglGraph = new Microsoft.Msagl.Drawing.Graph("");
            foreach (var vertex in Vertices)
            {
                msaglGraph.AddNode(vertex.ToString());
            }
            foreach (var edge in Edges)
            {
                msaglGraph.AddEdge(edge.Source.ToString(),edge.Target.ToString());
            }
        }     
        
        /// <summary>
        /// Erstellt ein mögöiches Layout des Graphen bereit gezeichnet zu werden.
        /// </summary>
        /// <param name="pb"></param>
        public void CreateImage(PictureBox pb)
        {
            CreateMsagl();
            renderer = new Microsoft.Msagl.GraphViewerGdi.GraphRenderer(msaglGraph);
            
            renderer.CalculateLayout();
        }

        /// <summary>
        /// Zeichnet den Graphen
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pb"></param>
        public void DrawImage(Graphics g,PictureBox pb)
        {
              renderer.Render(g, pb.ClientRectangle);
        }

        /// <summary>
        /// Färbt gegebenen Knoten ind gegebener Farbe ein
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="color"></param>
        public void ColorVertex (string vertex, Microsoft.Msagl.Drawing.Color color)
        {
            CreateMsagl();
            msaglGraph.FindNode(vertex).Attr.FillColor = color;
            renderer = new Microsoft.Msagl.GraphViewerGdi.GraphRenderer(msaglGraph);
            renderer.CalculateLayout();
        }

        public void ShapeVertex(string vertex, Microsoft.Msagl.Drawing.Shape shape)
        {
            CreateMsagl();
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

        /// <summary>
        /// erstellt einen GameTree (Spielverlauf) auf verschiedene Art und Weisen
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="gameTreeTyp"></param>
        /// <returns></returns>
        public GameTree<V> GetGameTree(Positions<V> startPos, GameTreeTyp gameTreeTyp)
        {
            var gameTree = new GameTree<V>(this, startPos);
            if (gameTreeTyp == GameTreeTyp.Iterativ) //wenn ierativer Aufbau
            {
                return gameTree.BuildIterativGameTree(startPos);
            }
            else if (gameTreeTyp == GameTreeTyp.Rekursiv) // wenn rekursiver Aufbau
            {                
                foreach (var finalState in gameTree.possibleFinalStates) // ruft rekursiven Aufruf auf alle Endzustände auf
                {
                    if (!gameTree.OutEdges(startPos).Any())
                    {
                        gameTree.BuildRecursiveGameTree(finalState);
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
                gameTree.BuildFixpointGameTree(false);
                return gameTree;
            }
            return gameTree;
        }

        /// <summary>
        /// gibt alle möglichen Zustände durch nächsten Move zurück
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public List<Positions<V>> GetNextPossibleSteps(Positions<V> pos)
        {
            List<Positions<V>> result = [];

            if (pos.detectivesTurn) // entscheidet ob Detectives oder Thief einen Zug spielen
            {
                result.Add(pos.ChangeTurn());
                if (pos.detectiveAmount > pos.detectives.Count)
                {
                    result.Add(pos.Clone().MoveDetective(default).ChangeTurn());
                }
                foreach (var detective in pos.detectives) // gehe jeden möglichen Move der Detectives durch
                {
                    result.Add(pos.Clone().MoveDetective(detective).ChangeTurn());
                }
            }
            else
            {
                List<V> possibleMoves = GetOutgoingVertex(pos.thief).Except(pos.detectives.ToList()).ToList(); // mögliche Züge des Diebes

                if (possibleMoves.Count != 0) //prüft ob Züge möglich sind
                {
                    for (int i = 0; i < possibleMoves.Count; i++) // geht jeden möglichen Zug
                    {
                        result.Add(pos.Clone().MoveThief(possibleMoves[i]).ChangeTurn());
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// gibt mögliche vorige Schritte zurück
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public List<Positions<V>> GetPreviousPossibleSteps(Positions<V> pos) 
        {
            List <Positions<V>> result = [];

            if (pos.detectivesTurn) // wenn detektiv dran ist war davor der Dieb dran (pos enthält den Zug des Dieb)
            {
                foreach (var edge in Edges)
                {
                    if (edge.Target.Equals(pos.thief)) // alle möglichen Knoten von den der Dieb kommen kann
                    {
                        var previousPos = pos.Clone().ChangeTurn();
                        previousPos.thief = edge.Source;
                        result.Add(previousPos); // werden zum Result hinzugefügt
                    }
                }
            }
            else // wenn Dieb dran ist waren davor die Detektive dran (Detektive dran)
            {
                if (pos.detectives.Contains(pos.thief)) // Wenn sich ein Detektiv auf der Position des Diebes befindet wurde dieser Detektiv bewegt
                {
                    var previousPos = pos.Clone().ChangeTurn();
                    previousPos.detectives.Remove(pos.thief);                    
                    result.Add(previousPos);
                    foreach (var vertex in Vertices.Except(pos.detectives.ToList())) //bewegt den Dieb zu jedem möglichen Knoten
                    {
                        previousPos = pos.Clone().ChangeTurn();
                        previousPos.detectives.Remove(pos.thief);
                        previousPos.detectives.Add(vertex);
                        result.Add(previousPos);
                    }
                    
                }
                else // Diebe haben sich nicht bewegt
                {
                    result.Add(pos.Clone().ChangeTurn()); // gleiche Position wird hinzugefügt, nur mit der Info das Dieb dran war
                }
            }
            return result;
        }

        /// <summary>
        /// nutzt den rekursiven Gamtree um Entanglement zu überprüfen
        /// </summary>
        /// <param name="startPos"></param>
        /// <returns></returns>
        public bool IsEntanglement(Positions<V> startPos)
        {
            var gameTree = GetGameTree(startPos, GameTreeTyp.Rekursiv);
            //Console.WriteLine(gameTree.OutEdges(startPos).Count());
            return gameTree.OutEdges(startPos).Any();

        }

        /// <summary>
        /// berechnet minimalstest Entanglement
        /// </summary>
        /// <param name="startPosOfThief"></param>
        /// <returns></returns>
        public int? MinEntanglement(V startPosOfThief)
        {
            for (int i = 0; i <= VertexCount; i++) // geht von 0 bis Anzahl an Knoten
            {
                if (IsEntanglement(new Positions<V>(i, startPosOfThief, true))) //prüft Entanglment
                {
                    return i; //minimalstes Entanglement
                }
            }
            return null; // Fehler wenn kein Entanglement gefunden wurde
        }
    }
}
