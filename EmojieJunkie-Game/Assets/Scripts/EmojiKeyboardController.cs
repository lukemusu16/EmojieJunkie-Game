using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiKeyboardController : MonoBehaviour
{
    public int rows = 24;
    public int cols = 21;
    public int spriteWidth = 33;
    public int spriteHeight = 33;
    public Texture2D img;

    public void Start()
    {
        //Texture2D sourceImage = GetComponent<SpriteRenderer>().sprite.texture;
        int width = img.width;
        int height = img.height;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                int xPos = x * spriteWidth;
                int yPos = y * spriteHeight;
                Sprite emoji = Sprite.Create(img, new Rect(xPos, yPos, spriteWidth, spriteHeight), new Vector2(0.5f, 0.5f));
                GameObject newSprite = new GameObject();
                newSprite.AddComponent<SpriteRenderer>().sprite = emoji;
                newSprite.transform.parent = GameObject.Find("EmojiKeyboard").transform.GetChild(0).GetChild(0).transform;
            }
        }
    }
}
