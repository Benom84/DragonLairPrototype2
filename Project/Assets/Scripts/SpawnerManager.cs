using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class SpawnerManager : MonoBehaviour {

    public GameObject Knight;
    public GameObject Cavalier;
    public GameObject Archer;
    public GameObject Healer;
    public GameObject Boss;

    public float rowDiff = 1.0f;
    public float colDiff = 1.0f;
    public int gridSize = 5;

    private List<Wave>[] wavesByDifficulty;
    private Wave[] wavesArray;
    private List<Attack> levelAttackPlan;
    private GameController gameController;
    private int levelNumber;
    private Attack nextAttack;
    private float startTime;
    private bool levelHard = false;
	
    // Use this for initialization
	void Start () {

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        levelNumber = gameController.currentLevel;
        startTime = Time.time;

        

        // We read all the waves and get only this level waves and transfer to an array
        Waves AllWaves = ReadWaves();
        List<Wave> thisLevelWaves = new List<Wave>();

        int maxDifficulty = 1;
        foreach (Wave wave in AllWaves.WavesList)
            if (wave.FromLevel <= levelNumber)
            {
                thisLevelWaves.Add(wave);
                if (wave.Difficulty > maxDifficulty)
                    maxDifficulty = wave.Difficulty;
            }

        Debug.Log("The max difficulty found is: " + maxDifficulty);
        // We seperate the waves of this level by their difficulty
        wavesByDifficulty = new List<Wave>[maxDifficulty];
        for (int i = 0; i < maxDifficulty; i++)
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

        // We set the first wave for the creation
        nextAttack = levelAttackPlan[0];
        levelAttackPlan.Remove(nextAttack);
	
	}
	
	void FixedUpdate () {

        // If there are more attacks and it is time for the next and the game has not ended
        if ((nextAttack != null) && (Time.time > startTime + nextAttack.Time) && !gameController.gameEnded)
        {
            WaveInstantiate(FindWave(nextAttack.Id, nextAttack.Difficulty));
            
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
        //string path = Application.dataPath + Path.DirectorySeparatorChar + "LevelData" + Path.DirectorySeparatorChar + "Waves.xml";
        XmlSerializer serializer = new XmlSerializer(typeof(Waves));
        //FileStream stream = new FileStream(path, FileMode.Open);
        Waves AllWaves = serializer.Deserialize(wavesTextReader) as Waves;
        //stream.Close();
        return AllWaves;
    }

    private Level ReadLevel(int level)
    {


        string filename = "Level" + level;
        TextAsset levelText = (TextAsset)Resources.Load(filename, typeof(TextAsset));
        StringReader levelTextReader = new StringReader(levelText.text);

        //string path = Application.dataPath + Path.DirectorySeparatorChar + "LevelData" + Path.DirectorySeparatorChar + "Level" + level + ".xml";
        XmlSerializer serializer = new XmlSerializer(typeof(Level));
        //FileStream stream = new FileStream(path, FileMode.Open);
        Level currentLevel = serializer.Deserialize(levelTextReader) as Level;
        //stream.Close();
        return currentLevel;
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
                Debug.Log("The new index is: " + newIndex + " And the count is: " + wavesByDifficulty[attack.Difficulty - 1].Count);
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
