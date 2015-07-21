using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GameController : MonoBehaviour
{

    public int knightPoints = 1;
    public int cavalierPoints = 2;
    public int archerPoints = 3;
    public int healerPoints = 4;
    public int bossPoints = 5;
    public int maxCoinsWhenLosing = 50;
    public float xpKnight = 1.5f;
    public float xpCavalier = 4f;
    public float xpArcher = 40.0f;
    public float xpHealer = 1.5f;
    public float xpBoss = 80.0f;
    public int perKillGoldFirstLevels = 5;
    public int perKillGold = 7;
    public int goldBonusPerKill = 4;
    public int levelToChangeGold = 4; // Including
    public bool isMusicOn = false;
    public bool isSoundEffectsOn = false;
    public float musicOnVolume = 0.4f;

    [HideInInspector]
    public bool gameEnded = false;
    [HideInInspector]
    public bool gameLost = false;
    public int currentLevel = 2;
    [HideInInspector]
    public bool noMoreWaves = false;
    [HideInInspector]
    public int manaCrystals = 5;

    private GameObject touchManager;
    private GameObject pauseMenu;
    private GameObject winLoseMenu;
    private GameObject pauseButton;
    private Player player;
    private Text manaCrystalsText;
    private GameObject[] specialAttackButtons;
    private ArrayList knightsOnBoard;
    private ArrayList archersOnBoard;
    private ArrayList cavaliersOnBoard;
    private ArrayList healersOnBoard;
    private ArrayList bossOnBoard;
    private ArrayList nonHealersOnBoard;
    private ArrayList allEnemiesOnBoard;
    private int enemiesOnBoardCount = 0;
    private int knightsKillCount = 0;
    private int archersKillCount = 0;
    private int cavaliersKillCount = 0;
    private int healersKillCount = 0;
    private int bossKillCount = 0;
    private int allKillCount = 0;
    private bool startEndTimeScaleEffect = false;
    private float endTimeScaleEffectStartTime = 0;
    private float delayInEndMenuDisplay = 3.0f;
    private bool calledLevelEnd = false;
    private SpriteRenderer blackBackground;
    private bool isGamePaused;






    void Start()
    {

        touchManager = GameObject.FindGameObjectWithTag("TouchManager");
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        winLoseMenu = GameObject.FindGameObjectWithTag("WinLose");
        pauseButton = GameObject.FindGameObjectWithTag("PauseButton");
        specialAttackButtons = GameObject.FindGameObjectsWithTag("SpecialAttack");
        manaCrystalsText = GameObject.FindGameObjectWithTag("ManaCrystalsText").GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        blackBackground = transform.Find("BlackBackground").GetComponent<SpriteRenderer>();
        setImageVisibilty(blackBackground, 0);

        pauseMenu.SetActive(false);
        winLoseMenu.SetActive(false);
        knightsOnBoard = new ArrayList();
        archersOnBoard = new ArrayList();
        cavaliersOnBoard = new ArrayList();
        healersOnBoard = new ArrayList();
        bossOnBoard = new ArrayList();
        nonHealersOnBoard = new ArrayList();
        allEnemiesOnBoard = new ArrayList();

        if (DataController.dataController != null)
            readLevelData();

        manaCrystalsText.text = "" + manaCrystals;

        if (isMusicOn)
        {
            GetComponent<AudioSource>().volume = musicOnVolume;
        }
        else
        {
            GetComponent<AudioSource>().volume = 0;
        }

        GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerManager>().InitializeSpawner();
    }

    private void readLevelData()
    {
        currentLevel = DataController.dataController.level;
        //Debug.Log("Reading level data, current level: " + currentLevel);
        manaCrystals = DataController.dataController.crystals;
        //isMusicOn = DataController.dataController.isMusicOn;
        //isSoundEffectsOn = DataController.dataController.isSoundEffectsOn;
    }

    void FixedUpdate()
    {


        // If there are no more waves and the board was emptied or if the game was lost
        if ((noMoreWaves && ((nonHealersOnBoard.Count + healersOnBoard.Count) == 0)) || gameLost)
        {
            if (!calledLevelEnd)
            {
                LevelEnd();
                calledLevelEnd = true;
            }

        }




    }

    void Update()
    {
        if (startEndTimeScaleEffect)
        {
            if (Time.realtimeSinceStartup - endTimeScaleEffectStartTime > delayInEndMenuDisplay)
            {
                startEndTimeScaleEffect = false;
                displayEndMenu();
            }
            else
            {
                float newTimeScale = Time.timeScale;
                newTimeScale -= 0.05f * (Time.realtimeSinceStartup - endTimeScaleEffectStartTime);
                setImageVisibilty(blackBackground, 1 - Time.timeScale);
                Time.timeScale = Mathf.Max(newTimeScale, 0.2f);
                GetComponent<AudioSource>().volume = newTimeScale;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void AddEnemy(GameObject enemyObject)
    {

        Enemy enemy = enemyObject.GetComponent<Enemy>();
        allEnemiesOnBoard.Add(enemyObject);

        if (enemy.enemyType == Enemy.EnemyType.Knight)
        {
            knightsOnBoard.Add(enemyObject);
            nonHealersOnBoard.Add(enemyObject);
            enemiesOnBoardCount++;

        }
        if (enemy.enemyType == Enemy.EnemyType.Archer)
        {
            archersOnBoard.Add(enemyObject);
            nonHealersOnBoard.Add(enemyObject);
            enemiesOnBoardCount++;
        }
        if (enemy.enemyType == Enemy.EnemyType.Cavalier)
        {
            cavaliersOnBoard.Add(enemyObject);
            nonHealersOnBoard.Add(enemyObject);
            enemiesOnBoardCount++;
        }
        if (enemy.enemyType == Enemy.EnemyType.Boss)
        {
            bossOnBoard.Add(enemyObject);
            nonHealersOnBoard.Add(enemyObject);
            enemiesOnBoardCount++;
        }
        if (enemy.enemyType == Enemy.EnemyType.Healer)
        {
            healersOnBoard.Add(enemyObject);
            enemiesOnBoardCount++;
        }
    }

    public void RemoveEnemy(GameObject enemyObject)
    {

        Enemy enemy = enemyObject.GetComponent<Enemy>();
        allEnemiesOnBoard.Remove(enemyObject);

        if (enemy.enemyType == Enemy.EnemyType.Knight)
        {
            knightsOnBoard.Remove(enemyObject);
            nonHealersOnBoard.Remove(enemyObject);
            knightsKillCount++;
            allKillCount++;
            enemiesOnBoardCount--;
        }
        if (enemy.enemyType == Enemy.EnemyType.Archer)
        {
            archersOnBoard.Remove(enemyObject);
            nonHealersOnBoard.Remove(enemyObject);
            archersKillCount++;
            allKillCount++;
            enemiesOnBoardCount--;
        }
        if (enemy.enemyType == Enemy.EnemyType.Cavalier)
        {
            cavaliersOnBoard.Remove(enemyObject);
            nonHealersOnBoard.Remove(enemyObject);
            cavaliersKillCount++;
            allKillCount++;
            enemiesOnBoardCount--;
        }
        if (enemy.enemyType == Enemy.EnemyType.Boss)
        {
            bossOnBoard.Remove(enemyObject);
            nonHealersOnBoard.Remove(enemyObject);
            bossKillCount++;
            allKillCount++;
            enemiesOnBoardCount--;
        }
        if (enemy.enemyType == Enemy.EnemyType.Healer)
        {
            healersOnBoard.Remove(enemyObject);
            healersKillCount++;
            allKillCount++;
            enemiesOnBoardCount--;
        }

        //GameObject.Destroy(enemyObject);
    }

    public ArrayList nonHealerEnemies()
    {
        if (nonHealersOnBoard.Count == 0)
            return null;
        return nonHealersOnBoard;
    }

    public void PauseGame()
    {

        Time.timeScale = 0;
        GetComponent<AudioSource>().Pause();
        touchManager.SetActive(false);
        pauseMenu.SetActive(true);
        foreach (GameObject button in specialAttackButtons)
        {
            button.SetActive(false);
        }

        isGamePaused = true;

    }

    public void ResumeGame()
    {

        Time.timeScale = 1;
        GetComponent<AudioSource>().Play();
        touchManager.SetActive(true);
        pauseMenu.SetActive(false);
        foreach (GameObject button in specialAttackButtons)
        {
            button.SetActive(true);
        }
    }

    public void Music()
    {
        isMusicOn = !isMusicOn;
        if (DataController.dataController != null)
        {
            //DataController.dataController.isMusicOn = isMusicOn;
        }
        if (isMusicOn)
        {
            GetComponent<AudioSource>().volume = musicOnVolume;
        }
        else
        {
            GetComponent<AudioSource>().volume = 0;
        }
    }

    public void SFX()
    {
        isSoundEffectsOn = !isSoundEffectsOn;
        if (DataController.dataController != null)
        {
            //DataController.dataController.isSoundEffectsOn = isSoundEffectsOn;
        }
    }

    public void Continue()
    {
        Time.timeScale = 1.0f;
        Application.LoadLevel("Store");
    }

    public void Exit()
    {
        gameLost = true;
        pauseMenu.SetActive(false);
        LevelEndOnExit();

    }

    public ArrayList getAllEnemiesOnBoard()
    {
        ArrayList returnedEnemies = new ArrayList();
        foreach (GameObject enemy in allEnemiesOnBoard)
        {
            returnedEnemies.Add(enemy);
        }
        return returnedEnemies;
    }

    private void LevelEnd()
    {
        gameEnded = true;
        pauseButton.SetActive(false);
        int points = CoinsCalculation(!gameLost);

        endTimeScaleEffectStartTime = Time.realtimeSinceStartup;
        startEndTimeScaleEffect = true;
    }

    private void LevelEndOnExit()
    {
        gameEnded = true;
        pauseButton.SetActive(false);
        int points = CoinsCalculation(!gameLost);
        displayEndMenu();

        
    }

    private void displayEndMenu()
    {
        //GameObject menu;
        //string message;

        Time.timeScale = 0;
        //if (!gameLost)
        //{

        //    if (DataController.dataController != null)
        //        DataController.dataController.crystalsFromStage = 2;
        //    menu = winMenu;
        //    message = "Level Won!";
        //}

        //else
        //{
        //    if (DataController.dataController != null)
        //        DataController.dataController.crystalsFromStage = 0;

        //    menu = loseMenu;
        //    message = "Level Lost!";
        //}

        //menu.transform.FindChild("Message").GetComponent<Text>().text = message;
        //menu.SetActive(true);

        winLoseMenu.SetActive(true);
        setImageVisibilty(blackBackground, 1);

        GameObject data = GameObject.Find("Data");
        Text enemiesText = data.transform.FindChild("EnemiesText").GetComponent<Text>();
        Text healthText = data.transform.FindChild("HealthText").GetComponent<Text>();
        Text coinsText = data.transform.FindChild("CoinsText").GetComponent<Text>();
        Text crystalsText = data.transform.FindChild("CrystalsText").GetComponent<Text>();

        foreach (GameObject button in specialAttackButtons)
        {
            button.SetActive(false);
        }

        int crystals = 0;

        if (!gameLost)
        {
            GameObject.Find("WinImage").SetActive(true);
            GameObject.Find("YouWonImage").SetActive(true);
            GameObject.Find("LoseImage").SetActive(false);
            GameObject.Find("GameOverImage").SetActive(false);
            crystals = 2;

        }
        else
        {
            GameObject.Find("WinImage").SetActive(false);
            GameObject.Find("YouWonImage").SetActive(false);
            GameObject.Find("LoseImage").SetActive(true);
            GameObject.Find("GameOverImage").SetActive(true);
        }

        int coins = CoinsCalculation(!gameLost);

        enemiesText.text = allKillCount.ToString();
        healthText.text = player.getCurrentHealth().ToString();
        coinsText.text = coins.ToString();
        crystalsText.text = crystals.ToString();

        if (DataController.dataController != null)
        {
            
            DataController.dataController.level += (gameLost) ? 0 : 1;
            DataController.dataController.coins += coins;
            DataController.dataController.crystals += crystals;
            DataController.dataController.Save();
        }

        

    }

    public void UseManaCrystals()
    {

        if (manaCrystals < 1 || isGamePaused || gameEnded)
            return;

        manaCrystals--;
        manaCrystalsText.text = "" + manaCrystals;
        player.AddMana();


    }

    private int CoinsCalculation(bool win)
    {

        int coins = allKillCount * ((currentLevel >= levelToChangeGold) ? perKillGold : perKillGoldFirstLevels);

        if (win)
        {
            coins += CoinsBonusCalculation();

        }
        return coins;
    }

    private int CoinsBonusCalculation()
    {

        return allKillCount * goldBonusPerKill;
    }

    private float XPCalculation(bool win)
    {

        float xp = 0.0f;
        if (win)
        {
            xp = knightsKillCount * xpKnight + cavaliersKillCount * xpCavalier + archersKillCount * xpArcher + healersKillCount * xpHealer + bossKillCount * xpBoss;
        }
        else
        {
            xp = allKillCount / 2;
        }
        return xp;
    }

    public int GetEnemiesOnBoardCount()
    {
        return enemiesOnBoardCount;
    }

    private void setImageVisibilty(SpriteRenderer imageToChangeVisibilty, float visibily)
    {
        if (imageToChangeVisibilty != null)
        {
            Color imageColor = imageToChangeVisibilty.color;
            imageColor.a = visibily;
            imageToChangeVisibilty.color = imageColor;
        }
    }


}

