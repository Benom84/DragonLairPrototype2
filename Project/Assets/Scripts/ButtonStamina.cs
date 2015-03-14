using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonStamina : MonoBehaviour {

    private int maxValue = 100;
    public int currValue;

    private Image image;


    // Use this for initialization
    void Awake()
    {

        image = GetComponent<Image>();
        image.fillAmount = 1.00f;

        maxValue = 100;
        currValue = maxValue;

    }

    void Update()
    {


    }



    public void setMaxValue(int amount)
    {

        maxValue = amount;

    }

    public void setValue(int amount)
    {

        currValue = amount;
        float valuePercent = Mathf.Max(((1.0f * currValue) / (1.0f * maxValue)), 0.0f);
        image.fillAmount = valuePercent;


    }
}
