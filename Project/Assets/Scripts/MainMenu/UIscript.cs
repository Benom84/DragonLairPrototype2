using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIscript : MonoBehaviour
{   
    private GameObject AirBreathCanvas;
    private GameObject AirMagicCanvas;
    private GameObject AirProtectCanvas;
    private GameObject FireBreathCanvas;
    private GameObject FireMagicCanvas;
    private GameObject FireProtectCanvas;
    private GameObject WaterBreathCanvas;
    private GameObject WaterMagicCanvas;
    private GameObject WaterProtectCanvas;

    private GameObject[] canvases;

    private GameObject[] totalCoins;
    private Text[] totalCoinsText;

    private GameObject[] totalCrystals;
    private Text[] totalCrystalsText;

    private bool damage;
    private bool range;
    private bool firstBreathSpecial;
    private bool secondBreathSpecial;
    private bool scream;
    private bool magicSpecial;
    private bool earthquake;
    private bool cave;
    private bool protectSpecial;

    private bool[] selectUpgraded;

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

            AirBreathCanvas = GameObject.Find("AirBreathCanvas");
            AirMagicCanvas = GameObject.Find("AirMagicCanvas");
            AirProtectCanvas = GameObject.Find("AirProtectCanvas");
            FireBreathCanvas = GameObject.Find("FireBreathCanvas");
            FireMagicCanvas = GameObject.Find("FireMagicCanvas");
            FireProtectCanvas = GameObject.Find("FireProtectCanvas");
            WaterBreathCanvas = GameObject.Find("WaterBreathCanvas");
            WaterMagicCanvas = GameObject.Find("WaterMagicCanvas");
            WaterProtectCanvas = GameObject.Find("WaterProtectCanvas");

            canvases = new GameObject[9] { AirBreathCanvas, AirMagicCanvas, AirProtectCanvas, 
                FireBreathCanvas, FireMagicCanvas, FireProtectCanvas, WaterBreathCanvas, WaterMagicCanvas, WaterProtectCanvas };

            damage = true;
            range = false;
            firstBreathSpecial = false;
            secondBreathSpecial = false;
            scream = false;
            magicSpecial = false;
            earthquake = false;
            cave = false;
            protectSpecial = false;

            selectUpgraded = new bool[9] { damage, range, firstBreathSpecial, secondBreathSpecial, 
                scream, magicSpecial, earthquake, cave, protectSpecial };

            for (int i = 0; i < 9; i++)
            {
                canvases[i].SetActive(false);
            } 
            
            switch (DataController.dataController.attackType)
            {
                case DragonAttack.AttackType.Air:
                    AirBreathCanvas.SetActive(true);
                    break;
                case DragonAttack.AttackType.Fire:
                    FireBreathCanvas.SetActive(true);
                    break;
                case DragonAttack.AttackType.Water:
                    WaterBreathCanvas.SetActive(true);
                    break;
            }

            totalCoins = GameObject.FindGameObjectsWithTag("TotalCoins");
            totalCrystals = GameObject.FindGameObjectsWithTag("TotalCrystals");
            for (int i = 0; i < 9; i++)
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
        Application.LoadLevel(sceneName);
        DataController.dataController.Save();
    }

    public void SwitchCanvas(int canvasNumber)
    {
        foreach (GameObject canvas in canvases)
        {
            canvas.SetActive(false);
        }
        canvases[canvasNumber - 1].SetActive(true);
        for (int i = 0; i < 9; i++)
        {
            selectUpgraded[i] = false;
        }
        if (canvasNumber == 1 || canvasNumber == 4 || canvasNumber == 7)
        {
            selectUpgraded[0] = true;
        }
        else if (canvasNumber % 3 == 0)
        {
            selectUpgraded[7] = true;
        }
        else
        {
            selectUpgraded[5] = true;
        }
    }

    public void LoadAttack(string attackName)
    {
        switch (attackName)
        {
            case "Air":
                DataController.dataController.attackType = DragonAttack.AttackType.Air;
                break;
            case "Fire":
                DataController.dataController.attackType = DragonAttack.AttackType.Fire;
                break;
            case "Water":
                DataController.dataController.attackType = DragonAttack.AttackType.Water;
                break;
        }
    }

    public void ChooseToUpgrade(int numberOfUpgraded)
    {
        for (int i = 0; i < 9; i++)
        {
            selectUpgraded[i] = false;
        }
        selectUpgraded[numberOfUpgraded - 1] = true;
    }

    public void Upgrade()
    {
        int numberToBeUpgraded = 0;

        for (int i = 0; i < 9; i++)
        {
            if (selectUpgraded[i])
            {
                numberToBeUpgraded = i + 1;
                break;
            }
        }

        if (DataController.dataController.attackType == DragonAttack.AttackType.Air)
        {
            switch (numberToBeUpgraded)
            {
                case 1:
                    DataController.dataController.b_airDamage++;
                    break;
                case 2:
                    DataController.dataController.b_airRange++;
                    break;
                case 3:
                    DataController.dataController.b_airSkyFall++;
                    break;
                case 4:
                    DataController.dataController.b_airCursedBreath++;
                    break;
                case 5:
                    DataController.dataController.m_scream++;
                    break;
                case 6:
                    DataController.dataController.m_airTornado++;
                    break;
                case 7:
                    DataController.dataController.m_earthquake++;
                    break;
                case 8:
                    DataController.dataController.p_cave++;
                    break;
                case 9:
                    DataController.dataController.p_airTornado++;
                    break;
            }
        } 
        else if (DataController.dataController.attackType == DragonAttack.AttackType.Fire) 
        {
            switch (numberToBeUpgraded)
            {
                case 1:
                    DataController.dataController.b_fireDamage++;
                    break;
                case 2:
                    DataController.dataController.b_fireRange++;
                    break;
                case 3:
                    DataController.dataController.b_fireHeavnlyFire++;
                    break;
                case 4:
                    DataController.dataController.b_fireThunder++;
                    break;
                case 5:
                    DataController.dataController.m_scream++;
                    break;
                case 6:
                    DataController.dataController.m_fireMeteor++;
                    break;
                case 7:
                    DataController.dataController.m_earthquake++;
                    break;
                case 8:
                    DataController.dataController.p_cave++;
                    break;
                case 9:
                    DataController.dataController.p_fireLava++;
                    break;
            }
        }
        else if (DataController.dataController.attackType == DragonAttack.AttackType.Water)
        {
            switch (numberToBeUpgraded)
            {
                case 1:
                    DataController.dataController.b_waterDamage++;
                    break;
                case 2:
                    DataController.dataController.b_waterRange++;
                    break;
                case 3:
                    DataController.dataController.b_waterFrozenSky++;
                    break;
                case 4:
                    DataController.dataController.b_waterCursedBreath++;
                    break;
                case 5:
                    DataController.dataController.m_scream++;
                    break;
                case 6:
                    DataController.dataController.m_waterMist++;
                    break;
                case 7:
                    DataController.dataController.m_earthquake++;
                    break;
                case 8:
                    DataController.dataController.p_cave++;
                    break;
                case 9:
                    DataController.dataController.p_waterIceWall++;
                    break;
            }
        }
        DataController.dataController.Save();

    }
}
