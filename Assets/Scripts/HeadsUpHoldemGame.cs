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

    private static Text playerHandText;
    private static Text enemyHandText;
    private static Text potText;
    private static GameObject dealerButton;


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

    public static void SetPotText(Text textUI)
    {
        potText = textUI;
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

    public static void ResetGame()
    {
        gameObj.GameReset();
        Player = new PlayerStatus("Player", 100);
        Enemy = new PlayerStatus("Enemy", 100);
        gameObj.SetPlayer(Player);
        gameObj.SetPlayer(Enemy);
        gameObj.SetPlayerPosition();
    }

    public static void Reset()
    {
        Mode = 0;
        gameObj.ResetPot();
        Debug.Log(string.Format("DealerPositon({0}) = {1}", gameObj.DealerPosition, gameObj.Players[gameObj.DealerPosition].Name));
        if (gameObj.Players[gameObj.DealerPosition].Name == "Player")
        {
            dealerButton.transform.position = new Vector3(99, 73);
        }
        else
        {
            dealerButton.transform.position = new Vector3(272, 210);
        }
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
    }

    public static void Preflop()
    {
        if (Mode == 1)
        {
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

            Mode = 2;
        }
    }

    public static void Flop()
    {
        if (Mode == 2)
        {
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

            Mode = 3;
        }
    }

    public static void Turn()
    {
        if (Mode == 3)
        {
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

            Mode = 4;
        }
    }

    public static void River()
    {
        if (Mode == 4)
        {
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

            Mode = 5;
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
            Array.Copy(Enemy.Cards, 0, tempArray, 0, Enemy.Cards.Length);
            Array.Copy(BoardCards, 0, tempArray, 2, BoardCards.Length);
            var result = PKCheck.CheckSevenHands(tempArray);
            enemyHandText.text = result.ToString();

            Mode = 6;
        }
    }

}
