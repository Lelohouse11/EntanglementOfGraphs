using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    /// <summary>
    /// Sturktur für Torusknoten (x_Label,x_Label)
    /// </summary>
    public struct TorusVertex(int ersterWert, int zweiterWert) : IEquatable<TorusVertex>
    {
        public int ErsterWert { get; set; } = ersterWert;

        public int ZweiterWert { get; set; } = zweiterWert;

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

        public override string ToString()
        {
            return $"({ErsterWert},{ZweiterWert})";
        }
    }
}
        
