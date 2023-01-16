using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalValues
{
    private static string player1 = "";
    private static string player2 = "";

    public static string Player1
    { get { return player1; } set { player1 = value; } }

    public static string Player2
    { get { return player2; } set { player2 = value; } }
}
