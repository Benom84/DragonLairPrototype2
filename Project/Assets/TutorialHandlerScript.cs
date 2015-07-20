using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialHandlerScript : MonoBehaviour {

    public float regularAttackTime = 1f;
    public float specialAttackTime = 10f;
    public float crystalUsageTime = 30f;

    private bool passedRegularAttack = false;
    private bool passedSpecialAttack = false;
    private bool passedCrystalUsage = false;
    private GameObject continueButton;
    private GameObject regularAttackBaloon;
    private GameObject specialAttackBaloon;
    private GameObject crystalUsageBaloon;


    private float startTime;
    
    // Use this for initialization
	void Awake () {
        startTime = Time.time;
        continueButton = transform.Find("Button").gameObject;
        regularAttackBaloon = transform.Find("regularAttackBaloon").gameObject;
        specialAttackBaloon = transform.Find("specialAttackBaloon").gameObject;
        crystalUsageBaloon = transform.Find("crystalUsageBaloon").gameObject;
        continueButton.SetActive(false);
        regularAttackBaloon.SetActive(false);
        specialAttackBaloon.SetActive(false);
        crystalUsageBaloon.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!passedRegularAttack && Time.time > startTime + regularAttackTime)
        {
            passedRegularAttack = true;
            Time.timeScale = 0;
            regularAttackBaloon.SetActive(true);
            continueButton.SetActive(true);
        }

        if (!passedSpecialAttack && Time.time > startTime + specialAttackTime)
        {
            passedSpecialAttack = true;
            Time.timeScale = 0;
            specialAttackBaloon.SetActive(true);
            continueButton.SetActive(true);
        }

        if (!passedCrystalUsage && Time.time > startTime + crystalUsageTime)
        {
            passedCrystalUsage = true;
            Time.timeScale = 0;
            crystalUsageBaloon.SetActive(true);
            continueButton.SetActive(true);
        }
	}

    public void Continue()
    {
        Time.timeScale = 1;
        continueButton.SetActive(false);
        regularAttackBaloon.SetActive(false);
        specialAttackBaloon.SetActive(false);
        crystalUsageBaloon.SetActive(false);
        Debug.Log("hi");
    }

    void Update()
    {

    }
}
