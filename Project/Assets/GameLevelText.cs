using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameLevelText : MonoBehaviour {

	// Use this for initialization
	void Start () {

        gameObject.GetComponent<Text>().text = "Current Level: " + DataController.dataController.level;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
