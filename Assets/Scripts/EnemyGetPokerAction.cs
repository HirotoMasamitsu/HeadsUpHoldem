using System.Collections;
using System.Collections.Generic;

public class EnemyGetPokerAction : IGetPokerAction
{
    public PlayerStatus status;
    public int BetSize { get; set; }

    public EnemyGetPokerAction(PlayerStatus status)
    {
        this.status = status;
    }


    public PokerAction GetPokerAction(HoldemStep step, int callStack, int minimumRaiseSize)
    {
        if (callStack == 0)
        {
            this.BetSize = 0;
            return new PokerAction(PlayerCommand.Check, 0);
        }
        else if (callStack < status.ChipStack)
        {
            var chip = status.PutChip(PlayerState.Call, callStack);
            this.BetSize = chip;
            return new PokerAction(PlayerCommand.Call, chip);
        } 
        else
        {
            var chip = status.PutChip(PlayerState.AllIn, callStack);
            this.BetSize = chip;
            return new PokerAction(PlayerCommand.AllIn, chip);
        }
    }
}
