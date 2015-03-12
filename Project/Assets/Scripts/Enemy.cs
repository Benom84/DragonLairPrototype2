using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    public enum EnemyType { Knight, Cavalier, Healer, Archer };

    public float maxHealth = 100f;
    public int attackDamage = 1;
    public float attackDelay = 1.0f;
    public float walkingSpeed = 3.0f;
    public float resistenceToFire = 0.0f;
    public float resistenceToIce = 0.0f;
    public float resistenceToAcid = 0.0f;
    public EnemyType enemyType;

    private GameController gameController;
    private bool arrivedAtDestination = false;
    private float lastAttackTime = 0.0f;
    private float health;
    private Player player;

    private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
    private Vector3 healthScale;				// The local scale of the health bar initially (with full health).


    // Use this for initialization
    void Start()
    {

        walkingSpeed = walkingSpeed / 100;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        health = maxHealth;
        healthBar = transform.Find("ui_healthDisplay/Enemy_HealthBar").GetComponent<SpriteRenderer>();
        healthScale = healthBar.transform.localScale;
        healthBar.transform.localScale = new Vector3(healthScale.x * health/maxHealth, 1, 1);


    }

    // Update is called once per frame
    void Update()
    {

        if (gameController.gameEnded)
            return;

        if ((arrivedAtDestination) && (lastAttackTime + attackDelay <= Time.time))
            Attack();

    }

    void FixedUpdate()
    {
        if (!arrivedAtDestination)
        {
            Vector3 newPos = transform.position;
            newPos.x -= walkingSpeed;
            transform.position = newPos;

        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.tag == "Player")
            arrivedAtDestination = true;

        if ((col.gameObject.tag == "ArcherStop") && (enemyType == EnemyType.Archer))
            arrivedAtDestination = true;

        if ((col.gameObject.tag == "HealerStop") && (enemyType == EnemyType.Healer))
            arrivedAtDestination = true;
        

    }

    public void Hit(int damage)
    {

        health -= damage;
        UpdateHealthBar();
        if (health <= 0f)
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

    public void UpdateHealthBar()
    {
        // Set the health bar's colour to proportion of the way between green and red based on the player's health.
        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health / maxHealth);

        // Set the scale of the health bar to be proportional to the player's health.
        healthBar.transform.localScale = new Vector3(healthScale.x * health / maxHealth, 1, 1);
    }
}
