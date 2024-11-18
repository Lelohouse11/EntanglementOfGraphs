using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    public struct TorusVertex : IComparable<TorusVertex>, IEquatable<TorusVertex>
    {
        public int ErsterWert { get; set; }

        public int ZweiterWert { get; set; }

        public TorusVertex(int ersterWert, int zweiterWert)
        {
            ErsterWert = ersterWert;
            ZweiterWert = zweiterWert;
        }

        public int CompareTo(TorusVertex other)
        {
            int result = ErsterWert.CompareTo(other.ErsterWert);
            if (result == 0)
            {
                return ZweiterWert.CompareTo(other.ZweiterWert);
            }
            return result;

        }

        public bool Equals(TorusVertex other)
        {
            return ErsterWert == other.ErsterWert && ZweiterWert == other.ZweiterWert;            
        }

        public string toString()
        {
            return $"({ErsterWert},{ZweiterWert})";
        }
    }
}
        
