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

    private static Text playerHandText;
    private static Text enemyHandText;

    public static Card[] PlayerHands { get; private set; }
    public static Card[] EnemyHands { get; private set; }
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



    public static void Reset()
    {
        Mode = 0;
        deck = new Deck(rand);
        deck.Shuffle();
        PlayerHands = new Card[2];
        EnemyHands = new Card[2];
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
            PlayerHands = new Card[2];
            EnemyHands = new Card[2];
            PlayerHands[0] = deck.Draw();
            EnemyHands[0] = deck.Draw();
            PlayerHands[1] = deck.Draw();
            EnemyHands[1] = deck.Draw();
            for (var i = 0; i < PlayerHands.Length; i++)
            {
                playerHandsEventList[i].SetCard(PlayerHands[i]);
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
            Array.Copy(PlayerHands, 0, tempArray, 0, PlayerHands.Length);
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
            Array.Copy(PlayerHands, 0, tempArray, 0, PlayerHands.Length);
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
            Array.Copy(PlayerHands, 0, tempArray, 0, PlayerHands.Length);
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
                enemyHandsEventList[i].SetCard(EnemyHands[i]);
                enemyHandsEventList[i].SetFace(true);
            }

            var tempArray = new Card[7];
            Array.Copy(EnemyHands, 0, tempArray, 0, EnemyHands.Length);
            Array.Copy(BoardCards, 0, tempArray, 2, BoardCards.Length);
            var result = PKCheck.CheckSevenHands(tempArray);
            enemyHandText.text = result.ToString();

            Mode = 6;
        }
    }

}
