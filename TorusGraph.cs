using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    internal class TorusGraph : FiniteDirectedGraph<int>
    {
        public TorusGraph(int mTorus, int nTorus)
        {
            mTorus = mTorus + 1;
            nTorus = nTorus + 1;
            if (mTorus == 1)
            {
                AddVertex(0);
                AddEdge(new Edge<int>(0, 0));
            }
            else
            {
                AddVertex(0);
                for (int i = 1; i < mTorus; i++)
                {
                    AddVertex(i);
                    AddEdge(new Edge<int>(i - 1, i));
                }
            }
            if (nTorus == 1)
            {
                AddVertex(mTorus);
                AddEdge(new Edge<int>(mTorus, mTorus));
            }
            else
            {
                AddVertex(mTorus);
            }
            for (int j = 0; j < mTorus; j++)
            {
                AddEdge(new Edge<int>(j, mTorus)); // in beide Richtungen?
                AddEdge(new Edge<int>(mTorus, j)); // in beide Richtungen?

            }
            for (int i = mTorus + 1; i < nTorus + mTorus; i++)
            {
                AddVertex(i);
                AddEdge(new Edge<int>(i - 1, i));
                for (int j = 0; j < mTorus; j++)
                {
                    AddEdge(new Edge<int>(j, i)); // in beide Richtungen?
                    AddEdge(new Edge<int>(i, j)); // in beide Richtungen?

                }
            }

        }
    }
}
