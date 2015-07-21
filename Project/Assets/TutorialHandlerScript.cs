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
    private GameObject currentMana;


    private float startTime;
    
    // Use this for initialization
	void Awake () {
        startTime = Time.time;
        regularAttackBaloon = transform.Find("Canvas/regularAttackBaloon").gameObject;
        specialAttackBaloon = transform.Find("Canvas/specialAttackBaloon").gameObject;
        crystalUsageBaloon = transform.Find("Canvas/crystalUsageBaloon").gameObject;
        currentMana = transform.Find("Canvas/crystalUsageBaloon/ManaBar").gameObject;
        regularAttackBaloon.SetActive(false);
        specialAttackBaloon.SetActive(false);
        crystalUsageBaloon.SetActive(false);
        int currentLevel = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().currentLevel;
        if (DataController.dataController != null)
        {
            currentLevel = DataController.dataController.level;
        }
        
        if (currentLevel != 1)
        {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!passedRegularAttack && Time.time > startTime + regularAttackTime)
        {
            passedRegularAttack = true;
            Time.timeScale = 0;
            regularAttackBaloon.SetActive(true);
        }

        if (!passedSpecialAttack && Time.time > startTime + specialAttackTime)
        {
            passedSpecialAttack = true;
            Time.timeScale = 0;
            specialAttackBaloon.SetActive(true);
        }

        if (!passedCrystalUsage && Time.time > startTime + crystalUsageTime)
        {
            passedCrystalUsage = true;
            Time.timeScale = 0;
            
            crystalUsageBaloon.SetActive(true);
            currentMana.GetComponent<BarMovement>().setValue(GameObject.FindGameObjectWithTag("ManaBar").GetComponent<BarMovement>().currValue);
        }
	}

    public void Continue()
    {
        Time.timeScale = 1;
        regularAttackBaloon.SetActive(false);
        specialAttackBaloon.SetActive(false);
        crystalUsageBaloon.SetActive(false);

    }

    void Update()
    {

    }
}
