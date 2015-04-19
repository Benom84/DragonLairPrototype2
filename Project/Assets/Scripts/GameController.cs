using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GameController : MonoBehaviour {

    public int knightPoints = 1;
    public int cavalierPoints = 2;
    public int archerPoints = 3;
    public int healerPoints = 4;
    public int bossPoints = 5;
    public int maxPointsWhenLosing = 50;
    
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


        
        

       if (DataController.dataController != null)
            readLevelData();

        manaCrystalsText.text = "" + manaCrystals;
 
     
	}

    private void readLevelData()
    {
        currentLevel = DataController.dataController.level;
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
        if (enemy.enemyType == Enemy.EnemyType.Knight) {
            knightsOnBoard.Add(enemyObject);
            nonHealersOnBoard.Add(enemyObject);

        }
        if (enemy.enemyType == Enemy.EnemyType.Archer)
        {
            archersOnBoard.Add(enemyObject);
            nonHealersOnBoard.Add(enemyObject);
        }
        if (enemy.enemyType == Enemy.EnemyType.Cavalier)
        {
            cavaliersOnBoard.Add(enemyObject);
            nonHealersOnBoard.Add(enemyObject);
        }
        if (enemy.enemyType == Enemy.EnemyType.Boss)
        {
            bossOnBoard.Add(enemyObject);
            nonHealersOnBoard.Add(enemyObject);
        }
        if (enemy.enemyType == Enemy.EnemyType.Healer)
        {
            healersOnBoard.Add(enemyObject);
        }
    }

    public void RemoveEnemy(GameObject enemyObject)
    {

        Enemy enemy = enemyObject.GetComponent<Enemy>();
        if (enemy.enemyType == Enemy.EnemyType.Knight) {
            knightsOnBoard.Remove(enemyObject);
            nonHealersOnBoard.Remove(enemyObject);
            knightsKillCount++;
            allKillCount++;
        }
        if (enemy.enemyType == Enemy.EnemyType.Archer)
        {
            archersOnBoard.Remove(enemyObject);
            nonHealersOnBoard.Remove(enemyObject);
            archersKillCount++;
            allKillCount++;
        }
        if (enemy.enemyType == Enemy.EnemyType.Cavalier)
        {
            cavaliersOnBoard.Remove(enemyObject);
            nonHealersOnBoard.Remove(enemyObject);
            cavaliersKillCount++;
            allKillCount++;
        }
        if (enemy.enemyType == Enemy.EnemyType.Boss)
        {
            bossOnBoard.Remove(enemyObject);
            nonHealersOnBoard.Remove(enemyObject);
            bossKillCount++;
            allKillCount++;
        }
        if (enemy.enemyType == Enemy.EnemyType.Healer)
        {
            healersOnBoard.Remove(enemyObject);
            healersKillCount++;
            allKillCount++;
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
            DataController.dataController.coinsFromStage = PointsCalculation(false);
            DataController.dataController.crystalsFromStage = 2;
            DataController.dataController.life = player.getCurrentHealth();
        }

        Time.timeScale = 1.0f;
        Application.LoadLevel("EndGameScreen");

    }

    private void LevelEnd()
    {
        gameEnded = true;
        pauseButton.SetActive(false);
        Time.timeScale = 0;
        int points = PointsCalculation(!gameLost);
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
    
    private int PointsCalculation(bool win)
    {

        int points = (knightsKillCount * knightPoints) + (cavaliersKillCount * cavalierPoints) + (archersKillCount * archerPoints) + (healersKillCount * healerPoints) + (bossKillCount * bossPoints);

        if (win)
            points = Mathf.Min(points, maxPointsWhenLosing);

        return points;
    }

   
}

