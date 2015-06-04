using UnityEngine;
using System.Collections;

public class MeteorAttack : MonoBehaviour {

    public float lifeTime = 2.0f;
    public float changeAngleFactor = 1f;
    public GameObject explosion;

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
    
    private float startTime;
    private bool attacked = false;
    private bool created = false;
    private GameController gameController;
    
    // Use this for initialization
	void Start () {

        startTime = Time.time;
	
	}
    
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
                fx.GetComponent<DestroyExplosion>().specialAttackChild = true;
                fx.GetComponent<DestroyExplosion>().meteorParent = this;
                allChildrenFX.Add(fx);

            }
        }

        created = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (Time.time > startTime + lifeTime)
        {
            explode();
        }

        float rand = Mathf.PerlinNoise(transform.position.x * changeAngleFactor, transform.position.y * changeAngleFactor);
        float rand_angle = Mathf.Lerp(rand, -30, 30);
        transform.rotation = transform.rotation * Quaternion.AngleAxis(rand_angle, transform.right);
 
	
	}

    private void explode()
    {
        GameObject explosionEffect = (GameObject)Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, -1), transform.rotation);
        GameObject.Destroy(gameObject);
    }
}
