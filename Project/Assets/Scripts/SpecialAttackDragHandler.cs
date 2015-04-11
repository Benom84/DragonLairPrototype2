using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SpecialAttackDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {


    public GameObject damageArea;
    
    private GameController gameController;
    private Player player;
    private GameObject draggedDamageArea;
    private GameObject specialAttackPosition;
    private bool specialAttackBegan = false;
    
    
    
    // Use this for initialization
	void Start () {

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        specialAttackPosition = GameObject.FindGameObjectWithTag("SpecialAttack");
        
	}
	

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (player.getCurrentMana() >= player.specialAttackManaCost)
        {
            draggedDamageArea = Instantiate(damageArea, specialAttackPosition.transform.position, specialAttackPosition.transform.rotation) as GameObject;
            specialAttackBegan = true;
        }
        
        

    }

    public void OnDrag(PointerEventData eventData)
    {

        if (specialAttackBegan)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
            pos.z = 0;
            draggedDamageArea.transform.position = pos;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (specialAttackBegan)
        {
            SpecialAttack specialAttack = draggedDamageArea.GetComponent<SpecialAttack>();
            player.SpecialAttack(specialAttack.enemiesInDamageArea);
            Destroy(draggedDamageArea);
            specialAttackBegan = false;
        }
        
    
    }
}
