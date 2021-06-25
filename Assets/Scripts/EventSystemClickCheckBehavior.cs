using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventSystemClickCheckBehavior : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    //private Dictionary<string, IMouseEvent> eventDic;
    private IMouseEvent mouseEvent;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData pointerData)
    {
        Debug.Log(gameObject.name + " Clicked!");
        if (this.mouseEvent != null)
        {
            this.mouseEvent.Clicked();
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        Debug.Log("Cursor Entering " + gameObject.name);
        if (this.mouseEvent != null)
        {
            this.mouseEvent.Entered();
        }
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Output the following message with the GameObject's name
        Debug.Log("Cursor Exiting " + gameObject.name);
        if (this.mouseEvent != null)
        {
            this.mouseEvent.Exited();
        }
    }
}
