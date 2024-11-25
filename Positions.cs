using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    /// <summary>
    /// erstellt Position im GameTree
    /// </summary>
    /// <param name="detectiveAmount"></param>
    /// <param name="initalThief"></param>
    /// <param name="initialTurn"></param>
    public class Positions <V>(int detectiveAmount, V initalThief, bool initialTurn) where V : IComparable<V>, IEquatable<V>
    {
        public V thief = initalThief;
        public SortedSet<V> detectives = [];
        public bool detectivesTurn = initialTurn;
        public int detectiveAmount = detectiveAmount;

        /// <summary>
        /// clont Instanz zur weitere Verarbeitung
        /// </summary>
        /// <returns></returns>
        public Positions<V> Clone()
        {
            var cloned = new Positions<V>(detectiveAmount,thief, detectivesTurn);
            foreach (var i in detectives)
            {
                cloned.detectives.Add(i);
            }
            return cloned;
        }

        /// <summary>
        /// bewegt den Detektiv
        /// </summary>
        /// <param name="detective"></param>
        /// <returns></returns>
        public Positions<V> MoveDetective(V? detective)
        {
            if (detective != null)
            {
                detectives.Remove(detective);
            }
            detectives.Add(thief);
            return this;
        }

        /// <summary>
        /// bewegt den Dieb
        /// </summary>
        /// <param name="newPos"></param>
        /// <returns></returns>
        public Positions<V> MoveThief(V newPos)
        {
            thief= newPos;
            return this;
        }

        /// <summary>
        /// ändert wer am Zug ist
        /// </summary>
        /// <returns></returns>
        public Positions<V> ChangeTurn()
        {
            detectivesTurn = !detectivesTurn;
            return this;
        }

        /// <summary>
        /// prüft zwei Positionen auf Gleichheit
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Positions<V> other)
        {
            if (!other.thief.Equals(thief)) return false;
            if (other.detectivesTurn != detectivesTurn) return false;
            if (other.detectives.Count != detectives.Count) return false;
            for (int i = 0; i < detectives.Count; i++)
            {
                if (!other.detectives.ElementAt(i).Equals(detectives.ElementAt(i))) return false;
            }
            return true;
        }

        /// <summary>
        /// gibt eine passenden String zur Position zurück
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {

            return $"({thief}, ({string.Join(",",detectives.ToArray())}), {detectivesTurn})";
        }
    }
}
