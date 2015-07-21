using UnityEngine;
using System.Collections;

public class DragonAttack : MonoBehaviour {

    public enum AttackType { Fire, Water, Air, Earthquake, Scream };
    public GameObject explosion;

    public AttackType attackType;
    [HideInInspector]
    public int attackDamage = 1;
    public int continuousDamage = 0;
    public float continuosDamageTime = 0;
    public float slowFactor = 0;
    public float slowTime = 0;


    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "Border")
            GameObject.Destroy(gameObject);

        if (collider.gameObject.tag == "Enemy")
        {

            GameObject explosionEffect = (GameObject)Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, -1), transform.rotation);
            explosionEffect.renderer.sortingLayerName = "Enemy";
            explosionEffect.renderer.sortingOrder = collider.gameObject.renderer.sortingOrder;

            Enemy enemyScript = collider.gameObject.GetComponent<Enemy>();
            if (enemyScript != null) {
                enemyScript.Hit(attackDamage, attackType, false);
                enemyScript.slowEnemy(slowFactor, slowTime);
                enemyScript.continuousDamageHit(continuousDamage, continuosDamageTime, attackType);
            }
            
            GameObject.Destroy(gameObject);
        }

    }
}
