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
        //var testGraph = new TorusGraph(1, 2);
        //var testGraphEnt = testGraph.isEntanglement(new Positions<int>(5,0,true));
        //Console.WriteLine(testGraph.createDot());
        //Console.WriteLine($"Ausgerechnetes Entamglement ist: {testGraphEnt}");


        
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                var testGraphEnt = new TorusGraph(i, j).minEntanglement(0);
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
