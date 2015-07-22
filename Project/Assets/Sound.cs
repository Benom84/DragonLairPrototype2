using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {

    public static Sound sound;

	public void Start () {
        if (sound == null)
        {
            DontDestroyOnLoad(gameObject);
            sound = this;
        }
        else if (sound != this)
        {
            Destroy(gameObject);
        }
	}
}
