using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EmojiKeyboardController : MonoBehaviour
{
    private int rows = 30;
    private int cols = 30;
    private int spriteWidth = 20;
    private int spriteHeight = 20;
    public Texture2D img;

    List<GameObject> emojis = new List<GameObject>();

    public void Start()
    {
        GameObject emojiPrefab = Resources.Load<GameObject>("Emoji");
        //Texture2D sourceImage = GetComponent<SpriteRenderer>().sprite.texture;
        int width = img.width;
        int height = img.height;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                int xPos = x * spriteWidth;
                int yPos = y * spriteHeight;
                Sprite emoji = Sprite.Create(img, new Rect(xPos, yPos, spriteWidth, spriteHeight), new Vector2(10f, 10f));
                GameObject newSprite = Instantiate(emojiPrefab, GameObject.Find("Content").transform);
                newSprite.GetComponent<UnityEngine.UI.Image>().sprite = emoji;
                emojis.Add(newSprite);
            }
        }
    }
}
