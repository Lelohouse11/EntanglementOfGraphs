using QuikGraph;
using QuikGraph.Graphviz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    // Testing durch Torus grapgen seite 2
    internal class GameTree<V> : AdjacencyGraph<Positions<V>, Edge<Positions<V>>> where V : IComparable<V>, IEquatable<V>
    {
        bool debug = true;

        public int vertexCounter = 1;
        public int edgeCounter = 0;
        public readonly int detectiveAmount;
        public readonly FiniteDirectedGraph<V> graph;
        public readonly Positions<V> startPosition;

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
                Console.WriteLine($"Startknoten hinzugefügt: {startPos.toString()}");
            }
        }

        /// <summary>
        /// erstellt String von .dot Datei für spätere Visualisierung
        /// </summary>
        /// <returns></returns>
        public String createDot()
        {
            var graphviz = new GraphvizAlgorithm<Positions<V>, Edge<Positions<V>>>(this);
            return graphviz.Generate();
        }

        /// <summary>
        /// verbindet alle gefundenen Iterationen des Gametrees zu GameTree
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public GameTree<V> buildIterativGameTree(Positions<V> pos)
        {
            var nextStep = graph.getNextPossibleSteps(pos);
            if (nextStep.Count() != 0)
            { //prüft ob keine weiteren Knoten gefunden wurden
                foreach (var nextPos in nextStep) // fügt gefunden Knoten hinzu und verbindet sie
                {
                    var isNewPos = getExistingPosition(nextPos);
                    if (isNewPos == null) // prüft ob nextPos neu ist
                    {
                        AddVertex(nextPos);
                        vertexCounter++;
                        if (debug)
                        {
                            Console.WriteLine($"Knoten hinzugefügt: {nextPos.toString()}");
                        }
                    }

                    var targetPos = isNewPos ?? nextPos; // wenn nextPos nicht neu, alte vorhandene Pos benutzen
                    AddEdge(new Edge<Positions<V>>(pos, targetPos));
                    edgeCounter++;
                    if (debug)
                    {
                        Console.WriteLine($"Kante von {pos.toString()} zu {targetPos.toString()} hinzugefügt.");
                    }
                    if (isNewPos == null || pos.detectivesTurn) // Wenn NextPos neu oder Detektive sich nicht bewegen
                    {
                        buildIterativGameTree(targetPos); //rekursiver Aufruf
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
        public void buildRecursiveGameTree(Positions<V> currentPos)
        {
            var previousPossibleSteps = graph.getPreviousPossibleSteps(currentPos);
            foreach (var previousPos in previousPossibleSteps) // gehe die vorig möglichen Spielzustände durch
            {
                bool addingPreviousPos = true;
                if (!previousPos.detectivesTurn) //wenn der Deib dran ist muss jeder möglicher Zug schon im GameTree gespeichert sein
                {
                    var nextPossibleSteps = graph.getNextPossibleSteps(previousPos);
                    foreach (var vertex in nextPossibleSteps)
                    {
                        if (debug)
                        {
                            Console.WriteLine($"Position {previousPos.toString()} führt zu {vertex.toString()}");
                        }
                        if (!containsPosition(vertex))
                        {
                            if (debug)
                            {
                                Console.WriteLine($"Position {previousPos.toString()} führt nicht sicher zu sieg");
                            }
                            addingPreviousPos = false;
                            continue;
                        }
                    }
                }

                if (addingPreviousPos)
                {
                    var isExistingPos = getExistingPosition(previousPos);
                    if (isExistingPos == null) // prüft ob previousPos neu ist
                    {
                        AddVertex(previousPos);
                        vertexCounter++;
                        if (debug)
                        {
                            Console.WriteLine($"Knoten hinzugefügt: {previousPos.toString()}");
                        }
                    }

                    var sourcePos = isExistingPos ?? previousPos; // wenn nextPos nicht neu, alte vorhandene Pos benutzen
                    AddEdge(new Edge<Positions<V>>(sourcePos, currentPos));
                    edgeCounter++;
                    if (debug)
                    {
                        Console.WriteLine($"Kante von {sourcePos.toString()} zu {currentPos.toString()} hinzugefügt.");
                    }
                    if (isExistingPos == null) // Wenn NextPos neu
                    {
                        buildRecursiveGameTree(sourcePos); //rekursiver Aufruf
                    }
                }
            }
        }

        /// <summary>
        /// baut den GameTree durch eine Fixpointiteration auf
        /// </summary>
        public void buildFixpointGameTree()
        {
            var possibleConnections = new List<(Positions<V>, Positions<V>)>();
            var temp = new List<Positions<V>>();
            bool continueFixpoint = false;
            var allVertices = Vertices.ToList();
            foreach (var currentPos in allVertices)
            {
                if (!currentPos.detectivesTurn) //wenn detektiv vorher dran war wird alles hinzugefügt
                {
                    foreach (var previousPos in graph.getPreviousPossibleSteps(currentPos)) // alle Knoten
                    {
                        var isNewVertex = getExistingPosition(previousPos); //checken ob schon vorhandener Knoten
                        if (isNewVertex == null)
                        {
                            AddVertex(previousPos); // Hinzufügen, wenn neu
                            if (debug)
                            {
                                Console.WriteLine($"Knoten hinzugefügt: {previousPos.toString()}");
                            }
                            continueFixpoint = true;
                        }
                        var sourcePos = isNewVertex ?? previousPos;
                        AddEdge(new Edge<Positions<V>>(sourcePos, currentPos)); // Kante hinzufügen
                        if (debug)
                        {
                            Console.WriteLine($"Kante von {sourcePos.toString()} zu {currentPos.toString()} hinzugefügt.");
                        }
                    }
                }
                else
                {
                    foreach (var previousPos in graph.getPreviousPossibleSteps(currentPos)) // alle Knoten
                    {
                        var isNewVertex = getExistingPosition(previousPos); //checken ob schon vorhandener Knoten
                        if (isNewVertex == null)
                        {
                            possibleConnections.Add((previousPos, currentPos)); // Hinzufügen, wenn neu
                            temp.Add(previousPos);
                            continueFixpoint = true;
                        }
                    }
                }
            }
            if (continueFixpoint) // erste verbindungen hinzufügen, die inerhalb von possibleConnections noch bestehen,
                                  // dann gucken ob alle next possible steps in GameTree oder Temp sind.
                                  // Wenn ja drin lassen
                                  // Wenn nein, aus liste löschen und alle vorgänger die in possibleConnections sind auch
                                  // Wenn mit oben fertig, dann alle von possibleConnections in GameTree einfügen
            {
                continueFixpoint = false ;
                bool winningPreviousPos = true;
                Console.WriteLine("-----While-Schleife betreten!-----");
                while (winningPreviousPos) // Iteriert solange durch die Liste, bis keine Änderungen mehr vorgenohmen werden, da dann alle Konten sicher gewinnen
                {
                    var temp2 = temp.ToArray().ToList(); // Liste zum Überprüfen der Änderung
                    foreach (var previousPos in temp)
                    {
                        foreach (var targetVertex in graph.getNextPossibleSteps(previousPos)) // checkt, ob alle ausgehenden Kanten wieder in den GameTree führen
                        {
                            if (!(containsPosition(targetVertex) || temp.Contains(targetVertex)))
                            {
                                temp.Remove(targetVertex);
                            }
                        }                        
                    }
                    foreach (var tempItem in temp)
                    {
                        foreach (var temp2Item in temp2)
                        {
                            if (tempItem.Equals(temp2Item))
                            {
                                winningPreviousPos = false;
                                Console.WriteLine("-----Keine Änderung mehr Festgestellt, Schleife wird verlassen!-----");
                                continue;
                            }
                        }
                    }                    
                }
                foreach (var vertex in temp) // fügt alle sicheren Knoten hinzu
                {
                    AddVertex(vertex);
                    if (debug)
                    {
                        Console.WriteLine($"Knoten hinzugefügt: {vertex.toString()}");
                    }
                    continueFixpoint = true;
                }
                foreach(var vertex in temp) //fügt zu den Knoten alle Edges hinzu
                {
                    foreach(var targetVertex in graph.getNextPossibleSteps(vertex))
                    {
                        AddEdge(new Edge<Positions<V>>(vertex, targetVertex));
                        if (debug)
                        {
                            Console.WriteLine($"Kante von {vertex.toString()} zu {targetVertex.toString()} hinzugefügt.");
                        }
                    }
                }
            }
            if (continueFixpoint)
            {
                buildFixpointGameTree();
            }
            else 
            {
                Console.WriteLine("----------Keine neuen Knoten mehr gefunden!----------");
            }
        }

    


        /// <summary>
        /// prüft ob gegebener Knoten schon im GameTree vorhanden ist
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool containsPosition(Positions<V> pos)
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
        public Positions<V>? getExistingPosition(Positions<V> pos)
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
        public List<Positions<V>> getPossibleFinalStates()
        {
            var result = new List<Positions<V>>();
            foreach (var vertex in graph.Vertices) //geht jeden Knoten des Graphen durch
            {
                var outgoingVertices = graph.getOutgoingVertex(vertex).Distinct().ToList();
                var outgoingVerticesCount = outgoingVertices.Count;
                if (outgoingVerticesCount <= detectiveAmount) // prüft ob Fluchtmöglichkeiten von Detektiven blockiert werden können
                {
                    var tempState = new Positions<V>(detectiveAmount, vertex, false);
                    for (var i = 0; i < outgoingVerticesCount; i++)
                    {
                        tempState.detectives.Add(outgoingVertices[i]);// setzt Detektive auf die Fluchtmöglichkeit
                    }
                    for (var i = outgoingVerticesCount; i < detectiveAmount; i++)
                    {
                        var emptyPos = graph.Vertices.Except(outgoingVertices).ToList();
                        tempState.detectives.Add(emptyPos[i]);
                    }
                    if (tempState.detectives.Count == outgoingVerticesCount)
                    {
                        result.Add(tempState);
                    }
                    else
                    {
                        addAllDetektives(tempState, outgoingVertices, result);
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
        private void addAllDetektives(Positions<V> pos, List<V> blockedVertices, List<Positions<V>> result)
        {
            var temp = blockedVertices.ToArray().ToList();   
            foreach (var vertex in graph.Vertices.Except(blockedVertices)) //Konten die noch frei sind
            {               
                var finalState = pos.clone();
                finalState.detectives.Add(vertex);
                blockedVertices.Add(vertex);
                if (blockedVertices.Count == detectiveAmount)
                {
                    result.Add(finalState);
                }
                else
                {
                    addAllDetektives(finalState, blockedVertices, result);
                }
                blockedVertices = temp;
            }
        }
    }
}
