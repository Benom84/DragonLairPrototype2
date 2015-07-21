using UnityEngine;
using System.Collections;

public class DestroyExplosion : MonoBehaviour {

 
    public bool meteor = false;
    public GameObject meteorExplosion;

    [HideInInspector]
    public SpecialAttack parent;
    [HideInInspector]
    public MeteorAttack meteorParent;
    [HideInInspector]
    public bool specialAttackChild;

    void Awake()
    {
        if (!GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().isSoundEffectsOn)
        {
            GetComponent<AudioSource>().volume = 0;
        }
    }


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
            {
                if (parent != null)
                {
                    parent.allChildrenFX.Remove(gameObject);
                }
                         }
                

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
