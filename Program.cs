using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EntaglementOfGraphs;
using QuikGraph;
using QuikGraph.Graphviz;
using static QuikGraph.Algorithms.Assignment.HungarianAlgorithm;

internal class Program
{
   

    /*
To-Dos:
- bricht zu früh ab
    - Positionen der Detectives stimmen nicht
- Kreislauferkennung einbauen

*/
    private static void Main(string[] args)
    {        
        var graph = new FiniteDirectedGraph([1,2,3], [(1,2),(2,3),(3,1)]);
        Console.WriteLine(graph.createDot());
        Console.WriteLine();

        Console.WriteLine(graph.minEntanglement(1));

        //var gameTree = graph.getGameTree(new Positions(1,1,true), false);
        //Console.WriteLine("Erstellen des Gametrees erfolgreich!");
        //Console.WriteLine($"Der Gametree hat {gameTree.vertexCounter} Knoten und {gameTree.edgeCounter} Kanten.");
        /*
        //Berechnung des Entanglements
        int temp = graph.VertexCount-1;
        while (IsEntanglement(graph, 1, temp))
        {
            temp--;
            Console.WriteLine($"Das Entanglement ist:{temp + 1}");
        }

        Console.WriteLine($"Das minimalste Entanglement ist:{temp+1}");
        */

    }
}
