using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpecialAttackButton : MonoBehaviour {

    public enum SpecialAttackType { Earthquake, Tail, Fireball };

    public SpecialAttackType attackType;
    public int damage = 2;
    public int numberOfAttacksLeft = 1;
    public float attackDelay = 1.0f;

    private CameraShake camShake;
    private Text numberOfAttacksText;
    private float lastAttackTime;
    
    // Use this for initialization
	void Start () {

        camShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        numberOfAttacksText = transform.Find("NumberOfAttacksText").GetComponent<Text>();
        numberOfAttacksText.text = "" + numberOfAttacksLeft; 
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setButtonPosition(Vector3 position)
    {

        transform.position = position;

    }
    
    public void StartAttack(Vector3 attackPosition)
    {

        if ((attackType == SpecialAttackType.Earthquake) && (numberOfAttacksLeft > 0) && (Time.time > lastAttackTime + attackDelay))
        {

            GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in allEnemies)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if ((enemyScript != null) &&(enemyScript.enemyType != Enemy.EnemyType.Healer))
                    enemyScript.Hit(damage);
            }

            camShake.shake = 1.0f;
            lastAttackTime = Time.time;
            numberOfAttacksLeft--;
            numberOfAttacksText.text = "" + numberOfAttacksLeft;

        }
            

    }
}
