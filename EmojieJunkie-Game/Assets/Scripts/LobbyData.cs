using System;

[Serializable]
public class LobbyData
{
    public String lobbyCode;
    public String player1Name;
    public String player2Name;
    public String gameState;

    public LobbyData(String LobbyCode, String Player1Name, String Player2Name, String GameState)
    {
        lobbyCode = LobbyCode;
        player1Name = Player1Name;
        player2Name = Player2Name;
        gameState = GameState;
    }

}