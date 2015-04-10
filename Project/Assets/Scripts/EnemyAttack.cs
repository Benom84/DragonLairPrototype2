using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

    [HideInInspector]
    public int attackDamage = 1;


    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.tag == "Border")
            GameObject.Destroy(gameObject);

        if (collider.tag == "Player")
        {

            collider.GetComponent<Player>().Hit(attackDamage);
            GameObject.Destroy(gameObject);
        }

    }
}
