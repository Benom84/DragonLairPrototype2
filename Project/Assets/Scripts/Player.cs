using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	public int maxHealth = 100;
	public int maxIceStamina = 100;
	public int maxFireStamina = 100;
    public int maxAcidStamina = 100;
    public float staminaRefillTime = 1.0f;
    public int endlessStaminaRefill = 1;
    public GameObject fireAttack;
    public GameObject iceAttack;
    public GameObject acidAttack;
    public float fireAttackSpeed = 5.0f;
    public float iceAttackSpeed = 5.0f;
    public float acidAttackSpeed = 5.0f;
    public int fireAttackDamage = 1;
    public int iceAttackDamage = 2;
    public int acidAttackDamage = 3;
    public int physicalAttackDamage = 1;
    public int fireAttackStaminaCost = 2;
    public int iceAttackStaminaCost = 2;
    public int acidAttackStaminaCost = 3;
    public float physicalAttackRestTime = 1.0f;
    public float physicalAttackRadius = 3.0f;
    



	private int currHealth;
	private int currIceStamina;
	private int currFireStamina;
    private int currAcidStamina;
    private int activeAttackManaCost;
	private BarMovement healthBar;
	private BarMovement iceManaBar;
    private GameObject[] iceManaBarGUI;
	private BarMovement staminaBar;
    private Vector3 dragonMouth;
    private GameObject activeAttack;
    private GameObject[] dragonAttacks;
    private float lastFireStaminaChange;
    private GameController gameController;
    private float lastPhysicalAttackTime;
    private GameObject dragonSlash;
    private bool dragonSlashVisible;
    private GameObject[] dragonAttackButtons;
    private GameObject attack1Button;
    private GameObject attack2Button;
    private GameObject attack3Button;
    private ButtonStamina fireStamina;
    private ButtonStamina iceStamina;




	// Use this for initialization
	void Start () {

		// Setting the attacks
        activeAttack = fireAttack;
        dragonAttacks = new GameObject[] { fireAttack, iceAttack, acidAttack };
        
        // Making sure the slash attack is invisible
        dragonSlash = transform.Find("DragonSlash").gameObject;
        Color dragonSlashColor = dragonSlash.GetComponent<SpriteRenderer>().color;
        dragonSlashColor.a = 0;
        dragonSlash.GetComponent<SpriteRenderer>().color = dragonSlashColor;
        dragonSlashVisible = false;

        // Getting the object references
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        dragonAttackButtons = GameObject.FindGameObjectsWithTag("AttackChooser");
        dragonMouth = transform.FindChild("Mouth").position;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<BarMovement>();
        foreach (GameObject button in dragonAttackButtons)
        {
            if (button.GetComponent<DragonAttackButton>().attack == DragonAttack.AttackType.Fire)
                fireStamina = button.transform.Find("Fill").GetComponent<ButtonStamina>();

            if (button.GetComponent<DragonAttackButton>().attack == DragonAttack.AttackType.Ice)
                iceStamina = button.transform.Find("Fill").GetComponent<ButtonStamina>();
        }

        // Setting the initial values
        currHealth = maxHealth;
        currIceStamina = maxIceStamina;
        currFireStamina = maxFireStamina;
        lastFireStaminaChange = Time.time;
        healthBar.setMaxValue(maxHealth);
        healthBar.setValue(currHealth);
        
        fireStamina.setMaxValue(maxFireStamina);
        fireStamina.setValue(currFireStamina);
        iceStamina.setMaxValue(maxIceStamina);
        iceStamina.setValue(currIceStamina);



        //iceManaBar = GameObject.FindGameObjectWithTag("ManaBar").GetComponent<BarMovement>();
        //iceManaBarGUI = GameObject.FindGameObjectsWithTag("IceManaBar");
        /*
        foreach (GameObject guiElement in iceManaBarGUI)
        {
            guiElement.SetActive(false);
        }
        */
        //iceManaBar.setMaxValue(maxIceStamina);
        //iceManaBar.setValue(currIceStamina);

	
	}
	
	// Update is called once per frame
	void Update () {

        if (dragonSlashVisible)
        {
            Color dragonSlashColor = dragonSlash.GetComponent<SpriteRenderer>().color;
            dragonSlashColor.a -= 0.05f;
            dragonSlashColor.a = Mathf.Max(dragonSlashColor.a, 0);
            if (dragonSlashColor.a == 0)
                dragonSlashVisible = false;
            dragonSlash.GetComponent<SpriteRenderer>().color = dragonSlashColor;
        }
        

	
	}

    void FixedUpdate()
    {

        if (Time.time >= lastFireStaminaChange + staminaRefillTime)
        {
            currFireStamina += endlessStaminaRefill;
            currFireStamina = Mathf.Min(currFireStamina, maxFireStamina);
            fireStamina.setValue(currFireStamina);
            lastFireStaminaChange = Time.time;
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

 
        // Check and change if neccessary the current attack vs current stamina
        if (!CheckStaminaBeforeAttack())
            return;

             
        // We will calculate the direction vector the attack is supposed to go
        Vector2 attackDirection = new Vector2(attackPosition.x - dragonMouth.x, attackPosition.y - dragonMouth.y);

        // Now we normalize it and multiply by the attack speed
        attackDirection = attackDirection.normalized * getAttackTypeSpeed(activeAttack.GetComponent<DragonAttack>().attackType);
        
        // Now we instantiate the attack at the dragon's mouth, set it's velocity and damage
        GameObject attack = (GameObject) Instantiate(activeAttack, dragonMouth, transform.rotation);
        DragonAttack dragonAttack = attack.GetComponent<DragonAttack>();
        dragonAttack.attackDamage = getAttackTypeDamage(dragonAttack.attackType);
        attack.rigidbody2D.velocity = attackDirection;
        StaminaManager();
        
	}


    private void StaminaManager()
    {

        if (activeAttack.GetComponent<DragonAttack>().attackType == DragonAttack.AttackType.Ice)
        {

            currIceStamina -= iceAttackStaminaCost;
            iceStamina.setValue(currIceStamina);
        }

        if (activeAttack.GetComponent<DragonAttack>().attackType == DragonAttack.AttackType.Fire)
        {

            currFireStamina -= fireAttackStaminaCost;
            lastFireStaminaChange = Time.time;
            fireStamina.setValue(currFireStamina);
        }

        



    }
    public void PhysicalAttack()
    {
        if (Time.time > lastPhysicalAttackTime + physicalAttackRestTime)
        {

            lastPhysicalAttackTime = Time.time;
            Collider2D[] allColliding = Physics2D.OverlapCircleAll(transform.position,physicalAttackRadius);

            GameObject[] allEnemiesInRadius = new GameObject[allColliding.Length];
            int enemyCount = 0;
            foreach (Collider2D col in allColliding)
	        {

		        if (col.tag == "Enemy") {
                    allEnemiesInRadius[enemyCount] = col.gameObject;
                    enemyCount++;
                }
	        }



            Color dragonSlashColor = dragonSlash.GetComponent<SpriteRenderer>().color;
            dragonSlashColor.a = 1;
            dragonSlash.GetComponent<SpriteRenderer>().color = dragonSlashColor;
            dragonSlashVisible = true;


            for (int i = 0; i < enemyCount; i++)
            {
                Enemy enemyScript = allEnemiesInRadius[i].GetComponent<Enemy>();
                if (enemyScript != null)
                    enemyScript.Hit(physicalAttackDamage);
            }
        }

    }

    private bool CheckStaminaBeforeAttack()
    {
        
        // If we have enough stamina for the current attack we return true
        if (activeAttack == iceAttack)
        {
            if (currIceStamina >= iceAttackStaminaCost)
                return true;
            else
                activeAttack = fireAttack;
        }
            

        else if (activeAttack == acidAttack)
        {
            if (currAcidStamina >= acidAttackStaminaCost)
                return true;
            else
                activeAttack = fireAttack;
        }

        else
        {
            if (currFireStamina >= fireAttackStaminaCost)
                return true;
            else
                return false;
        }

        return false;
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
