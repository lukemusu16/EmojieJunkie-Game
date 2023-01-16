using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmojiKeyboardController : MonoBehaviour
{
    private GameManager gm;


    private int rows = 30;
    private int cols = 30;
    private int spriteWidth = 20;
    private int spriteHeight = 20;

    private GameObject backBTN;
    public GameObject confirmBTN;

    public GameObject keyboard;

    public bool submitted;

    public Texture2D img;

    public List<GameObject> emojis = new List<GameObject>();

    private void Awake()
    {
        keyboard = GameObject.Find("EmojiKeyboard");
        backBTN = GameObject.Find("BackspaceBTN");
        confirmBTN = GameObject.Find("ConfirmBTN");
    }

    public void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        backBTN.GetComponent<Button>().onClick.AddListener(() => {
            RemoveEmoji();
        });

        backBTN.SetActive(false);

        confirmBTN.GetComponent<Button>().onClick.AddListener(() =>
        {
            gm.StopAllCoroutines();
            keyboard.SetActive(false);
            submitted = true;
            GlobalValues.State = GameState.GUESS;
            StartCoroutine(gm.StartTimer(5));

        });
        confirmBTN.SetActive(false);
 
        gm.hiddenSen.SetActive(false);
        gm.guessing.SetActive(false);


        GameObject emojiPrefab = Resources.Load<GameObject>("Emoji");
        
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

                newSprite.GetComponent<Button>().onClick.AddListener(() => {
                    if (emojis.Count < 5)
                    {
                        GameObject selectedEmoji = Instantiate(emojiPrefab, GameObject.Find("EmojiContainer").transform);
                        selectedEmoji.GetComponent<UnityEngine.UI.Image>().sprite = emoji;
                        emojis.Add(selectedEmoji);
                    }
                });  
            }
        }
    }

    private void Update() 
    {
        if (emojis.Count > 0 && !submitted)
        {
            backBTN.SetActive(true);
            confirmBTN.SetActive(true);
        }
        else
        { 
            backBTN.SetActive(false);
            confirmBTN.SetActive(false);

            if (submitted)
            { 
                gm.chosenSen.SetActive(false);
                gm.hiddenSen.SetActive(true);
                gm.guessing.SetActive(true);
                
            }
        }
    }

    private void RemoveEmoji()
    {
        Destroy(emojis[emojis.Count - 1]);
        emojis.RemoveAt(emojis.Count - 1);
    }
}
