﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	public int maxHealth = 100;
    public int maxMana = 100;
    public int crystalToManaRatio = 20;
    public int manaRefillAmount = 1;
    public float manaRefillDelay = 1.0f;
    public float attackSpeed = 5.0f;
    public float attackDelay = 1.0f;
    public int attackDamage = 1;
    public int earthquakeAttackDamage = 2;
    public int screamAttackDamage = 1;
    public int specialAttackDamage = 1;
    public int earthquakeManaCost = 30;
    public int screamManaCost = 30;
    public int specialAttackManaCost = 30;
    public GameObject fireAttack;
    public GameObject waterAttack;
    public GameObject airAttack;



	private int currHealth;
    private int currMana;
	private BarMovement healthBar;
    private BarMovement manaBar;
    private Vector3 dragonMouth;
    private GameObject activeAttack;
    private GameController gameController;
    private CameraShake camShake;
    private float lastAttack = 0f;
    private float lastManaChange = 0f;
    private DragonAttack.AttackType specialAttackType;






	
	void Awake () {

		// Setting the attacks
        activeAttack = fireAttack;
        
        // Getting the object references
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        dragonMouth = transform.FindChild("Mouth").position;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<BarMovement>();
        manaBar = GameObject.FindGameObjectWithTag("ManaBar").GetComponent<BarMovement>();
        camShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();

        // set special attack type
        specialAttackType = DragonAttack.AttackType.Fire;

        if (DataController.dataController != null)
        {
            loadFromDataController();
        }
        
        
        // Setting the initial values
        currHealth = maxHealth;
        currMana = maxMana;
        healthBar.setMaxValue(maxHealth);
        healthBar.setValue(currHealth);
        manaBar.setMaxValue(maxMana);
        manaBar.setValue(currMana);

        

        



	
	}

    private void loadFromDataController()
    {

        earthquakeAttackDamage = DataController.dataController.m_earthquake;
        screamAttackDamage = DataController.dataController.m_scream;
        maxHealth = DataController.dataController.p_cave;

        
        if (DataController.dataController.attackType == DragonAttack.AttackType.Fire)
        {
            activeAttack = fireAttack;
            specialAttackType = DragonAttack.AttackType.Fire;
            attackSpeed = DataController.dataController.b_fireRange * 1.0f;
            attackDamage = DataController.dataController.b_fireDamage;
            /*
                    b_fireHeavnlyFire;
                    b_fireThunder;
                    p_fireLava;
                    m_fireMeteor;
             */
            
        }
        
        if (DataController.dataController.attackType == DragonAttack.AttackType.Water)
        {
            activeAttack = waterAttack;
            specialAttackType = DragonAttack.AttackType.Water;
            attackSpeed = DataController.dataController.b_waterRange * 1.0f;
            attackDamage = DataController.dataController.b_waterDamage;
            /*
                    add special breaths and defend
             */

        }
        if (DataController.dataController.attackType == DragonAttack.AttackType.Air)
        {
            activeAttack = airAttack;
            specialAttackType = DragonAttack.AttackType.Air;
            attackSpeed = DataController.dataController.b_airRange * 1.0f;
            attackDamage = DataController.dataController.b_airDamage;
            /*
                    add special breaths and defend
             */

        }
    }

    void FixedUpdate()
    {

        if ((currMana < maxMana) && (Time.time > lastManaChange + manaRefillDelay))
        {
            currMana += manaRefillAmount;
            if (currMana > maxMana)
                currMana = maxMana;
            lastManaChange = Time.time;
            manaBar.setValue(currMana);
        }

    }
	

	public void Hit(int damage) {

        currHealth -= damage;
        currHealth = Mathf.Max(currHealth, 0);
		healthBar.setValue (currHealth);
        if (currHealth == 0)
            Die();
	
	}

	public void StartAttack(Vector2 attackPosition) {

        if (gameController.gameEnded)
            return;

        if (Time.time < lastAttack + attackDelay)
            return;
            
             
        // We will calculate the direction vector the attack is supposed to go
        Vector2 attackDirection = new Vector2(attackPosition.x - dragonMouth.x, attackPosition.y - dragonMouth.y);

        // Now we normalize it and multiply by the attack speed
        attackDirection = attackDirection.normalized * attackSpeed;
        
        // Now we instantiate the attack at the dragon's mouth, set it's velocity and damage
        GameObject attack = (GameObject) Instantiate(activeAttack, dragonMouth, transform.rotation);
        DragonAttack dragonAttack = attack.GetComponent<DragonAttack>();
        dragonAttack.attackDamage = attackDamage;
        attack.rigidbody2D.velocity = attackDirection;
        lastAttack = Time.time;
        
	}

    
    private void Die()
    {
        gameController.gameEnded = true;
        gameController.gameLost = true;
        GameObject.Destroy(gameObject);
    }


    public void Scream()
    {
        if (gameController.gameEnded)
            return;

        if (currMana < screamManaCost)
            return;

        currMana -= screamManaCost;

    }

    public void Earthquake()
    {
        if (gameController.gameEnded)
            return;

        if (currMana < earthquakeManaCost)
            return;

        currMana -= earthquakeManaCost;
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in allEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
                enemyScript.Hit(earthquakeAttackDamage, DragonAttack.AttackType.Earthquake);
        }

        camShake.shake = 1.0f;



    }

    public void SpecialAttack(ArrayList enemiesToDamage)
    {
        if (gameController.gameEnded)
            return;

        if (currMana < specialAttackManaCost)
            return;

        currMana -= specialAttackManaCost;

        if (specialAttackType == DragonAttack.AttackType.Fire)
            SpecialAttackFire();

        if (specialAttackType == DragonAttack.AttackType.Water)
            SpecialAttackWater();

        if (specialAttackType == DragonAttack.AttackType.Air)
            SpecialAttackAir();

        foreach (GameObject enemy in enemiesToDamage)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
                enemyScript.SpecialHit(specialAttackDamage, specialAttackType);
        }

    }

    private void SpecialAttackFire()
    {

    }

    private void SpecialAttackWater()
    {

    }

    private void SpecialAttackAir()
    {

    }

    public void AddMana()
    {
        currMana += crystalToManaRatio;
        currMana = Mathf.Min(currMana, maxMana);
    }

    public int getCurrentMana()
    {
        return currMana;
    }

    public int getCurrentHealth()
    {
        return currHealth;
    }
}
