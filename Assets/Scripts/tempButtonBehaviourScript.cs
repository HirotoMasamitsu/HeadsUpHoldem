using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempButtonBehaviourScript : MonoBehaviour
{
    private bool resetFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        Debug.Log("tempButton OnClick! " + gameObject.name);
        if (resetFlag)
        {
            switch (HeadsUpHoldemGame.Mode)
            {
                case 1:
                    HeadsUpHoldemGame.Preflop();
                    break;
                case 2:
                    HeadsUpHoldemGame.Flop();
                    break;
                case 3:
                    HeadsUpHoldemGame.Turn();
                    break;
                case 4:
                    HeadsUpHoldemGame.River();
                    break;
                case 5:
                    HeadsUpHoldemGame.Showdown();
                    break;
                default:
                    HeadsUpHoldemGame.Reset();
                    break;
            }
        }
        else
        {
            HeadsUpHoldemGame.ResetGame();
            resetFlag = true;
        }
    }

}
