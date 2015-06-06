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

    //store stuff

    //private GameObject BreathCanvas;
    //private GameObject LifeCanvas;
    //private GameObject MagicCanvas;

    private Text totalCoinsText;
    private Text totalCrystalsText;

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

    private Text upgradeCostText;
    private Image upgradeButtonImage;
    private Text upgradeDescription;
    private Text upgradeNameInDescription;

    //private GameObject[] canvases;
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
            DataController.dataController.Load();
        }
        else if (gameObject.tag == "Store")
        {
            //// DataController.dataController.SetLevels();
            //BreathCanvas = GameObject.Find("BreathCanvas");
            //LifeCanvas = GameObject.Find("LifeCanvas");
            //MagicCanvas = GameObject.Find("MagicCanvas");

            //canvases = new GameObject[] { BreathCanvas, LifeCanvas, MagicCanvas };

            NotEnoughMoneyCanvas = GameObject.Find("NotEnoughMoneyCanvas");
            NotEnoughMoneyCanvas.SetActive(false);

            selectUpgradeToUpgrade = new bool[14];

            for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
            {
                selectUpgradeToUpgrade[i] = false;
            }

            totalCoinsText = GameObject.Find("Coins").GetComponentInChildren<Text>();
            totalCoinsText.text = DataController.dataController.coins.ToString();

            totalCrystalsText = GameObject.Find("Crystals").GetComponentInChildren<Text>();
            totalCrystalsText.text = DataController.dataController.crystals.ToString();

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

            upgradeCostText = GameObject.Find("CostText").GetComponent<Text>();
            upgradeButtonImage = GameObject.Find("CostImage").GetComponent<Image>();
            upgradeDescription = GameObject.Find("Description").GetComponent<Text>();
            upgradeNameInDescription = GameObject.Find("UpgradeName").GetComponent<Text>();

            upgradeCostText.text = string.Empty;
            upgradeDescription.text = string.Empty;
            upgradeNameInDescription.text = string.Empty;
            upgradeButtonImage.sprite = null;

            //sets the button on transperant
            Color buttonColor = upgradeButtonImage.color;
            buttonColor.a = 0;
            upgradeButtonImage.color = buttonColor;

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

            //for (int i = 0; i < amountOfCanvases; i++)
            //{
            //    canvases[i].SetActive(false);
            //}

            //BreathCanvas.SetActive(true);
        }
    }

    

    public void LoadScene(string sceneName)
    {
        //if (gameObject.tag == "FirstScreen" && DataController.dataController.level == 1)
        //{
        //    Application.LoadLevel("GameLevel");
        //    DataController.dataController.Save();
        //}
        //else
        //{
        //   Application.LoadLevelAdditive(sceneName);
        DataController.dataController.Save();
        Application.LoadLevel(sceneName);

        //if ((Application.loadedLevelName == "Store" && sceneName == "PlayerProgression") || (Application.loadedLevelName == "PlayerProgression" && sceneName == "MainMenu"))
        //{
        //    Swipper.swipper.loadScene(Application.loadedLevelName, sceneName, eDirectionOfSwipe.right);
        //}
        //else
        //{
        //    Swipper.swipper.loadScene(Application.loadedLevelName, sceneName, eDirectionOfSwipe.left);
        //}
        
       // Application.LoadLevel(sceneName);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    //public void SwitchCanvas(int canvasNumber)
    //{
    //    foreach (GameObject canvas in canvases)
    //    {
    //        canvas.SetActive(false);
    //    }

    //    canvases[canvasNumber - 1].SetActive(true);
    //    for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
    //    {
    //        selectUpgradeToUpgrade[i] = false;
    //    }

    //    upgradeCostText.text = string.Empty;
    //    upgradeNameInDescription.text = string.Empty;
    //    upgradeDescription.text = string.Empty;

    //    upgradeButtonImage.sprite = null;
    //    Color buttonColor = upgradeButtonImage.color;
    //    buttonColor.a = 0;
    //    upgradeButtonImage.color = buttonColor;

    //    for (int i = 0; i < selectUpgradeToUpgrade.Length; i++)
    //    {
    //        selectUpgradeToUpgrade[i] = false;
    //        if (isUnlocked[i] && i > 3)
    //        {
    //            buttons[i].GetComponent<Image>().sprite = regularUpgrade;
    //        }
    //    }

    //    for (int i = 0; i < 4; i++)
    //    {
    //        if (isUnlocked[i] && i % 2 == 0)
    //        {
    //            buttons[i].GetComponent<Image>().sprite = damageRegularUpgrade;
    //        }
    //        else if (isUnlocked[i] && i % 2 != 0)
    //        {
    //            buttons[i].GetComponent<Image>().sprite = agilityRegularUpgrade;
    //        }
    //        else if (!isUnlocked[i] && i % 2 == 0)
    //        {
    //            buttons[i].GetComponent<Image>().sprite = damageLockedUpgrade;
    //        }
    //        else
    //        {
    //            buttons[i].GetComponent<Image>().sprite = agilityLockedUpgrade;
    //        }
    //    }
    //}

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

        if (!isUnlocked[numberOfUpgraded - 1])
        {
            if (numberOfUpgraded == 3 || numberOfUpgraded == 4 || numberOfUpgraded == 12 || numberOfUpgraded == 13)
            {
                strToDisplayInDescription = "UNLOCKS AT LEVEL 4" + Environment.NewLine + allUpgrades[numberOfUpgraded - 1][DataController.dataController.upgradesLevel[numberOfUpgraded - 1]].Description;
            }
            else if (numberOfUpgraded > 4 && numberOfUpgraded < 9)
            {
                strToDisplayInDescription = "UNLOCKS AT LEVEL 5" + Environment.NewLine + allUpgrades[numberOfUpgraded - 1][DataController.dataController.upgradesLevel[numberOfUpgraded - 1]].Description;
            }
            else if (numberOfUpgraded == 9)
            {
                strToDisplayInDescription = "UNLOCKS AT LEVEL 9" + Environment.NewLine + allUpgrades[numberOfUpgraded - 1][DataController.dataController.upgradesLevel[numberOfUpgraded - 1]].Description;
            }
        }

        upgradeCostText.text = strToDisplayInUpgrade;
        upgradeNameInDescription.text = strToDisplayInDescriptionName;
        upgradeDescription.text = strToDisplayInDescription;

        if (numberOfUpgraded < 10)
        {
            upgradeButtonImage.sprite = coin;
        }
        else
        {
            upgradeButtonImage.sprite = crystal;
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

                    totalCoinsText.text = DataController.dataController.coins.ToString();
                    upgradeCostText.text = allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Cost.ToString();
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

                    totalCrystalsText.text = DataController.dataController.crystals.ToString();
                    upgradeCostText.text = allUpgrades[numberOfUpgrade][DataController.dataController.upgradesLevel[numberOfUpgrade]].Cost.ToString();
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameObject.tag == "FirstScreen")
            {
                Application.Quit();
            }
            else if (gameObject.tag == "Store")
            {
                LoadScene("MainMenu");
            }
        }
    }

}

public enum eDirectionOfSwipe
{
    right,
    left
}
