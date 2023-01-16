using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalValues
{
    private static string player1 = "";
    private static string player2 = "";
    private static string currentGuesser = "";
    private static string currentConverter = "";
    private static int state;
    private static string thisPlayer = "";
    private static string lobbyCode = "";
    private static string secretPhrase = "";
    private static string gameObject0 = "";
    private static string gameObject1 = "";
    private static string gameObject2 = "";
    private static string gameObject3 = "";
    private static string gameObject4 = "";



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

    public static string SecretPhrase
    { get { return secretPhrase; } set { secretPhrase = value; } }

    public static string GameObject0
    { get { return gameObject0; } set { gameObject0 = value; } }
    public static string GameObject1
    { get { return gameObject1; } set { gameObject1 = value; } }
    public static string GameObject2
    { get { return gameObject2; } set { gameObject2 = value; } }
    public static string GameObject3
    { get { return gameObject3; } set { gameObject3 = value; } }
    public static string GameObject4
    { get { return gameObject4; } set { gameObject4 = value; } }

    public static string CurrentGuesser
    { get { return currentGuesser; } set { currentGuesser = value; } }
    public static string CurrentConverter
    { get { return currentConverter; } set { currentConverter = value; } }


}
