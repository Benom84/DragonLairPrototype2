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
    [HideInInspector]
    public int currentLevel = 1;
    [HideInInspector]
    public bool noMoreWaves = false;
    [HideInInspector]
    public int manaCrystals = 5;
    
    private GameObject touchManager;
    private GameObject pauseMenu;
    private GameObject winMenu;
    private GameObject loseMenu;
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
    

    

	void Start () {

        touchManager = GameObject.FindGameObjectWithTag("TouchManager");
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        winMenu = GameObject.FindGameObjectWithTag("WinMenu");
        loseMenu = GameObject.FindGameObjectWithTag("LoseMenu");
        specialAttackButtons = GameObject.FindGameObjectsWithTag("SpecialAttack");
        manaCrystalsText = GameObject.FindGameObjectWithTag("ManaCrystalsText").GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        manaCrystalsText.text = "" + manaCrystals;
        pauseMenu.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        knightsOnBoard = new ArrayList();
        archersOnBoard = new ArrayList();
        cavaliersOnBoard = new ArrayList();
        healersOnBoard = new ArrayList();
        bossOnBoard = new ArrayList();
        nonHealersOnBoard = new ArrayList();
     
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
        }
        if (enemy.enemyType == Enemy.EnemyType.Archer)
        {
            archersOnBoard.Remove(enemyObject);
            nonHealersOnBoard.Remove(enemyObject);
            archersKillCount++;
        }
        if (enemy.enemyType == Enemy.EnemyType.Cavalier)
        {
            cavaliersOnBoard.Remove(enemyObject);
            nonHealersOnBoard.Remove(enemyObject);
            cavaliersKillCount++;
        }
        if (enemy.enemyType == Enemy.EnemyType.Boss)
        {
            bossOnBoard.Remove(enemyObject);
            nonHealersOnBoard.Remove(enemyObject);
            bossKillCount++;
        }
        if (enemy.enemyType == Enemy.EnemyType.Healer)
        {
            healersOnBoard.Remove(enemyObject);
            healersKillCount++;
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

    public void Exit()
    {
        Application.Quit();
    }

    private void LevelEnd()
    {
        gameEnded = true;
        Time.timeScale = 0;
        int points = PointsCalculation(!gameLost);
        GameObject menu;
        string message;
        
        if (!gameLost) {
            menu = winMenu;
            message = "Level Won!\nNumber of points is: " + points;
        }

        else
        {
            menu = loseMenu;
            message = "Level Lost :(\nNumber of points is: " + points;
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

