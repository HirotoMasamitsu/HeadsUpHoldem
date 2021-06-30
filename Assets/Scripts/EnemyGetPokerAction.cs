using System;
using System.Collections;
using System.Collections.Generic;

using TEMP;

public class EnemyGetPokerAction : IGetPokerAction
{
    public PlayerStatus status;
    public int BetSize { get; set; }

    private Card[] board;
    private System.Random rand;
    private int potSize;

    public EnemyGetPokerAction(PlayerStatus status)
    {
        this.status = status;
        this.rand = new System.Random();
    }

    public void SetBoard(Card[] board, int potSize)
    {
        this.board = board;
        this.potSize = potSize;
    }

    public PokerAction GetPokerAction(HoldemStep step, int callStack, int minimumRaiseSize)
    {
        var isPairHand = (this.status.Cards[0].Number == this.status.Cards[1].Number);
        var isSuited = (this.status.Cards[0].Suit == this.status.Cards[1].Suit);
        var total = 0;
        total += (this.status.Cards[0].Number == PCNumber.A) ? 14 : 1;
        total += (this.status.Cards[1].Number == PCNumber.A) ? 14 : 1;
        switch (step)
        {
            case HoldemStep.Preflop:
                if (isPairHand || (isSuited && total >= 16) || total >= 21)
                {
                    if (GetRandFlag(0.7))
                    {
                        return Raise(Math.Max(callStack, minimumRaiseSize) * 3);
                    }
                    else
                    {
                        return Call(callStack);
                    }
                }
                else if (total >= 13 || callStack == 0)
                {
                    if (GetRandFlag(0.1))
                    {
                        return Raise(Math.Max(callStack, minimumRaiseSize) * 3);
                    }
                    else
                    {
                        return Call(callStack);
                    }
                }
                else
                {
                    if (GetRandFlag(0.3))
                    {
                        return Call(callStack);
                    }
                    else
                    {
                        return Fold();
                    }
                }
                break;
            case HoldemStep.Flop:
                {
                    var tempArray = new Card[5];
                    Array.Copy(this.status.Cards, 0, tempArray, 0, this.status.Cards.Length);
                    Array.Copy(this.board, 0, tempArray, 2, 3);
                    var result = PKCheck.CheckHands(tempArray);
                    var pivotHands = new PokerResults(PKHands.OnePair, new int[] { 11, 2 });
                    if (result.CompareTo(pivotHands) >= 0)
                    {
                        if (GetRandFlag(0.7))
                        {
                            return Raise(Math.Max(callStack * 2, potSize / 2));
                        }
                        else
                        {
                            return Call(callStack);
                        }
                    }
                    else
                    {
                        if (GetRandFlag(0.05))
                        {
                            return Raise(Math.Max(callStack * 2, potSize / 2));
                        }
                        else if (GetRandFlag(0.7) || callStack == 0)
                        {
                            return Call(callStack);
                        } 
                        else
                        {
                            return Fold();
                        }

                    }

                }
                break;
            case HoldemStep.Turn:
                {
                    var tempArray = new Card[6];
                    Array.Copy(this.status.Cards, 0, tempArray, 0, this.status.Cards.Length);
                    Array.Copy(this.board, 0, tempArray, 2, 4);
                    var result = PKCheck.CheckSixHands(tempArray);
                    var pivotHands = new PokerResults(PKHands.OnePair, new int[] { 12, 2 });
                    if (result.CompareTo(pivotHands) >= 0)
                    {
                        if (GetRandFlag(0.6))
                        {
                            return Raise(Math.Max(callStack * 2, potSize / 2));
                        }
                        else
                        {
                            return Call(callStack);
                        }
                    }
                    else
                    {
                        if (GetRandFlag(0.05))
                        {
                            return Raise(Math.Max(callStack * 2, potSize / 2));
                        }
                        else if (GetRandFlag(0.7) || callStack == 0)
                        {
                            return Call(callStack);
                        }
                        else
                        {
                            return Fold();
                        }

                    }
                }
                break;
            case HoldemStep.River:
                {
                    var tempArray = new Card[7];
                    Array.Copy(this.status.Cards, 0, tempArray, 0, this.status.Cards.Length);
                    Array.Copy(this.board, 0, tempArray, 2, this.board.Length);
                    var result = PKCheck.CheckSevenHands(tempArray);
                    var pivotHands = new PokerResults(PKHands.TwoPair, new int[] { 2, 3 });
                    if (result.CompareTo(pivotHands) >= 0)
                    {
                        if (GetRandFlag(0.5))
                        {
                            return Raise(Math.Max(callStack * 2, potSize / 2));
                        }
                        else
                        {
                            return Call(callStack);
                        }
                    }
                    else
                    {
                        if (GetRandFlag(0.05))
                        {
                            return Raise(Math.Max(callStack * 2, potSize / 2));
                        }
                        else if (GetRandFlag(0.7) || callStack == 0)
                        {
                            return Call(callStack);
                        }
                        else
                        {
                            return Fold();
                        }

                    }
                }
                break;
            default:
                return Fold();
        }
    }

    private bool GetRandFlag(double p)
    {
        return (rand.NextDouble() <= p);
    }

    private PokerAction Call(int callStack)
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

    private PokerAction Raise(int betSize)
    {
        if (betSize < status.ChipStack)
        {
            var chip = status.PutChip(PlayerState.Raise, betSize);
            this.BetSize = chip;
            return new PokerAction(PlayerCommand.Raise, chip);
        }
        else
        {
            var chip = status.PutChip(PlayerState.AllIn, betSize);
            this.BetSize = chip;
            return new PokerAction(PlayerCommand.AllIn, chip);
        }
    }

    private PokerAction Fold()
    {
        return new PokerAction(PlayerCommand.Fold, 0);
    }
}
