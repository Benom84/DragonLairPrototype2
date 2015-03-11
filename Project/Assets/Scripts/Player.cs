using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int maxHealth = 100;
	public int maxMana = 100;
	public float maxStamina = 3.0f;
    public float staminaRefillTime = 1.0f;
    public float staminaPrice = 0.5f;
    public GameObject attack1;
    public GameObject attack2;
    public GameObject attack3;
    public float fireAttackSpeed = 5.0f;
    public float iceAttackSpeed = 5.0f;
    public float acidAttackSpeed = 5.0f;
    public int fireAttackDamage = 1;
    public int iceAttackDamage = 2;
    public int acidAttackDamage = 3;
    public int attack1ManaCost = 0;
    public int attack2ManaCost = 2;
    public int attack3ManaCost = 3;
    



	private int currHealth;
	private int currMana;
	private float currStamina;
    private int activeAttackManaCost;
	private BarMovement healthBar;
	private BarMovement manaBar;
	private BarMovement staminaBar;
    private Vector3 dragonMouth;
    private GameObject activeAttack;
    private GameObject[] dragonAttacks;
    private float lastStaminaChange;
    private GameController gameController;




	// Use this for initialization
	void Start () {

		activeAttack = attack1;
        dragonAttacks = new GameObject[] { attack1, attack2, attack3 };

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        
        dragonMouth = transform.FindChild("Mouth").position;

        currHealth = maxHealth;
        currMana = maxMana;

        currStamina = maxStamina;
        lastStaminaChange = Time.time;

        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<BarMovement>();
        manaBar = GameObject.FindGameObjectWithTag("ManaBar").GetComponent<BarMovement>();

        manaBar.setMaxValue(maxMana);
        manaBar.setValue(currMana);
        healthBar.setMaxValue(maxHealth);
        healthBar.setValue(currHealth);
	
	}
	
	// Update is called once per frame
	void Update () {

	
	}

    void FixedUpdate()
    {

        if (lastStaminaChange + staminaRefillTime >= Time.time)
            currStamina = maxStamina;

    }

	public void Hit(int damage) {

        currHealth -= damage;
        currHealth = Mathf.Max(currHealth, 0);
		healthBar.setValue (currHealth);
        if (currHealth == 0)
            Die();
	
	}

	public void StartAttack(Vector2 attackPosition) {

        // Check if we have stamina
        if (currStamina == 0.0f)
            return;
        
        // Check and change if neccessary the current attack vs current mana
        CheckManaBeforeAttack();
        
        // We will calculate the direction vector the attack is supposed to go
        Vector2 attackDirection = new Vector2(attackPosition.x - dragonMouth.x, attackPosition.y - dragonMouth.y);

        // Now we normalize it and multiply by the attack speed
        attackDirection = attackDirection.normalized * getAttackTypeSpeed(activeAttack.GetComponent<DragonAttack>().attackType);
        
        // Now we instantiate the attack at the dragon's mouth, set it's velocity and damage
        GameObject attack = (GameObject) Instantiate(activeAttack, dragonMouth, transform.rotation);
        DragonAttack dragonAttack = attack.GetComponent<DragonAttack>();
        dragonAttack.attackDamage = getAttackTypeDamage(dragonAttack.attackType);
        attack.rigidbody2D.velocity = attackDirection;
        currMana -= activeAttackManaCost;
        manaBar.setValue(currMana);
        currStamina = Mathf.Max(currStamina - staminaPrice, 0.0f);
        lastStaminaChange = Time.time;
        
	}

    private void CheckManaBeforeAttack()
    {
        
        if (activeAttack == attack2)
        {
            if (currMana >= attack2ManaCost)
            {

                activeAttackManaCost = attack2ManaCost;
                return;
            }


        }

        else if (activeAttack == attack3)
        {
            if (currMana >= attack3ManaCost)
            {

                activeAttackManaCost = attack3ManaCost;
                return;
            }

        }

        activeAttackManaCost = attack1ManaCost;
        activeAttack = attack1;
    }

    private int getAttackTypeDamage(DragonAttack.AttackType attackType) {

        if (attackType == DragonAttack.AttackType.Fire)
            return fireAttackDamage;

        if (attackType == DragonAttack.AttackType.Ice)
            return iceAttackDamage;

        else
            return acidAttackDamage;
    
    }

    private float getAttackTypeSpeed(DragonAttack.AttackType attackType)
    {

        if (attackType == DragonAttack.AttackType.Fire)
            return fireAttackSpeed;

        if (attackType == DragonAttack.AttackType.Ice)
            return iceAttackSpeed;

        else
            return acidAttackSpeed;

    }

    public void setActiveAttack(DragonAttack.AttackType attackType)
    {
        for (int i = 0; i < dragonAttacks.Length; i++)
        {
            if ((dragonAttacks[i] != null) && (dragonAttacks[i].GetComponent<DragonAttack>().attackType == attackType)) 
                activeAttack = dragonAttacks[i];
        }
    }

    private void Die()
    {
        gameController.gameEnded = true;
        gameController.gameLost = true;
        GameObject.Destroy(gameObject);
    }
}
