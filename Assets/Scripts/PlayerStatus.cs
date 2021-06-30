using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TEMP;


public class PlayerStatus
{
    public string Name { get; private set; }
    public Card[] Cards { get; private set; }
    public PlayerState State { get; private set; }
    public int ChipStack { get; private set; }

    private Text text;

    public PlayerStatus (string name, int defaultStack)
    {
        this.Name = name;
        this.ChipStack = defaultStack;
    }

    public void SetCards(Card[] cards)
    {
        this.Cards = cards;
        this.State = PlayerState.Active;
    }

    public int PutChip(PlayerState state, int value)
    {
        var ret = 0;
        if (0 <= value && value < this.ChipStack)
        {
            this.ChipStack -= value;
            ret = value;
            this.State = state;
        }
        else if (value >= this.ChipStack)
        {
            ret = this.ChipStack;
            this.ChipStack = 0;
            this.State = PlayerState.AllIn;
        }
        UpdateText();
        return ret;
    }

    public void Fold()
    {
        this.State = PlayerState.Fold;
    }

    public void GetChip(int value)
    {
        this.ChipStack += value;
        UpdateText();
    }

    public void SetText(Text textUI)
    {
        this.text = textUI;
        UpdateText();
    }

    private void UpdateText()
    {
        if (this.text != null)
        {
            this.text.text = string.Format("{0} Chip: ${1}", this.Name, this.ChipStack);
        }
    }
}
