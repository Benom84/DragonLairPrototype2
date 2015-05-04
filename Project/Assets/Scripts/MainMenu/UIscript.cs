using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;

public class UIscript : MonoBehaviour
{
    private const int amountOfCanvases = 3;
    
    //store stuff

    private GameObject BreathCanvas;
    private GameObject LifeCanvas;
    private GameObject MagicCanvas;

    private GameObject FireDamage;
    private GameObject FireAgility;
    private GameObject WaterDamage;
    private GameObject WaterAgility;
    private GameObject HeavenlyFire;
    private GameObject Thunder;
    private GameObject FrozenSky;
    private GameObject CursedBreath;
    private GameObject Cave;
    private GameObject Scream;
    private GameObject Meteor;
    private GameObject Ice;
    private GameObject Earthquake;

    private GameObject[] buttons;

    private GameObject NotEnoughMoneyCanvas;

    private GameObject[] canvases;

    private GameObject[] totalCoins;
    private Text[] totalCoinsText;

    private GameObject[] totalCrystals;
    private Text[] totalCrystalsText;

    private bool fireDamage;
    private bool fireAgility;
    private bool waterDamage;
    private bool waterAgility; 
    private bool heavenlyFire;
    private bool frozenSky;
    private bool thunder;
    private bool cursedBreath;
    private bool cave;
    private bool scream;
    private bool meteor;
    private bool ice;
    private bool earthquake;

    private bool[] selectUpgradeToUpgrade;

    private bool[] isUnlocked;

    public Sprite lockedUpgrade;
    public Sprite selectedUpgrade;
    public Sprite regularUpgrade;

    private GameObject[] upgradeDescriptions;
    private Text[] upgradeDescriptionsText;

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
    private UpgradeLevel[] earthquakeLevels;
    private UpgradeLevel[] specialAttackLevels;

    private UpgradeLevel[][] allUpgrades;

    //win/lose stuff
    private Text stageStatus;
    private Text numOfKills;
    private Text percentageOfLife;
    private Text coinsBonus;
    private Text crystalsBonus;

    public void Start()
    {
        if (gameObject.tag == "FirstScreen")
        {
       //     DataController.dataController.Load();
        }
        else if (gameObject.tag == "Store")
        {
            FindAllObjectsInStore();

            canvases = new GameObject[] { BreathCanvas, LifeCanvas, MagicCanvas };

            NotEnoughMoneyCanvas.SetActive(false);

            selectUpgradeToUpgrade = new bool[] { fireDamage, fireAgility, waterDamage, waterAgility, heavenlyFire, frozenSky, 
                thunder, cursedBreath, cave, scream, ice, earthquake };

            for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
            {
                selectUpgradeToUpgrade[i] = false;
            }

            InitCoinsAndCrystals();

            buttons = new GameObject[] { FireDamage, FireAgility, WaterDamage, WaterAgility, HeavenlyFire, FrozenSky, Thunder,
                CursedBreath, Cave, Scream, Meteor, Ice, Earthquake};

            upgradeDescriptionsText = new Text[upgradeDescriptions.Length];

            for (int i = 0; i < upgradeDescriptions.Length; i++)
            {
                upgradeDescriptionsText[i] = upgradeDescriptions[i].GetComponent<Text>();
            }

            foreach (Text text in upgradeDescriptionsText)
            {
                text.text = "";
            }

            isUnlocked = new bool[11];

            for (int i = 0; i < isUnlocked.Length; i++)
            {
                isUnlocked[i] = true;
            }

            //if conditions to unlock the upgrades
            if (DataController.dataController.level < 4)
            {
                isUnlocked[2] = false;
                isUnlocked[3] = false;
                isUnlocked[5] = false;
                isUnlocked[7] = false;
                isUnlocked[11] = false;
            }

            if (DataController.dataController.level < 5)
            {

                isUnlocked[4] = false;
                isUnlocked[6] = false;
                isUnlocked[5] = false;
                isUnlocked[7] = false;
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                if (isUnlocked[i])
                {
                    buttons[i].GetComponent<Image>().sprite = regularUpgrade;
                    if (DataController.dataController.upgradesLevel[i] != 0)
                    {
                        buttons[i].transform.FindChild("LevelNumber").GetComponent<Text>().text = DataController.dataController.upgradesLevel[i].ToString();
                    } 
                }
                else
                {
                    buttons[i].GetComponent<Image>().sprite = lockedUpgrade;
                 //   buttons[i].transform.FindChild("Name").GetComponent<Text>().font.material.color = Color.gray;
                    buttons[i].transform.FindChild("LevelNumber").GetComponent<Text>().text = "";
                }
            }

            allUpgrades = new UpgradeLevel[][] {fireDamageLevels, fireAgilityLevels, waterDamageLevels, waterAgilityLevels, heavenlyFireLevels, 
                frozenSkyLevels, thunderLevels, cursedBreathLevels, caveLevels, screamLevels, meteorLevels, iceLevels, earthquakeLevels, specialAttackLevels};

            for (int i = 0; i < amountOfCanvases; i++)
            {
                canvases[i].SetActive(false);
            }

            BreathCanvas.SetActive(true);
        }
        else if (gameObject.tag == "WinOrLose")
        {
            WinOrLose();
        }
    }

    private void FindAllObjectsInStore()
    {
        BreathCanvas = GameObject.Find("BreathCanvas");
        LifeCanvas = GameObject.Find("LifeCanvas");
        MagicCanvas = GameObject.Find("MagicCanvas");

        NotEnoughMoneyCanvas = GameObject.Find("NotEnoughMoneyCanvas");

        totalCoins = GameObject.FindGameObjectsWithTag("TotalCoins");
        totalCrystals = GameObject.FindGameObjectsWithTag("TotalCrystals");

        FireDamage = GameObject.Find("FireDamage");
        FireAgility = GameObject.Find("FireAgility");
        WaterDamage = GameObject.Find("WaterDamage");
        WaterAgility = GameObject.Find("WaterAgility");
        HeavenlyFire = GameObject.Find("HeavenlyFire");
        Thunder = GameObject.Find("Thunder");
        FrozenSky = GameObject.Find("FrozenSky");
        CursedBreath = GameObject.Find("CursedBreath");
        Cave = GameObject.Find("Cave");
        Scream = GameObject.Find("Scream");
        Meteor = GameObject.Find("Meteor");
        Ice = GameObject.Find("Ice");
        Earthquake = GameObject.Find("Earthquake");

        upgradeDescriptions = GameObject.FindGameObjectsWithTag("upgradeDescriptions");

        fireDamageLevels = ReadUpgradeLevels("FireDamageUpgrade");
        fireAgilityLevels = ReadUpgradeLevels("FireAgilityUpgrade");
        waterDamageLevels = ReadUpgradeLevels("WaterDamageUpgrade");
        waterAgilityLevels = ReadUpgradeLevels("WaterAgilityUpgrade");
        heavenlyFireLevels = null; //change this
        frozenSkyLevels = null; //change this
        thunderLevels = null;
        cursedBreathLevels = null;
        caveLevels = ReadUpgradeLevels("CaveUpgrade");
        screamLevels = ReadUpgradeLevels("ScreamUpgrade");
        meteorLevels = ReadUpgradeLevels("MeteorUpgrade");
        iceLevels = ReadUpgradeLevels("IceUpgrade");
        earthquakeLevels = ReadUpgradeLevels("EarthquakeUpgrade");
        specialAttackLevels = ReadUpgradeLevels("SpecialAttackUpgrade");
    }

    private void InitCoinsAndCrystals()
    {
        totalCoinsText = new Text[amountOfCanvases];
        totalCrystalsText = new Text[amountOfCanvases];

        for (int i = 0; i < amountOfCanvases; i++)
        {
            totalCoinsText[i] = totalCoins[i].GetComponent<Text>();
            totalCrystalsText[i] = totalCrystals[i].GetComponent<Text>();
        }

        foreach (Text text in totalCoinsText)
        {
            text.text = "" + DataController.dataController.coins;
        }

        foreach (Text text in totalCrystalsText)
        {
            text.text = "" + DataController.dataController.crystals;
        }
    }

    private void WinOrLose()
    {
        numOfKills = GameObject.Find("NumOfKills").GetComponent<Text>();
        percentageOfLife = GameObject.Find("PercentageOfLife").GetComponent<Text>();
        coinsBonus = GameObject.Find("CoinsBonus").GetComponent<Text>();
        crystalsBonus = GameObject.Find("CrystalsBonus").GetComponent<Text>();
        stageStatus = GameObject.Find("StageWonOrLost").GetComponent<Text>();

        numOfKills.text = "" + DataController.dataController.kills;
        percentageOfLife.text = "" + DataController.dataController.life;
        coinsBonus.text = "" + DataController.dataController.coinsFromStage;
        crystalsBonus.text = "" + DataController.dataController.crystalsFromStage;

        if (DataController.dataController.won)
        {
            stageStatus.text = "Stage cleared";
            DataController.dataController.level++;
        }
        else
        {
            stageStatus.text = "Stage failed";
        }

        DataController.dataController.coins += DataController.dataController.coinsFromStage;
        DataController.dataController.crystals += DataController.dataController.crystalsFromStage;
        DataController.dataController.Save();
    }

    public void LoadScene(string sceneName)
    {
        DataController.dataController.Save();
        Application.LoadLevel(sceneName);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void SwitchCanvas(int canvasNumber)
    {
        foreach (GameObject canvas in canvases)
        {
            canvas.SetActive(false);
        }

        canvases[canvasNumber - 1].SetActive(true);
        for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
        {
            selectUpgradeToUpgrade[i] = false;
        }
    }

    public void ChooseToUpgrade(int numberOfUpgraded)
    {
        for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
        {
            selectUpgradeToUpgrade[i] = false;
            if (isUnlocked[i])
            {
                buttons[i].GetComponent<Image>().sprite = regularUpgrade;
            } 
        }
        
        if (isUnlocked[numberOfUpgraded - 1])
        {
            selectUpgradeToUpgrade[numberOfUpgraded - 1] = true;
            buttons[numberOfUpgraded - 1].GetComponent<Image>().sprite = selectedUpgrade;
        }

        string strToDisplayInDescription = "";

        switch (numberOfUpgraded)
        {
            case 1:
                strToDisplayInDescription = "Increases the damage your fire creates";
                break;
            case 2:
                strToDisplayInDescription = "Decreases the delay between your fire balls";
                break;
            case 3:
                strToDisplayInDescription = "Increases the damage your water creates";
                break;
            case 4:
                strToDisplayInDescription = "Decreases the delay between your water balls";
                break;
            case 5:
                strToDisplayInDescription = "After being hit continue to lower the heroes life bar";
                break;
            case 6:
                strToDisplayInDescription = "After being hit stops the heroes for little time";
                break;
            case 7:
                strToDisplayInDescription = "After being hit continue to lower the heroes life bar and slower them down";
                break;
            case 8:
                strToDisplayInDescription = "After being hit it stops the heroes and continue to lower the heroes life bar";
                break;
            case 9:
                strToDisplayInDescription = "Increase the life bar";
                break;
            case 10:
                strToDisplayInDescription = "Pushes the heroes back";
                break;
            case 11:
                strToDisplayInDescription = "Lowers the life bar of all the heroes and continue to lower them for a while";
                break;
            case 12:
                strToDisplayInDescription = "Stops all the heroes for a while";
                break;
            case 13:
                strToDisplayInDescription = "Lowers the life bar of all the heroes and stop them for a while";
                break;
        }

        foreach (Text text in upgradeDescriptionsText)
        {
            text.text = strToDisplayInDescription;
        }
    }

    public void Upgrade()
    {

        int numberOfUpgrade = 0;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (selectUpgradeToUpgrade[i])
            {
                numberOfUpgrade = i;
                break;
            }
        }

        int costOfUpgrade = allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Cost;
        if (costOfUpgrade > 100)
        {
            if (EnoughMoney(costOfUpgrade))
            {
                DataController.dataController.coins -= costOfUpgrade;
                if (numberOfUpgrade != 2 && numberOfUpgrade != 4) {
                    DataController.dataController.upgradesData[numberOfUpgrade] += (int)allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Data;
                }
                else
                {
                    DataController.dataController.upgradesData[numberOfUpgrade] -= allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Data;
                }
                DataController.dataController.upgradesLevel[numberOfUpgrade]++;
            }
            else
            {
                NotEnoughMoney();
            }
        }
        else
        {
            if (EnoughCrystals(costOfUpgrade))
            {
                DataController.dataController.crystals -= costOfUpgrade;
                DataController.dataController.upgradesData[numberOfUpgrade] += (int)allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Data;
                DataController.dataController.upgradesLevel[numberOfUpgrade]++;
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
        XmlSerializer serializer = new XmlSerializer(typeof(UpgradeLevel));
        Upgrade upgradeLevels = serializer.Deserialize(upgradeTextReader) as Upgrade;
        int numOfLevels = 0;

        foreach (UpgradeLevel upgradelevel in upgradeLevels.upgradeLevels)
        {
            numOfLevels++;
        }

        UpgradeLevel[] allUpgradeLevel = new UpgradeLevel[numOfLevels];
        int i = 0;
        
        foreach (UpgradeLevel upgradeLevel in upgradeLevels.upgradeLevels)
        {
            allUpgradeLevel[i] = upgradeLevel;
            i++;
        }

        return allUpgradeLevel;
    }
}
