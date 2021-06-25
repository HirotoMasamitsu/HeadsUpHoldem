using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "PlayerHand")
        {
            var cme = new CardMouseEvent(gameObject, false);
            HeadsUpHoldemGame.SetPlayerHandsEvent(cme);
        }
        else if (gameObject.tag == "EnemyHand")
        {
            var cme = new CardMouseEvent(gameObject, false);
            HeadsUpHoldemGame.SetEnemyHandsEvent(cme);
        }
        else if (gameObject.tag == "FlopCard")
        {
            var cme = new CardMouseEvent(gameObject, false);
            HeadsUpHoldemGame.SetFlopCardsEvent(cme);
        }
        else if (gameObject.tag == "TurnCard")
        {
            var cme = new CardMouseEvent(gameObject, false);
            HeadsUpHoldemGame.SetTurnCardsEvent(cme);
        }
        else if (gameObject.tag == "RiverCard")
        {
            var cme = new CardMouseEvent(gameObject, false);
            HeadsUpHoldemGame.SetRiverCardsEvent(cme);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
