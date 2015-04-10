using UnityEngine;
using System.Collections;

public class GameMenuButtons : MonoBehaviour {

    public enum GameButton { Pause, Resume, Exit, Sound, FX };

    public GameButton gameButton;

    void OnClick()
    {
        Debug.Log("There was a click on a button");
        if (gameButton == GameButton.Pause)
        {
            Time.timeScale = 0;
        }

    }
}
