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
    }

    public void CreateLobby(String lobbyCode, String player1Name)
    {
        LobbyData lobby = new LobbyData(lobbyCode, player1Name, "", "InLobby");
        _reference.Child("lobbyData").Child(lobbyCode).SetRawJsonValueAsync(JsonUtility.ToJson(lobby));

        TextMeshProUGUI p1TXT = GameObject.Find("Player1TXT").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI p2TXT = GameObject.Find("Player2TXT").GetComponent<TextMeshProUGUI>();

        GlobalValues.Player1 = lobby.player1Name;

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
                RenamePlayer2(lobbyCode, snapshotData);

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

    public bool CheckForPlayer(String lobbyCode)
    {
        _reference.Child("lobbyData").Child(lobbyCode).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            DataSnapshot snapshot = task.Result;
            Dictionary<String, object> snapshotData = new Dictionary<string, object>();
            snapshotData = (Dictionary<String, object>)snapshot.Value;

            if (!snapshotData["player2Name"].Equals(""))
            {
                TextMeshProUGUI p2TXT = GameObject.Find("Player2TXT").GetComponent<TextMeshProUGUI>();
                GlobalValues.Player2 = (string)snapshotData["player2Name"];
                p2TXT.text = GlobalValues.Player2;
                return true;
            };

            return false;
        });

        return false;
    }

    private void RenamePlayer2(String lobbyCode, Dictionary<String, object> snapshot)
    {
        _reference.Child("lobbyData").Child(lobbyCode).UpdateChildrenAsync(snapshot);
    }
}
