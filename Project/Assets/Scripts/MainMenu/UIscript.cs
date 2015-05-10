﻿using UnityEngine;
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

    private GameObject[] upgradeButtonsImage;
    private Image[] upgradeButtonCostImage;

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

    public Sprite coin;
    public Sprite crystal;

    public Sprite waterLocked;
    public Sprite waterUnlocked;
    public Sprite screamLocked;
    public Sprite screamUnlocked;
    public Sprite iceLocked;
    public Sprite iceUnlocked;
    public Sprite tailLocked;
    public Sprite tailUnlocked;


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
            upgradeButtonsImage = GameObject.FindGameObjectsWithTag("upgradeImage");
            upgradeDescriptions = GameObject.FindGameObjectsWithTag("upgradeDescriptions");
            nameOfUpgradeInDescription = GameObject.FindGameObjectsWithTag("nameOfUpgradeInDescription");

            upgradeDescriptionsText = new Text[upgradeDescriptions.Length];
            upgradeButtonCostText = new Text[upgradeButtons.Length];
            upgradeButtonCostImage = new Image[upgradeButtonsImage.Length];
            nameOfUpgradeInDescriptionText = new Text[nameOfUpgradeInDescription.Length];

            for (int i = 0; i < upgradeDescriptions.Length; i++)
            {
                upgradeDescriptionsText[i] = upgradeDescriptions[i].GetComponent<Text>();
                upgradeButtonCostText[i] = upgradeButtons[i].GetComponent<Text>();
                upgradeButtonCostImage[i] = upgradeButtonsImage[i].GetComponent<Image>();
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

            foreach (Image image in upgradeButtonCostImage)
            {
                image.sprite = null;

                Color buttonColor = image.color;
                buttonColor.a = 0;
                image.color = buttonColor;
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
                isUnlocked[11] = false;
                isUnlocked[12] = false;

                GameObject.Find("Water").GetComponent<Image>().sprite = waterLocked;
                GameObject.Find("IceImage").GetComponent<Image>().sprite = iceLocked;
                GameObject.Find("TailImage").GetComponent<Image>().sprite = tailLocked;
            }
            else
            {
                GameObject.Find("Water").GetComponent<Image>().sprite = waterUnlocked;
                GameObject.Find("IceImage").GetComponent<Image>().sprite = iceUnlocked;
                GameObject.Find("TailImage").GetComponent<Image>().sprite = tailUnlocked;
            }

            if (DataController.dataController.level < 5)
            {

                isUnlocked[4] = false;
                isUnlocked[6] = false;
                isUnlocked[5] = false;
                isUnlocked[7] = false;
            }

            if (DataController.dataController.level < 7)
            {
                isUnlocked[9] = false;
                GameObject.Find("ScreamImage").GetComponent<Image>().sprite = screamLocked;
            }
            else
            {
                GameObject.Find("ScreamImage").GetComponent<Image>().sprite = screamUnlocked;            
            }

            for (int i = 0; i < 4; i++)
            {
                if (isUnlocked[i] && i % 2 == 0)
                {
                    buttons[i].GetComponent<Image>().sprite = damageRegularUpgrade;
                    if (DataController.dataController.upgradesLevel[i] != 0)
                    {
                        buttons[i].transform.FindChild("LevelNumber").GetComponent<Text>().text = DataController.dataController.upgradesLevel[i].ToString();
                    }
                }
                else if (isUnlocked[i] && i % 2 != 0)
                {
                    buttons[i].GetComponent<Image>().sprite = agilityRegularUpgrade;
                    if (DataController.dataController.upgradesLevel[i] != 0)
                    {
                        buttons[i].transform.FindChild("LevelNumber").GetComponent<Text>().text = DataController.dataController.upgradesLevel[i].ToString();
                    }
                    
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
                        buttons[i].transform.FindChild("LevelNumber").GetComponent<Text>().text = DataController.dataController.upgradesLevel[i].ToString();
                    }
                }
                else
                {
                    buttons[i].GetComponent<Image>().sprite = lockedUpgrade;
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

        foreach (Text text in nameOfUpgradeInDescriptionText)
        {
            text.text = "";
        }

        foreach (Text text in upgradeDescriptionsText)
        {
            text.text = "";
        }

        foreach (Image image in upgradeButtonCostImage)
        {
            image.sprite = null;
            Color buttonColor = image.color;
            buttonColor.a = 0;
            image.color = buttonColor;
        }

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

        if (numberOfUpgraded < 10)
        {
            foreach (Image image in upgradeButtonCostImage)
            {
                image.sprite = coin;
                Color buttonColor = image.color;
                buttonColor.a = 255;
                image.color = buttonColor;
            }
        }
        else
        {
            foreach (Image image in upgradeButtonCostImage)
            {
                image.sprite = crystal;
                Color buttonColor = image.color;
                buttonColor.a = 255;
                image.color = buttonColor;
            }
        }
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

        Debug.Log("was selected " + wasSelected.ToString());
        Debug.Log("isUnlocked[numberOfUpgrade] " + isUnlocked[numberOfUpgrade]);


        int costOfUpgrade = allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Cost;
        if (wasSelected && isUnlocked[numberOfUpgrade])
        {
            if (costOfUpgrade > 100)
            {
                if (EnoughMoney(costOfUpgrade))
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

                    buttons[numberOfUpgrade].transform.FindChild("LevelNumber").GetComponent<Text>().text = DataController.dataController.upgradesLevel[numberOfUpgrade].ToString();

                    foreach (Text text in totalCoinsText)
                    {
                        text.text = "" + DataController.dataController.coins;
                    }

                    foreach (Text text in upgradeButtonCostText)
                    {
                        text.text = allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Cost.ToString();
                    }
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

                    foreach (Text text in totalCrystalsText)
                    {
                        text.text = "" + DataController.dataController.crystals;
                    }

                    foreach (Text text in upgradeButtonCostText)
                    {
                        text.text = allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Cost.ToString();
                    }
                }
                else
                {
                    NotEnoughMoney();
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

}
