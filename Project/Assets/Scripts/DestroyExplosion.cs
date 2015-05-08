using UnityEngine;
using System.Collections;

public class DestroyExplosion : MonoBehaviour {

 
    public bool meteor = false;
    public GameObject meteorExplosion;

    [HideInInspector]
    public SpecialAttack parent;
    [HideInInspector]
    public bool specialAttackChild;

    // Update is called once per frame
	void Update () {
        if (!particleSystem.IsAlive())
        {
            if (meteor)
            {
                GameObject explosion = (GameObject)Instantiate(meteorExplosion, transform.position, transform.rotation);
                explosion.renderer.sortingLayerName = "Enemy";
            }
            if (specialAttackChild)
                parent.allChildrenFX.Remove(gameObject);

            Destroy(gameObject);

        }
            

        if (meteor)
        {
            Vector3 pos = transform.position;
            pos.x += 1 * Time.deltaTime;
            pos.y -= 1 * Time.deltaTime;
            transform.position = pos;
        }
	}
}
