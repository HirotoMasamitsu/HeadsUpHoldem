using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEMP
{
    public class Odds
    {
        private readonly int[] DEFAULT_ODDS = new int[] { 500, 100, 25, 10, 8, 5, 3, 2, 1, 0 };

        private bool useJacksOrBetter;
        private int[] odds;

        public Odds(bool useJacksOrBetter, int[] odds = null)
        {
            this.useJacksOrBetter = useJacksOrBetter;
            this.odds = new int[10];
            Array.Copy(DEFAULT_ODDS, this.odds, DEFAULT_ODDS.Length);
            if (odds != null)
            {
                Array.Copy(odds, this.odds, Math.Min(odds.Length, this.odds.Length));
            }
        }

        public bool UseJacksOrBetter
        {
            get { return this.useJacksOrBetter; }
        }

        public int GetWinScore(PokerResults result, int betScore)
        {
            var ret = 0;
            if (result != null)
            {
                switch(result.Rank)
                {
                    case PKHands.ERROR:
                        break;
                    case PKHands.OnePair:
                        if (this.useJacksOrBetter)
                        {
                            if (result.NumCount != null && result.NumCount.Length > 0 && (result.NumCount[0] == 1 || result.NumCount[0] >= 11))
                            {
                                ret = betScore * this.odds[(int)result.Rank];
                            }
                        }
                        else
                        {
                            ret = betScore * this.odds[(int)result.Rank];
                        }
                        break;
                    default:
                        ret = betScore * this.odds[(int)result.Rank];
                        break;
                }
            }
            return ret;
        }


    }
}