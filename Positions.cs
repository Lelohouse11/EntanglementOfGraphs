using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    public class Positions <V> where V : IComparable<V>, IEquatable<V>
    {
        public V thief;
        public SortedSet<V> detectives = new();
        public bool detectivesTurn;
        public int detectiveAmount;

        /// <summary>
        /// erstellt Position im GameTree
        /// </summary>
        /// <param name="detectiveAmount"></param>
        /// <param name="initalThief"></param>
        /// <param name="initialTurn"></param>
        public Positions(int detectiveAmount, V initalThief, bool initialTurn) 
        {
            /*
            for (int i = 0; i < detectiveAmount; i++)
            {
                detectives.Add(-(i+1),-1);
            }
            */
            this.detectiveAmount = detectiveAmount;
            thief = initalThief;
            detectivesTurn = initialTurn;
        }

        /// <summary>
        /// clont Instanz zur weitere Verarbeitung
        /// </summary>
        /// <returns></returns>
        public Positions<V> clone()
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
        public Positions<V> moveDetective(V? detective)
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
        public Positions<V> moveThief(V newPos)
        {
            thief= newPos;
            return this;
        }

        public Positions<V> changeTurn()
        {
            detectivesTurn = !detectivesTurn;
            return this;
        }


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

        public String toString()
        {

            return $"({thief}, ({string.Join(",",detectives.ToArray())}), {detectivesTurn})";
        }
    }
}
