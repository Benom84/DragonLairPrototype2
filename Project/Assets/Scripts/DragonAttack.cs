using UnityEngine;
using System.Collections;

public class DragonAttack : MonoBehaviour {

    public enum AttackType { Fire, Water, Air, Earthquake, Scream };

    public AttackType attackType;
    [HideInInspector]
    public int attackDamage = 1;



    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "Border")
            GameObject.Destroy(gameObject);

        if (collider.gameObject.tag == "Enemy")
        {

            collider.gameObject.GetComponent<Enemy>().Hit(attackDamage, attackType);
            GameObject.Destroy(gameObject);
        }

    }
}
