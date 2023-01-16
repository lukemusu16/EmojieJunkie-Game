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

public class FirebaseDatabaseController : MonoBehaviour
{
    private DatabaseReference _reference;

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
        LobbyData lobby = new LobbyData(lobbyCode, player1Name, "", (int)GameState.LOBBY, "");
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

        if (GlobalValues.State == (int)GameState.CONVERT)
        {

            SceneManager.LoadScene("MainGame");
        }

    }
}
