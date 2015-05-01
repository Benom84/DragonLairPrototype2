using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GameController : MonoBehaviour {

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
    private GameObject winMenu;
    private GameObject loseMenu;
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

    

    

	void Start () {

        touchManager = GameObject.FindGameObjectWithTag("TouchManager");
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        winMenu = GameObject.FindGameObjectWithTag("WinMenu");
        loseMenu = GameObject.FindGameObjectWithTag("LoseMenu");
        pauseButton = GameObject.FindGameObjectWithTag("PauseButton");
        specialAttackButtons = GameObject.FindGameObjectsWithTag("SpecialAttack");
        manaCrystalsText = GameObject.FindGameObjectWithTag("ManaCrystalsText").GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();


        
        pauseMenu.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
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

        GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerManager>().InitializeSpawner();
 
     
	}

    private void readLevelData()
    {
        currentLevel = DataController.dataController.level;
        Debug.Log("Reading level data, current level: " + currentLevel); 
        manaCrystals = DataController.dataController.crystals;
    }

    void FixedUpdate()
    {

        
        // If there are no more waves and the board was emptied or if the game was lost
        if ((noMoreWaves && ((nonHealersOnBoard.Count + healersOnBoard.Count) == 0)) || gameLost)
            LevelEnd();

    }

    public void AddEnemy(GameObject enemyObject)
    {

        Enemy enemy = enemyObject.GetComponent<Enemy>();
        allEnemiesOnBoard.Add(enemyObject);
        
        if (enemy.enemyType == Enemy.EnemyType.Knight) {
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
        
        if (enemy.enemyType == Enemy.EnemyType.Knight) {
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

        GameObject.Destroy(enemyObject);
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
        touchManager.SetActive(false);
        pauseMenu.SetActive(true);
        foreach (GameObject button in specialAttackButtons)
        {
            button.SetActive(false);
        }

    }

    public void ResumeGame()
    {

        Time.timeScale = 1;
        touchManager.SetActive(true);
        pauseMenu.SetActive(false);
        foreach (GameObject button in specialAttackButtons)
        {
            button.SetActive(true);
        }
    }

    public void Music()
    {

    }

    public void SFX()
    {

    }

    public void Continue()
    {
        Time.timeScale = 1.0f;
        Application.LoadLevel("EndGameScreen");
    }

    public void Exit()
    {
        if (DataController.dataController != null)
        {
            DataController.dataController.kills = allKillCount;
            DataController.dataController.won = false;
            DataController.dataController.coinsFromStage = CoinsCalculation(false);
            DataController.dataController.crystalsFromStage = 2;
            DataController.dataController.life = player.getCurrentHealth();
        }

        Time.timeScale = 1.0f;
        Application.LoadLevel("EndGameScreen");

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
        Time.timeScale = 0;
        int points = CoinsCalculation(!gameLost);
        GameObject menu;
        string message;

        if (DataController.dataController != null)
        {
            DataController.dataController.kills = allKillCount;
            DataController.dataController.won = !gameLost;
            DataController.dataController.coinsFromStage = points;
            DataController.dataController.life = player.getCurrentHealth();
        }
        
        
        
        if (!gameLost) {
            
            if (DataController.dataController != null)
                DataController.dataController.crystalsFromStage = 2;
            menu = winMenu;
            message = "Level Won!";
        }

        else
        {
            if (DataController.dataController != null)
                DataController.dataController.crystalsFromStage = 0;
            
            menu = loseMenu;
            message = "Level Lost!";
        }



        menu.transform.FindChild("Message").GetComponent<Text>().text = message;
        menu.SetActive(true);
        
    }

    public void UseManaCrystals()
    {

        if (manaCrystals < 1)
            return;

        manaCrystals--;
        manaCrystalsText.text = "" + manaCrystals;
        player.AddMana();


    }
    
    private int CoinsCalculation(bool win)
    {

        int coins = allKillCount * ((currentLevel >= levelToChangeGold) ? perKillGold : perKillGoldFirstLevels);
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

   
}

