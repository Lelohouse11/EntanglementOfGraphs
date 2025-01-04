using EntaglementOfGraphs;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntanglementOfGraphs
{
    internal class UnaryFunctionGraph: FiniteDirectedGraph<int>
    {
        public delegate int UnaryFunction(int x);

        /// <summary>
        /// erstellt einen Graphen der durch eine unäre Funktion entsteht
        /// </summary>
        /// <param name="function"></param>
        /// <param name="startDomain"></param>
        /// <param name="endDomain"></param>
        public UnaryFunctionGraph(Func<int,int> function, int startDomain, int endDomain) 
        {
            AddVertex(function(startDomain));
            for (int i = startDomain+1; i <= endDomain; i++)
            {
                AddVertex(function(i));
                AddVertexToMsagl(function(i));
                AddEdge(new Edge<int>(function(i-1), function(i)));
                AddEdgeToMsagl(function(i-1), function(i));
            }

        }
    }
}
