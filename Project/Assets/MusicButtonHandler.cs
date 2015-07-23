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

    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        if (music)
        {
            if (DataController.dataController.isMusicOn)
            {
                image.sprite = OnSprite;
                SpriteState st = button.spriteState;
                st.highlightedSprite = OnSprite;
                st.highlightedSprite = OnSpritePressed;
                button.spriteState = st;

            }
            else
            {
                image.sprite = OffSprite;
                SpriteState st = button.spriteState;
                st.highlightedSprite = OffSprite;
                st.highlightedSprite = OffSpritePressed;
                button.spriteState = st;
            }
        }

        else
        {
            if (DataController.dataController.isSoundEffectsOn)
            {
                image.sprite = OnSprite;
                SpriteState st = button.spriteState;
                st.highlightedSprite = OnSprite;
                st.highlightedSprite = OnSpritePressed;
                button.spriteState = st;

            }
            else
            {
                image.sprite = OffSprite;
                SpriteState st = button.spriteState;
                st.highlightedSprite = OffSprite;
                st.highlightedSprite = OffSpritePressed;
                button.spriteState = st;
            }
        }
    }

    public void Press()
    {
        if (music)
        {
            if (DataController.dataController.isMusicOn)
            {
                image.sprite = OffSprite;
                SpriteState st = button.spriteState;
                st.highlightedSprite = OffSprite;
                st.highlightedSprite = OffSpritePressed;
                button.spriteState = st;
                DataController.dataController.isMusicOn = false;

            }
            else
            {
                image.sprite = OnSprite;
                SpriteState st = button.spriteState;
                st.highlightedSprite = OnSprite;
                st.highlightedSprite = OnSpritePressed;
                button.spriteState = st;
                DataController.dataController.isMusicOn = true;
            }
        }

        else
        {
            if (DataController.dataController.isSoundEffectsOn)
            {
                image.sprite = OffSprite;
                SpriteState st = button.spriteState;
                st.highlightedSprite = OffSprite;
                st.highlightedSprite = OffSpritePressed;
                button.spriteState = st;
                DataController.dataController.isSoundEffectsOn = false;

            }
            else
            {
                image.sprite = OnSprite;
                SpriteState st = button.spriteState;
                st.highlightedSprite = OnSprite;
                st.highlightedSprite = OnSpritePressed;
                button.spriteState = st;
                DataController.dataController.isSoundEffectsOn = true;
            }
        }
    }
	
}
