using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderChangedBehavior : MonoBehaviour
{
    private Slider slider;
    private InputField betInput;

    // Start is called before the first frame update
    void Start()
    {
        this.slider = gameObject.GetComponent<Slider>();
        this.slider.enabled = false;
        this.betInput = GameObject.Find("InputField").GetComponent<InputField>();
        slider.onValueChanged.AddListener((value) => {
            OnChanged(value);
        });
        this.slider.enabled = false;
        this.betInput.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnChanged(float value)
    {
        Debug.Log(string.Format("SliderChangedBehavior OnChanged: value:{0} betInput.text:{1}", value, this.betInput.text));
        var inputTextNum = 0;
        if (int.TryParse(this.betInput.text, out inputTextNum))
        {
            Debug.Log(string.Format("SliderChangedBehavior OnChanged: inputTextNum:{0}", inputTextNum));
            var intValue = (int)value;
            if (inputTextNum != intValue)
            {
                this.betInput.text = intValue.ToString();
            }
        }
    }
}
