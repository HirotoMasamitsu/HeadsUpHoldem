using System;
using System.Collections;
using System.Collections.Generic;

using TEMP;
using UnityEngine;
using UnityEngine.UI;

public static class HeadsUpHoldemGame
{
    private static System.Random rand = new System.Random();
    private static Deck deck;
    private static HeadsUpGameObject gameObj = new HeadsUpGameObject();
    private static bool firstBetFlag;

    private static Text playerHandText;
    private static Text enemyHandText;
    private static Text actionText;
    private static Text playerActionText = GameObject.Find("PlayerActionText").GetComponent<Text>();
    private static Text enemyActionText = GameObject.Find("EnemyActionText").GetComponent<Text>();
    private static GameObject dealerButton;
    private static Slider betSlider = GameObject.Find("Slider").GetComponent<Slider>();


    public static PlayerStatus Player { get; private set; }
    public static PlayerStatus Enemy { get; private set; }

    public static PlayerGetPokerAction PlayerAction { get; private set; }
    public static EnemyGetPokerAction EnemyAction { get; private set; }

    public static Card[] BoardCards { get; private set; }
    public static Card[] BannedCards { get; private set; }

    public static List<CardMouseEvent> playerHandsEventList = new List<CardMouseEvent>();
    public static List<CardMouseEvent> enemyHandsEventList = new List<CardMouseEvent>();
    public static List<CardMouseEvent> flopCardsEventList = new List<CardMouseEvent>();
    public static List<CardMouseEvent> turnCardsEventList = new List<CardMouseEvent>();
    public static List<CardMouseEvent> riverCardsEventList = new List<CardMouseEvent>();

    public static int Mode { get; private set; }


    public static void SetPlayerHandText(Text textUI)
    {
        playerHandText = textUI;
    }

    public static void SetEnemyHandText(Text textUI)
    {
        enemyHandText = textUI;
    }

    public static void SetActionText(Text textUI)
    {
        actionText = textUI;
    }

    public static void SetPotText(Text textUI)
    {
        gameObj.SetPotText(textUI);
    }

    public static void SetBlindText(Text textUI)
    {
        gameObj.SetBlindText(textUI);
    }

    public static void SetDealerButton(GameObject obj)
    {
        dealerButton = obj;
    }

    public static void SetPlayerHandsEvent(CardMouseEvent cme)
    {
        playerHandsEventList.Add(cme);
    }

    public static void SetEnemyHandsEvent(CardMouseEvent cme)
    {
        enemyHandsEventList.Add(cme);
    }

    public static void SetFlopCardsEvent(CardMouseEvent cme)
    {
        flopCardsEventList.Add(cme);
    }

    public static void SetTurnCardsEvent(CardMouseEvent cme)
    {
        turnCardsEventList.Add(cme);
    }

    public static void SetRiverCardsEvent(CardMouseEvent cme)
    {
        riverCardsEventList.Add(cme);
    }

    public static void ResetGame(Text playerChipUI, Text enemyChipTextUI)
    {
        gameObj.GameReset();
        Player = new PlayerStatus("Player", 100);
        Player.SetText(playerChipUI);
        PlayerAction = new PlayerGetPokerAction(Player);
        Enemy = new PlayerStatus("Enemy", 100);
        Enemy.SetText(enemyChipTextUI);
        EnemyAction = new EnemyGetPokerAction(Enemy);
        gameObj.SetPlayer(Player);
        gameObj.SetPlayer(Enemy);
        gameObj.SetPlayerPosition();
        SetButtonMode(false);
    }

    public static void Reset()
    {
        Mode = 0;
        gameObj.ResetPot();
        deck = new Deck(rand);
        deck.Shuffle();
        BoardCards = new Card[5];
        BannedCards = new Card[3];
        foreach (var cme in playerHandsEventList)
        {
            cme.SetFace(false);
        }
        foreach (var cme in enemyHandsEventList)
        {
            cme.SetFace(false);
        }
        foreach (var cme in flopCardsEventList)
        {
            cme.SetFace(false);
        }
        foreach (var cme in turnCardsEventList)
        {
            cme.SetFace(false);
        }
        foreach (var cme in riverCardsEventList)
        {
            cme.SetFace(false);
        }
        playerHandText.text = "";
        enemyHandText.text = "";
        Mode = 1;
        Preflop();
    }

    public static void Preflop()
    {
        Debug.Log(string.Format("Preflop Mode:{0}", Mode));
        if (Mode == 1)
        {
            firstBetFlag = true;
            var playerHands = new Card[2];
            var enemyHands = new Card[2];
            playerHands[0] = deck.Draw();
            enemyHands[0] = deck.Draw();
            playerHands[1] = deck.Draw();
            enemyHands[1] = deck.Draw();
            Player.SetCards(playerHands);
            Enemy.SetCards(enemyHands);
            for (var i = 0; i < Player.Cards.Length; i++)
            {
                playerHandsEventList[i].SetCard(Player.Cards[i]);
                playerHandsEventList[i].SetFace(true);
            }

            Debug.Log(string.Format("DealerPositon({0}) = {1}", gameObj.DealerPosition, gameObj.Players[gameObj.DealerPosition].Name));
            if (gameObj.Players[gameObj.DealerPosition].Name == "Player")
            {
                dealerButton.transform.position = new Vector3(105, 137);
                Player.PutChip(PlayerState.Active, gameObj.SmallBlind);
                gameObj.AddPot(gameObj.SmallBlind);
                Enemy.PutChip(PlayerState.Active, gameObj.BigBlind);
                gameObj.AddPot(gameObj.BigBlind);
                actionText.text = string.Format("Big Blind: ${0}", gameObj.BigBlind);
                playerActionText.text = string.Format("Small Blind: ${0}", gameObj.SmallBlind);
                enemyActionText.text = string.Format("Big Blind: ${0}", gameObj.BigBlind);
                EnemyAction.BetSize = gameObj.BigBlind;
                SetPlayerActionControl(gameObj.BigBlind - gameObj.SmallBlind, gameObj.BigBlind);

            }
            else
            {
                dealerButton.transform.position = new Vector3(277, 291);
                Player.PutChip(PlayerState.Active, gameObj.BigBlind);
                gameObj.AddPot(gameObj.BigBlind);
                Enemy.PutChip(PlayerState.Active, gameObj.SmallBlind);
                gameObj.AddPot(gameObj.SmallBlind);
                enemyActionText.text = string.Format("Small Blind: ${0}", gameObj.SmallBlind);
                playerActionText.text = string.Format("Big Blind: ${0}", gameObj.BigBlind);

                EnemyAction.SetBoard(null, gameObj.GetPot());
                var enemyAction = EnemyAction.GetPokerAction(HoldemStep.Preflop, gameObj.BigBlind - gameObj.SmallBlind, gameObj.BigBlind);
                Debug.Log(enemyAction);
                actionText.text = string.Format("Enemy: {0}", enemyAction);
                enemyActionText.text = string.Format("Enemy: {0}", enemyAction);
                if (enemyAction.Command == PlayerCommand.Fold)
                {
                    actionText.text = "Player Win!!";
                    Player.GetChip(gameObj.GetPot());
                    Mode = 0;
                    SetButtonMode(false);
                    return;
                }
                else
                {
                    gameObj.AddPot(enemyAction.Chip);
                    SetPlayerActionControl(enemyAction.Chip - (gameObj.BigBlind - gameObj.SmallBlind), gameObj.BigBlind);

                }
            }

            //Mode = 2;
            SetButtonMode(true);
        }
    }

    public static void Flop()
    {
        if (Mode == 2)
        {
            firstBetFlag = false;
            BannedCards[0] = deck.Draw();
            BoardCards[0] = deck.Draw();
            BoardCards[1] = deck.Draw();
            BoardCards[2] = deck.Draw();
            for (var i = 0; i < 3; i++)
            {
                flopCardsEventList[i].SetCard(BoardCards[i]);
                flopCardsEventList[i].SetFace(true);
            }

            var tempArray = new Card[5];
            Array.Copy(Player.Cards, 0, tempArray, 0, Player.Cards.Length);
            Array.Copy(BoardCards, 0, tempArray, 2, 3);
            var result = PKCheck.CheckHands(tempArray);
            playerHandText.text = result.ToString();

            actionText.text = "Flop Open";

            if (Player.State == PlayerState.AllIn || Enemy.State == PlayerState.AllIn)
            {
                for (var i = 0; i < enemyHandsEventList.Count; i++)
                {
                    enemyHandsEventList[i].SetCard(Enemy.Cards[i]);
                    enemyHandsEventList[i].SetFace(true);
                }
                Mode += 1;
                Turn();
                return;
            }

            if (gameObj.Players[gameObj.DealerPosition].Name == "Player")
            {
                EnemyAction.SetBoard(BoardCards, gameObj.GetPot());
                var enemyAction = EnemyAction.GetPokerAction(HoldemStep.Flop, 0, gameObj.BigBlind);
                Debug.Log(enemyAction);
                actionText.text = string.Format("Enemy: {0}", enemyAction);
                enemyActionText.text = string.Format("Enemy: {0}", enemyAction);
                if (enemyAction.Command == PlayerCommand.Fold)
                {
                    actionText.text = "Player Win!!";
                    Player.GetChip(gameObj.GetPot());
                    Mode = 0;
                    SetButtonMode(false);
                    return;
                }
                else
                {
                    gameObj.AddPot(enemyAction.Chip);
                    SetPlayerActionControl(enemyAction.Chip, gameObj.BigBlind);

                }
            }
            else
            {
                SetPlayerActionControl(0, gameObj.BigBlind);
            }

            //Mode = 3;
        }
    }

    public static void Turn()
    {
        if (Mode == 3)
        {
            firstBetFlag = false;
            BannedCards[1] = deck.Draw();
            BoardCards[3] = deck.Draw();
            for (var i = 0; i < turnCardsEventList.Count; i++)
            {
                turnCardsEventList[i].SetCard(BoardCards[i + 3]);
                turnCardsEventList[i].SetFace(true);
            }

            var tempArray = new Card[6];
            Array.Copy(Player.Cards, 0, tempArray, 0, Player.Cards.Length);
            Array.Copy(BoardCards, 0, tempArray, 2, 4);
            var result = PKCheck.CheckSixHands(tempArray);
            playerHandText.text = result.ToString();

            actionText.text = "Turn Open";

            if (Player.State == PlayerState.AllIn || Enemy.State == PlayerState.AllIn)
            {
                for (var i = 0; i < enemyHandsEventList.Count; i++)
                {
                    enemyHandsEventList[i].SetCard(Enemy.Cards[i]);
                    enemyHandsEventList[i].SetFace(true);
                }
                Mode += 1;
                River();
                return;
            }

            if (gameObj.Players[gameObj.DealerPosition].Name == "Player")
            {
                EnemyAction.SetBoard(BoardCards, gameObj.GetPot());
                var enemyAction = EnemyAction.GetPokerAction(HoldemStep.Flop, 0, gameObj.BigBlind);
                Debug.Log(enemyAction);
                actionText.text = string.Format("Enemy: {0}", enemyAction);
                enemyActionText.text = string.Format("Enemy: {0}", enemyAction);
                if (enemyAction.Command == PlayerCommand.Fold)
                {
                    actionText.text = "Player Win!!";
                    Player.GetChip(gameObj.GetPot());
                    Mode = 0;
                    SetButtonMode(false);
                    return;
                }
                else
                {
                    gameObj.AddPot(enemyAction.Chip);
                    SetPlayerActionControl(enemyAction.Chip, gameObj.BigBlind);

                }
            }
            else
            {
                SetPlayerActionControl(0, gameObj.BigBlind);
            }

            //Mode = 4;
        }
    }

    public static void River()
    {
        if (Mode == 4)
        {
            firstBetFlag = false;
            BannedCards[2] = deck.Draw();
            BoardCards[4] = deck.Draw();
            for (var i = 0; i < riverCardsEventList.Count; i++)
            {
                riverCardsEventList[i].SetCard(BoardCards[i + 4]);
                riverCardsEventList[i].SetFace(true);
            }

            var tempArray = new Card[7];
            Array.Copy(Player.Cards, 0, tempArray, 0, Player.Cards.Length);
            Array.Copy(BoardCards, 0, tempArray, 2, BoardCards.Length);
            var result = PKCheck.CheckSevenHands(tempArray);
            playerHandText.text = result.ToString();

            actionText.text = "River Open";

            if (Player.State == PlayerState.AllIn || Enemy.State == PlayerState.AllIn)
            {
                for (var i = 0; i < enemyHandsEventList.Count; i++)
                {
                    enemyHandsEventList[i].SetCard(Enemy.Cards[i]);
                    enemyHandsEventList[i].SetFace(true);
                }
                Mode += 1;
                Showdown();
                return;
            }

            if (gameObj.Players[gameObj.DealerPosition].Name == "Player")
            {
                EnemyAction.SetBoard(BoardCards, gameObj.GetPot());
                var enemyAction = EnemyAction.GetPokerAction(HoldemStep.Flop, 0, gameObj.BigBlind);
                Debug.Log(enemyAction);
                actionText.text = string.Format("Enemy: {0}", enemyAction);
                enemyActionText.text = string.Format("Enemy: {0}", enemyAction);
                if (enemyAction.Command == PlayerCommand.Fold)
                {
                    Player.GetChip(gameObj.GetPot());
                    actionText.text = "Player Win!!";
                    Mode = 0;
                    SetButtonMode(false);
                    return;
                }
                else
                {
                    gameObj.AddPot(enemyAction.Chip);
                    SetPlayerActionControl(enemyAction.Chip, gameObj.BigBlind);

                }
            }
            else
            {
                SetPlayerActionControl(0, gameObj.BigBlind);
            }

            //Mode = 5;
        }

    }

    public static void Showdown()
    {
        if (Mode == 5)
        {
            for (var i = 0; i < enemyHandsEventList.Count; i++)
            {
                enemyHandsEventList[i].SetCard(Enemy.Cards[i]);
                enemyHandsEventList[i].SetFace(true);
            }

            var tempArray = new Card[7];
            Array.Copy(Player.Cards, 0, tempArray, 0, Player.Cards.Length);
            Array.Copy(BoardCards, 0, tempArray, 2, BoardCards.Length);
            var result = PKCheck.CheckSevenHands(tempArray);
            Array.Copy(Enemy.Cards, 0, tempArray, 0, Enemy.Cards.Length);
            Array.Copy(BoardCards, 0, tempArray, 2, BoardCards.Length);
            var enemyResult = PKCheck.CheckSevenHands(tempArray);
            enemyHandText.text = enemyResult.ToString();

            var compare = result.CompareTo(enemyResult);
            if (compare == 0)
            {
                actionText.text = string.Format("Draw...");
                var pot = gameObj.GetPot();
                Player.GetChip(pot / 2);
                Enemy.GetChip(pot / 2);
                if ((pot % 2) == 1)
                {
                    if (gameObj.Players[gameObj.DealerPosition].Name == "Player")
                    {
                        Enemy.GetChip(1);
                    }
                    else
                    {
                        Player.GetChip(1);
                    }

                }
            }
            else if (compare > 0)
            {
                actionText.text = "Player Win!!";
                var pot = gameObj.GetPot();
                Player.GetChip(pot);
            }
            else
            {
                actionText.text = "Player Lose...";
                var pot = gameObj.GetPot();
                Enemy.GetChip(pot);
            }

            Mode = 6;
            SetButtonMode(false);
        }
    }


    private static void SetPlayerActionControl(int callSize, int minimumBetSize)
    {
        betSlider.minValue = PlayerAction.GetMinimumRaiseSize(callSize, minimumBetSize);
        betSlider.maxValue = PlayerAction.GetMaximumRaiseSize();
        betSlider.value = betSlider.minValue;
        if (firstBetFlag)
        {
            GameObject.Find("RaiseButtonText").GetComponent<Text>().text = "Raise";
        }
        else
        {
            GameObject.Find("RaiseButtonText").GetComponent<Text>().text = "Bet";
        }
        if (callSize == 0)
        {
            GameObject.Find("CallButtonText").GetComponent<Text>().text = "Check";
        } 
        else
        {
            GameObject.Find("CallButtonText").GetComponent<Text>().text = string.Format("Call(${0})", callSize);
        }
        PlayerAction.CallSize = callSize;
        Debug.Log(string.Format("SetPlayerActionControl - callsize:{0} minBetSize:{1}", callSize, minimumBetSize));

    }

    public static void PlayerRaise(int betSize)
    {
        PlayerAction.Action = PlayerCommand.Raise;
        PlayerAction.BetSize = betSize;
        ExecutePlayerAction();
    }

    public static void PlayerCall()
    {
        PlayerAction.Action = PlayerCommand.Call;
        ExecutePlayerAction();
    }

    public static void PlayerFold()
    {
        PlayerAction.Action = PlayerCommand.Fold;
        ExecutePlayerAction();
    }

    private static void ExecutePlayerAction()
    {
        var action = PlayerAction.GetPokerAction();
        actionText.text = string.Format("Player: {0}", action);
        playerActionText.text = string.Format("Player: {0}", action);
        gameObj.AddPot(action.Chip);
        switch (action.Command)
        {
            case PlayerCommand.Fold:
                Enemy.GetChip(gameObj.GetPot());
                actionText.text = "Player Lose...";
                Mode = 0;
                SetButtonMode(false);
                return;
            default:
                var callSize = Math.Max(0, action.Chip - (((gameObj.GetPot() - action.Chip) == (gameObj.SmallBlind + gameObj.BigBlind)) ? 1 : 0));
                Debug.Log(string.Format("EnemyCallSize:{0}", callSize));
                if (gameObj.Players[gameObj.DealerPosition].Name == "Player")
                {
                    var mode = HoldemStep.Preflop;
                    switch (Mode)
                    {
                        case 2:
                            mode = HoldemStep.Flop;
                            break;
                        case 3:
                            mode = HoldemStep.Turn;
                            break;
                        case 4:
                            mode = HoldemStep.River;
                            break;
                        case 5:
                            mode = HoldemStep.Showdown;
                            break;
                        default:
                            mode = HoldemStep.Preflop;
                            break;
                    }
                    EnemyAction.SetBoard(BoardCards, gameObj.GetPot());
                    var enemyAction = EnemyAction.GetPokerAction(mode, callSize, gameObj.BigBlind);
                    Debug.Log(enemyAction);
                    actionText.text = string.Format("Enemy: {0}", enemyAction);
                    enemyActionText.text = string.Format("Enemy: {0}", enemyAction);
                    switch (enemyAction.Command)
                    {
                        case PlayerCommand.Fold:
                            actionText.text = "Player Win!!";
                            Player.GetChip(gameObj.GetPot());
                            Mode = 0;
                            SetButtonMode(false);
                            break;
                        case PlayerCommand.Bet:
                        case PlayerCommand.Raise:
                            firstBetFlag = true;
                            gameObj.AddPot(enemyAction.Chip);
                            SetPlayerActionControl(enemyAction.Chip - action.Chip, gameObj.BigBlind);
                            break;
                        case PlayerCommand.AllIn:
                            var toCallSize = enemyAction.Chip - action.Chip;
                            if (toCallSize <= 0)
                            {
                                gameObj.AddPot(enemyAction.Chip);
                                gameObj.AddPot(toCallSize * -1);
                                Player.GetChip(toCallSize * -1);
                                Mode += 1;
                                switch (Mode)
                                {
                                    case 2:
                                        Flop();
                                        break;
                                    case 3:
                                        Turn();
                                        break;
                                    case 4:
                                        River();
                                        break;
                                    case 5:
                                        Showdown();
                                        break;
                                }
                                break;
                            } 
                            else
                            {
                                firstBetFlag = true;
                                gameObj.AddPot(enemyAction.Chip);
                                SetPlayerActionControl(enemyAction.Chip - action.Chip, gameObj.BigBlind);
                                break;
                            }
                            break;
                        default:
                            gameObj.AddPot(enemyAction.Chip);
                            Mode += 1;
                            switch (Mode)
                            {
                                case 2:
                                    Flop();
                                    break;
                                case 3:
                                    Turn();
                                    break;
                                case 4:
                                    River();
                                    break;
                                case 5:
                                    Showdown();
                                    break;
                            }
                            break;

                    }
                }
                else
                {
                    switch (action.Command)
                    {
                        case PlayerCommand.Bet:
                        case PlayerCommand.Raise:
                            firstBetFlag = true;
                            EnemyAction.SetBoard(BoardCards, gameObj.GetPot());
                            var enemyAction = EnemyAction.GetPokerAction(HoldemStep.Preflop, callSize, gameObj.BigBlind);
                            Debug.Log(enemyAction);
                            actionText.text = string.Format("Enemy: {0}", enemyAction);
                            enemyActionText.text = string.Format("Enemy: {0}", enemyAction);
                            switch (enemyAction.Command)
                            {
                                case PlayerCommand.Fold:
                                    actionText.text = "Player Win!!";
                                    Player.GetChip(gameObj.GetPot());
                                    Mode = 0;
                                    SetButtonMode(false);
                                    break;
                                case PlayerCommand.Bet:
                                case PlayerCommand.Raise:
                                    firstBetFlag = true;
                                    gameObj.AddPot(enemyAction.Chip);
                                    SetPlayerActionControl(enemyAction.Chip - action.Chip, gameObj.BigBlind);
                                    break;
                                case PlayerCommand.AllIn:
                                    var toCallSize = enemyAction.Chip - action.Chip;
                                    if (toCallSize <= 0)
                                    {
                                        gameObj.AddPot(enemyAction.Chip);
                                        gameObj.AddPot(toCallSize);
                                        Player.GetChip(toCallSize * -1);
                                        Mode += 1;
                                        switch (Mode)
                                        {
                                            case 2:
                                                Flop();
                                                break;
                                            case 3:
                                                Turn();
                                                break;
                                            case 4:
                                                River();
                                                break;
                                            case 5:
                                                Showdown();
                                                break;
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        firstBetFlag = true;
                                        gameObj.AddPot(enemyAction.Chip);
                                        SetPlayerActionControl(enemyAction.Chip - action.Chip, gameObj.BigBlind);
                                        break;
                                    }
                                    break;
                                default:
                                    gameObj.AddPot(enemyAction.Chip);
                                    Mode += 1;
                                    switch (Mode)
                                    {
                                        case 2:
                                            Flop();
                                            break;
                                        case 3:
                                            Turn();
                                            break;
                                        case 4:
                                            River();
                                            break;
                                        case 5:
                                            Showdown();
                                            break;
                                    }
                                    break;

                            }
                            break;
                        case PlayerCommand.AllIn:
                            if (callSize <= 0)
                            {
                                var diff = EnemyAction.BetSize - action.Chip;
                                gameObj.AddPot(diff * -1);
                                Enemy.GetChip(diff);

                                Mode += 1;
                                switch (Mode)
                                {
                                    case 2:
                                        Flop();
                                        break;
                                    case 3:
                                        Turn();
                                        break;
                                    case 4:
                                        River();
                                        break;
                                    case 5:
                                        Showdown();
                                        break;
                                }
                            }
                            else
                            {
                                firstBetFlag = true;
                                EnemyAction.SetBoard(BoardCards, gameObj.GetPot());
                                var enemyAction2 = EnemyAction.GetPokerAction(HoldemStep.Preflop, callSize, gameObj.BigBlind);
                                Debug.Log(enemyAction2);
                                actionText.text = string.Format("Enemy: {0}", enemyAction2);
                                enemyActionText.text = string.Format("Enemy: {0}", enemyAction2);
                                switch (enemyAction2.Command)
                                {
                                    case PlayerCommand.Fold:
                                        actionText.text = "Player Win!!";
                                        Player.GetChip(gameObj.GetPot());
                                        Mode = 0;
                                        SetButtonMode(false);
                                        break;
                                    case PlayerCommand.Bet:
                                    case PlayerCommand.Raise:
                                        firstBetFlag = true;
                                        gameObj.AddPot(enemyAction2.Chip);
                                        SetPlayerActionControl(enemyAction2.Chip - action.Chip, gameObj.BigBlind);
                                        break;
                                    case PlayerCommand.AllIn:
                                        var toCallSize = enemyAction2.Chip - action.Chip;
                                        if (toCallSize <= 0)
                                        {
                                            gameObj.AddPot(enemyAction2.Chip);
                                            gameObj.AddPot(toCallSize * -1);
                                            Player.GetChip(toCallSize * -1);
                                            Mode += 1;
                                            switch (Mode)
                                            {
                                                case 2:
                                                    Flop();
                                                    break;
                                                case 3:
                                                    Turn();
                                                    break;
                                                case 4:
                                                    River();
                                                    break;
                                                case 5:
                                                    Showdown();
                                                    break;
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            firstBetFlag = true;
                                            gameObj.AddPot(enemyAction2.Chip);
                                            SetPlayerActionControl(enemyAction2.Chip - action.Chip, gameObj.BigBlind);
                                            break;
                                        }
                                        break;
                                    default:
                                        gameObj.AddPot(enemyAction2.Chip);
                                        Mode += 1;
                                        switch (Mode)
                                        {
                                            case 2:
                                                Flop();
                                                break;
                                            case 3:
                                                Turn();
                                                break;
                                            case 4:
                                                River();
                                                break;
                                            case 5:
                                                Showdown();
                                                break;
                                        }
                                        break;

                                }
                            }
                            break;
                        default:
                            Mode += 1;
                            switch (Mode)
                            {
                                case 2:
                                    Flop();
                                    break;
                                case 3:
                                    Turn();
                                    break;
                                case 4:
                                    River();
                                    break;
                                case 5:
                                    Showdown();
                                    break;
                            }
                            break;
                    }
                }
                break;

        }
    }

    private static void SetButtonMode(bool commandEnable)
    {
        GameObject.Find("RaiseButton").GetComponent<Button>().enabled = commandEnable;
        GameObject.Find("Slider").GetComponent<Slider>().enabled = commandEnable;
        GameObject.Find("InputField").GetComponent<InputField>().enabled = commandEnable;
        GameObject.Find("CallButton").GetComponent<Button>().enabled = commandEnable;
        GameObject.Find("FoldButton").GetComponent<Button>().enabled = commandEnable;
        GameObject.Find("TempButton").GetComponent<Button>().enabled = !commandEnable;
    }
}
