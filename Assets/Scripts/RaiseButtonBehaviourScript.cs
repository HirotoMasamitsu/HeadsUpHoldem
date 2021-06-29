using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaiseButtonBehaviourScript : MonoBehaviour
{
    private InputField betInput;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().enabled = false;
        this.betInput = GameObject.Find("InputField").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        Debug.Log("RaiseButton OnClick! " + gameObject.name);
        var inputTextNum = 0;
        if (int.TryParse(this.betInput.text, out inputTextNum))
        {
            HeadsUpHoldemGame.PlayerRaise(inputTextNum);
        }
    }
}
