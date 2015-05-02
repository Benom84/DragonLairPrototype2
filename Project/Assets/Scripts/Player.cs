using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{

    public int maxHealth = 100;
    public int maxMana = 100;
    public float fireAttackSpeed = 5.0f;
    public float fireAttackDelay = 1.0f;
    public int fireAttackDamage = 1;
    public float waterAttackSpeed = 5.0f;
    public float waterAttackDelay = 1.0f;
    public int waterAttackDamage = 1;
    public int crystalToManaRatio = 20;
    public int manaRefillAmount = 1;
    public float manaRefillDelay = 1.0f;
    public int fireSpecialAttackDamage = 1;
    public int fireSpecialAttackManaCost = 30;
    public int waterSpecialAttackDamage = 1;
    public int waterSpecialAttackManaCost = 30;
    public int earthquakeAttackDamage = 2;
    public int screamAttackDamage = 1;
    public int earthquakeManaCost = 30;
    public int screamManaCost = 30;
    public float changeAttackDelay = 3.0f;
    public float specialAttackChargeTime = 5.0f;
    public GameObject fireAttack;
    public GameObject waterAttack;
    public GameObject airAttack;
    public Sprite fireButtonImage;
    public Sprite waterButtonImage;
    public Sprite fireSpecialAttackImage;
    public Sprite waterSpecialAttackImage;



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
    private float activeAttackSpeed = 5.0f;
    private float activeAttackDelay = .4f;
    private int activeAttackDamage = 1;
    private int activeSpecialAttackDamage = 1;
    private int activeSpecialAttackManaCost = 30;
    private DragonAttack.AttackType activeAttackType;
    private GameObject changeAttackButton;
    private bool changeAttackButtonEnabled = true;
    private double changeAttackButtonLastPress;
    private Color changeAttackButtonColor;
    private GameObject specialAttackButton;
    private GameObject specialAttackAura;
    private bool specialAttackEnabled = true;
    private GameObject earthquakeButton;
    private GameObject earthquakeAura;
    private bool earthquakeEnabled = true;
    private GameObject screamAttackButton;
    private GameObject screamAttackAura;
    private bool screamAttackEnabled = true;
    private float specialAttackCharge = 0;
    private int specialAttackChargeFinish = 0;
    private float earthquakeAttackCharge = 0;
    private float screamAttackCharge = 0;









    void Awake()
    {

       

        // Getting the object references
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        dragonMouth = transform.FindChild("Mouth").position;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<BarMovement>();
        manaBar = GameObject.FindGameObjectWithTag("ManaBar").GetComponent<BarMovement>();
        camShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();

        // Load from data file
        if (DataController.dataController != null)
        {
            loadFromDataController();
        }
        
        // Get the Change Attack button and color
        changeAttackButton = GameObject.FindGameObjectWithTag("ChangeAttackButton");
        changeAttackButtonColor = new Color();
        changeAttackButtonColor.r = changeAttackButton.GetComponent<Image>().color.r;
        changeAttackButtonColor.g = changeAttackButton.GetComponent<Image>().color.g;
        changeAttackButtonColor.b = changeAttackButton.GetComponent<Image>().color.b;
        changeAttackButtonLastPress = Time.time;

        // Get the special attacks buttons
        GameObject[] specialAttackButtons = GameObject.FindGameObjectsWithTag("SpecialAttack");
        foreach (GameObject button in specialAttackButtons)
        {
            if (button.name == "SpecialAttack"){
                specialAttackButton = button.transform.FindChild("SpecialAttackButton").gameObject;
                specialAttackAura = button.transform.FindChild("Aura").gameObject;
            }
                

            else if (button.name == "Earthquake") {
                earthquakeButton = button.transform.FindChild("EarthquakeButton").gameObject;
                earthquakeAura = button.transform.FindChild("Aura").gameObject;
            }
                

            else if (button.name == "Scream") {
                screamAttackButton = button.transform.FindChild("ScreamButton").gameObject;
                screamAttackAura = button.transform.FindChild("Aura").gameObject;
            }
                
        }

        if (specialAttackButton != null)
        {
            specialAttackButton.GetComponent<Image>().sprite = fireSpecialAttackImage;
            specialAttackButton.GetComponentInChildren<Text>().text = "" + fireSpecialAttackManaCost;
        }

        if (earthquakeButton != null)
        {
            earthquakeButton.GetComponentInChildren<Text>().text = "" + earthquakeManaCost;
        }

        if (screamAttackButton != null)
        {
            screamAttackButton.GetComponentInChildren<Text>().text = "" + screamManaCost;
        }




        // Setting the initial values
        currHealth = maxHealth;
        currMana = maxMana;
        healthBar.setMaxValue(maxHealth);
        healthBar.setValue(currHealth);
        manaBar.setMaxValue(maxMana);
        manaBar.setValue(currMana);


        // Setting the attacks - setting as water and switching to fire;
        activeAttackType = DragonAttack.AttackType.Water;
        SwitchAttack();

    }

    private void loadFromDataController()
    {

        earthquakeAttackDamage = DataController.dataController.m_earthquake;
        screamAttackDamage = DataController.dataController.m_scream;
        maxHealth = DataController.dataController.p_cave;


        // Load Fire Input
        fireAttackSpeed = DataController.dataController.b_fireRange * 1.0f;
        fireAttackDamage = DataController.dataController.b_fireDamage;
        fireAttackDelay = activeAttackDelay;
        fireSpecialAttackDamage = DataController.dataController.b_airSkyFall;
        fireSpecialAttackManaCost = activeSpecialAttackManaCost;

        // Load Water Input
        waterAttackSpeed = DataController.dataController.b_waterRange * 1.0f;
        waterAttackDamage = DataController.dataController.b_waterDamage;
        waterAttackDelay = activeAttackDelay;
        waterSpecialAttackDamage = DataController.dataController.b_airCursedBreath;
        waterSpecialAttackManaCost = activeSpecialAttackManaCost;

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

        if (!changeAttackButtonEnabled)
        {
            if (Time.time > changeAttackButtonLastPress + changeAttackDelay)
            {
                changeAttackButtonEnabled = true;
                changeAttackButtonColor.a = 1.0f;
                changeAttackButton.GetComponent<Image>().color = changeAttackButtonColor;
            }
            
        }

        if (!specialAttackEnabled)
        {
            specialAttackCharge += 1.0f / specialAttackChargeTime;
            float chargeAmount = Mathf.Min(specialAttackCharge / specialAttackChargeFinish, 1.0f);
            specialAttackAura.GetComponent<Image>().fillAmount = chargeAmount;
            if (chargeAmount >= 1.0f)
                specialAttackEnabled = true;
        }

        if (!earthquakeEnabled)
        {
            earthquakeAttackCharge += 1.0f / specialAttackChargeTime;
            float chargeAmount = Mathf.Min(earthquakeAttackCharge / earthquakeManaCost, 1.0f);
            earthquakeAura.GetComponent<Image>().fillAmount = chargeAmount;
            if (chargeAmount >= 1.0f)
                earthquakeEnabled = true;
        }

        if (!screamAttackEnabled)
        {
            screamAttackCharge += 1.0f / specialAttackChargeTime;
            float chargeAmount = Mathf.Min(screamAttackCharge / screamManaCost, 1.0f);
            screamAttackAura.GetComponent<Image>().fillAmount = chargeAmount;
            if (chargeAmount >= 1.0f)
                screamAttackEnabled = true;
        }

    }


    public void Hit(int damage)
    {

        currHealth -= damage;
        currHealth = Mathf.Max(currHealth, 0);
        healthBar.setValue(currHealth);
        if (currHealth == 0)
            Die();

    }

    public void StartAttack(Vector2 attackPosition)
    {

        if (gameController.gameEnded)
            return;

        if (Time.time < lastAttack + activeAttackDelay)
            return;


        // We will calculate the direction vector the attack is supposed to go
        Vector2 attackDirection = new Vector2(attackPosition.x - dragonMouth.x, attackPosition.y - dragonMouth.y);

        // Now we normalize it and multiply by the attack speed
        attackDirection = attackDirection.normalized * activeAttackSpeed;

        // Now we instantiate the attack at the dragon's mouth, set it's velocity and damage
        GameObject attack = (GameObject)Instantiate(activeAttack, dragonMouth, transform.rotation);
        DragonAttack dragonAttack = attack.GetComponent<DragonAttack>();
        dragonAttack.attackDamage = activeAttackDamage;
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

        if (!screamAttackEnabled)
            return;

        currMana -= screamManaCost;

        foreach (GameObject enemy in gameController.getAllEnemiesOnBoard())
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
                enemyScript.Hit(screamAttackDamage, DragonAttack.AttackType.Scream);
        }

        screamAttackEnabled = false;
        screamAttackCharge = 0;

    }

    public void Earthquake()
    {
        if (gameController.gameEnded)
            return;

        if (currMana < earthquakeManaCost)
            return;

        if (!earthquakeEnabled)
            return;
        
        currMana -= earthquakeManaCost;
        foreach (GameObject enemy in gameController.getAllEnemiesOnBoard())
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
                enemyScript.Hit(earthquakeAttackDamage, DragonAttack.AttackType.Earthquake);
        }

        Handheld.Vibrate();
        earthquakeEnabled = false;
        earthquakeAttackCharge = 0;
        camShake.shake = 1.0f;



    }

    public void SpecialAttack()
    {
        if (gameController.gameEnded)
            return;

        if (currMana < activeSpecialAttackManaCost)
            return;

        if (!specialAttackEnabled)
            return;

        currMana -= activeSpecialAttackManaCost;

        if (activeAttackType == DragonAttack.AttackType.Fire)
            SpecialAttackFire();

        if (activeAttackType == DragonAttack.AttackType.Water)
            SpecialAttackWater();


        specialAttackEnabled = false;
        specialAttackCharge = 0;
        specialAttackChargeFinish = activeSpecialAttackManaCost;
        //specialAttackButton.GetComponent<Image>().fillAmount = 0;

        foreach (GameObject enemy in gameController.getAllEnemiesOnBoard())
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
                enemyScript.SpecialHit(activeSpecialAttackDamage, activeAttackType);
        }

    }
    
    private void SpecialAttackFire()
    {

    }

    private void SpecialAttackWater()
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

    public double getCurrentHealthPrecentage()
    {
        return ((1.0f * currHealth) / maxHealth);
    }

    public int getCurrentSpecialAttackManaCost()
    {
        return activeSpecialAttackManaCost;
    }


    public void SwitchAttack()
    {
        if (!changeAttackButtonEnabled)
            return;


        Image attackButtonImage = changeAttackButton.GetComponent<Image>();
        Image specialAttackButtonImage = specialAttackButton.GetComponent<Image>();
        Sprite selectedButtonSprite;
        Sprite selectedSpecialAttackSprite;
        string selectedAttackText;
        
        if (activeAttackType == DragonAttack.AttackType.Fire)
        {
            // Attack parameters
            activeAttack = waterAttack;
            activeAttackDamage = waterAttackDamage;
            activeAttackSpeed = waterAttackSpeed;
            activeAttackDelay = waterAttackDelay;
            activeAttackType = DragonAttack.AttackType.Water;
            selectedButtonSprite = fireButtonImage;
            selectedAttackText = "Fire";
            


            // Special Attack parameters
            selectedSpecialAttackSprite = waterSpecialAttackImage;
            activeSpecialAttackDamage = waterSpecialAttackDamage;
            activeSpecialAttackManaCost = waterSpecialAttackManaCost;

        }
        else
        {
            
            // Attack Parameters
            activeAttack = fireAttack;
            activeAttackDamage = fireAttackDamage;
            activeAttackSpeed = fireAttackSpeed;
            activeAttackDelay = fireAttackDelay;
            activeAttackType = DragonAttack.AttackType.Fire;
            selectedButtonSprite = waterButtonImage;
            selectedAttackText = "Water";


            // Special Attack parameters
            selectedSpecialAttackSprite = fireSpecialAttackImage;
            activeSpecialAttackDamage = fireSpecialAttackDamage;
            activeSpecialAttackManaCost = fireSpecialAttackManaCost;
        }

        changeAttackButtonEnabled = false;
       
        // Change the attack button
        changeAttackButtonLastPress = Time.time;
        changeAttackButtonColor.a = 0.5f;
        attackButtonImage.sprite = selectedButtonSprite;
        attackButtonImage.color = changeAttackButtonColor;
        changeAttackButton.GetComponentInChildren<Text>().text = selectedAttackText;

        // Change the special attack button
        specialAttackButtonImage.sprite = selectedSpecialAttackSprite;
        specialAttackButton.GetComponentInChildren<Text>().text = "" + activeSpecialAttackManaCost;

        
        

    }
}
