using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using System.ComponentModel;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.UI;

public class FirebaseDatabaseController : MonoBehaviour
{
    private DatabaseReference _reference;
    private GameManager gm;

    public GameObject startBTN;

    //Singleton
    public static FirebaseDatabaseController Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
        _reference = FirebaseDatabase.DefaultInstance.RootReference;
        startBTN = GameObject.Find("StartBTN");
        
    }

    public void CreateLobby(String lobbyCode, String player1Name)
    {
        LobbyData lobby = new LobbyData(lobbyCode, player1Name, "", (int)GameState.LOBBY, "", GlobalValues.GameObject0, GlobalValues.GameObject1, GlobalValues.GameObject2, GlobalValues.GameObject3, GlobalValues.GameObject4);
        _reference.Child("lobbyData").Child(lobbyCode).SetRawJsonValueAsync(JsonUtility.ToJson(lobby));
        _reference.Child("lobbyData").Child(lobbyCode).ValueChanged += HandleValueChanged;

        TextMeshProUGUI p1TXT = GameObject.Find("Player1TXT").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI p2TXT = GameObject.Find("Player2TXT").GetComponent<TextMeshProUGUI>();

        GlobalValues.Player1 = lobby.player1Name;
        GlobalValues.LobbyCode = lobby.lobbyCode;

        p1TXT.text = GlobalValues.Player1;
        p2TXT.text = "Waiting for player";
    }

    

    public void JoinLobby(String lobbyCode, String player2Name)
    {
        _reference.Child("lobbyData").Child(lobbyCode).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            
            if (task.IsFaulted)
            {
                Debug.LogError("Unable to load data from Firebast RTDB");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Dictionary<String, object> snapshotData = new Dictionary<string, object>();
                snapshotData = (Dictionary<String, object>)snapshot.Value;

                //Rename Player2 name and therefore join the lobby
                snapshotData["player2Name"] = player2Name;
                UpdateLobbyData(lobbyCode, snapshotData);

                TextMeshProUGUI p1TXT = GameObject.Find("Player1TXT").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI p2TXT = GameObject.Find("Player2TXT").GetComponent<TextMeshProUGUI>();

                GlobalValues.Player1 = (string)snapshotData["player1Name"];
                GlobalValues.Player2 = (string)snapshotData["player2Name"];
                GlobalValues.LobbyCode = lobbyCode;


                p1TXT.text = GlobalValues.Player1;
                p2TXT.text = GlobalValues.Player2;

                GameObject.Find("StartBTN").SetActive(false);
            }
        });
        _reference.Child("lobbyData").Child(lobbyCode).ValueChanged += HandleValueChanged;
    }

    private void UpdateLobbyData(String lobbyCode, Dictionary<String, object> snapshot)
    {
        _reference.Child("lobbyData").Child(lobbyCode).UpdateChildrenAsync(snapshot);
    }

    public void UpdateGameState(string lobbyCode, int gamestate)
    {
        _reference.Child("lobbyData").Child(lobbyCode).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            DataSnapshot snapshot = task.Result;
            Dictionary<String, object> snapshotData = new Dictionary<string, object>();
            snapshotData = (Dictionary<String, object>)snapshot.Value;

            snapshotData["gameState"] = gamestate;
            UpdateLobbyData(lobbyCode, snapshotData);
        });
    }

    public void UpdatePhrase(string lobbyCode, string secretPhrase)
    {
        _reference.Child("lobbyData").Child(lobbyCode).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            DataSnapshot snapshot = task.Result;
            Dictionary<String, object> snapshotData = new Dictionary<string, object>();
            snapshotData = (Dictionary<String, object>)snapshot.Value;

            snapshotData["secretPhrase"] = secretPhrase;
            UpdateLobbyData(lobbyCode, snapshotData);
        });
    }

    public void UpdateGameObjects(string lobbyCode, string go0, string go1, string go2, string go3, string go4)
    {
        _reference.Child("lobbyData").Child(lobbyCode).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            DataSnapshot snapshot = task.Result;
            Dictionary<String, object> snapshotData = new Dictionary<string, object>();
            snapshotData = (Dictionary<String, object>)snapshot.Value;

            snapshotData["gameObjects0"] = go0;
            snapshotData["gameObjects1"] = go1;
            snapshotData["gameObjects2"] = go2;
            snapshotData["gameObjects3"] = go3;
            snapshotData["gameObjects4"] = go4;

            UpdateLobbyData(lobbyCode, snapshotData);
        });
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        Dictionary<String, object> snapshotData = new Dictionary<string, object>();
        snapshotData = (Dictionary<String, object>)args.Snapshot.Value;

        GlobalValues.Player1 = (string)snapshotData["player1Name"];
        GlobalValues.Player2 = (string)snapshotData["player2Name"];
        GlobalValues.State = Convert.ToInt16(snapshotData["gameState"]);
        GlobalValues.SecretPhrase = (string)snapshotData["secretPhrase"];

        GlobalValues.GameObject0 = (string)snapshotData["gameObjects0"];
        GlobalValues.GameObject1 = (string)snapshotData["gameObjects1"];
        GlobalValues.GameObject2 = (string)snapshotData["gameObjects2"];
        GlobalValues.GameObject3 = (string)snapshotData["gameObjects3"];
        GlobalValues.GameObject4 = (string)snapshotData["gameObjects4"];



        if (GlobalValues.State == (int)GameState.LOBBY)
        {
            TextMeshProUGUI p1TXT = GameObject.Find("Player1TXT").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI p2TXT = GameObject.Find("Player2TXT").GetComponent<TextMeshProUGUI>();

            p1TXT.text = GlobalValues.Player1;
            p2TXT.text = GlobalValues.Player2;

            if (GlobalValues.ThisPlayer == GlobalValues.Player1 && GlobalValues.Player2 != string.Empty)
            {
                startBTN.SetActive(true);
            }
        }

        if (GlobalValues.State == (int)GameState.CONVERT && SceneManager.GetActiveScene().name == "MainGame")
        {

           
        }
        else if(GlobalValues.State == (int)GameState.CONVERT && SceneManager.GetActiveScene().name != "MainGame")
        {
            print("bruh");
            SceneManager.LoadScene("MainGame");
        }

        if (GlobalValues.State == (int)GameState.GUESS)
        {
            if (GlobalValues.ThisPlayer == GlobalValues.CurrentGuesser)
            {
                print("guesser");
                gm = GameObject.Find("GameManager").GetComponent<GameManager>();
                gm.hiddenSen.GetComponent<TextMeshProUGUI>().text = gm.HideString(GlobalValues.SecretPhrase);
                gm.hiddenSen.SetActive(true);
                gm.guessing.SetActive(true);
                gm.waitingTXT.SetActive(false);

                GameObject emojiPrefab = Resources.Load<GameObject>("Emoji");
                EmojiKeyboardController ekc = GameObject.Find("EmojiKeyboard").GetComponent<EmojiKeyboardController>();
                if ((string)snapshotData["gameObjects0"] != string.Empty)
                {
                    ekc.keyboard.SetActive(true);
                    Sprite img = GameObject.Find(GlobalValues.GameObject0).GetComponent<Image>().sprite;
                    GameObject newImg = Instantiate(emojiPrefab, GameObject.Find("EmojiContainer").transform);
                    newImg.GetComponent<Image>().sprite = img;
                    ekc.keyboard.SetActive(false);
                }
                /*if (GlobalValues.GameObject1 != string.Empty)
                    GameObject.Find(GlobalValues.GameObject1).GetComponent<Image>().sprite;
                if (GlobalValues.GameObject2 != string.Empty)
                    GameObject.Find(GlobalValues.GameObject2).GetComponent<Image>().sprite;
                if (GlobalValues.GameObject3 != string.Empty)
                    GameObject.Find(GlobalValues.GameObject3).GetComponent<Image>().sprite;
                if (GlobalValues.GameObject4 != string.Empty)
                    GameObject.Find(GlobalValues.GameObject4).GetComponent<Image>().sprite;*/
            }
            else
            {
                print("converter");
                gm = GameObject.Find("GameManager").GetComponent<GameManager>();
                gm.chosenSen.SetActive(true);
                gm.waitingTXT.SetActive(true);
                gm.hiddenSen.SetActive(false);
                gm.guessing.SetActive(false);
            }
        }

    }
}
