using UnityEngine;
using System.Collections;

public class UIscript : MonoBehaviour
{

    public void LoadScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }
}
