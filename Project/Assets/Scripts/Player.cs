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
    public int fireContinuousDamage = 0;
    public float fireContinuousDamageTime = 0;
    public float fireSlowFactor = 0;
    public float fireSlowTime = 0;
    public float waterAttackSpeed = 5.0f;
    public float waterAttackDelay = 1.0f;
    public int waterAttackDamage = 1;
    public int waterContinuousDamage = 0;
    public float waterContinuousDamageTime = 0;
    public float waterSlowFactor = 0;
    public float waterSlowTime = 0;
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
    public Sprite lockedAttack;
    public GameObject fireSpecialAttackEffect;
    public GameObject waterSpecialAttackEffect;



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
    private int activeContinuousDamage = 0;
    private float activeContinuousDamageTime = 0;
    private float activeSlowFactor = 0;
    private float activeSlowTime = 0;
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
    private PolygonCollider2D specialAttackAreaCollider;
    private float specialAreaMinX = float.MaxValue;
    private float specialAreaMaxX = float.MinValue;
    private float specialAreaMinY = float.MaxValue;
    private float specialAreaMaxY = float.MinValue;
    private bool changeAttackButtonAvailable = false;
    private bool screamAttackButtonAvailable = false;
    private bool earthquakeButtonAvailable = false;









    void Awake()
    {



        // Getting the object references
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        dragonMouth = transform.FindChild("Mouth").position;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<BarMovement>();
        manaBar = GameObject.FindGameObjectWithTag("ManaBar").GetComponent<BarMovement>();
        camShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        specialAttackAreaCollider = GameObject.FindGameObjectWithTag("SpecialAttackArea").GetComponent<PolygonCollider2D>();
        getSpecialAttackAreaPoints();

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
            if (button.name == "SpecialAttack")
            {
                specialAttackButton = button.transform.FindChild("SpecialAttackButton").gameObject;
                specialAttackAura = button.transform.FindChild("Aura").gameObject;
            }


            else if (button.name == "Earthquake")
            {
                earthquakeButton = button.transform.FindChild("EarthquakeButton").gameObject;
                earthquakeAura = button.transform.FindChild("Aura").gameObject;
                if (!earthquakeButtonAvailable)
                {
                    earthquakeButton.GetComponent<Image>().sprite = lockedAttack;
                }
            }


            else if (button.name == "Scream")
            {
                screamAttackButton = button.transform.FindChild("ScreamButton").gameObject;
                screamAttackAura = button.transform.FindChild("Aura").gameObject;
                if (!screamAttackButtonAvailable)
                {
                    screamAttackButton.GetComponent<Image>().sprite = lockedAttack;
                }
            }

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
        if (!changeAttackButtonAvailable)
        {
            changeAttackButtonAvailable = true;
            SwitchAttack();
            changeAttackButtonAvailable = false;
            changeAttackButton.GetComponent<Image>().sprite = lockedAttack;
        }
        else
        {
            SwitchAttack();
        }
        

    }

    private void getSpecialAttackAreaPoints()
    {
        foreach (Vector2 point in specialAttackAreaCollider.points)
        {
            if (point.x > specialAreaMaxX)
                specialAreaMaxX = point.x;

            if (point.y > specialAreaMaxY)
                specialAreaMaxY = point.y;

            if (point.x < specialAreaMinX)
                specialAreaMinX = point.x;

            if (point.y < specialAreaMinY)
                specialAreaMinY = point.y;
        }


    }

    private void loadFromDataController()
    {

        earthquakeAttackDamage = DataController.dataController.m_earthquakeData;
        earthquakeManaCost = DataController.dataController.earthquakeManaValue;
        screamAttackDamage = DataController.dataController.m_screamData;
        screamManaCost = DataController.dataController.screamManaValue;
        maxHealth = DataController.dataController.p_caveData;


        // Load Fire Input
        //fireAttackSpeed = 
        fireAttackDamage = DataController.dataController.b_fireDamageData;
        fireAttackDelay = DataController.dataController.b_fireAgilityData * 1.0f;
        fireSpecialAttackDamage = DataController.dataController.m_meteorData;
        fireSpecialAttackManaCost = DataController.dataController.meteorManaValue;

        // Load Water Input
        //waterAttackSpeed = ;
        waterAttackDamage = DataController.dataController.b_waterDamageData;
        waterAttackDelay = DataController.dataController.b_waterAgilityData * 1.0f;
        waterSpecialAttackDamage = DataController.dataController.m_iceData;
        waterSpecialAttackManaCost = DataController.dataController.iceManaValue;


        // Disable UnavailableAttacks
        earthquakeButtonAvailable = (DataController.dataController.m_earthquakeLevel > 0);
        screamAttackButtonAvailable = (DataController.dataController.m_screamLevel > 0);
        changeAttackButtonAvailable = (DataController.dataController.isWaterUnlocked);

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
        dragonAttack.continuousDamage = activeContinuousDamage;
        dragonAttack.continuosDamageTime = activeContinuousDamageTime;
        dragonAttack.slowFactor = activeSlowFactor;
        dragonAttack.slowTime = activeSlowTime;
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

        if (!screamAttackEnabled || !screamAttackButtonAvailable)
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

        if (!earthquakeEnabled || !earthquakeButtonAvailable)
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

        for (int i = 0; i < 20; i++)
        {
            float x = Random.Range(specialAreaMinX, specialAreaMaxX);
            float y = Random.Range(specialAreaMinY, specialAreaMaxY);
            if (specialAttackAreaCollider == Physics2D.OverlapPoint(new Vector2(x, y), LayerMask.GetMask("SpecialEffects")))
            {
                GameObject effect = (GameObject) Instantiate(fireSpecialAttackEffect, new Vector3(x - 1, y + 2, 0), transform.rotation);
                effect.renderer.sortingLayerName = "Enemy";
                
            }
        }

    }

    private void SpecialAttackWater()
    {
        for (int i = 0; i < 20; i++)
        {
            float x = Random.Range(specialAreaMinX, specialAreaMaxX);
            float y = Random.Range(specialAreaMinY, specialAreaMaxY);
            if (specialAttackAreaCollider == Physics2D.OverlapPoint(new Vector2(x, y), LayerMask.GetMask("SpecialEffects")))
            {
                GameObject effect = (GameObject) Instantiate(waterSpecialAttackEffect, new Vector3(x, y, 0), transform.rotation);
                effect.renderer.sortingLayerName = "Enemy";
            }
                
            
        }
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
        if (!changeAttackButtonEnabled || !changeAttackButtonAvailable)
            return;


        Image attackButtonImage = changeAttackButton.GetComponent<Image>();
        Image specialAttackButtonImage = specialAttackButton.GetComponent<Image>();
        Sprite selectedButtonSprite;
        Sprite selectedSpecialAttackSprite;

        if (activeAttackType == DragonAttack.AttackType.Fire)
        {
            // Attack parameters
            activeAttack = waterAttack;
            activeAttackDamage = waterAttackDamage;
            activeAttackSpeed = waterAttackSpeed;
            activeAttackDelay = waterAttackDelay;
            activeAttackType = DragonAttack.AttackType.Water;
            selectedButtonSprite = fireButtonImage;


            // Special Attack parameters
            selectedSpecialAttackSprite = waterSpecialAttackImage;
            activeSpecialAttackDamage = waterSpecialAttackDamage;
            activeSpecialAttackManaCost = waterSpecialAttackManaCost;

            // Cotinuous attack and slowing down parameters
            activeContinuousDamage = waterContinuousDamage;
            activeContinuousDamageTime = waterContinuousDamageTime;
            activeSlowFactor = waterSlowFactor;
            activeSlowTime = waterSlowTime;

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


            // Special Attack parameters
            selectedSpecialAttackSprite = fireSpecialAttackImage;
            activeSpecialAttackDamage = fireSpecialAttackDamage;
            activeSpecialAttackManaCost = fireSpecialAttackManaCost;

            // Cotinuous attack and slowing down parameters
            activeContinuousDamage = fireContinuousDamage;
            activeContinuousDamageTime = fireContinuousDamageTime;
            activeSlowFactor = fireSlowFactor;
            activeSlowTime = fireSlowTime;
        }

        changeAttackButtonEnabled = false;

        // Change the attack button
        changeAttackButtonLastPress = Time.time;
        changeAttackButtonColor.a = 0.5f;
        attackButtonImage.sprite = selectedButtonSprite;
        attackButtonImage.color = changeAttackButtonColor;

        // Change the special attack button
        specialAttackButtonImage.sprite = selectedSpecialAttackSprite;




    }
}
