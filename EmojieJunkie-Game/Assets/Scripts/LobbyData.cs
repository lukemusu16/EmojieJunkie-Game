using System;

[Serializable]
public class LobbyData
{
    public String lobbyCode;
    public String player1Name;
    public String player2Name;
    public String secretPhrase;
    public int gameState;

    public LobbyData(String LobbyCode, String Player1Name, String Player2Name, int GameState, String SecretPhrase)
    {
        lobbyCode = LobbyCode;
        player1Name = Player1Name;
        player2Name = Player2Name;
        gameState = GameState;
        secretPhrase = SecretPhrase;
    }

}