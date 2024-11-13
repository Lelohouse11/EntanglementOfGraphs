using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    public class Positions 
    {
        public int thief;
        public SortedList<int,int> detectives = new();
        public bool detectivesTurn;

        /// <summary>
        /// erstellt Position im GameTree
        /// </summary>
        /// <param name="detectiveAmount"></param>
        /// <param name="initalThief"></param>
        /// <param name="initialTurn"></param>
        public Positions(int detectiveAmount, int initalThief, bool initialTurn) 
        {
            for (int i = 0; i < detectiveAmount; i++)
            {
                detectives.Add(-1,-1);
            }
            thief = initalThief;
            detectivesTurn = initialTurn;
        }

        /// <summary>
        /// clont Instanz zur weitere Verarbeitung
        /// </summary>
        /// <returns></returns>
        public Positions clone()
        {
            var cloned = new Positions(0,0,true);
            cloned.thief = thief;
            cloned.detectivesTurn = detectivesTurn;
            foreach (var i in detectives)
            {
                cloned.detectives.Add(i.Value,i.Value);
            }
            return cloned;
        }

        /// <summary>
        /// bewegt den Detektiv
        /// </summary>
        /// <param name="detective"></param>
        /// <returns></returns>
        public Positions moveDetective(int detective)
        {
            detectives.RemoveAt(detective);
            detectives.Add(thief, thief);
            return this;
        }

        /// <summary>
        /// bewegt den Dieb
        /// </summary>
        /// <param name="newPos"></param>
        /// <returns></returns>
        public Positions moveThief(int newPos)
        {
            thief= newPos;
            return this;
        }

        public Positions changeTurn()
        {
            detectivesTurn = !detectivesTurn;
            return this;
        }


        public bool Equals(Positions other)
        {
            if (other.thief != thief) return false;
            if (other.detectivesTurn != detectivesTurn) return false;
            for (int i = 0; i < detectives.Count; i++)
            {
                if (other.detectives.Values[i] != detectives.Values[i]) return false;
            }
            return true;
        }

        public String toString()
        {

            return $"({thief}, ({string.Join(",",detectives.Values.ToArray())}), {detectivesTurn})";
        }
    }
}
