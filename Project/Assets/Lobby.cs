using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Lobby : MonoBehaviour {

	private GameObject levelOne;
    private GameObject levelTwo;
    private GameObject levelThree;
    private GameObject levelFour;
    private GameObject levelFive;
    private GameObject levelSix;
    private GameObject levelSeven;
    private GameObject levelEight;
    private GameObject levelNine;
    private GameObject levelTen;
    private GameObject levelEleven;
    private GameObject levelTwelve;
    private GameObject dragon;

    private GameObject[] levels;

    private GameObject stages;

	void Start () 
    {
        levelOne = GameObject.Find("01");
        levelTwo = GameObject.Find("02");
        levelThree = GameObject.Find("03");
        levelFour = GameObject.Find("04");
        levelFive = GameObject.Find("05");
        levelSix = GameObject.Find("06");
        levelSeven = GameObject.Find("07");
        levelEight = GameObject.Find("08");
        levelNine = GameObject.Find("09");
        levelTen = GameObject.Find("10");
        levelEleven = GameObject.Find("11");
        levelTwelve = GameObject.Find("12");
        dragon = GameObject.Find("13");

        levels = new GameObject[] { levelOne, levelTwo, levelThree, levelFour, levelFive, levelSix, levelSeven, levelEight, levelNine, levelTen, levelEleven, levelTwelve, dragon };

        Color color;
        Image selectedImage;

        for (int i = 0; i < levels.Length; i++)
        {
            if (DataController.dataController.level != (i + 1))
            {
                selectedImage = levels[i].transform.FindChild("Glow").GetComponent<Image>();
                color = selectedImage.color;
                color.a = 0;
                selectedImage.color = color;
            }

            if (DataController.dataController.level <= (i + 1))
            {
                selectedImage = levels[i].transform.FindChild("Fill").GetComponent<Image>();
                color = selectedImage.color;
                color.a = 0;
                selectedImage.color = color;
            }
        }

        stages = GameObject.Find("Stages");

        stages.GetComponent<Text>().text = "Stage: " + (DataController.dataController.level - 1).ToString() + "/12";
	}
	

	
}
