using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTextBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HeadsUpHoldemGame.SetPlayerHandText(gameObject.GetComponent<Text>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
