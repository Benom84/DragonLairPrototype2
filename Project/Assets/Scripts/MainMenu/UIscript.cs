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
    public Sprite regurlarUpgrade;

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

            selectUpgradeToUpgrade = new bool[] { fireDamage, fireAgility, waterDamage, waterAgility, heavenlyFire, frozenSky, 
                thunder, cursedBreath, cave, scream, ice, earthquake };

            for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
            {
                selectUpgradeToUpgrade[i] = false;
            }

            //coins and crystals
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
                text.text = "" + DataController.dataController.coins;
            }

            foreach (Text text in totalCrystalsText)
            {
                text.text = "" + DataController.dataController.crystals;
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
            Earthquake = GameObject.Find("Earthquake");

            buttons = new GameObject[] { FireDamage, FireAgility, WaterDamage, WaterAgility, HeavenlyFire, FrozenSky, Thunder,
                CursedBreath, Cave, Scream, Meteor, Ice, Earthquake};

            upgradeDescriptions = GameObject.FindGameObjectsWithTag("upgradeDescriptions");
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
                isUnlocked[10] = false;
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
                if (isUnlocked[i] && DataController.dataController.upgradesLevel[i] != 0)
                {
                    buttons[i].GetComponent<Image>().sprite = regurlarUpgrade;
                    buttons[i].transform.FindChild("LevelNumber").GetComponent<Text>().text = DataController.dataController.upgradesLevel[i].ToString();
                }
                else
                {
                    buttons[i].GetComponent<Image>().sprite = lockedUpgrade;
                    buttons[i].transform.FindChild("Name").GetComponent<Text>().font.material.color = Color.gray;
                    buttons[i].transform.FindChild("LevelNumber").GetComponent<Text>().text = "";
                }
            }

            for (int i = 0; i < amountOfCanvases; i++)
            {
                canvases[i].SetActive(false);
            }

            fireDamageLevels = ReadUpgradeLevels("FireDamageUpgrade");
            fireAgilityLevels = ReadUpgradeLevels("FireAgilityUpgrade");
            waterDamageLevels = ReadUpgradeLevels("WaterDamageUpgrade");
            waterAgilityLevels = ReadUpgradeLevels("WaterAgilityUpgrade");
            caveLevels = ReadUpgradeLevels("CaveUpgrade");
            screamLevels = ReadUpgradeLevels("ScreamUpgrade");
            meteorLevels = ReadUpgradeLevels("MeteorUpgrade");
            iceLevels = ReadUpgradeLevels("IceUpgrade");
            earthquakeLevels = ReadUpgradeLevels("EarthquakeUpgrade");
            specialAttackLevels = ReadUpgradeLevels("SpecialAttackUpgrade");

            BreathCanvas.SetActive(true);
        }
        else if (gameObject.tag == "WinOrLose")
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
    }

    private void StoreInit()
    {
        
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

        //if (canvasNumber == 1 || canvasNumber == 4)
        //{
            //selectUpgradeToUpgrade[0] = true;
        //}
        //else if (canvasNumber % 3 == 0)
        //{
          //  selectUpgradeToUpgrade[7] = true;
       // }
        //else
       // {
         //   selectUpgraded[5] = true;
        //}
    }

    public void ChooseToUpgrade(int numberOfUpgraded)
    {
        for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
        {
            selectUpgradeToUpgrade[i] = false;
            if (isUnlocked[i])
            {
                buttons[i].GetComponent<Image>().sprite = regurlarUpgrade;
            } 
        }
        if (isUnlocked[numberOfUpgraded - 1])
        {
            selectUpgradeToUpgrade[numberOfUpgraded - 1] = true;
            buttons[numberOfUpgraded - 1].GetComponent<Image>().sprite = selectedUpgrade;
        }
        

    }

    public void Upgrade()
    {
        GameObject buttonToUpgrade = null;




        for (int i = 0; i < buttons.Length; i++)
        {
            if (selectUpgradeToUpgrade[i])
            {
                buttonToUpgrade = buttons[i];
                break;
            }
        }
        
        //here comes enough money/enough crystals check
        


        int numberToBeUpgraded = 0;

        for (int i = 0; i < 6; i++)
        {
            if (selectUpgradeToUpgrade[i])
            {
                numberToBeUpgraded = i + 1;
                break;
            }
        }

        if (EnoughMoney(0))
        {
            
        }
        else
        {
            NotEnoughMoney();
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
