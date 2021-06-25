using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEMP
{
    /// <summary>
    /// トランプの役
    /// </summary>
    public enum PKHands
    {
        /// <summary>
        /// ロイヤルストレートフラッシュ
        /// </summary>
        RoyalStraightFlush = 0,
        /// <summary>
        /// ストレートフラッシュ
        /// </summary>
        StraightFlush = 1,
        /// <summary>
        /// フォーカード
        /// </summary>
        FourCards = 2,
        /// <summary>
        /// フルハウス
        /// </summary>
        FullHouse = 3,
        /// <summary>
        /// フラッシュ
        /// </summary>
        Flush = 4,
        /// <summary>
        /// ストレート
        /// </summary>
        Straight = 5,
        /// <summary>
        /// スリーカード
        /// </summary>
        ThreeCards = 6,
        /// <summary>
        /// ツーペア
        /// </summary>
        TwoPair = 7,
        /// <summary>
        /// ワンペア
        /// </summary>
        OnePair = 8,
        /// <summary>
        /// ノーペア
        /// </summary>
        HighCard = 9,
        /// <summary>
        /// その他判定エラー
        /// </summary>
        ERROR = 99
    }

    /// <summary>
    /// ポーカーの役判定を返すクラス
    /// </summary>
    public class PokerResults : IComparable<PokerResults>
    {
        /// <summary>
        /// 役ランク
        /// </summary>
        public PKHands Rank { get; private set; }
        /// <summary>
        /// 番号カウント
        /// </summary>
        public int[] NumCount { get; private set; }

        public PokerResults(PKHands rank, int[] numCount)
        {
            this.Rank = rank;
            this.NumCount = numCount;
        }

        public int CompareTo(PokerResults other)
        {
            var ret = 0;
            if (other != null)
            {
                var rankComp = this.Rank.CompareTo(other.Rank);
                if (rankComp != 0)
                {
                    ret = rankComp * -1;
                } 
                else if (this.NumCount != null && other.NumCount != null)
                {
                    for (var i = 0; i < Math.Min(this.NumCount.Length, other.NumCount.Length); i++)
                    {
                        var comp = this.NumCount[i].CompareTo(other.NumCount[i]);
                        if (comp != 0)
                        {
                            if (this.NumCount[i] == 1)
                            {
                                ret = 1;
                            }
                            else if (other.NumCount[i] == 1)
                            {
                                ret = -1;
                            }
                            else
                            {
                                ret = comp;
                            }
                            break;
                        }
                    }
                }
            }
            return ret;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(this.Rank);
            if (this.NumCount != null)
            {
                sb.Append(" [");
                for (var i = 0; i < this.NumCount.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(this.NumCount[i]);
                }
                sb.Append("]");
            }
            return sb.ToString();
        }

        public bool IsJacksOrBetter
        {
            get
            {
                return (this.Rank == PKHands.OnePair && this.NumCount != null && this.NumCount.Length > 0 && (this.NumCount[0] == 1 || this.NumCount[0] >= 11));
            }
        }
    }

    /// <summary>
    /// ポーカーの役をチェックする静的クラス
    /// </summary>
    public static class PKCheck
    {
        /// <summary>
        /// 7枚のカードから5枚を選出する組み合わせを定義した配列
        /// </summary>
        private static readonly int[][] SEVEN_HANDS = new int[][]{
                                     new int[]{ 0, 1, 2, 3, 4 },
                                     new int[]{ 0, 1, 2, 3, 5 },
                                     new int[]{ 0, 1, 2, 3, 6 },
                                     new int[]{ 0, 1, 2, 4, 5 },
                                     new int[]{ 0, 1, 2, 4, 6 },
                                     new int[]{ 0, 1, 2, 5, 6 },
                                     new int[]{ 0, 1, 3, 4, 5 },
                                     new int[]{ 0, 1, 3, 4, 6 },
                                     new int[]{ 0, 1, 3, 5, 6 },
                                     new int[]{ 0, 1, 4, 5, 6 },
                                     new int[]{ 0, 2, 3, 4, 5 },
                                     new int[]{ 0, 2, 3, 4, 6 },
                                     new int[]{ 0, 2, 3, 5, 6 },
                                     new int[]{ 0, 2, 4, 5, 6 },
                                     new int[]{ 0, 3, 4, 5, 6 },
                                     new int[]{ 1, 2, 3, 4, 5 },
                                     new int[]{ 1, 2, 3, 4, 6 },
                                     new int[]{ 1, 2, 3, 5, 6 },
                                     new int[]{ 1, 2, 4, 5, 6 },
                                     new int[]{ 1, 3, 4, 5, 6 },
                                     new int[]{ 2, 3, 4, 5, 6 }
        };

        /// <summary>
        /// 6枚のカードから5枚を選出する組み合わせを定義した配列
        /// </summary>
        private static readonly int[][] SIX_HANDS = new int[][]{
                                     new int[]{ 0, 1, 2, 3, 4 },
                                     new int[]{ 0, 1, 2, 3, 5 },
                                     new int[]{ 0, 1, 2, 4, 5 },
                                     new int[]{ 0, 1, 3, 4, 5 },
                                     new int[]{ 0, 2, 3, 4, 5 },
                                     new int[]{ 1, 2, 3, 4, 5 }
        };


        /// <summary>
        /// 7枚のカードから5枚を選び出した時の最も強い役を取得する静的メソッド
        /// </summary>
        /// <param name="cards">カードの配列</param>
        /// <returns>成立する最強の役</returns>
        public static PokerResults CheckSevenHands(Card[] cards)
        {
            // 初期値
            PokerResults ret = null;
            // 7枚のカードが渡されていることを確認する
            if (cards != null && cards.Length == 7)
            {
                // 7枚から5枚選出のすべての組み合わせを試す
                for (int i = 0; i < SEVEN_HANDS.Length; i++)
                {
                    int[] handPattern = SEVEN_HANDS[i];
                    // 5枚を選出する
                    List<Card> handCards = new List<Card>();
                    for (int j = 0; j < handPattern.Length; j++)
                    {
                        handCards.Add(cards[handPattern[j]]);
                    }
                    // 選出した5枚で成立する役を取得
                    var handResult = CheckHands(handCards.ToArray());
                    // すでに取得した役より強ければ更新
                    if (handResult != null)
                    {
                        if (ret == null)
                        {
                            ret = handResult;
                        } 
                        else if (handResult.CompareTo(ret) > 0)
                        {
                            ret = handResult;
                        }
                    }
                }
            }
            // 取得した役を返す
            return ret;
        }

        /// <summary>
        /// 6枚のカードから5枚を選び出した時の最も強い役を取得する静的メソッド
        /// </summary>
        /// <param name="cards">カードの配列</param>
        /// <returns>成立する最強の役</returns>
        public static PokerResults CheckSixHands(Card[] cards)
        {
            // 初期値
            PokerResults ret = null;
            // 7枚のカードが渡されていることを確認する
            if (cards != null && cards.Length == 6)
            {
                // 7枚から5枚選出のすべての組み合わせを試す
                for (int i = 0; i < SIX_HANDS.Length; i++)
                {
                    int[] handPattern = SIX_HANDS[i];
                    // 5枚を選出する
                    List<Card> handCards = new List<Card>();
                    for (int j = 0; j < handPattern.Length; j++)
                    {
                        handCards.Add(cards[handPattern[j]]);
                    }
                    // 選出した5枚で成立する役を取得
                    var handResult = CheckHands(handCards.ToArray());
                    // すでに取得した役より強ければ更新
                    if (handResult != null)
                    {
                        if (ret == null)
                        {
                            ret = handResult;
                        }
                        else if (handResult.CompareTo(ret) > 0)
                        {
                            ret = handResult;
                        }
                    }
                }
            }
            // 取得した役を返す
            return ret;
        }

        /// <summary>
        /// 5枚の手札のポーカー役を取得する静的メソッド
        /// </summary>
        /// <param name="cards">カードの配列</param>
        /// <returns>成立するポーカーの役</returns>
        public static PokerResults CheckHands(Card[] cards)
        {
            // 初期値
            PKHands rank = PKHands.ERROR;
            int[] numCards = null;
            // 5枚のカードが渡されていることを確認する
            if (cards == null || cards.Length < 5)
            {
                return null;
            }
            else if (cards.Length > 5)
            {
                return null;
            }
            else
            {
                rank = PKHands.HighCard;
                // 5枚のカードを並べ替え
                Array.Sort(cards);
                // 解析を行う
                bool flushFlag = true;
                Suit flushSuit = Suit.Joker;
                bool straightFlag = true;
                int straightCount = -1;
                Dictionary<PCNumber, int> pcDic = new Dictionary<PCNumber, int>();
                for (int i = 0; i < cards.Length; i++)
                {
                    // ストレート、フラッシュのチェック
                    if (i == 0)
                    {
                        // 1枚目はスートと番号を記憶
                        flushSuit = cards[i].Suit;
                        straightCount = (int)cards[i].Number;
                    }
                    else
                    {
                        // 2枚目以降の処理
                        // 同スートチェック
                        if (flushFlag == true && cards[i].Suit != flushSuit)
                        {
                            flushFlag = false;
                        }
                        // 連番チェック
                        if (straightFlag == true)
                        {
                            if (straightCount == 1)
                            {
                                // 1枚目がAの場合は2、10がストレートチェックの対象になる
                                int cardNum = (int)cards[i].Number;
                                if (cardNum == 2 || cardNum == 10)
                                {
                                    straightCount = cardNum;
                                }
                                else
                                {
                                    straightFlag = false;
                                }
                            }
                            else
                            {
                                // 1枚目がA以外なら連番チェック
                                straightCount += 1;
                                if ((int)cards[i].Number != straightCount)
                                {
                                    straightFlag = false;
                                }
                            }
                        }
                    }
                    // 同一番号枚数チェック
                    if (pcDic.Keys.Contains<PCNumber>(cards[i].Number) == true)
                    {
                        pcDic[cards[i].Number]++;
                    }
                    else
                    {
                        pcDic.Add(cards[i].Number, 1);
                    }
                }
                // ストレートフラッシュのチェック
                if (flushFlag && straightFlag)
                {
                    // 1枚目がA、5枚目がKならロイヤル成立
                    if (cards[0].Number == PCNumber.A && cards[cards.Length - 1].Number == PCNumber.King)
                    {
                        rank = PKHands.RoyalStraightFlush;
                        numCards = new int[] { 1, 13, 12, 11, 10 };
                    }
                    else
                    {
                        rank = PKHands.StraightFlush;
                        Array.Sort(cards, (a, b) => b.CompareTo(a));
                        numCards = Array.ConvertAll<Card, int>(cards, _ => (int)_.Number);
                    }
                }
                // フラッシュ
                else if (flushFlag)
                {
                    rank = PKHands.Flush;
                    Array.Sort(cards, (a, b) => a.ReverseCompareTo(b));
                    numCards = Array.ConvertAll<Card, int>(cards, _ => (int)_.Number);
                }
                // ストレート
                else if (straightFlag)
                {
                    rank = PKHands.Straight;
                    if (cards[0].Number == PCNumber.A && cards[cards.Length - 1].Number == PCNumber.King)
                    {
                        numCards = new int[] { 1, 13, 12, 11, 10 };
                    }
                    else
                    {
                        Array.Sort(cards, (a, b) => b.CompareTo(a));
                        numCards = Array.ConvertAll<Card, int>(cards, _ => (int)_.Number);
                    }
                }
                // その他役のチェック
                else
                {
                    // ペア以上チェック
                    int maxCards = 0;
                    int pairCount = 0;
                    foreach (PCNumber key in pcDic.Keys)
                    {
                        if (maxCards < pcDic[key])
                        {
                            maxCards = pcDic[key];
                        }
                        if (pcDic[key] >= 2)
                        {
                            pairCount++;
                        }
                    }
                    if (maxCards >= 4)
                    {
                        // フォーカード
                        rank = PKHands.FourCards;
                        numCards = new int[2];
                        foreach (PCNumber key in pcDic.Keys)
                        {
                            if (pcDic[key] == 4)
                            {
                                numCards[0] = (int)key;
                                break;
                            }
                        }
                        foreach (PCNumber key in pcDic.Keys)
                        {
                            if (pcDic[key] == 1)
                            {
                                numCards[1] = (int)key;
                                break;
                            }
                        }
                    }
                    else if (maxCards == 3 && pairCount >= 2)
                    {
                        // フルハウス
                        rank = PKHands.FullHouse;
                        numCards = new int[2];
                        foreach (PCNumber key in pcDic.Keys)
                        {
                            if (pcDic[key] == 3)
                            {
                                numCards[0] = (int)key;
                                break;
                            }
                        }
                        foreach (PCNumber key in pcDic.Keys)
                        {
                            if (pcDic[key] == 2)
                            {
                                numCards[1] = (int)key;
                                break;
                            }
                        }
                    }
                    else if (maxCards == 3)
                    {
                        // スリーカード
                        rank = PKHands.ThreeCards;
                        numCards = new int[3];
                        PCNumber temp = PCNumber.A;
                        foreach (PCNumber key in pcDic.Keys)
                        {
                            if (pcDic[key] == 3)
                            {
                                numCards[0] = (int)key;
                                temp = key;
                                break;
                            }
                        }
                        var tempArray = cards.Where((_) => _.Number != temp).ToArray();
                        Array.Sort(tempArray, (a, b) => a.ReverseCompareTo(b));
                        for (var i = 0; i < 2; i++)
                        {
                            numCards[i + 1] = (int)tempArray[i].Number;
                        }
                    }
                    else if (pairCount >= 2)
                    {
                        // ツーペア
                        rank = PKHands.TwoPair;
                        numCards = new int[3];
                        var index = 0;
                        if (pcDic.Keys.Contains<PCNumber>(PCNumber.A) == true && pcDic[PCNumber.A] == 2)
                        {
                            numCards[index++] = 1;
                        }
                        for (var i = 13; i > 1; i--)
                        {
                            if (pcDic.Keys.Contains<PCNumber>((PCNumber)i) == true && pcDic[(PCNumber)i] == 2)
                            {
                                numCards[index++] = i;
                                if (index == 2)
                                {
                                    break;
                                }
                            }
                        }
                        foreach (PCNumber key in pcDic.Keys)
                        {
                            if (pcDic[key] == 1)
                            {
                                numCards[2] = (int)key;
                                break;
                            }
                        }
                    }
                    else if (pairCount == 1)
                    {
                        // ワンペア
                        rank = PKHands.OnePair;
                        numCards = new int[4];
                        var index = 0;
                        if (pcDic.Keys.Contains<PCNumber>(PCNumber.A) == true && pcDic[PCNumber.A] == 2)
                        {
                            numCards[index++] = 1;
                        }
                        for (var i = 13; i > 1; i--)
                        {
                            if (pcDic.Keys.Contains<PCNumber>((PCNumber)i) == true && pcDic[(PCNumber)i] == 2)
                            {
                                numCards[index++] = i;
                                if (index == 1)
                                {
                                    break;
                                }
                            }
                        }
                        if (pcDic.Keys.Contains<PCNumber>(PCNumber.A) == true && pcDic[PCNumber.A] == 1)
                        {
                            numCards[index++] = 1;
                        }
                        for (var i = 13; i > 1; i--)
                        {
                            if (pcDic.Keys.Contains<PCNumber>((PCNumber)i) == true && pcDic[(PCNumber)i] == 1)
                            {
                                numCards[index++] = i;
                                if (index == 4)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        // ノーペア
                        rank = PKHands.HighCard;
                        Array.Sort(cards, (a, b) => a.ReverseCompareTo(b));
                        numCards = Array.ConvertAll<Card, int>(cards, _ => (int)_.Number);
                    }
                }
                // 取得した役を返す
                return new PokerResults(rank, numCards);
            }
        }

    }
}
