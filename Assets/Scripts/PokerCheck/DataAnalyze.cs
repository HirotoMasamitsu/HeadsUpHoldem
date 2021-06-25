using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace TEMP
{
    public class DataAnalyze
    {
        private int highScore;
        private int totalPlay;
        private bool useJacksOrBetter;
        private Dictionary<string, int> resultCountDic;

        public DataAnalyze(bool useJacksOrBetter)
        {
            this.highScore = 0;
            this.totalPlay = 0;
            this.resultCountDic = new Dictionary<string, int>();
            this.resultCountDic[PKHands.RoyalStraightFlush.ToString()] = 0;
            this.resultCountDic[PKHands.StraightFlush.ToString()] = 0;
            this.resultCountDic[PKHands.FourCards.ToString()] = 0;
            this.resultCountDic[PKHands.FullHouse.ToString()] = 0;
            this.resultCountDic[PKHands.Flush.ToString()] = 0;
            this.resultCountDic[PKHands.Straight.ToString()] = 0;
            this.resultCountDic[PKHands.ThreeCards.ToString()] = 0;
            this.resultCountDic[PKHands.TwoPair.ToString()] = 0;
            this.useJacksOrBetter = useJacksOrBetter;
            if (useJacksOrBetter)
            {
                this.resultCountDic[PKHands.OnePair.ToString() + "(Jacks or Better)"] = 0;
                this.resultCountDic[PKHands.OnePair.ToString() + "(Tens or Lesser)"] = 0;
            }
            else
            {
                this.resultCountDic[PKHands.OnePair.ToString()] = 0;
            }
            this.resultCountDic[PKHands.HighCard.ToString()] = 0;
        }

        public void Load()
        {
            var filePath = Path.Combine(Application.persistentDataPath, "scoresave.dat");
            Debug.Log(filePath);
            if (File.Exists(filePath))
            {
                try
                {
                    using (var fs = new FileStream(filePath, FileMode.Open))
                    using (var sr = new StreamReader(fs, Encoding.UTF8))
                    {
                        this.highScore = int.Parse(sr.ReadLine());
                        this.totalPlay = int.Parse(sr.ReadLine());
                        this.resultCountDic[PKHands.RoyalStraightFlush.ToString()] = int.Parse(sr.ReadLine());
                        this.resultCountDic[PKHands.StraightFlush.ToString()] = int.Parse(sr.ReadLine());
                        this.resultCountDic[PKHands.FourCards.ToString()] = int.Parse(sr.ReadLine());
                        this.resultCountDic[PKHands.FullHouse.ToString()] = int.Parse(sr.ReadLine());
                        this.resultCountDic[PKHands.Flush.ToString()] = int.Parse(sr.ReadLine());
                        this.resultCountDic[PKHands.Straight.ToString()] = int.Parse(sr.ReadLine());
                        this.resultCountDic[PKHands.ThreeCards.ToString()] = int.Parse(sr.ReadLine());
                        this.resultCountDic[PKHands.TwoPair.ToString()] = int.Parse(sr.ReadLine());
                        if (this.useJacksOrBetter)
                        {
                            this.resultCountDic[PKHands.OnePair.ToString() + "(Jacks or Better)"] = int.Parse(sr.ReadLine());
                            this.resultCountDic[PKHands.OnePair.ToString() + "(Tens or Lesser)"] = int.Parse(sr.ReadLine());
                        }
                        else
                        {
                            this.resultCountDic[PKHands.OnePair.ToString()] = int.Parse(sr.ReadLine());
                        }
                        this.resultCountDic[PKHands.HighCard.ToString()] = int.Parse(sr.ReadLine());
                    }
                }
                catch (System.Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }

        public void Save()
        {
            var filePath = Path.Combine(Application.persistentDataPath, "scoresave.dat");
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Create))
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine(this.highScore);
                    sw.WriteLine(this.totalPlay);
                    sw.WriteLine(this.resultCountDic[PKHands.RoyalStraightFlush.ToString()]);
                    sw.WriteLine(this.resultCountDic[PKHands.StraightFlush.ToString()]);
                    sw.WriteLine(this.resultCountDic[PKHands.FourCards.ToString()]);
                    sw.WriteLine(this.resultCountDic[PKHands.FullHouse.ToString()]);
                    sw.WriteLine(this.resultCountDic[PKHands.Flush.ToString()]);
                    sw.WriteLine(this.resultCountDic[PKHands.Straight.ToString()]);
                    sw.WriteLine(this.resultCountDic[PKHands.ThreeCards.ToString()]);
                    sw.WriteLine(this.resultCountDic[PKHands.TwoPair.ToString()]);
                    if (this.useJacksOrBetter)
                    {
                        sw.WriteLine(this.resultCountDic[PKHands.OnePair.ToString() + "(Jacks or Better)"]);
                        sw.WriteLine(this.resultCountDic[PKHands.OnePair.ToString() + "(Tens or Lesser)"]);
                    }
                    else
                    {
                        sw.WriteLine(this.resultCountDic[PKHands.OnePair.ToString()]);
                    }
                    sw.WriteLine(this.resultCountDic[PKHands.HighCard.ToString()]);
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public void Update(int score, PokerResults result)
        {
            if (score > this.highScore)
            {
                this.highScore = score;
            }
            totalPlay += 1;
            switch (result.Rank)
            {
                case PKHands.ERROR:
                    break;
                case PKHands.OnePair:
                    if (this.useJacksOrBetter)
                    {
                        if (result.IsJacksOrBetter)
                        {
                            this.resultCountDic[PKHands.OnePair.ToString() + "(Jacks or Better)"] += 1;
                        }
                        else
                        {
                            this.resultCountDic[PKHands.OnePair.ToString() + "(Tens or Lesser)"] += 1;
                        }
                    }
                    else
                    {
                        this.resultCountDic[result.Rank.ToString()] += 1;
                    }
                    break;
                default:
                    this.resultCountDic[result.Rank.ToString()] += 1;
                    break;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Total Play Count: {0}", this.totalPlay));
            sb.AppendLine(string.Format("High Score: {0}", this.highScore));
            sb.AppendLine();
            sb.AppendLine("- Hand Results -");
            sb.AppendLine(string.Format(" {0}: {1}", PKHands.RoyalStraightFlush, this.resultCountDic[PKHands.RoyalStraightFlush.ToString()]));
            sb.AppendLine(string.Format(" {0}: {1}", PKHands.StraightFlush, this.resultCountDic[PKHands.StraightFlush.ToString()]));
            sb.AppendLine(string.Format(" {0}: {1}", PKHands.FourCards, this.resultCountDic[PKHands.FourCards.ToString()]));
            sb.AppendLine(string.Format(" {0}: {1}", PKHands.FullHouse, this.resultCountDic[PKHands.FullHouse.ToString()]));
            sb.AppendLine(string.Format(" {0}: {1}", PKHands.Flush, this.resultCountDic[PKHands.Flush.ToString()]));
            sb.AppendLine(string.Format(" {0}: {1}", PKHands.Straight, this.resultCountDic[PKHands.Straight.ToString()]));
            sb.AppendLine(string.Format(" {0}: {1}", PKHands.ThreeCards, this.resultCountDic[PKHands.ThreeCards.ToString()]));
            sb.AppendLine(string.Format(" {0}: {1}", PKHands.TwoPair, this.resultCountDic[PKHands.TwoPair.ToString()]));
            if (this.useJacksOrBetter)
            {
                sb.AppendLine(string.Format(" {0} (Jacks or Better): {1}", PKHands.OnePair, this.resultCountDic[PKHands.OnePair.ToString() + "(Jacks or Better)"]));
                sb.AppendLine(string.Format(" {0} (Tens or Lesser): {1}", PKHands.OnePair, this.resultCountDic[PKHands.OnePair.ToString() + "(Tens or Lesser)"]));
            }
            else
            {
                sb.AppendLine(string.Format(" {0}: {1}", PKHands.OnePair, this.resultCountDic[PKHands.OnePair.ToString()]));
            }
            sb.AppendLine(string.Format(" {0}: {1}", PKHands.HighCard, this.resultCountDic[PKHands.HighCard.ToString()]));
            return sb.ToString();
        }
    }
}