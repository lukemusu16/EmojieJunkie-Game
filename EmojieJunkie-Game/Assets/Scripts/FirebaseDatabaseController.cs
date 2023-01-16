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
        LobbyData lobby = new LobbyData(lobbyCode, player1Name, "", GameState.LOBBY);
        _reference.Child("lobbyData").Child(lobbyCode).SetRawJsonValueAsync(JsonUtility.ToJson(lobby));
        _reference.Child("lobbyData").Child(lobbyCode).ValueChanged += HandleValueChanged;

        TextMeshProUGUI p1TXT = GameObject.Find("Player1TXT").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI p2TXT = GameObject.Find("Player2TXT").GetComponent<TextMeshProUGUI>();

        GlobalValues.Player1 = lobby.player1Name;

        p1TXT.text = GlobalValues.Player1;
        p2TXT.text = "Waiting for player";
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
        if (GlobalValues.State == GameState.LOBBY)
        {
            TextMeshProUGUI p1TXT = GameObject.Find("Player1TXT").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI p2TXT = GameObject.Find("Player2TXT").GetComponent<TextMeshProUGUI>();

            GlobalValues.Player1 = (string)snapshotData["player1Name"];
            GlobalValues.Player2 = (string)snapshotData["player2Name"];
            
            p1TXT.text = GlobalValues.Player1;
            p2TXT.text = GlobalValues.Player2;

            if (GlobalValues.ThisPlayer == GlobalValues.Player1 && GlobalValues.Player2 != string.Empty)
            { 
                startBTN.SetActive(true);
            }
        }

        if (GlobalValues.State == GameState.CONVERT)
        { 
            
        }
        
    }

    public void JoinLobby(String lobbyCode, String player2Name)
    {
        _reference.Child("lobbyData").Child(lobbyCode).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            _reference.Child("lobbyData").Child(lobbyCode).ValueChanged += HandleValueChanged;
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

                p1TXT.text = GlobalValues.Player1;
                p2TXT.text = GlobalValues.Player2;

                GameObject.Find("StartBTN").SetActive(false);
            }
        });
    }

    private void UpdateLobbyData(String lobbyCode, Dictionary<String, object> snapshot)
    {
        _reference.Child("lobbyData").Child(lobbyCode).UpdateChildrenAsync(snapshot);
    }

    public void UpdateGameState(string lobbyCode, string gamestate)
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
}
