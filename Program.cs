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
        //var testGraphEnt = new FiniteDirectedGraph(1, 2).isEntanglement(new Positions(1,0,true));
        //Console.WriteLine(testGraphEnt.createDot());
        //Console.WriteLine($"Teste {1}x{2} Torrus Graph auf korrektes Entanglement: {testGraphEnt == 2}");
        //Console.WriteLine($"Ausgerechnetes Entamglement ist: {testGraphEnt}");


        
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                var testGraphEnt = new FiniteDirectedGraph(i, j).minEntanglement(0);
                if (i == j)
                {                    
                    Console.WriteLine($"Teste {i}x{j} Torrus Graph auf korrektes Entanglement: {testGraphEnt == i}");
                    Console.WriteLine($"Ausgerechnetes Entamglement ist: {testGraphEnt}, korrekt wäre: {i}");
                }
                else
                {
                    int temp = int.Min(i, j) +1;
                    Console.WriteLine($"Teste {i}x{j} Torrus Graph auf korrektes Entanglement: {testGraphEnt == temp}");
                    Console.WriteLine($"Ausgerechnetes Entamglement ist: {testGraphEnt}, korrekt wäre: {temp}");
                }
            }
        }
        
    }
}
