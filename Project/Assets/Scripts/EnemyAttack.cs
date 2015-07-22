using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{

    [HideInInspector]
    public int attackDamage = 1;
    public AudioClip attackHitSound;
    public GameObject soundPlayer;

    private GameController gameController;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.tag == "Border")
            GameObject.Destroy(gameObject);

        if (collider.tag == "Player")
        {

            collider.GetComponent<Player>().Hit(attackDamage);

            if (gameController.isSoundEffectsOn)
            {
                GameObject createdSoundPlayer = (GameObject)Instantiate(soundPlayer);
                SoundPlayerScript soundPlayerScript = createdSoundPlayer.GetComponent<SoundPlayerScript>();
                if (soundPlayerScript != null)
                {
                    soundPlayerScript.PlaySound(attackHitSound);
                }
                else
                {
                    GameObject.Destroy(createdSoundPlayer);
                }
                
            }

            GameObject.Destroy(gameObject);
        }
    }
}
