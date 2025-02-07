using ABI.System;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntaglementOfGraphs
{
    /// <summary>
    /// erstellt Position im GameStateGraph
    /// </summary>
    /// <param name="detectiveAmount"></param>
    /// <param name="initalThief"></param>
    /// <param name="initialTurn"></param>
    public class GameState <V>(int detectiveAmount, V initalThief, bool initialTurn) where V : IComparable<V>, IEquatable<V>
    {
        public V thiefPos = initalThief;
        public SortedSet<V> detectives = [];
        public bool detectivesTurn = initialTurn;
        public int detectiveAmount = detectiveAmount;
        public int distanceToWin;
        public double winningChance;
        public int possiblePreviousStepsCount;
        public bool savePathFound = false;

        /// <summary>
        /// clont Instanz zur weitere Verarbeitung
        /// </summary>
        /// <returns></returns>
        public GameState<V> Clone()
        {
            var cloned = new GameState<V>(detectiveAmount,thiefPos, detectivesTurn);
            //cloned.savePathFound = savePathFound;
            //cloned.winningChance = winningChance;
            //cloned.distanceToWin = distanceToWin;
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
        public GameState<V> MoveDetective(V? detective)
        {
            if (detective != null)
            {
                detectives.Remove(detective);
            }
            detectives.Add(thiefPos);
            return this;
        }

        /// <summary>
        /// bewegt den Dieb
        /// </summary>
        /// <param name="newPos"></param>
        /// <returns></returns>
        public GameState<V> MoveThief(V newPos)
        {
            thiefPos= newPos;
            return this;
        }

        /// <summary>
        /// ändert wer am Zug ist
        /// </summary>
        /// <returns></returns>
        public GameState<V> ChangeTurn()
        {
            detectivesTurn = !detectivesTurn;
            return this;
        }

        /// <summary>
        /// prüft zwei Positionen auf Gleichheit
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(GameState<V> other)
        {
            if (!other.thiefPos.Equals(thiefPos)) return false;
            if (other.detectivesTurn != detectivesTurn) return false;
            if (other.detectives.Count != detectives.Count) return false;
            for (int i = 0; i < detectives.Count; i++)
            {
                if (!other.detectives.ElementAt(i).Equals(detectives.ElementAt(i))) return false;
            }
            return true;
        }

        /// <summary>
        /// gibt den Detektive zurück der Position geändert hat
        /// </summary>
        /// <param name="nextState"></param>
        /// <returns></returns>
        public V getMovedDetective (GameState<V> nextState)
        {
            for (int i = 0; i < detectives.Count; i++)
            {
                if (!nextState.detectives.ElementAt(i).Equals(detectives.ElementAt(i))) return detectives.ElementAt(i);
            }
            return default;
        }


        /// <summary>
        /// gibt eine passenden String zur Position zurück
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {

            return $"({thiefPos}, ({string.Join(",",detectives.ToArray())}), {detectivesTurn})";
        }
    }
}
