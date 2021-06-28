using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TEMP;


public class PlayerStatus
{
    public string Name { get; private set; }
    public Card[] Cards { get; private set; }
    public PlayerState State { get; private set; }
    public int ChipStack { get; private set; }

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
        if (value >= this.ChipStack)
        {
            ret = this.ChipStack;
            this.ChipStack = 0;
            this.State = PlayerState.AllIn;
        }
        return ret;
    }

    public void Fold()
    {
        this.State = PlayerState.Fold;
    }

    public void GetChip(int value)
    {
        this.ChipStack += value;
    }
}
