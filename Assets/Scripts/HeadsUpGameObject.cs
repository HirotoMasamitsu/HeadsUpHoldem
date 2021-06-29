using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

public class HeadsUpGameObject
{

    private List<PlayerStatus> playerList;
    private List<int> potList;
    private System.Random rand;

    private Text potText;
    private Text blindText;

    public int SmallBlind { get; private set; }
    public int BigBlind { get; private set; }
    public int Anty { get; private set; }
    public int BBAnty { get; private set; }
    public int DealerPosition { get; private set; }

    public int PlayersCount
    {
        get { return this.playerList.Count; }
    }

    public PlayerStatus[] Players
    {
        get
        {
            return this.playerList.ToArray();
        }
    }

    public HeadsUpGameObject()
    {
        this.SmallBlind = 1;
        this.BigBlind = 2;
        this.Anty = 0;
        this.BBAnty = 0;
        this.potList = new List<int>();
        this.playerList = new List<PlayerStatus>();
        this.rand = new System.Random();
        this.DealerPosition = 0;
    }

    public void SetPotText(Text textUI)
    {
        this.potText = textUI;
        UpdatePotText();
    }

    public void SetBlindText(Text textUI)
    {
        this.blindText = textUI;
        UpdateBlindText();
    }

    public void SetPlayer(PlayerStatus player)
    {
        if (player != null)
        {
            this.playerList.Add(player);
        }
    }

    public void SetPlayerPosition()
    {
        var swap = new System.Action(() =>
        {
            var a = rand.Next(this.playerList.Count);
            var b = rand.Next(this.playerList.Count);
            if (a != b)
            {
                var temp = this.playerList[a];
                this.playerList[a] = this.playerList[b];
                this.playerList[b] = temp;
            }
        });
        for (var i = 0; i < rand.Next(100, 256); i++)
        {
            swap();
        }
        this.DealerPosition = rand.Next(this.playerList.Count);
    }

    public void AddPot(int value, int overBet = 0)
    {
        if (this.potList.Count == 0)
        {
            this.potList.Add(0);
        }
        this.potList[this.potList.Count - 1] += value;
        if (overBet > 0)
        {
            this.potList.Add(overBet);
        }
        UpdatePotText();
    }

    public int GetPot(int potIndex = 0)
    {
        var ret = 0;
        if (0 <= potIndex && potIndex < this.potList.Count)
        {
            ret = this.potList[potIndex];
        }
        return ret;
    }

    public void ResetPot()
    {
        this.potList.Clear();
        this.DealerPosition = (this.DealerPosition + 1) % this.playerList.Count;
        UpdatePotText();
    }

    public void GameReset()
    {
        this.potList.Clear();
        this.playerList.Clear();
        UpdatePotText();
    }

    private void UpdatePotText()
    {
        if (this.potText != null)
        {
            if (this.potList.Count > 0)
            {
                var sb = new StringBuilder();
                for (var i = 0; i < this.potList.Count; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.AppendFormat("${0}", this.potList[i]);
                }
                this.potText.text = string.Format("Pot: {0}", sb.ToString());
            }
            else
            {
                this.potText.text = "Pot: $0";
            }
        }
    }

    private void UpdateBlindText()
    {
        if (this.blindText != null)
        {
            this.potText.text = string.Format("SB/BB : ${0}/${1}", this.SmallBlind, this.BigBlind);
        }
    }
}
