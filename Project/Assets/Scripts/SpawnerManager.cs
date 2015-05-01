using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;

public class SpawnerManager : MonoBehaviour {

    public GameObject Knight;
    public GameObject Cavalier;
    public GameObject Archer;
    public GameObject Healer;
    public GameObject Boss;
    public double precentageOfLifeToGetHarder = 0.85f;
    public double precentageOfLevelToGetHarder = 0.6f;
    public double maxTimeForClearBoard = 3.0f;

    public float rowDiff = 1.0f;
    public float colDiff = 1.0f;
    public int gridSize = 5;

    private List<Wave>[] wavesByDifficulty;
    private Wave[] wavesArray;
    private List<Attack> levelAttackPlan;
    private GameController gameController;
    private int levelNumber;
    private int maxWaveDifficulty = 1;
    private Attack nextAttack;
    private float startTime;
    private bool levelHard = false;
    private Waves allWaves;
    private int totalNumberOfAttacks = 0;
    private Player player;
    private double attackTimeAcceleration = 0;
    private double boaredClearedTime = 0;
    private TimerBarMovement timer;
    
	
    // Use this for initialization
	void Awake () {

       
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<TimerBarMovement>();
       
	
	}

    public void InitializeSpawner()
    {

        levelNumber = gameController.currentLevel;
        startTime = Time.time;
        Debug.Log("Level number is: " + levelNumber);
        GameObject.FindGameObjectWithTag("GameLevelText").GetComponent<Text>().text = "Level: " + levelNumber;
        
        // We read all the waves and get only this level waves and transfer to an array
        allWaves = ReadWaves();
        List<Wave> thisLevelWaves = new List<Wave>();

        maxWaveDifficulty = 1;
        foreach (Wave wave in allWaves.WavesList)
            if (wave.FromLevel <= levelNumber)
            {
                thisLevelWaves.Add(wave);
                if (wave.Difficulty > maxWaveDifficulty)
                    maxWaveDifficulty = wave.Difficulty;
            }

        // We seperate the waves of this level by their difficulty
        wavesByDifficulty = new List<Wave>[maxWaveDifficulty];
        for (int i = 0; i < maxWaveDifficulty; i++)
        {
            wavesByDifficulty[i] = new List<Wave>();
        }
        wavesArray = thisLevelWaves.ToArray();
        for (int i = 0; i < wavesArray.Length; i++)
        {
            Wave wave = wavesArray[i];
            int difficulty = wave.Difficulty;

            wavesByDifficulty[difficulty - 1].Add(wave);

        }


        // We read the current level attack plan
        Level currentLevel = ReadLevel(levelNumber);

        // We create the current level attack plan according to the attacks given
        levelAttackPlan = LevelPlan(currentLevel);

        totalNumberOfAttacks = levelAttackPlan.Count;
        timer.SetMaxValue(totalNumberOfAttacks);


        // We set the first wave for the creation
        nextAttack = levelAttackPlan[0];
        levelAttackPlan.Remove(nextAttack);
    }

    void FixedUpdate()
    {
        if (gameController.gameEnded)
            return;


        if ((gameController.GetEnemiesOnBoardCount() == 0) && (boaredClearedTime == 0))
        {
            boaredClearedTime = Time.time;

        }
            
        
        // If there are more attacks and it is time for the next and the game has not ended
        if ((nextAttack != null) && ((Time.time > startTime + nextAttack.Time) || ((boaredClearedTime != 0) && (Time.time - boaredClearedTime > maxTimeForClearBoard))))
        {

            boaredClearedTime = 0;
            bool createStrongerAttack = false;
            Attack attackReplacement = null;

            // If the wave was created prematurely, increase the time acceleration of the other waves
            if (Time.time + attackTimeAcceleration < startTime + nextAttack.Time)
            {
                attackTimeAcceleration = nextAttack.Time - Time.time;
            }
            
            // If the player has a certain life precentage and we are close to the end of the level
            createStrongerAttack = ((player.getCurrentHealthPrecentage() >= precentageOfLifeToGetHarder) && 
                ((levelAttackPlan.Count + 1.0f) / totalNumberOfAttacks) <= (1 - precentageOfLevelToGetHarder));

            // If the conditions for a stronger wave attack exist replace the current attack with a stronger one
            if (createStrongerAttack)
            {
                attackReplacement = getRandomAttack(nextAttack.Difficulty + 1);
            }


            if (attackReplacement != null)
            {
                nextAttack = attackReplacement;
            }

            WaveInstantiate(FindWave(nextAttack.Id, nextAttack.Difficulty));
            timer.increaseCurrentValue();

            // If there are more attacks waiting
            if (levelAttackPlan.Count > 0)
            {
                nextAttack = levelAttackPlan[0];
                levelAttackPlan.Remove(nextAttack);
            }
            else
            {
                nextAttack = null;
                gameController.noMoreWaves = true;
            }


        }
    }
    private Waves ReadWaves()
    {

        TextAsset allWavesText = (TextAsset) Resources.Load("Waves", typeof(TextAsset));
        StringReader wavesTextReader = new StringReader(allWavesText.text);
        XmlSerializer serializer = new XmlSerializer(typeof(Waves));
        Waves AllWaves = serializer.Deserialize(wavesTextReader) as Waves;
        return AllWaves;
    }

    private Level ReadLevel(int level)
    {


        string filename = "Level" + level;
        TextAsset levelText = (TextAsset)Resources.Load(filename, typeof(TextAsset));
        StringReader levelTextReader = new StringReader(levelText.text);

        XmlSerializer serializer = new XmlSerializer(typeof(Level));
        Level currentLevel = serializer.Deserialize(levelTextReader) as Level;
        return currentLevel;
    }

    private Attack getRandomAttack(int waveDifficuly)
    {

        
        
        if (waveDifficuly > maxWaveDifficulty)
            return null;

        Attack attack = new Attack();
        System.Random random = new System.Random();
        int newIndex = random.Next(0, wavesByDifficulty[waveDifficuly - 1].Count);
        attack.Id = wavesByDifficulty[waveDifficuly - 1][newIndex].Id;
        attack.Difficulty = waveDifficuly;

        return attack;


    }
    private List<Attack> LevelPlan(Level currentLevel)
    {

        System.Random random = new System.Random();


        List<Attack> modifiedLevelPlan = new List<Attack>();

        foreach (Attack attack in currentLevel.attackList)
        {

            
            // We check if we should replace the id
            double chance = random.NextDouble();
            if (chance < attack.ChangeChance)
            {
                // Create a random number to get a new wave of equal difficulty
                int newIndex = random.Next(0, wavesByDifficulty[attack.Difficulty - 1].Count);
                attack.Id = wavesByDifficulty[attack.Difficulty - 1][newIndex].Id;
            }


            modifiedLevelPlan.Add(attack);

        }

        return modifiedLevelPlan;
    }
    
    private void WaveInstantiate(Wave wave)
    {

        foreach (Wave.Attacker attacker in wave.Attackers)
        {

            int row = (attacker.Position % gridSize);
            int col = (attacker.Position / gridSize);

            float colPosition = col * colDiff;
            float rowPosition = row * rowDiff;

            // Will make knight as default
            GameObject toInstantiate = Knight;

            if (attacker.Type == "Knight")
                toInstantiate = Knight;
            if (attacker.Type == "Cavalier")
                toInstantiate = Cavalier;
            if (attacker.Type == "Healer")
                toInstantiate = Healer;
            if (attacker.Type == "Archer")
                toInstantiate = Archer;
            if (attacker.Type == "Boss")
                toInstantiate = Boss;

            Vector3 position = this.transform.position;
            position.x += colPosition;
            position.y += rowPosition;

            GameObject newEnemy = (GameObject) Instantiate(toInstantiate, position, this.transform.rotation);
            newEnemy.GetComponent<SpriteRenderer>().sortingOrder = gridSize - (attacker.Position % gridSize);
            gameController.AddEnemy(newEnemy);

        }


    }

    private Wave FindWave(int id, int difficulty)
    {

        // Default wave is the first in the same difficulty result
        Wave result = wavesByDifficulty[difficulty - 1][0];
        
        // Find the right one with the ID
        foreach (Wave wave in wavesByDifficulty[difficulty - 1]) {
            if (wave.Id == id)
                result = wave;
        }

        return result;
    }
    
}
