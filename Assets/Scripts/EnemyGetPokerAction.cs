using System.Collections;
using System.Collections.Generic;

public class EnemyGetPokerAction : IGetPokerAction
{
    public PlayerStatus status;

    public EnemyGetPokerAction(PlayerStatus status)
    {
        this.status = status;
    }


    public PokerAction GetPokerAction(HoldemStep step, int callStack, int minimumRaiseSize)
    {
        if (callStack == 0)
        {
            return new PokerAction(PlayerCommand.Check, 0);
        }
        else if (callStack < status.ChipStack)
        {
            var chip = status.PutChip(PlayerState.Call, callStack);
            return new PokerAction(PlayerCommand.Call, chip);
        } 
        else
        {
            var chip = status.PutChip(PlayerState.AllIn, callStack);
            return new PokerAction(PlayerCommand.AllIn, chip);
        }
    }
}
