using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Graphviz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    internal class FiniteDirectedGraph : AdjacencyGraph<int, Edge<int>>
    {
        /// <summary>
        /// Konstruktor für Grapherstellung
        /// </summary>
        /// <param name="vertexes"></param>
        /// <param name="edges"></param>
        public FiniteDirectedGraph(List<int> vertexes, List<(int,int)> edges) 
        { 
           foreach (var vertex in vertexes)
           {
                this.AddVertex(vertex);
           }

            foreach (var edge in edges)
            {
                this.AddEdge(new Edge<int>(edge.Item1,edge.Item2));
            }
        }
        /// <summary>
        /// erstellt String von .dot Datei für spätere Visualisierung
        /// </summary>
        /// <returns></returns>
        public String createDot()
        {
            var graphviz = new GraphvizAlgorithm<int, Edge<int>>(this);
            return graphviz.Generate();
        }

        /// <summary>
        /// gibt die ereichbaren Knoten von einem anderen Knoten zurück
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public List<int> getOutgoingVertex(int vertex)
        {
            List<int> result = new List<int>();
            foreach (var edge in this.OutEdges(vertex))
            {
                result.Add(edge.GetOtherVertex(vertex));
            }
            return result;
        }



        /// <summary>
        /// erstellt einen GameTree (kompleter Spielverlauf)
        /// </summary>
        /// <param name="startPos"></param>
        /// <returns></returns>
        public GameTree getGameTree(Positions startPos, bool gameTreeTyp)
        {
            var gameTree = new GameTree(this, startPos);
            if (gameTreeTyp)
            {
                return gameTree.buildIterativGameTree(startPos);
            }
            else
            {
                return gameTree.buildRecusiveGameTree(gameTree.getPossibleFinalStates());
            }
        }

        /// <summary>
        /// gibt alle möglichen Zustände durch nächsten Move zurück
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public List<Positions> getNextPossibleSteps(Positions pos)
        {
            List<Positions> result = [];

            if (pos.detectivesTurn) // entscheidet ob Detectives oder Thief einen Zug spielen
            {
                result.Add(pos);
                for (int i = 0; i < pos.detectives.Count; i++) // gehe jeden möglichen Move der Detectives durch
                {
                    result.Add(pos.clone().moveDetective(i));
                }
            }
            else
            {
                List<int> possibleMoves = getOutgoingVertex(pos.thief).Except(pos.detectives).ToList(); // mögliche Züge des Diebes

                if (possibleMoves.Count != 0) //prüft ob Züge möglich sind
                {
                    for (int i = 0; i < possibleMoves.Count(); i++) // geht jeden möglichen Zug
                    {
                        result.Add(pos.clone().moveThief(possibleMoves[i]));
                    }
                }
            }
            return result;
        }

        public List<Positions> getPreviousPossibleSteps(Positions pos) 
        {
            List <Positions> result = [];

            if (pos.detectivesTurn) // entscheidet ob Detectives oder Thief einen Zug spielen
            {
                foreach (var edge in this.Edges)
                {
                    if (edge.Target.Equals(pos.thief))
                    {
                        var previousPos = pos.clone();
                        previousPos.thief = edge.Source;
                        result.Add(previousPos);
                    }
                }
            }
            else
            {
                foreach (var edge in this.Edges)
                {

                }
                
            }
            return result;
        }

        // veraltet
        public bool IsEntanglement(int initalPos, int candidate)
        {
            Random rnd = new Random();
            int thiefPos = initalPos; // Dieb startet am Start
            List<int> detectivePos = new List<int>();

            for (int i = 0; i < candidate; i++) // es werden "candidate" viele Detectives hinzugefügt
            {
                detectivePos.Add(this.VertexCount + 1);
                // ihnen wird die startpos zugewiesen (Anzahl der Knoten+1 steht für noch nicht auf Graph)
            }

            var gameHistory = new List<(int, List<int>)>();

            while (true)
            {
                int randomIntInRange = rnd.Next(0, candidate); // Zufälliger Zug der Detectives
                if (randomIntInRange != candidate)
                {
                    detectivePos[randomIntInRange] = thiefPos; // Zug wird ausgeführt
                }

                List<int> possibleMoves = getOutgoingVertex(thiefPos).Except(detectivePos).ToList(); // mögliche Züge des Diebes

                if (possibleMoves.Count() == 0)
                {
                    return true; // es gibt keine möglichen Züge mehr
                }
                else
                {
                    int nextMove = rnd.Next(0, possibleMoves.Count() - 1); // zufällger Zug für den Dieb
                    thiefPos = possibleMoves[nextMove]; // Zug wird ausgeführt
                }

                if (!detectivePos.Exists(x => x == candidate))
                {
                    var sortedDPos = detectivePos.OrderByDescending(x => x).ToList();
                    (int, List<int>) currentState = (thiefPos, sortedDPos);

                    if (gameHistory.Exists(x => x.Item1 == thiefPos && x.Item2.SequenceEqual(sortedDPos))) // cycle detection um entlosschleife zu verhindern (noch nicht vollständig)
                    {
                        return false;
                    }

                    gameHistory.Add((thiefPos, sortedDPos));
                }


            }
        }



    }
}
