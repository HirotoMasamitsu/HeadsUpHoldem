using System.Collections;
using System.Collections.Generic;

public interface IGetPokerAction
{
    public PokerAction GetPokerAction(HoldemStep step, int callStack, int minimumRaiseSize);
}
