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

    private bool firstWaveStarted = false;
    private bool secondWaveStarted = false;
    private bool thirdWaveStarted = false;
    private bool fourthWaveStarted = false;
    private bool fifthWaveStarted = false;
    private bool sixthWaveStarted = false;

    private float startTime = 0;

	void Start () {

        bottomSpawner = GameObject.Find("BottomSpawner");
        topSpawner = GameObject.Find("TopSpawner");

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
        if (!firstWaveStarted) {
            startTime = Time.time;
            firstWaveStarted = true;
        }

        float currentTime = Time.time;
        if (currentTime - startTime > 5) {
            bottomSpawner.GetComponent<Spawner>().CancelInvoke();
            if (currentTime - startTime > 8)
                firstWaveEnded = true;
        }
    }

    void SecondWave() {

        if (!secondWaveStarted) {
            startTime = Time.time;
            secondWaveStarted = true;
            bottomSpawner.GetComponent<Spawner>().InvokeRepeating("Spawn", 2f, 2f);
        }

        float currentTime = Time.time;
        if (currentTime - startTime > 6) {
            bottomSpawner.GetComponent<Spawner>().CancelInvoke();
            if (currentTime - startTime > 8)
                secondWaveEnded = true;
        }
    }

    void ThirdWave() {
       
        if (!thirdWaveStarted) {
            startTime = Time.time;
            thirdWaveStarted = true;
            bottomSpawner.GetComponent<Spawner>().InvokeRepeating("Spawn", 2f, 2.5f); 
            topSpawner.SetActive(true);
        }

        float currentTime = Time.time;
        if (currentTime - startTime > 5) {
            bottomSpawner.GetComponent<Spawner>().CancelInvoke();
            topSpawner.GetComponent<Spawner>().CancelInvoke();
            if (currentTime - startTime > 10)
                thirdWaveEnded = true;
        }
    }

    void FourthWave() {
     
        if (!fourthWaveStarted) {
            startTime = Time.time;
            fourthWaveStarted = true;
            bottomSpawner.GetComponent<Spawner>().InvokeRepeating("Spawn", 2f, 2.5f);
            topSpawner.GetComponent<Spawner>().InvokeRepeating("Spawn", 2f, 2f); 

        }

        float currentTime = Time.time;
        if (currentTime - startTime > 4) {
            bottomSpawner.GetComponent<Spawner>().CancelInvoke();
            topSpawner.GetComponent<Spawner>().CancelInvoke();
            if (currentTime - startTime > 10)
                fourthWaveEnded = true;
        }
    }

    void FifthWave() {

        if (!fifthWaveStarted) {
            startTime = Time.time;
            fifthWaveStarted = true;
            bottomSpawner.GetComponent<Spawner>().InvokeRepeating("Spawn", 2f, 1.5f);
            topSpawner.GetComponent<Spawner>().InvokeRepeating("Spawn", 3f, 2.5f); 
        }

        float currentTime = Time.time;

        if (currentTime - startTime > 6) 
            topSpawner.GetComponent<Spawner>().CancelInvoke();
        

        if (currentTime - startTime > 8) {
            bottomSpawner.GetComponent<Spawner>().CancelInvoke();
            if (currentTime - startTime > 14)
                fifthWaveEnded = true;
        }
    }

    void SixthWave() {

        if (!sixthWaveStarted) {
            startTime = Time.time;
            sixthWaveStarted = true;
            bottomSpawner.GetComponent<Spawner>().InvokeRepeating("Spawn", 1f, 1f);
            topSpawner.GetComponent<Spawner>().InvokeRepeating("Spawn", 2f, 2.5f); 
        }

        float currentTime = Time.time;

        if (currentTime - startTime > 6)
            topSpawner.GetComponent<Spawner>().CancelInvoke();       

        if (currentTime - startTime > 8) {
            bottomSpawner.GetComponent<Spawner>().CancelInvoke();
            if (currentTime - startTime > 14)
                sixthWaveEnded = true;
        }
    }
}
