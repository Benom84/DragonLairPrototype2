using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public bool gameEnded = false;
    public bool gameLost = false;
    public bool gameWon = false;

    private GameObject bottomSpawner;
    private GameObject topSpawner;

    private bool firstWaveEnded;
    private bool secondWaveEnded;
    private bool thirdWaveEnded;
    private bool fourthWaveEnded;
    private bool fifthWaveEnded;
    private bool sixthWaveEnded;
    
	void Start () {

        bottomSpawner = GameObject.Find("BottomSpawner");
        topSpawner = GameObject.Find("TopSpawner");

        bottomSpawner.SetActive(false);
        topSpawner.SetActive(false); 

        firstWaveEnded = false;
        secondWaveEnded = false;
        thirdWaveEnded = false;
        fourthWaveEnded = false;
        fifthWaveEnded = false;
        sixthWaveEnded = false;

	}
	
	void Update () {

        if (!firstWaveEnded)
            FirstWave();
        else if (!secondWaveEnded)
            SecondWave();
        else if (!thirdWaveEnded)
            ThirdWave();
        else if (!fourthWaveEnded)
            FourthWave();
        else if (!fifthWaveEnded)
            FifthWave();
        else if (!sixthWaveEnded)
            SixthWave();
        else
            gameWon = true;
	
	}

    void FirstWave() {
       
        bool firstEnter = true;
        float startTime = 0;
        if (firstEnter) {
            startTime = Time.time;
            firstEnter = false;
        }

        float currentTime = Time.time;
        if (!(currentTime - startTime > 4)) {
            bottomSpawner.SetActive(true);
            bottomSpawner.GetComponent<Spawner>().spawnTime = 2f;
        } else {
            bottomSpawner.SetActive(false);
            if (currentTime - startTime > 14)
                firstWaveEnded = true;
        }
    }

    void SecondWave() {
       
        bool firstEnter = true;
        float startTime = 0;
        if (firstEnter) {
            startTime = Time.time;
            firstEnter = false;
        }

        float currentTime = Time.time;
        if (!(currentTime - startTime > 6)) {
            bottomSpawner.SetActive(true);
            bottomSpawner.GetComponent<Spawner>().spawnTime = 2f;
        } else {
            bottomSpawner.SetActive(false);
            if (currentTime - startTime > 16)
                secondWaveEnded = true;
        }
    }

    void ThirdWave() {
       
        bool firstEnter = true;
        float startTime = 0;
        if (firstEnter) {
            startTime = Time.time;
            firstEnter = false;
        }

        float currentTime = Time.time;
        if (!(currentTime - startTime > 4)) {
            bottomSpawner.SetActive(true);
            bottomSpawner.GetComponent<Spawner>().spawnTime = 2.5f;
            topSpawner.SetActive(true);
            topSpawner.GetComponent<Spawner>().spawnTime = 2.0f;
        } else {
            bottomSpawner.SetActive(false);
            topSpawner.SetActive(false);
            if (currentTime - startTime > 16)
                thirdWaveEnded = true;
        }
    }

    void FourthWave() {
       
        bool firstEnter = true;
        float startTime = 0;
        if (firstEnter) {
            startTime = Time.time;
            firstEnter = false;
        }

        float currentTime = Time.time;
        if (!(currentTime - startTime > 4)) {
            bottomSpawner.SetActive(true);
            bottomSpawner.GetComponent<Spawner>().spawnTime = 2.5f;
            topSpawner.SetActive(true);
            topSpawner.GetComponent<Spawner>().spawnTime = 2.0f;
        } else {
            bottomSpawner.SetActive(false);
            topSpawner.SetActive(false);
            if (currentTime - startTime > 16)
                fourthWaveEnded = true;
        }
    }

    void FifthWave() {
       
        bool firstEnter = true;
        float startTime = 0;
        if (firstEnter) {
            startTime = Time.time;
            firstEnter = false;
        }

        float currentTime = Time.time;
        if (!(currentTime - startTime > 10)) {
            bottomSpawner.SetActive(true);
            bottomSpawner.GetComponent<Spawner>().spawnTime = 2.0f;
            if (currentTime - startTime > 4) 
                topSpawner.SetActive(false);
            else {
                topSpawner.SetActive(true);
                topSpawner.GetComponent<Spawner>().spawnTime = 2.5f;
            }
        } else {
            bottomSpawner.SetActive(false);
            if (currentTime - startTime > 20)
                fifthWaveEnded = true;
        }
    }

    void SixthWave() {

        bool firstEnter = true;
        float startTime = 0;
        if (firstEnter) {
            startTime = Time.time;
            firstEnter = false;
        }

        float currentTime = Time.time;
        if (!(currentTime - startTime > 10)) {
            bottomSpawner.SetActive(true);
            bottomSpawner.GetComponent<Spawner>().spawnTime = 2.0f;
            if (currentTime - startTime > 6)
                topSpawner.SetActive(false);
            else {
                topSpawner.SetActive(true);
                topSpawner.GetComponent<Spawner>().spawnTime = 2.5f;
            }
        } else {
            bottomSpawner.SetActive(false);
            if (currentTime - startTime > 20)
                sixthWaveEnded = true;
        }
    }
}
