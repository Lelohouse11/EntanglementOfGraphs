using EntaglementOfGraphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntanglementOfGraphs
{
    internal class Move<V> where V : IComparable<V>, IEquatable<V>
    {
        public Positions<V> source;
        public List<Positions<V>> target;

        public Move(Positions<V> s, List<Positions<V>> t) 
        {
            source = s;
            target = t;
        }

        public Positions<V> GetSource() { return source; }

        public List<Positions<V>> GetTarget() {  return target; }
    }
}
