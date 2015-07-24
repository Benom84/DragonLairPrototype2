using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;
using UnityEngine.UI;




public class UIscript : MonoBehaviour
{
    private const int amountOfCanvases = 3;

    private GameObject FireRoarCanvas;
    private GameObject SpecialAttacksCanvas;
    private GameObject ManaLifeCanvas;

    private Text totalCoinsText;
    private Text totalCrystalsText;
    private Text currentLevelText;

    private GameObject FireDamage;
    private GameObject FireAgility;
    private GameObject WaterDamage;
    private GameObject WaterAgility;
    private GameObject HeavenlyFire;
    private GameObject Thunder;
    private GameObject FrozenSky;
    private GameObject CursedBreath;
    private GameObject HealthBar;
    private GameObject Scream;
    private GameObject Meteor;
    private GameObject Ice;
    private GameObject Tail;
    private GameObject ManaBar;

    private GameObject[] buttons;

    private GameObject fireRoarIcon;
    private GameObject manaLifeIcon;
    private GameObject specialAttacksIcon;

    private GameObject[] icons;

    private GameObject NotEnoughMoneyCanvas;

    private Text upgradeCostText;
    private Image upgradeButtonImage;
    private Text upgradeDescription;
    private Text upgradeNameInDescription;

    private GameObject upgradeButton;

    private GameObject[] canvases;
    private bool[] selectUpgradeToUpgrade;
    private bool[] isUnlocked;

    public Sprite lockedUpgrade;

    public Sprite regularUpgradeButton;
    public Sprite notEnoughMoneyUpgradeButton;
    private bool enoughMoney;
    
    private UpgradeLevel[] fireDamageLevels;
    private UpgradeLevel[] fireAgilityLevels;
    private UpgradeLevel[] waterDamageLevels;
    private UpgradeLevel[] waterAgilityLevels;
    private UpgradeLevel[] heavenlyFireLevels;
    private UpgradeLevel[] frozenSkyLevels;
    private UpgradeLevel[] thunderLevels;
    private UpgradeLevel[] cursedBreathLevels;
    private UpgradeLevel[] caveLevels;
    private UpgradeLevel[] screamLevels;
    private UpgradeLevel[] meteorLevels;
    private UpgradeLevel[] iceLevels;
    private UpgradeLevel[] tailLevels;
    private UpgradeLevel[] manaLevels;

    private UpgradeLevel[][] allUpgrades;

    public Sprite coin;
    public Sprite crystal;
    public bool LoadDataFromPreviousSession = true;

    public void Start()
    {
        if (gameObject.tag == "FirstScreen")
        {
            if (LoadDataFromPreviousSession)
            {
                DataController.dataController.Load();
                GameObject.FindGameObjectWithTag("MusicButton").GetComponent<MusicButtonHandler>().SetImage(DataController.dataController.isMusicOn);
                GameObject.FindGameObjectWithTag("SoundEffectsButton").GetComponent<MusicButtonHandler>().SetImage(DataController.dataController.isSoundEffectsOn);
            }
                
        }
        else if (gameObject.tag == "Store")
        {
            // DataController.dataController.SetLevels();
            FireRoarCanvas = GameObject.Find("FireRoarCanvas");
            SpecialAttacksCanvas = GameObject.Find("SpecialAttacksCanvas");
            ManaLifeCanvas = GameObject.Find("ManaLifeCanvas");

            canvases = new GameObject[] { FireRoarCanvas, SpecialAttacksCanvas, ManaLifeCanvas };

            //NotEnoughMoneyCanvas = GameObject.Find("NotEnoughMoneyCanvas");
            //NotEnoughMoneyCanvas.SetActive(false);

            fireRoarIcon = GameObject.Find("FireRoarIcon");
            manaLifeIcon = GameObject.Find("ManaLifeIcon");
            specialAttacksIcon = GameObject.Find("SpecialAttacksIcon");

            icons = new GameObject[] { fireRoarIcon, specialAttacksIcon, manaLifeIcon };

            for (int i = 1; i < 3; i++)
            {
                Image selectedImage = icons[i].GetComponent<Image>();
                Color color = selectedImage.color;
                color.a = 0;
                selectedImage.color = color;
            }

                selectUpgradeToUpgrade = new bool[14];

            for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
            {
                selectUpgradeToUpgrade[i] = false;
            }

            totalCoinsText = GameObject.Find("Coins").GetComponentInChildren<Text>();
            totalCoinsText.text = DataController.dataController.coins.ToString();

            totalCrystalsText = GameObject.Find("Crystals").GetComponentInChildren<Text>();
            totalCrystalsText.text = DataController.dataController.crystals.ToString();

            //currentLevelText = GameObject.Find("CurrentLevelText").GetComponent<Text>();
            //currentLevelText.text = "Lv." + DataController.dataController.level;

            FireDamage = GameObject.Find("Damage");
            FireAgility = GameObject.Find("Agility");
            HeavenlyFire = GameObject.Find("HeavenlyFire");
            Thunder = GameObject.Find("Thunder");
            HealthBar = GameObject.Find("HealthBar");
            Scream = GameObject.Find("Scream");
            Meteor = GameObject.Find("Meteor");
            Tail = GameObject.Find("Tail");
            ManaBar = GameObject.Find("ManaBar");

            buttons = new GameObject[] { FireDamage, FireAgility, WaterDamage, WaterAgility, HeavenlyFire, FrozenSky, Thunder,
                CursedBreath, HealthBar, Scream, Meteor, Ice, Tail, ManaBar};

            upgradeButton = GameObject.Find("Upgrade");
            enoughMoney = false;

            isUnlocked = new bool[14];

            for (int i = 0; i < isUnlocked.Length; i++)
            {
                isUnlocked[i] = true;

                if (buttons[i] != null && i != 1)
                {
                    Image selectedImage = buttons[i].GetComponent<Image>();
                    Color color = selectedImage.color;
                    color.a = 0;
                    selectedImage.color = color;
                }  
            }

            //if conditions to unlock the upgrades
            if (DataController.dataController.level < 4)
            {
                isUnlocked[2] = false;
                isUnlocked[3] = false;
                isUnlocked[9] = false;

                Scream.transform.FindChild("Image").GetComponent<Image>().sprite = lockedUpgrade;

            }

            if (DataController.dataController.level < 5)
            {

                isUnlocked[4] = false;
                isUnlocked[6] = false;
                isUnlocked[5] = false;
                isUnlocked[7] = false;

                HeavenlyFire.transform.FindChild("Image").GetComponent<Image>().sprite = lockedUpgrade;
                Thunder.transform.FindChild("Image").GetComponent<Image>().sprite = lockedUpgrade;
            }

            if (DataController.dataController.level < 7)
            {
                isUnlocked[12] = false;
                isUnlocked[11] = false;


                Tail.transform.FindChild("Image").GetComponent<Image>().sprite = lockedUpgrade;
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] != null)
                {
                    if (DataController.dataController.upgradesLevel[i] > 0)
                    {
                        buttons[i].transform.FindChild("Text").GetComponent<Text>().text = DataController.dataController.upgradesLevel[i].ToString();
                    }
                    else
                    {
                        buttons[i].transform.FindChild("Text").GetComponent<Text>().text = string.Empty;
                    }
                }
            }

            fireDamageLevels = ReadUpgradeLevels("FireDamageUpgrade");
            fireAgilityLevels = ReadUpgradeLevels("FireAgilityUpgrade");
            waterDamageLevels = ReadUpgradeLevels("WaterDamageUpgrade");
            waterAgilityLevels = ReadUpgradeLevels("WaterAgilityUpgrade");
            heavenlyFireLevels = ReadUpgradeLevels("HeavenlyFireUpgrade");
            frozenSkyLevels = ReadUpgradeLevels("FrozenSkyUpgrade");
            thunderLevels = ReadUpgradeLevels("ThunderUpgrade");
            cursedBreathLevels = ReadUpgradeLevels("CursedBreathUpgrade");
            caveLevels = ReadUpgradeLevels("CaveUpgrade");
            screamLevels = ReadUpgradeLevels("ScreamUpgrade");
            meteorLevels = ReadUpgradeLevels("MeteorUpgrade");
            iceLevels = ReadUpgradeLevels("IceUpgrade");
            tailLevels = ReadUpgradeLevels("EarthquakeUpgrade");
            manaLevels = ReadUpgradeLevels("SpecialAttackUpgrade");

            allUpgrades = new UpgradeLevel[][] {fireDamageLevels, fireAgilityLevels, waterDamageLevels, waterAgilityLevels, heavenlyFireLevels, 
                frozenSkyLevels, thunderLevels, cursedBreathLevels, caveLevels, screamLevels, meteorLevels, iceLevels, tailLevels, manaLevels};

            upgradeCostText = GameObject.Find("CostText").GetComponent<Text>();
            upgradeButtonImage = GameObject.Find("CostImage").GetComponent<Image>();
            upgradeDescription = GameObject.Find("Description").GetComponent<Text>();
            upgradeNameInDescription = GameObject.Find("UpgradeName").GetComponent<Text>();

            ChooseToUpgrade(2);

            for (int i = 0; i < amountOfCanvases; i++)
            {
                canvases[i].SetActive(false);
            }

            FireRoarCanvas.SetActive(true);
        }
    }

    public void LoadLevel(string sceneName)
    {
        DataController.dataController.Save();
        DataController.dataController.fromLevel = false;

        if (DataController.dataController.level == 1)
        {
            GameObject sound = GameObject.Find("Sound");
            Destroy(sound);
            Application.LoadLevel("GameLevel");
        }
        else
        {
            Application.LoadLevel(sceneName);
        }
    }
    
    public void LoadScene(string sceneName)
    {
        DataController.dataController.Save();
        DataController.dataController.fromLevel = false;

        if (gameObject.tag == "Store" && sceneName == "GameLevel")
        {
            GameObject sound = GameObject.Find("Sound");
            Destroy(sound);
        }
            Application.LoadLevel(sceneName);
        
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void SwitchCanvas(int canvasNumber)
    {
        for (int i = 0; i < 3; i++)
        {
            Image selectedImage = icons[i].GetComponent<Image>();
            Color color = selectedImage.color;
            color.a = 0;
            selectedImage.color = color;
        }

        Image icon = icons[canvasNumber - 1].GetComponent<Image>();
        Color colorOfIcon = icon.color;
        colorOfIcon.a = 255;
        icon.color = colorOfIcon;
        
        foreach (GameObject canvas in canvases)
        {
            canvas.SetActive(false);
        }

        canvases[canvasNumber - 1].SetActive(true);
        for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
        {
            selectUpgradeToUpgrade[i] = false;
        }

        upgradeCostText.text = string.Empty;
        upgradeNameInDescription.text = string.Empty;
        upgradeDescription.text = string.Empty;

        upgradeButtonImage.sprite = null;
        Color buttonColor = upgradeButtonImage.color;
        buttonColor.a = 0;
        upgradeButtonImage.color = buttonColor;

        for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
        {
            selectUpgradeToUpgrade[i] = false;

            if (buttons[i] != null)
            {
                Image selectedImage = buttons[i].GetComponent<Image>();
                Color color = selectedImage.color;
                color.a = 0;
                selectedImage.color = color;
            } 
        }

        upgradeButton.GetComponent<Image>().sprite = regularUpgradeButton;
    }

    public void ChooseToUpgrade(int numberOfUpgraded)
    {
        for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
        {
            selectUpgradeToUpgrade[i] = false;

            if (buttons[i] != null)
            {
                Image selectedImage = buttons[i].GetComponent<Image>();
                Color color = selectedImage.color;
                color.a = 0;
                selectedImage.color = color;
            }
            
        }

        if (isUnlocked[numberOfUpgraded - 1])
        {
            Image selectedImage = buttons[numberOfUpgraded - 1].GetComponent<Image>();
            Color color = selectedImage.color;
            color.a = 255;
            selectedImage.color = color;

            selectUpgradeToUpgrade[numberOfUpgraded - 1] = true;
        }

        string strToDisplayInDescription = allUpgrades[numberOfUpgraded - 1][DataController.dataController.upgradesLevel[numberOfUpgraded - 1]].Description;
        string strToDisplayInUpgrade = allUpgrades[numberOfUpgraded - 1][DataController.dataController.upgradesLevel[numberOfUpgraded - 1]].Cost.ToString();
        string strToDisplayInDescriptionName = allUpgrades[numberOfUpgraded - 1][DataController.dataController.upgradesLevel[numberOfUpgraded - 1]].Name;

        if (!isUnlocked[numberOfUpgraded - 1])
        {
            if (numberOfUpgraded == 3 || numberOfUpgraded == 4 || numberOfUpgraded == 10)
            {
                strToDisplayInDescription = "UNLOCKS AT LEVEL 4" + Environment.NewLine + allUpgrades[numberOfUpgraded - 1][DataController.dataController.upgradesLevel[numberOfUpgraded - 1]].Description;
            }
            else if (numberOfUpgraded > 4 && numberOfUpgraded < 9)
            {
                strToDisplayInDescription = "UNLOCKS AT LEVEL 5" + Environment.NewLine + allUpgrades[numberOfUpgraded - 1][DataController.dataController.upgradesLevel[numberOfUpgraded - 1]].Description;
            }
            else if (numberOfUpgraded == 12 || numberOfUpgraded == 13)
            {
                strToDisplayInDescription = "UNLOCKS AT LEVEL 7" + Environment.NewLine + allUpgrades[numberOfUpgraded - 1][DataController.dataController.upgradesLevel[numberOfUpgraded - 1]].Description;
            }
        }

        upgradeCostText.text = strToDisplayInUpgrade;
        upgradeNameInDescription.text = strToDisplayInDescriptionName;
        upgradeDescription.text = strToDisplayInDescription;

        int costOfUpgrade = allUpgrades[numberOfUpgraded - 1][DataController.dataController.upgradesLevel[numberOfUpgraded - 1]].Cost;

        Image buttonImage = upgradeButton.GetComponent<Image>();
        if (numberOfUpgraded < 10)
        {
            upgradeButtonImage.sprite = coin;
            if (!EnoughMoney(costOfUpgrade))
            {
                buttonImage.sprite = notEnoughMoneyUpgradeButton;
                enoughMoney = false;
            }
            else
            {
                buttonImage.sprite = regularUpgradeButton;
                enoughMoney = true;
            }
        }
        else
        {
            upgradeButtonImage.sprite = crystal;
            if (!EnoughCrystals(costOfUpgrade))
            {
                buttonImage.sprite = notEnoughMoneyUpgradeButton;
                enoughMoney = false;
            }
            else
            {
                buttonImage.sprite = regularUpgradeButton;
                enoughMoney = true;
            }
        }

        Color buttonColor = upgradeButtonImage.color;
        buttonColor.a = 255;
        upgradeButtonImage.color = buttonColor;
    }

    public void Upgrade()
    {
        int numberOfUpgrade = 0;
        bool wasSelected = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (selectUpgradeToUpgrade[i])
            {
                numberOfUpgrade = i;
                wasSelected = true;
                break;
            }
        }

        int costOfUpgrade = allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Cost;
        if (wasSelected && isUnlocked[numberOfUpgrade] && enoughMoney)
        {
            if (costOfUpgrade > 100)
                {
                    if (DataController.dataController.coins >= costOfUpgrade)
                    {
                        DataController.dataController.coins -= costOfUpgrade;
                        if (numberOfUpgrade != 1 && numberOfUpgrade != 3)
                        {
                            DataController.dataController.upgradesData[numberOfUpgrade] += allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Data;
                        }
                        else
                        {
                            DataController.dataController.upgradesData[numberOfUpgrade] -= allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Data;
                        }

                        DataController.dataController.upgradesLevel[numberOfUpgrade]++;

                        buttons[numberOfUpgrade].transform.FindChild("Text").GetComponent<Text>().text = DataController.dataController.upgradesLevel[numberOfUpgrade].ToString();

                        totalCoinsText.text = DataController.dataController.coins.ToString();

                        costOfUpgrade = allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Cost;
                        upgradeCostText.text = costOfUpgrade.ToString();

                        Image buttonImage = upgradeButton.GetComponent<Image>();
                        if (!EnoughMoney(costOfUpgrade))
                        {
                            buttonImage.sprite = notEnoughMoneyUpgradeButton;
                            enoughMoney = false;
                        }
                    }
                }
            else if (DataController.dataController.crystals >= costOfUpgrade)
                {
                    DataController.dataController.crystals -= costOfUpgrade;
                    DataController.dataController.upgradesData[numberOfUpgrade] += (int)allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Data;
                    DataController.dataController.upgradesLevel[numberOfUpgrade]++;

                    buttons[numberOfUpgrade].transform.FindChild("Text").GetComponent<Text>().text = DataController.dataController.upgradesLevel[numberOfUpgrade].ToString();

                    totalCrystalsText.text = DataController.dataController.crystals.ToString();

                    costOfUpgrade = allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Cost;
                    upgradeCostText.text = costOfUpgrade.ToString();

                    Image buttonImage = upgradeButton.GetComponent<Image>();
                    if (!EnoughCrystals(costOfUpgrade))
                    {
                        buttonImage.sprite = notEnoughMoneyUpgradeButton;
                        enoughMoney = false;
                    }
                }
            
        }
    }

    private bool EnoughMoney(int priceOFUpgrade)
    {
        bool res;
        if (DataController.dataController.coins >= priceOFUpgrade)
        {
            res = true;
        }
        else
        {
            res = false;
        }
        return res;
    }

    private bool EnoughCrystals(int priceOFUpgrade)
    {
        bool res;
        if (DataController.dataController.crystals >= priceOFUpgrade)
        {
            res = true;
        }
        else
        {
            res = false;
        }
        return res;
    }

    private void NotEnoughMoney()
    {
        NotEnoughMoneyCanvas.SetActive(true);
    }

    public void DisableNotEnoughMoneyCanvas()
    {
        NotEnoughMoneyCanvas.SetActive(false);
    }

    public UpgradeLevel[] ReadUpgradeLevels(string nameOfUpgrade)
    {
        TextAsset upgradeText = (TextAsset)Resources.Load(nameOfUpgrade, typeof(TextAsset));
        StringReader upgradeTextReader = new StringReader(upgradeText.text);
        XmlSerializer serializer = new XmlSerializer(typeof(Upgrade));
        Upgrade upgradeLevels = serializer.Deserialize(upgradeTextReader) as Upgrade;

        UpgradeLevel[] allUpgradeLevel = new UpgradeLevel[upgradeLevels.upgradeLevels.Capacity];
        upgradeLevels.upgradeLevels.CopyTo(allUpgradeLevel);

        return allUpgradeLevel;
    }

    public void Rate()
    {
        Application.OpenURL("market://details?id=com.TeamAvocado.DragonRage");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameObject.tag == "FirstScreen")
            {
                Application.Quit();
            }
            else if (gameObject.tag == "Lobby")
            {
                LoadScene("MainMenu");
            }
            else if (gameObject.tag == "AboutScreen")
            {
                LoadScene("MainMenu");
            }
            else if (gameObject.tag == "Store")
            {
                LoadScene("Lobby");
            }
            
        }
    }
}
