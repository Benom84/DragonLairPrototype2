using UnityEngine;
using System.Collections;

public class DragonAttack : MonoBehaviour {

    public enum AttackType { Fire, Water, Air, Earthquake, Scream };
    public GameObject explosion;

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
            Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, -1), transform.rotation);
            GameObject.Destroy(gameObject);
        }

    }
}
