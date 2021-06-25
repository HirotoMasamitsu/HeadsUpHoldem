using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEMP
{
    /// <summary>
    /// トランプ1組のデッククラス
    /// </summary>
    public class Deck
    {
        /// <summary>
        /// 乱数発生源
        /// </summary>
        private Random rand;
        /// <summary>
        /// カードを格納するリスト
        /// </summary>
        private List<Card> cards;

        /// <summary>
        /// トランプ1組を作成するコンストラクタ
        /// </summary>
        public Deck()
        {
            Init(new Random());
        }

        /// <summary>
        /// 乱数源を指定してトランプ1組を作成するコンストラクタ
        /// </summary>
        public Deck(Random rand)
        {
            Init(rand);
        }

        /// <summary>
        /// トランプ1組を初期化する
        /// </summary>
        /// <param name="rand"></param>
        private void Init(Random rand)
        {
            this.rand = rand;
            // トランプ1組を作成する
            this.cards = new List<Card>();
            // スペードからクラブまで
            for (int i = 1; i <= 4; i++)
            {
                // AからKまで
                for (int j = 1; j <= 13; j++)
                {
                    // 1枚ずつリストに追加
                    Card card = new Card(i, j);
                    this.cards.Add(card);
                }
            }
            // トランプをシャッフルする
            Shuffle();
        }

        /// <summary>
        /// デックの残り枚数
        /// </summary>
        public int Count
        {
            get
            {
                return this.cards.Count;
            }
        }

        /// <summary>
        /// デックをシャッフルする
        /// </summary>
        public void Shuffle()
        {
            // シャッフル後のリストを作成
            List<Card> newDeck = new List<Card>();
            // シャッフル前のリストをすべて取り除くまでループ
            while (this.cards.Count > 0)
            {
                // シャッフル前のリストのどこかからカードを1枚取り出す
                int point = this.rand.Next(this.cards.Count);
                Card c = this.cards[point];
                this.cards.Remove(c);
                // シャッフル後リストに移動
                newDeck.Add(c);
            }
            // シャッフル後リストを新たなリストとする
            this.cards = newDeck;
        }

        /// <summary>
        /// デックの一番上のカードを見る
        /// </summary>
        /// <returns>デックの一番上のカード</returns>
        public Card LookTop()
        {
            if (this.cards.Count > 0)
            {
                return this.cards[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// デックからカードをドローする
        /// </summary>
        /// <returns>ドローしたカード</returns>
        public Card Draw()
        {
            if (this.cards.Count > 0)
            {
                // リストの一番上のカードを取得して取り除く
                Card c = this.cards[0];
                this.cards.RemoveAt(0);
                return c;
            }
            else
            {
                return null;
            }
        }
    }
}
