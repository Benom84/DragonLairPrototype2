using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicButtonHandler : MonoBehaviour {

    public Sprite OnSprite;
    public Sprite OnSpritePressed;
    public Sprite OffSprite;
    public Sprite OffSpritePressed;
    public bool music;

    private Sprite currentSprite;
    private Sprite currentSpritePressed;
    private Image image;
    private Button button;

    void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        if (music)
        {
            SetImage(DataController.dataController.isMusicOn);
           
        }

        else
        {
            SetImage(DataController.dataController.isSoundEffectsOn);
           
        }
    }

    public void Press()
    {
        if (music)
        {
            SetImage(!DataController.dataController.isMusicOn);
            DataController.dataController.isMusicOn = !DataController.dataController.isMusicOn;
           
        }

        else
        {
            SetImage(!DataController.dataController.isSoundEffectsOn);
            DataController.dataController.isSoundEffectsOn = !DataController.dataController.isSoundEffectsOn;
            
        }

        DataController.dataController.Save();
    }

    public void SetImage(bool on)
    {
        if (on)
        {
            image.sprite = OnSprite;
            SpriteState st = button.spriteState;
            st.highlightedSprite = OnSprite;
            st.pressedSprite = OnSpritePressed;
            button.spriteState = st;
        }
        else
        {
            image.sprite = OffSprite;
            SpriteState st = button.spriteState;
            st.highlightedSprite = OffSprite;
            st.pressedSprite = OffSpritePressed;
            button.spriteState = st;
        }
    }
	
}
