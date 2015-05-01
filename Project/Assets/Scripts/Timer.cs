using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

    private float startTime;
    private float currTime;

    private RectTransform barTransform;
    private float cachedY;



    public float numOfSeconds = 65;

	void Start () {
        startTime = Time.time;

        barTransform = transform.GetComponent<RectTransform>();
        cachedY = barTransform.anchoredPosition.y;

        barTransform.anchoredPosition = new Vector3(400f, cachedY);

	}
	
	void Update () {
        if (barTransform.anchoredPosition.x > 142)
        {
            currTime = Time.time;
            if (currTime - startTime > 1f)
            {
                float newXPos = barTransform.anchoredPosition.x - (258f / numOfSeconds);
                barTransform.anchoredPosition = new Vector3(newXPos, cachedY);
                startTime = currTime;
            }
        }
	}


}
