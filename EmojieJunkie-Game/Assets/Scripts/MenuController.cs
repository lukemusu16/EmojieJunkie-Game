using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    GameObject playerNameModal;
    GameObject joinLobbyModal;
    GameObject lobby;

    // Start is called before the first frame update
    void Awake()
    {
        playerNameModal = GameObject.Find("PlayerNameModal");    
        joinLobbyModal = GameObject.Find("JoinLobbyModal");    
        lobby = GameObject.Find("Lobby");
    }

    public void GoToPlayerNameModal()
    {
        playerNameModal.SetActive(true);
        joinLobbyModal.SetActive(false);
        lobby.SetActive(false);
    }

    public void GoToLobbyJoinOptions()
    { 
        playerNameModal.SetActive(false);
        joinLobbyModal.SetActive(true);
        lobby.SetActive(false);
    }

    public void GoToLobby()
    {
        playerNameModal.SetActive(false);
        joinLobbyModal.SetActive(false);
        lobby.SetActive(true);
    }
}
