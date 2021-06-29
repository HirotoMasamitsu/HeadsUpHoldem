using System.Collections;
using System.Collections.Generic;

public struct PokerAction
{
    public PlayerCommand Command { get; private set; }
    public int Chip { get; private set; }

    public PokerAction(PlayerCommand command, int chip)
    {
        this.Command = command;
        this.Chip = chip;
    }

    public override string ToString()
    {
        switch(this.Command)
        {
            case PlayerCommand.Fold:
            case PlayerCommand.Check:
                return string.Format("{0}", Command);
            default:
                return string.Format("{0} - ${1}", Command, Chip);
        }
    }
}
