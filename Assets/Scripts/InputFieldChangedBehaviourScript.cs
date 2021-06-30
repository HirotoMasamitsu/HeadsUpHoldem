using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldChangedBehaviourScript : MonoBehaviour
{
    private Slider slider;
    private InputField betInput;

    // Start is called before the first frame update
    void Start()
    {
        this.slider = GameObject.Find("Slider").GetComponent<Slider>();
        this.betInput = gameObject.GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnChanged()
    {
        Debug.Log(string.Format("InputFieldChangedBehavior OnChanged: value:{0} betInput.text:{1}", slider.value, this.betInput.text));
        var inputTextNum = 0;
        if (int.TryParse(this.betInput.text, out inputTextNum))
        {
            Debug.Log(string.Format("InputFieldChangedBehavior OnChanged: inputTextNum:{0}", inputTextNum));
            var intValue = (int)slider.value;
            if (inputTextNum != intValue)
            {
                slider.value = inputTextNum;
            }
        }
    }

}
