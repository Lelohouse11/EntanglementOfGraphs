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
        public List<int> detectives = new List<int>();
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
                detectives.Add(-1);
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
            var temp = new Positions(0,0,true);
            temp.thief = thief;
            temp.detectivesTurn = detectivesTurn;
            temp.detectives = detectives.ToArray().ToList();
            return temp;
        }

        /// <summary>
        /// bewegt den Detektiv
        /// </summary>
        /// <param name="detective"></param>
        /// <returns></returns>
        public Positions moveDetective(int detective)
        {
            detectives[detective] = thief;
            return this;
        }

        public Positions setDetectivePos(int detective, int newPos) 
        {
            detectives[detective] = newPos;
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

        //int IComparable<Positions>.CompareTo(Positions? other)
        //{
        //    if (other.thief < thief) return 1;
        //    if (other.thief > thief) return -1;
        //    for (int i = 0;i < detectives.Count; i++)
        //    {
        //        if (other.detectives[i] < detectives[i]) return 1;
        //        if (other.detectives[i] > detectives[i]) return -1;
        //    }
        //    return 0;
        //}
        //public  int CompareTo(object? obj)
        //{
        //    return CompareTo(obj as Positions);
        //}

        public bool Equals(Positions other)
        {
            if (other.thief != thief) return false;
            if (other.detectivesTurn != detectivesTurn) return false;
            for (int i = 0; i < detectives.Count; i++)
            {
                if (other.detectives[i] != detectives[i]) return false;
            }
            return true;
        }

    }
}
