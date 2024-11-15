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
- ixi richtig aber ixj immer ein zu wenig
- fixpoint itteration impl.
- weitere Klasse an grahen zum testen finden
- was passiert wenn Torus graph x torrus Graph

*/
    private static void Main(string[] args)
    {
        for (int i = 1; i <= 5; i++)
        {
            for (int j = 1; j <= 5; j++)
            {
                var testGraphEnt = new FiniteDirectedGraph(i, j).minEntanglement(0);
                if (i == j)
                {
                    Console.WriteLine($"Teste {i}x{j} Torrus Graph auf korrektes Entanglement: {testGraphEnt == i}");
                    Console.WriteLine($"Ausgerechnetes Entamglement ist: {testGraphEnt}, korrekt wäre: {i}");
                }
                else
                {
                    Console.WriteLine($"Teste {i}x{j} Torrus Graph auf korrektes Entanglement: {testGraphEnt == (int.Min(i, j) + 1)}");
                    Console.WriteLine($"Ausgerechnetes Entamglement ist: {testGraphEnt}, korrekt wäre: {int.Min(i, j) + 1}");
                }
            }
        }
    }
}
