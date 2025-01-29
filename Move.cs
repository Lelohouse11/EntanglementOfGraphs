using EntaglementOfGraphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntanglementOfGraphs
{
    /// <summary>
    /// Klasse für die Strategie
    /// </summary>
    /// <typeparam name="V"></typeparam>
    /// <param name="s"></param>
    /// <param name="t"></param>
    internal class Move<V>(GameState<V> s, List<GameState<V>> t) where V : IComparable<V>, IEquatable<V>
    {       
        public GameState<V> source = s; // aktueller Status des Spiels
        public List<GameState<V>> target = t; // mögliche Züge
    }
}
