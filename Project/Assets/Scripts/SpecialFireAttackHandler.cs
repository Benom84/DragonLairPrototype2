using UnityEngine;
using System.Collections;

public class SpecialFireAttackHandler : MonoBehaviour {

    public GameObject meteor;
    public float maxAngleChange = 30;
    public float speed = 12.0f;
    public int numberOfMeteors = 20;
    public float meteorCreationDelta = 0.02f;
    public float changeAngleFactor = 1f;
    public float speedVariance = 1.5f;
    [HideInInspector]
    public Vector3 mouthPoisiton;
    [HideInInspector]
    public Transform headTransform;
    [HideInInspector]
    public int damage;

    private float lastMeteorCreationTime = 0;
    private int numberOfMeteorsCreated = 0;
    private float creationTime = 0;
    private float timeToLive = 2f;
    private GameController gameController;
    
    // Use this for initialization
	void Start () {

        creationTime = Time.time;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {

        if (mouthPoisiton != null && headTransform != null)
        {
            if (Time.time > lastMeteorCreationTime + meteorCreationDelta && numberOfMeteorsCreated < numberOfMeteors)
            {
                // We will create a meteor from the dragon mouth
                Vector2 attackDirection = headTransform.right;

                // Now we instantiate the attack at the dragon's mouth, set it's velocity and damage
                GameObject attack = (GameObject)Instantiate(meteor, mouthPoisiton, transform.rotation);
                MeteorAttack dragonAttack = attack.GetComponent<MeteorAttack>();

                float rand_angle = Random.Range(40f, 70f);
                rand_angle = rand_angle * Mathf.Deg2Rad;
                Vector2 dir = new Vector2((float)Mathf.Cos(rand_angle), (float)Mathf.Sin(rand_angle));

                float thisAttackSpeed = Random.Range(speed - speedVariance, speed + speedVariance);
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                attack.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                attack.rigidbody2D.AddForce(dir.normalized * thisAttackSpeed * 100);

                
                //dragonAttack.speed = thisAttackSpeed;
                numberOfMeteorsCreated++;
                lastMeteorCreationTime = Time.time;
            }
        }

        if (Time.time > timeToLive + creationTime)
        {
            foreach (GameObject enemy in gameController.getAllEnemiesOnBoard())
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.SpecialHit(damage, DragonAttack.AttackType.Fire);
                    enemyScript.continuousDamageHit(2, 2.0f, DragonAttack.AttackType.Fire);

                }


            }
            GameObject.Destroy(gameObject);
        }
        
	
	}
}
