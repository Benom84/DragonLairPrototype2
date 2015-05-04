using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIscript : MonoBehaviour
{
    private const int amountOfCanvases = 3;
    
    //store stuff
    private GameObject WaterBreathCanvas; //old
    private GameObject WaterMagicCanvas;//old
    private GameObject WaterProtectCanvas;//old
    private GameObject FireBreathCanvas;//old
    private GameObject FireMagicCanvas;//old
    private GameObject FireProtectCanvas;//old

    private GameObject BreathCanvas;
    private GameObject LifeCanvas;
    private GameObject MagicCanvas;

    private GameObject Damage;
    private GameObject Agility;
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

    private bool damage;
    private bool agility;
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
            DataController.dataController.Load();
        }
        else if (gameObject.tag == "Store")
        {
            //canvases
            WaterBreathCanvas = GameObject.Find("WaterBreathCanvas");//old
            WaterMagicCanvas = GameObject.Find("WaterMagicCanvas");//old
            WaterProtectCanvas = GameObject.Find("WaterProtectCanvas");//old
            FireBreathCanvas = GameObject.Find("FireBreathCanvas");//old
            FireMagicCanvas = GameObject.Find("FireMagicCanvas");//old
            FireProtectCanvas = GameObject.Find("FireProtectCanvas");//old
            NotEnoughMoneyCanvas = GameObject.Find("NotEnoughMoneyCanvas");//old

            BreathCanvas = GameObject.Find("BreathCanvas");
            LifeCanvas = GameObject.Find("LifeCanvas");
            MagicCanvas = GameObject.Find("MagicCanvas");
            
            canvases = new GameObject[] { BreathCanvas, LifeCanvas, MagicCanvas };

            NotEnoughMoneyCanvas.SetActive(false);

            //select upgrade to upgrade
            damage = true;
            agility = false;
            heavenlyFire = false;
            frozenSky = false;
            thunder = false;
            cursedBreath = false;
            cave = false;
            scream = false;
            ice = false;
            earthquake = false;

            selectUpgradeToUpgrade = new bool[] { damage, agility, heavenlyFire, frozenSky, 
                thunder, cursedBreath, cave, scream, ice, earthquake };
            
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

            Damage = GameObject.Find("Damage");
            Agility = GameObject.Find("Agility");
            HeavenlyFire = GameObject.Find("HeavenlyFire");
            Thunder = GameObject.Find("Thunder");
            FrozenSky = GameObject.Find("FrozenSky");
            CursedBreath = GameObject.Find("CursedBreath");
            Cave = GameObject.Find("Cave");
            Scream = GameObject.Find("Scream");
            Meteor = GameObject.Find("Meteor");
            Ice = GameObject.Find("Ice");
            Earthquake = GameObject.Find("Earthquake");

            buttons = new GameObject[] { Damage, Agility, HeavenlyFire, FrozenSky, Thunder,
                CursedBreath, Cave, Scream, Meteor, Ice, Earthquake};

            isUnlocked = new bool[11];

            for (int i = 0; i < isUnlocked.Length; i++)
            {
                isUnlocked[i] = true;
            }

            //if conditions to unlock the upgrades
            if (DataController.dataController.level < 5)
            {
                isUnlocked[2] = false;
                isUnlocked[3] = false;
                isUnlocked[4] = false;
                isUnlocked[5] = false;
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
}
