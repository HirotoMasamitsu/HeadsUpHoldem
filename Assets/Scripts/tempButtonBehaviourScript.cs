using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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
            HeadsUpHoldemGame.Reset();
        }
        else
        {
            HeadsUpHoldemGame.ResetGame(GameObject.Find("PlayerChipText").GetComponent<Text>(), GameObject.Find("EnemyChipText").GetComponent<Text>());
            resetFlag = true;
            Debug.Log("tempButton ResetGame! " + gameObject.name);
            GameObject.Find("TempButtonText").GetComponent<Text>().text = "Next Game";
        }
    }

}
