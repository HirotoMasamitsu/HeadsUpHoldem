using System.Collections;
using System.Collections.Generic;

public enum PlayerState
{
    Active,
    Raise,
    Call,
    Fold,
    AllIn,
    Lose
}

public enum HoldemStep
{
    Preflop,
    Flop,
    Turn,
    River,
    Showdown
}

public enum PlayerCommand
{
    Bet,
    Raise,
    Call,
    Check,
    Fold,
    AllIn
}