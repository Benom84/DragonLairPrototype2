using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimerBarMovement : MonoBehaviour {

    private int maxValue = 100;
    public int currValue;

    private RectTransform barTransform;
    private float cachedY;
    private float minXPosValue;
    private float maxXPosValue;
    private bool firstRun = true;
    private bool passive = false;
    private RectTransform passiveTransform;
    private TimerBarMovement activeTimer;
    private int lastValue;
    private Text timerText;


    void Awake()
    {


        if (tag == "Timer") {
            barTransform = transform.GetComponent<RectTransform>();
            cachedY = barTransform.anchoredPosition.y;
            minXPosValue = barTransform.anchoredPosition.x;
            maxXPosValue = minXPosValue + barTransform.rect.width;
            timerText = GameObject.FindGameObjectWithTag("TimerText").GetComponent<Text>();
            if (timerText != null)
                timerText.text = "0 / " + maxValue;
        }
        else {
            passiveTransform = transform.GetComponent<RectTransform>();
            barTransform =  GameObject.FindGameObjectWithTag("Timer").GetComponent<RectTransform>();
            activeTimer = GameObject.FindGameObjectWithTag("Timer").GetComponent<TimerBarMovement>();
            passive = true;
            cachedY = transform.GetComponent<RectTransform>().anchoredPosition.y;
            minXPosValue = barTransform.anchoredPosition.x - barTransform.rect.width/2;
            maxXPosValue = minXPosValue + barTransform.rect.width;

        }
        
     
        

        currValue = 0;
        lastValue = 0;

    }
	
	// Update is called once per frame
	void Update () {
        if (passive)
        {
            if (firstRun)
                maxValue = activeTimer.GetMaxValue();

            currValue = activeTimer.GetCurrentValue();
            if (currValue != lastValue)
                updatePassiveBar();
            
        }

        
        
	
	}

    public void SetMaxValue(int maxValue)
    {
        this.maxValue = maxValue;
        if (timerText != null)
        {
            timerText.text = "0 / " + maxValue;
        }
            
    }

    public void setCurrentValue(int currentValue)
    {
        this.currValue = currentValue;

    }

    public void increaseCurrentValue()
    {
        currValue++;

        float valuePercent = Mathf.Max(((1.0f * currValue) / (1.0f * maxValue)), 0.0f);
        float newXPos = valuePercent * (minXPosValue - maxXPosValue) + minXPosValue;

        if (timerText != null)
        {
            timerText.text = currValue + " / " + maxValue;
        }
            

        if (barTransform != null)
            barTransform.anchoredPosition = new Vector3(newXPos, cachedY);

	
    }

    private void updatePassiveBar()
    {
        lastValue = currValue;
        float valuePercent = Mathf.Max(((1.0f * currValue) / (1.0f * maxValue)), 0.0f);
        float newXPos = valuePercent * (minXPosValue - maxXPosValue) + minXPosValue;

        if (barTransform != null)
            passiveTransform.anchoredPosition = new Vector3(newXPos, cachedY);

    }
    
    
    public int GetMaxValue()
    {
        return maxValue;
    }

    public int GetCurrentValue()
    {
        return currValue;
    }
}
