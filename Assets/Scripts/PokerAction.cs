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
}
