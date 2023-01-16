using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum GameState
{ 
    LOBBY,
    CONVERT,
    GUESS
}
public class GameManager : MonoBehaviour
{
    private EmojiKeyboardController ekc;

    private string currentGuesser;
    private string currentConverter;

    public GameObject chosenSen;
    public GameObject hiddenSen;
    public GameObject guessing;
    private GameObject timer;

    private List<string> senteces = new List<string>();

    public TextAsset file;

    private bool timerExpired = false;

    private void Awake()
    {
        chosenSen = GameObject.Find("ChosenSentence");
        hiddenSen = GameObject.Find("HiddenSentence");
        guessing = GameObject.Find("Guessing");
        timer = GameObject.Find("Timer");
    }

    // Start is called before the first frame update
    void Start()
    {
        ekc = GameObject.Find("EmojiKeyboard").GetComponent<EmojiKeyboardController>();
        ReadFile();

        guessing.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => {
            CheckGuess(guessing.transform.GetChild(0).GetComponent<TMP_InputField>().text);
        });

        currentConverter = GlobalValues.Player1;
        currentGuesser   = GlobalValues.Player2;

        GenerateRandomPhrase();
        GlobalValues.State = GameState.CONVERT;
        StartCoroutine(StartTimer(5));
    }

    private void Update()
    {
        if (timerExpired && GlobalValues.State == GameState.CONVERT)
        {
            print("Give Full Points to Guesser");
        }
        else if (timerExpired && GlobalValues.State == GameState.GUESS)
        {
            print("Bozo didnt guess it");
        }
    }

    private string ReadFile()
    {

        var splitFile = new string[] { "\r\n", "\r", "\n" };
        var splitLine = new char[] { ',' };
        var lines = file.text.Split(splitFile, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++)
        {
            //print("Line: " + lines[i]);
            var line = lines[i].Split(splitLine, System.StringSplitOptions.None);
            string final = "";
            foreach (var word in line)
            {
                final += word + " ";
            }
            senteces.Add(final);
        }

        return "";
    }

    private string HideString(string textToHide)
    {
        hiddenSen.GetComponent<TextMeshProUGUI>().text = "";
        string toHideString = textToHide;
        string hiddenString = "";

        foreach (char letter in toHideString)
        {
            if (letter != ' ')
            {
                hiddenString += "_ ";
            }
            else
            {
                hiddenString += "  ";
            }
        }

        hiddenString.Remove(hiddenString.Length-1);
        return hiddenString;
    }

    private void GenerateRandomPhrase()
    {
        chosenSen.GetComponent<TextMeshProUGUI>().text = senteces[Random.Range(0, senteces.Count)];
        hiddenSen.GetComponent<TextMeshProUGUI>().text = HideString(chosenSen.GetComponent<TextMeshProUGUI>().text);
    }

    private void CheckGuess(string guess)
    {
        string answer = chosenSen.GetComponent<TextMeshProUGUI>().text;
        if (guess.ToUpper() == answer.Remove(answer.Length-1))
        {
            print(string.Format("Correct\tGuess: {0} Ans: {1}", guess.ToUpper(), chosenSen.GetComponent<TextMeshProUGUI>().text));
        }
        else
        {
            print(string.Format("Incorrect\tGuess: {0} Ans: {1}", guess.ToUpper(), chosenSen.GetComponent<TextMeshProUGUI>().text));
        }
    }

    public IEnumerator StartTimer(float seconds)
    {
        while (seconds > -1)
        { 
            timer.GetComponent<TextMeshProUGUI>().text = seconds.ToString();
            seconds -= 1;
            yield return new WaitForSeconds(1f);
        }

        timerExpired= true;
        
    }

    public void startTurn(string converter, string guesser)
    { 
        
    }

        

}
