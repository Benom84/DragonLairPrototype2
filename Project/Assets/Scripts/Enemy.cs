using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public enum EnemyType { Knight, Rider, Healer, Archer };
    
    public int health = 100;
    public int attackDamage = 1;
    public float attackDelay = 1.0f;
    public float walkingSpeed = 3.0f;
    public float resistenceToFire = 0.0f;
    public float resistenceToIce = 0.0f;
    public float resistenceToAcid = 0.0f;
    public EnemyType enemyType;

    private Player player;
    private GameController gameController;
    private bool arrivedAtDestination = false;
    private float lastAttackTime = 0.0f;

    
    // Use this for initialization
	void Start () {

        walkingSpeed = walkingSpeed / 100;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	
	}
	
	// Update is called once per frame
	void Update () {

        if (gameController.gameEnded)
            return;
        
        if ((arrivedAtDestination) && (lastAttackTime + attackDelay <= Time.time))
            Attack();
	
	}

    void FixedUpdate()
    {
        if (!arrivedAtDestination) {
            Vector3 newPos = transform.position;
            newPos.x -= walkingSpeed;
            transform.position = newPos;

        }
            


    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.tag == "Player")
            arrivedAtDestination = true;
        

    }

    public void Hit(int damage)
    {

        health -= damage;
        if (health <= 0)
            Death();

    }

    private void Death()
    {

        GameObject.Destroy(gameObject);
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        player.Hit(attackDamage);

    }
}
