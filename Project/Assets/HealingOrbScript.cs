using UnityEngine;
using System.Collections;

public class HealingOrbScript : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Animator anim;
    
    // Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 0);
        anim = GetComponent<Animator>();
	}

    public void ShowOrb()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
        anim.SetTrigger("heal");
        
    }

    public void HideOrb()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
    }
}
