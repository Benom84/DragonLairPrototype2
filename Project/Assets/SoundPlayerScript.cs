using UnityEngine;
using System.Collections;

public class SoundPlayerScript : MonoBehaviour {

    private float endTime = float.MaxValue;

    public void PlaySound(AudioClip soundToPlay) {

        endTime = soundToPlay.length + Time.timeSinceLevelLoad;
        GetComponent<AudioSource>().PlayOneShot(soundToPlay);


    }

    void Update()
    {
        if (Time.timeSinceLevelLoad > endTime) {
            GameObject.Destroy(gameObject);
            
        }
    }
	
	
}
