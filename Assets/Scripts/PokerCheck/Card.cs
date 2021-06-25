using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEMP
{
    /// <summary>
    /// トランプのカード情報を定義するクラス。
    /// </summary>
    public class Card : IComparable<Card>
    {
        /// <summary>
        /// スート(Suit列挙体に対応した整数値を入れる)
        /// </summary>
        private int suitCount;
        /// <summary>
        /// 番号(PCNumber列挙体に対応した整数値を入れる)
        /// </summary>
        private int numberCount;

        /// <summary>
        /// トランプカードのコンストラクタ
        /// </summary>
        /// <param name="suitCount">スート(1:スペード、2：ハート、3：ダイヤ、4：クラブ)</param>
        /// <param name="numberCount">番号</param>
        public Card(int suitCount, int numberCount)
        {
            this.suitCount = suitCount;
            this.numberCount = numberCount;
        }

        /// <summary>
        /// トランプカードのコンストラクタ
        /// </summary>
        /// <param name="suit">スート</param>
        /// <param name="number">番号</param>
        public Card(Suit suit, PCNumber number)
        {
            this.suitCount = (int)suit;
            this.numberCount = (int)number;
        }

        /// <summary>
        /// スート
        /// </summary>
        public Suit Suit
        {
            get
            {
                if (Enum.IsDefined(typeof(Suit), this.suitCount))
                {
                    return (Suit)this.suitCount;
                }
                else
                {
                    // 列挙体未定義値ならJokerと見なす
                    return Suit.Joker;
                }
            }
        }

        /// <summary>
        /// 番号
        /// </summary>
        public PCNumber Number
        {
            get
            {
                if (Enum.IsDefined(typeof(PCNumber), this.numberCount))
                {
                    return (PCNumber)this.numberCount;
                }
                else
                {
                    // 列挙体未定義値ならAと見なす
                    return PCNumber.A;
                }
            }
        }

        /// <summary>
        /// テキスト表示にする(テスト用機能のため表示は適当)
        /// </summary>
        /// <returns>[(スート)(番号)]</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            bool isJoker = false;
            sb.Append("[");
            switch (this.Suit)
            {
                case Suit.Spade:
                    sb.Append("▲");
                    break;
                case Suit.Heart:
                    sb.Append("▼");
                    break;
                case Suit.Diamond:
                    sb.Append("◆");
                    break;
                case Suit.Club:
                    sb.Append("士");
                    break;
                case Suit.Joker:
                    sb.Append("Joker");
                    isJoker = true;
                    break;
                default:
                    break;
            }
            if (isJoker == false)
            {
                switch (this.Number)
                {
                    case PCNumber.A:
                        sb.Append(" A");
                        break;
                    case PCNumber.Two:
                        sb.Append(" 2");
                        break;
                    case PCNumber.Three:
                        sb.Append(" 3");
                        break;
                    case PCNumber.Four:
                        sb.Append(" 4");
                        break;
                    case PCNumber.Five:
                        sb.Append(" 5");
                        break;
                    case PCNumber.Six:
                        sb.Append(" 6");
                        break;
                    case PCNumber.Seven:
                        sb.Append(" 7");
                        break;
                    case PCNumber.Eight:
                        sb.Append(" 8");
                        break;
                    case PCNumber.Nine:
                        sb.Append(" 9");
                        break;
                    case PCNumber.Ten:
                        sb.Append("10");
                        break;
                    case PCNumber.Jack:
                        sb.Append(" J");
                        break;
                    case PCNumber.Queen:
                        sb.Append(" Q");
                        break;
                    case PCNumber.King:
                        sb.Append(" K");
                        break;
                    default:
                        break;
                }
            }
            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// リソースファイル用の文字列を取得します。
        /// </summary>
        /// <returns></returns>
        public string ToResourceString()
        {
            var sb = new StringBuilder();
            bool isJoker = false;
            switch (this.Suit)
            {
                case Suit.Spade:
                    sb.Append("Spade");
                    break;
                case Suit.Heart:
                    sb.Append("Heart");
                    break;
                case Suit.Diamond:
                    sb.Append("Diamond");
                    break;
                case Suit.Club:
                    sb.Append("Club");
                    break;
                case Suit.Joker:
                    sb.Append("Joker");
                    isJoker = true;
                    break;
                default:
                    break;
            }
            if (isJoker == false)
            {
                sb.AppendFormat("{0:D2}", this.numberCount);
            } 
            else
            {
                if (this.numberCount == 2)
                {
                    sb.Append("_Monochrome");
                } 
                else
                {
                    sb.Append("_Color");
                }
            }
            return sb.ToString();
        }

        #region IComparable<Card> メンバ

        /// <summary>
        /// ソート用CompareTo
        /// </summary>
        /// <param name="other">Cardクラスのインスタンス</param>
        /// <returns>比較対象より前：マイナス、後：プラス</returns>
        public int CompareTo(Card other)
        {
            if (this.Suit == Suit.Joker)
            {
                // ジョーカーなら先頭に持ってくる
                return -1;
            }
            else
            {
                // 番号でコンペアする
                int numCompare = this.numberCount.CompareTo(other.numberCount);
                if (numCompare != 0)
                {
                    return numCompare;
                }
                else
                {
                    // 番号が同じならスートで比較
                    return this.suitCount.CompareTo(other.suitCount);
                }
            }
        }

        #endregion

        /// <summary>
        /// 逆順ソート用CompareTo
        /// </summary>
        /// <param name="other">Cardクラスのインスタンス</param>
        /// <returns>比較対象より前：マイナス、後：プラス</returns>
        public int ReverseCompareTo(Card other)
        {
            if (this.Suit == Suit.Joker)
            {
                // ジョーカーなら先頭に持ってくる
                return -1;
            }
            else
            {
                // 番号でコンペアする
                int numCompare = 0;
                if (this.numberCount == other.numberCount)
                {
                    numCompare = 0;
                } 
                else if (this.numberCount == 1)
                {
                    numCompare = -1;
                } 
                else if (other.numberCount == 1)
                {
                    numCompare = 1;
                } 
                else
                {
                    numCompare = other.numberCount.CompareTo(this.numberCount);
                }
                if (numCompare != 0)
                {
                    return numCompare;
                }
                else
                {
                    // 番号が同じならスートで比較
                    return this.suitCount.CompareTo(other.suitCount);
                }
            }
        }

    }
}
