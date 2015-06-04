using UnityEngine;
using System.Collections;

public class SpecialAttack : MonoBehaviour {

    public int damage = 0;
    public DragonAttack.AttackType attackType;
    public GameObject effect;
    [HideInInspector]
    public float specialAreaMinX;
    [HideInInspector]
    public float specialAreaMaxX;
    [HideInInspector]
    public float specialAreaMinY;
    [HideInInspector]
    public float specialAreaMaxY;
    [HideInInspector]
    public PolygonCollider2D specialAttackAreaCollider;
    [HideInInspector]
    public ArrayList allChildrenFX;
    
    
    private bool attacked = false;
    private bool created = false;
    private GameController gameController;


    public void StartAttack()
    {
        allChildrenFX = new ArrayList();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        for (int i = 0; i < 20; i++)
        {
            float x = Random.Range(specialAreaMinX, specialAreaMaxX);
            float y = Random.Range(specialAreaMinY, specialAreaMaxY);
            if (specialAttackAreaCollider == Physics2D.OverlapPoint(new Vector2(x, y), LayerMask.GetMask("SpecialEffects")))
            {
                GameObject fx = (GameObject)Instantiate(effect, new Vector3(x - 1, y + 2, 0), transform.rotation);
                fx.renderer.sortingLayerName = "Enemy";
                //fx.GetComponent<DestroyExplosion>().specialAttackChild = true;
                //fx.GetComponent<DestroyExplosion>().parent = this;
                allChildrenFX.Add(fx);

            }
        }

        created = true;
    }
    
    void FixedUpdate()
    {
        if ((created) && (allChildrenFX.Count == 0) && (!attacked))
        {
            foreach (GameObject enemy in gameController.getAllEnemiesOnBoard())
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.SpecialHit(damage, attackType);
                    if (attackType == DragonAttack.AttackType.Fire)
                        enemyScript.continuousDamageHit(2, 2.0f, attackType);
                    else
                        enemyScript.slowEnemy(1.0f, 1.0f);
                }
                    

            }
            attacked = true;
            Destroy(gameObject);
        }
    }
}
