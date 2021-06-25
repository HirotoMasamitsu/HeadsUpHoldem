using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TEMP
{
    /// <summary>
    /// トランプのスート
    /// </summary>
    public enum Suit
    {
        /// <summary>
        /// スペード
        /// </summary>
        Spade = 1,
        /// <summary>
        /// ハート
        /// </summary>
        Heart = 2,
        /// <summary>
        /// ダイヤ
        /// </summary>
        Diamond = 3,
        /// <summary>
        /// クラブ
        /// </summary>
        Club = 4,
        /// <summary>
        /// ジョーカー(未使用)
        /// </summary>
        Joker = 0
    }

    /// <summary>
    /// トランプの番号
    /// </summary>
    public enum PCNumber
    {
        A = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    }
}
