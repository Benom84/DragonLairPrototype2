using UnityEngine;
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
        }
    }

    public void LoadScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public void SwitchCanvas(int canvasNumber)
    {
        foreach (GameObject canvas in canvases)
        {
            canvas.SetActive(false);
        }
        canvases[canvasNumber].SetActive(true);
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
}
