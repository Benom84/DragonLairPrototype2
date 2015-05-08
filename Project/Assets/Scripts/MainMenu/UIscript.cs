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
    private GameObject Tail;
    private GameObject Mana;

    private GameObject[] buttons;

    private GameObject NotEnoughMoneyCanvas;

    private GameObject[] canvases;

    private GameObject[] totalCoins;
    private Text[] totalCoinsText;

    private GameObject[] totalCrystals;
    private Text[] totalCrystalsText;

    private bool[] selectUpgradeToUpgrade;

    private bool[] isUnlocked;

    public Sprite lockedUpgrade;
    public Sprite selectedUpgrade;
    public Sprite regularUpgrade;
    public Sprite damageRegularUpgrade;
    public Sprite damageSelectedUpgrade;
    public Sprite damageLockedUpgrade;
    public Sprite agilityRegularUpgrade;
    public Sprite agilitySelectedUpgrade;
    public Sprite agilityLockedUpgrade;

    private GameObject[] upgradeDescriptions;
    private Text[] upgradeDescriptionsText;

    private GameObject[] upgradeButtons;
    private Text[] upgradeButtonCostText;

    private GameObject[] nameOfUpgradeInDescription;
    private Text[] nameOfUpgradeInDescriptionText;

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
            BreathCanvas = GameObject.Find("BreathCanvas");
            LifeCanvas = GameObject.Find("LifeCanvas");
            MagicCanvas = GameObject.Find("MagicCanvas");

            canvases = new GameObject[] { BreathCanvas, LifeCanvas, MagicCanvas };

            NotEnoughMoneyCanvas = GameObject.Find("NotEnoughMoneyCanvas");
            NotEnoughMoneyCanvas.SetActive(false);

            selectUpgradeToUpgrade = new bool[14];

            for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
            {
                selectUpgradeToUpgrade[i] = false;
            }

            totalCoins = GameObject.FindGameObjectsWithTag("TotalCoins");
            totalCrystals = GameObject.FindGameObjectsWithTag("TotalCrystals");

            totalCoinsText = new Text[amountOfCanvases];
            totalCrystalsText = new Text[amountOfCanvases];

            for (int i = 0; i < amountOfCanvases; i++)
            {
                totalCoinsText[i] = totalCoins[i].GetComponent<Text>();
                totalCrystalsText[i] = totalCrystals[i].GetComponent<Text>();
            }

            foreach (Text text in totalCoinsText)
            {
                text.text = "" + DataController.dataController.coins + " coins";
            }

            foreach (Text text in totalCrystalsText)
            {
                text.text = "" + DataController.dataController.crystals + " crystals";
            }

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
            Tail = GameObject.Find("Tail");
            Mana = GameObject.Find("Mana");

            buttons = new GameObject[] { FireDamage, FireAgility, WaterDamage, WaterAgility, HeavenlyFire, FrozenSky, Thunder,
                CursedBreath, Cave, Scream, Meteor, Ice, Tail, Mana};

            upgradeButtons = GameObject.FindGameObjectsWithTag("upgradeButton");
            upgradeDescriptions = GameObject.FindGameObjectsWithTag("upgradeDescriptions");
            nameOfUpgradeInDescription = GameObject.FindGameObjectsWithTag("nameOfUpgradeInDescription");

            upgradeDescriptionsText = new Text[upgradeDescriptions.Length];
            upgradeButtonCostText = new Text[upgradeButtons.Length];
            nameOfUpgradeInDescriptionText = new Text[nameOfUpgradeInDescription.Length];

            for (int i = 0; i < upgradeDescriptions.Length; i++)
            {
                upgradeDescriptionsText[i] = upgradeDescriptions[i].GetComponent<Text>();
                upgradeButtonCostText[i] = upgradeButtons[i].GetComponent<Text>();
                nameOfUpgradeInDescriptionText[i] = nameOfUpgradeInDescription[i].GetComponent<Text>();
            }

            foreach (Text text in upgradeDescriptionsText)
            {
                text.text = "";
            }

            foreach (Text text in upgradeButtonCostText)
            {
                text.text = "";
            }

            foreach (Text text in nameOfUpgradeInDescriptionText)
            {
                text.text = "";
            }

            isUnlocked = new bool[14];

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
                isUnlocked[12] = false;
            }

            if (DataController.dataController.level < 5)
            {

                isUnlocked[4] = false;
                isUnlocked[6] = false;
                isUnlocked[5] = false;
                isUnlocked[7] = false;
            }

            for (int i = 0; i < 4; i++)
            {
                if (isUnlocked[i] && i % 2 == 0)
                {
                    buttons[i].GetComponent<Image>().sprite = damageRegularUpgrade;
                    
                }
                else if (isUnlocked[i] && i % 2 != 0)
                {
                    buttons[i].GetComponent<Image>().sprite = agilityRegularUpgrade;
                    
                }
                else if (!isUnlocked[i] && i % 2 == 0)
                {
                    buttons[i].GetComponent<Image>().sprite = damageLockedUpgrade;
                    
                }
                else
                {
                    buttons[i].GetComponent<Image>().sprite = agilityLockedUpgrade;
                    
                }
            }

            for (int i = 4; i < buttons.Length; i++)
            {
                if (isUnlocked[i])
                {
                    buttons[i].GetComponent<Image>().sprite = regularUpgrade;
                    if (DataController.dataController.upgradesLevel[i] != 0)
                    {
                        //buttons[i].transform.FindChild("LevelNumber").GetComponent<Text>().text = DataController.dataController.upgradesLevel[i].ToString();
                    }
                }
                else
                {
                    buttons[i].GetComponent<Image>().sprite = lockedUpgrade;
                    //buttons[i].transform.FindChild("Name").GetComponent<Text>().font.material.color = Color.gray;
                    //buttons[i].transform.FindChild("LevelNumber").GetComponent<Text>().text = "";
                }
            }

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
            tailLevels = ReadUpgradeLevels("EarthquakeUpgrade");
            manaLevels = ReadUpgradeLevels("SpecialAttackUpgrade");

            allUpgrades = new UpgradeLevel[][] {fireDamageLevels, fireAgilityLevels, waterDamageLevels, waterAgilityLevels, heavenlyFireLevels, 
                frozenSkyLevels, thunderLevels, cursedBreathLevels, caveLevels, screamLevels, meteorLevels, iceLevels, tailLevels, manaLevels};

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

        foreach (Text text in upgradeButtonCostText)
        {
            text.text = "";
        }
    }

    public void ChooseToUpgrade(int numberOfUpgraded)
    {
        for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
        {
            selectUpgradeToUpgrade[i] = false;
            if (isUnlocked[i] && i > 3)
            {
                buttons[i].GetComponent<Image>().sprite = regularUpgrade;
            } 
        }

        for (int i = 0; i < 4; i++)
        {
            if (isUnlocked[i] && i % 2 == 0)
            {
                buttons[i].GetComponent<Image>().sprite = damageRegularUpgrade;
            }
            else if (isUnlocked[i] && i % 2 != 0)
            {
                buttons[i].GetComponent<Image>().sprite = agilityRegularUpgrade;
            }
            else if (!isUnlocked[i] && i % 2 == 0)
            {
                buttons[i].GetComponent<Image>().sprite = damageLockedUpgrade;
            }
            else
            {
                buttons[i].GetComponent<Image>().sprite = agilityLockedUpgrade;
            }
        }

        if (isUnlocked[numberOfUpgraded - 1] && numberOfUpgraded > 3)
        {
            selectUpgradeToUpgrade[numberOfUpgraded - 1] = true;
            buttons[numberOfUpgraded - 1].GetComponent<Image>().sprite = selectedUpgrade;
        }
        else if (isUnlocked[numberOfUpgraded - 1])
        {
            if (numberOfUpgraded - 1 % 2 == 0)
            {
                selectUpgradeToUpgrade[numberOfUpgraded - 1] = true;
                buttons[numberOfUpgraded - 1].GetComponent<Image>().sprite = damageSelectedUpgrade;
            }
            else
            {
                selectUpgradeToUpgrade[numberOfUpgraded - 1] = true;
                buttons[numberOfUpgraded - 1].GetComponent<Image>().sprite = agilitySelectedUpgrade;
            }
        }

        string strToDisplayInDescription = allUpgrades[numberOfUpgraded - 1][DataController.dataController.upgradesLevel[numberOfUpgraded - 1]].Description;
        string strToDisplayInUpgrade = allUpgrades[numberOfUpgraded - 1][DataController.dataController.upgradesLevel[numberOfUpgraded - 1]].Cost.ToString();
        string strToDisplayInDescriptionName = allUpgrades[numberOfUpgraded - 1][DataController.dataController.upgradesLevel[numberOfUpgraded - 1]].Name;

        foreach (Text text in upgradeDescriptionsText)
        {
            text.text = strToDisplayInDescription;
        }

        foreach (Text text in upgradeButtonCostText)
        {
            text.text = strToDisplayInUpgrade;
        }

        foreach (Text text in nameOfUpgradeInDescriptionText)
        {
            text.text = strToDisplayInDescriptionName;
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
                    DataController.dataController.upgradesData[numberOfUpgrade] += allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Data;
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
        XmlSerializer serializer = new XmlSerializer(typeof(Upgrade));
        Upgrade upgradeLevels = serializer.Deserialize(upgradeTextReader) as Upgrade;

        UpgradeLevel[] allUpgradeLevel = new UpgradeLevel[upgradeLevels.upgradeLevels.Capacity];
        upgradeLevels.upgradeLevels.CopyTo(allUpgradeLevel);

        return allUpgradeLevel;
    }

    public void FixedUpgrade()
    {
        int numberToUpgrade = 0;
        
        for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
        {
            if (selectUpgradeToUpgrade[i])
            {
                numberToUpgrade = i;
            }
        }

        if (isUnlocked[numberToUpgrade])
        {
            switch (numberToUpgrade)
            {
                case 0:
                    if (DataController.dataController.b_fireDamageLevel == 1 && DataController.dataController.coins >= 500) { 
                        DataController.dataController.coins -= 500;
                        DataController.dataController.b_fireDamageData += 2;
                        DataController.dataController.b_fireDamageLevel++;
                    }
                    else if (DataController.dataController.b_fireDamageLevel == 2 && DataController.dataController.coins >= 700)
                    {
                        DataController.dataController.coins -= 700;
                        DataController.dataController.b_fireDamageData += 2;
                        DataController.dataController.b_fireDamageLevel++;
                    }
                    else if (DataController.dataController.b_fireDamageLevel == 3 && DataController.dataController.coins >= 900)
                    {
                        DataController.dataController.coins -= 900;
                        DataController.dataController.b_fireDamageData += 2;
                        DataController.dataController.b_fireDamageLevel++;
                    }  
                    else if (DataController.dataController.b_fireDamageLevel == 4 && DataController.dataController.coins >= 1100)
                    {
                        DataController.dataController.coins -= 1100;
                        DataController.dataController.b_fireDamageData += 2;
                        DataController.dataController.b_fireDamageLevel++;
                    }  
                    else
                    {
                        NotEnoughMoney();
                    }
                    break;
                case 1:
                    if (DataController.dataController.b_waterDamageLevel == 1 && DataController.dataController.coins >= 500)
                    {
                        DataController.dataController.coins -= 500;
                        DataController.dataController.b_waterDamageData += 2;
                        DataController.dataController.b_waterDamageLevel++;
                    }
                    else if (DataController.dataController.b_waterDamageLevel == 2 && DataController.dataController.coins >= 700)
                    {
                        DataController.dataController.coins -= 700;
                        DataController.dataController.b_waterDamageData += 2;
                        DataController.dataController.b_waterDamageLevel++;
                    }
                    else if (DataController.dataController.b_waterDamageLevel == 3 && DataController.dataController.coins >= 900)
                    {
                        DataController.dataController.coins -= 900;
                        DataController.dataController.b_waterDamageData += 2;
                        DataController.dataController.b_waterDamageLevel++;
                    }
                    else if (DataController.dataController.b_waterDamageLevel == 4 && DataController.dataController.coins >= 1100)
                    {
                        DataController.dataController.coins -= 1100;
                        DataController.dataController.b_waterDamageData += 2;
                        DataController.dataController.b_waterDamageLevel++;
                    }
                    else
                    {
                        NotEnoughMoney();
                    }
                    break;
                case 2:
                    if (DataController.dataController.b_fireAgilityLevel == 0 && DataController.dataController.coins >= 500)
                    {
                        DataController.dataController.coins -= 500;
                        DataController.dataController.b_fireAgilityData -= 0.1f;
                        DataController.dataController.b_fireAgilityLevel++;
                    }
                    else if (DataController.dataController.b_fireAgilityLevel == 1 && DataController.dataController.coins >= 700)
                    {
                        DataController.dataController.coins -= 700;
                        DataController.dataController.b_fireAgilityData -= 0.1f;
                        DataController.dataController.b_fireAgilityLevel++;
                    }
                    else if (DataController.dataController.b_fireAgilityLevel == 2 && DataController.dataController.coins >= 900)
                    {
                        DataController.dataController.coins -= 900;
                        DataController.dataController.b_fireAgilityData -= 0.1f;
                        DataController.dataController.b_fireAgilityLevel++;
                    }
                    else if (DataController.dataController.b_fireAgilityLevel == 3 && DataController.dataController.coins >= 1100)
                    {
                        DataController.dataController.coins -= 1100;
                        DataController.dataController.b_fireAgilityData -= 0.1f;
                        DataController.dataController.b_fireAgilityLevel++;
                    }
                    else
                    {
                        NotEnoughMoney();
                    }
                    break;
                case 3:
                    if (DataController.dataController.b_waterAgilityLevel == 0 && DataController.dataController.coins >= 500)
                    {
                        DataController.dataController.coins -= 500;
                        DataController.dataController.b_waterAgilityData -= 0.1f;
                        DataController.dataController.b_waterAgilityLevel++;
                    }
                    else if (DataController.dataController.b_waterAgilityLevel == 1 && DataController.dataController.coins >= 700)
                    {
                        DataController.dataController.coins -= 700;
                        DataController.dataController.b_waterAgilityData -= 0.1f;
                        DataController.dataController.b_waterAgilityLevel++;
                    }
                    else if (DataController.dataController.b_waterAgilityLevel == 2 && DataController.dataController.coins >= 900)
                    {
                        DataController.dataController.coins -= 900;
                        DataController.dataController.b_waterAgilityData -= 0.1f;
                        DataController.dataController.b_waterAgilityLevel++;
                    }
                    else if (DataController.dataController.b_waterAgilityLevel == 3 && DataController.dataController.coins >= 1100)
                    {
                        DataController.dataController.coins -= 1100;
                        DataController.dataController.b_waterAgilityData -= 0.1f;
                        DataController.dataController.b_waterAgilityLevel++;
                    }
                    else
                    {
                        NotEnoughMoney();
                    }
                    break;
                case 8:
                    if (DataController.dataController.p_caveLevel == 0 && DataController.dataController.coins >= 1500)
                    {
                        DataController.dataController.coins -= 1500;
                        DataController.dataController.p_caveData += 10;
                        DataController.dataController.p_caveLevel++;
                    }
                    else if (DataController.dataController.p_caveLevel == 1 && DataController.dataController.coins >= 2000)
                    {
                        DataController.dataController.coins -= 2000;
                        DataController.dataController.p_caveData += 10;
                        DataController.dataController.p_caveLevel++;
                    }
                    else
                    {
                        NotEnoughMoney();
                    }
                    break;
                case 9:
                    if (DataController.dataController.m_screamLevel == 0 && DataController.dataController.crystals >= 5)
                    {
                        DataController.dataController.crystals -= 5;
                        DataController.dataController.m_screamData += 5;
                        DataController.dataController.m_screamLevel++;
                    }
                    else if (DataController.dataController.m_screamLevel == 1 && DataController.dataController.crystals >= 7)
                    {
                        DataController.dataController.crystals -= 7;
                        DataController.dataController.m_screamData += 5;
                        DataController.dataController.m_screamLevel++;
                    }
                    else if (DataController.dataController.m_screamLevel == 1 && DataController.dataController.crystals >= 9)
                    {
                        DataController.dataController.crystals -= 9;
                        DataController.dataController.m_screamData += 5;
                        DataController.dataController.m_screamLevel++;
                    }
                    else
                    {
                        NotEnoughMoney();
                    }
                    break;
                case 10:
                    if (DataController.dataController.m_meteorLevel == 0 && DataController.dataController.crystals >= 5)
                    {
                        DataController.dataController.crystals -= 5;
                        DataController.dataController.m_meteorData += 5;
                        DataController.dataController.m_meteorLevel++;
                    }
                    else if (DataController.dataController.m_meteorLevel == 1 && DataController.dataController.crystals >= 7)
                    {
                        DataController.dataController.crystals -= 7;
                        DataController.dataController.m_meteorData += 5;
                        DataController.dataController.m_meteorLevel++;
                    }
                    else if (DataController.dataController.m_meteorLevel == 2 && DataController.dataController.crystals >= 9)
                    {
                        DataController.dataController.crystals -= 9;
                        DataController.dataController.m_meteorData += 5;
                        DataController.dataController.m_meteorLevel++;
                    }
                    else 
                    {
                        NotEnoughMoney();
                    }
                    break;
                case 13:
                    if (DataController.dataController.m_manaLevel == 0 && DataController.dataController.crystals >= 10)
                    {
                        DataController.dataController.crystals -= 10;
                        DataController.dataController.m_manaData += 10;
                        DataController.dataController.m_manaLevel++;
                    }
                    else
                    {
                        NotEnoughMoney();
                    }
                    break;
            }
        }


    }

    public void FixedUpdate()
    {

        if (gameObject.tag == "Store")
        {
            foreach (Text text in totalCoinsText)
            {
                text.text = "" + DataController.dataController.coins + " coins";
            }

            foreach (Text text in totalCrystalsText)
            {
                text.text = "" + DataController.dataController.crystals + " crystals";
            }
        }
    }

}

//string strToDisplayInDescription = "";
//string strToDisplayInUpgrade = "";

//switch (numberOfUpgraded)
//{
//    case 1:
//        strToDisplayInDescription = "Increases the damage your fire creates";
//        if (DataController.dataController.b_fireDamageLevel == 1) 
//        {
//            strToDisplayInUpgrade = "500 coins";
//        } 
//        else if (DataController.dataController.b_fireDamageLevel == 2)
//        {
//            strToDisplayInUpgrade = "700 coins";
//        }else if (DataController.dataController.b_fireDamageLevel == 3)
//        {
//            strToDisplayInUpgrade = "900 coins";
//        }else if (DataController.dataController.b_fireDamageLevel == 4)
//        {
//            strToDisplayInUpgrade = "1100 coins";
//        }
//        break;
//    case 2:
//        strToDisplayInDescription = "Decreases the delay between your fire balls";
//        if (DataController.dataController.b_fireAgilityLevel == 1) 
//        {
//            strToDisplayInUpgrade = "500 coins";
//        }
//        else if (DataController.dataController.b_fireAgilityLevel == 2)
//        {
//            strToDisplayInUpgrade = "700 coins";
//        }
//        else if (DataController.dataController.b_fireAgilityLevel == 3)
//        {
//            strToDisplayInUpgrade = "900 coins";
//        }
//        else if (DataController.dataController.b_fireAgilityLevel == 4)
//        {
//            strToDisplayInUpgrade = "1100 coins";
//        }
//        break;
//    case 3:
//        strToDisplayInDescription = "Increases the damage your water creates";
//        if (DataController.dataController.b_waterDamageLevel == 0)
//        {
//            strToDisplayInUpgrade = "500 coins";
//        }
//        else if (DataController.dataController.b_waterDamageLevel == 1)
//        {
//            strToDisplayInUpgrade = "700 coins";
//        }
//        else if (DataController.dataController.b_waterDamageLevel == 2)
//        {
//            strToDisplayInUpgrade = "900 coins";
//        }
//        else if (DataController.dataController.b_waterDamageLevel == 3)
//        {
//            strToDisplayInUpgrade = "1100 coins";
//        }
//        break;
//    case 4:
//        strToDisplayInDescription = "Decreases the delay between your water balls";
//        if (DataController.dataController.b_waterAgilityLevel == 0)
//        {
//            strToDisplayInUpgrade = "500 coins";
//        }
//        else if (DataController.dataController.b_waterAgilityLevel == 1)
//        {
//            strToDisplayInUpgrade = "700 coins";
//        }
//        else if (DataController.dataController.b_waterAgilityLevel == 2)
//        {
//            strToDisplayInUpgrade = "900 coins";
//        }
//        else if (DataController.dataController.b_waterAgilityLevel == 3)
//        {
//            strToDisplayInUpgrade = "1100 coins";
//        }
//        break;
//    case 5:
//        strToDisplayInDescription = "After being hit continue to lower the heroes life bar";
//        strToDisplayInUpgrade = "900 coins";
//        break;
//    case 6:
//        strToDisplayInDescription = "After being hit stops the heroes for little time";
//        strToDisplayInUpgrade = "900 coins";
//        break;
//    case 7:
//        strToDisplayInDescription = "After being hit continue to lower the heroes life bar and slower them down";
//        strToDisplayInUpgrade = "900 coins";
//        break;
//    case 8:
//        strToDisplayInDescription = "After being hit it stops the heroes and continue to lower the heroes life bar";
//        strToDisplayInUpgrade = "900 coins";
//        break;
//    case 9:
//        strToDisplayInDescription = "Increase the life bar";
//        if (DataController.dataController.p_caveLevel == 0)
//        {
//            strToDisplayInUpgrade = "1500 coins";
//        }
//        else if (DataController.dataController.p_caveLevel == 1)
//        {
//            strToDisplayInUpgrade = "2000 coins";
//        }
//        break;
//    case 10:
//        strToDisplayInDescription = "Pushes the heroes back";
//        if (DataController.dataController.m_screamLevel == 0)
//        {
//            strToDisplayInUpgrade = "5 crystals";
//        }
//        else if (DataController.dataController.m_screamLevel == 1)
//        {
//            strToDisplayInUpgrade = "7 crystals";
//        }
//        else if (DataController.dataController.m_screamLevel == 2)
//        {
//            strToDisplayInUpgrade = "9 crystals";
//        }
//        break;
//    case 11:
//        strToDisplayInDescription = "Lowers the life bar of all the heroes and continue to lower them for a while";
//        if (DataController.dataController.m_meteorLevel == 0)
//        {
//            strToDisplayInUpgrade = "5 crystals";
//        }
//        else if (DataController.dataController.m_meteorLevel == 1)
//        {
//            strToDisplayInUpgrade = "7 crystals";
//        }
//        else if (DataController.dataController.m_meteorLevel == 2)
//        {
//            strToDisplayInUpgrade = "9 crystals";
//        }
//        break;
//    case 12:
//        strToDisplayInDescription = "Stops all the heroes for a while";
//        strToDisplayInUpgrade = "9 crystals";
//        break;
//    case 13:
//        strToDisplayInDescription = "Lowers the life bar of all the heroes and stop them for a while";
//        strToDisplayInUpgrade = "9 crystals";
//        break;
//    case 14:
//        strToDisplayInDescription = "Increase the mana bar";
//        strToDisplayInUpgrade = "10 crystals";
//        break;
//}
