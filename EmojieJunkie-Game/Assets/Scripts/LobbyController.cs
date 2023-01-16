using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LobbyController : MonoBehaviour
{
    private TMP_InputField lobbyCodeINP;
    private TMP_InputField playerNameINP;

    private GameObject startBTN;

    private string lobbyText;

    private bool playerJoined = false;
    private bool checking = false;

    MenuController mc;

    private List<string> _uniqueCodes;
    void Awake()
    {
        //Load the unique codes
        StartCoroutine(LoadUniqueCodes(Application.dataPath + "/Scripts/uniqueCodes.xml"));
    }
    void Start()
    {
        playerNameINP = GameObject.Find("PlayerNameINP").GetComponent<TMP_InputField>();
        lobbyCodeINP = GameObject.Find("LobbyCodeINP").GetComponent<TMP_InputField>();

        startBTN = GameObject.Find("StartBTN");
        startBTN.GetComponent<Button>().onClick.AddListener(() => {
            SceneManager.LoadScene("GameScene");
        });
        startBTN.SetActive(false);

        mc = GameObject.Find("MenuController").GetComponent<MenuController>();
        mc.GoToPlayerNameModal();
    }

    private void Update()
    {
        if (checking)
        {
            StartCoroutine(checkForPlayer(lobbyText));
        }
    }

    public void TransitionToPanel(int id)
    {
        switch (id)
        {
            //Create Create Panel
            case 1:
                if (playerNameINP.text.Length > 2)
                {
                    //Load a random unique code from the list
                    lobbyCodeINP.text = _uniqueCodes[Random.Range(0, _uniqueCodes.Count)];
                    playerNameINP.text = playerNameINP.text;

                    lobbyText = lobbyCodeINP.text;

                    mc.GoToLobby();

                    //Saving the lobby to firebase RTDB
                    FirebaseDatabaseController.Instance.CreateLobby(lobbyCodeINP.text, playerNameINP.text);

                    checking = true;
                    
                }
                break;
            //Join Lobby Panel
            case 2:
                if (playerNameINP.text.Length > 2)
                {
                    mc.GoToLobbyJoinOptions();
                }
                break;
            case 3:
                if (lobbyCodeINP.text.Length > 2)
                { 
                    mc.GoToLobby();
                    FirebaseDatabaseController.Instance.JoinLobby(lobbyCodeINP.text, playerNameINP.text);
                }
                break;
            //Welcome Panel
            default:
                mc.GoToPlayerNameModal();
                break;
        }
    }

    public void JoinLobbyWithCode()
    {
        FirebaseDatabaseController.Instance.JoinLobby(lobbyCodeINP.text, playerNameINP.text);
    }


    IEnumerator LoadUniqueCodes(String path)
    {
        XDocument doc = new XDocument(XDocument.Load(path));
        _uniqueCodes = new List<String>();
        foreach (XElement xElement in doc.Root.Elements())
        {
            _uniqueCodes.Add(xElement?.Value);
        }

        yield return null;
    }

    IEnumerator checkForPlayer(String lobbyCode)
    {
        if (FirebaseDatabaseController.Instance.CheckForPlayer(lobbyCode))
        {
            playerJoined = true;
            checking = false;
            startBTN.SetActive(true);
            
        }
        yield return new WaitForSeconds(2);

        if(!playerJoined)
            checkForPlayer(lobbyCode);
    }
}
