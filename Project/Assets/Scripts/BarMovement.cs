using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarMovement : MonoBehaviour {

	
	public int currValue;
    private int maxValue = 100;
    private Text valueText;

    void Start()
    {
        //valueText = GetComponentInChildren<Text>();
    }
    
    void Awake () {


        maxValue = 100;
        currValue = maxValue;
        valueText = GetComponentInChildren<Text>();
	
	}



	public void setMaxValue (int amount) {

		maxValue = amount;

	}

	public void setValue(int amount) {
        
		currValue = amount;
        if (valueText == null)
            valueText = GetComponentInChildren<Text>();    
        valueText.text = "" + amount;
		float valuePercent = Mathf.Max(((1.0f * currValue) / (1.0f * maxValue)), 0.0f);
        GetComponent<Image>().fillAmount = valuePercent;
	
	}

}
