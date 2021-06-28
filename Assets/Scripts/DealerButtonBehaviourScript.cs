using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DealerButtonBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HeadsUpHoldemGame.SetDealerButton(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
