using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarMovement : MonoBehaviour {

	private int maxValue = 100;
	public int currValue;

	private RectTransform barTransform;
	private float cachedY;
	private float minXPosValue;
	private float maxXPosValue;

	void Awake () {


        barTransform = transform.GetComponent<RectTransform>();
        cachedY = barTransform.anchoredPosition.y;

        maxXPosValue = barTransform.anchoredPosition.x;
        minXPosValue = maxXPosValue - barTransform.rect.width;

        maxValue = 100;
        currValue = maxValue; 
	
	}
	
	void Update () {
        
	}



	public void setMaxValue (int amount) {

		maxValue = amount;

	}

	public void setValue(int amount) {
        
		currValue = amount;
		float valuePercent = Mathf.Max(((1.0f * currValue) / (1.0f * maxValue)), 0.0f);
		float newXPos = valuePercent * (maxXPosValue - minXPosValue) + minXPosValue;

		if (barTransform != null)
            barTransform.anchoredPosition = new Vector3 (newXPos, cachedY);

	
	}

    public void addToValue()
    {
        setValue(currValue + 1);
    }
}
