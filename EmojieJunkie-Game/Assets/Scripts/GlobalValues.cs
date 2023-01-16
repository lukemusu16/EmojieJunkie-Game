using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalValues
{
    private static string player1 = "";
    private static string player2 = "";
    private static int state;
    private static string thisPlayer = "";
    private static string lobbyCode = "";


    public static string Player1
    { get { return player1; } set { player1 = value; } }

    public static string Player2
    { get { return player2; } set { player2 = value; } }

    public static int State
    { get { return state; } set { state = value; } }

    public static string ThisPlayer
    { get { return thisPlayer; } set { thisPlayer = value; } }

    public static string LobbyCode
    { get { return lobbyCode; } set { lobbyCode = value; } }


}
