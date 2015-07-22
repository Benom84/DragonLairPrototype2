using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {

    public static Sound sound;
    public static float volume = 0.4f;

    private AudioSource audio;

	public void Start () {
        if (sound == null)
        {
            DontDestroyOnLoad(gameObject);
            sound = this;
            audio = GetComponent<AudioSource>();
            if (!DataController.dataController.isMusicOn)
            {
                if (audio != null)
                {
                    audio.volume = 0;
                }
            }
            else
            {
                if (audio != null)
                {
                    audio.volume = volume;
                }
            }
            audio.Play();
        }
        else if (sound != this)
        {
            Destroy(gameObject);
        }
	}

    void Update()
    {
        if (!DataController.dataController.isMusicOn)
        {
            if (audio != null)
            {
                audio.volume = 0;
            }
        }
        else
        {
            if (audio != null)
            {
                audio.volume = volume;
            }
        }
    }
}
