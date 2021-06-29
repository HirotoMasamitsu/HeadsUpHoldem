using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerGetPokerAction : IGetPokerAction
{
    public PlayerStatus status;

    public PlayerCommand Action { get; set; }
    public int BetSize { get; set; }
    public int CallSize { get; set; }


    public PlayerGetPokerAction(PlayerStatus status)
    {
        this.status = status;
    }

    public int GetMinimumRaiseSize(int callStack, int bigBlind)
    {
        return bigBlind + Math.Max(0, (callStack - bigBlind) * 2);
    }

    public int GetMaximumRaiseSize()
    {
        return this.status.ChipStack;
    }


    public PokerAction GetPokerAction(HoldemStep step, int callStack, int minimumRaiseSize)
    {
        return GetPokerAction();
    }

    public PokerAction GetPokerAction()
    {
        switch (this.Action)
        {
            case PlayerCommand.Bet:
            case PlayerCommand.Raise:
            case PlayerCommand.AllIn:
                var betChip = status.PutChip(PlayerState.Raise, this.BetSize);
                return new PokerAction(this.Action, betChip);
            case PlayerCommand.Check:
            case PlayerCommand.Call:
                if (CallSize == 0)
                {
                    return new PokerAction(PlayerCommand.Check, 0);
                }
                else if (CallSize < status.ChipStack)
                {
                    var chip = status.PutChip(PlayerState.Call, CallSize);
                    return new PokerAction(PlayerCommand.Call, chip);
                }
                else
                {
                    var chip = status.PutChip(PlayerState.AllIn, CallSize);
                    return new PokerAction(PlayerCommand.AllIn, chip);
                }
            default:
                status.Fold();
                return new PokerAction(PlayerCommand.Fold, 0);
        }
    }
}
