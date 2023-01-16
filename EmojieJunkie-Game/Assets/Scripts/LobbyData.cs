using System;

[Serializable]
public class LobbyData
{
    public String lobbyCode;
    public String player1Name;
    public String player2Name;
    public String secretPhrase;
    public int gameState;
    public String gameObjects0;
    public String gameObjects1;
    public String gameObjects2;
    public String gameObjects3;
    public String gameObjects4;

    public LobbyData(String LobbyCode, String Player1Name, String Player2Name, int GameState, String SecretPhrase, String GameObjects0, String GameObjects1, String GameObjects2, String GameObjects3, String GameObjects4)
    {
        lobbyCode = LobbyCode;
        player1Name = Player1Name;
        player2Name = Player2Name;
        gameState = GameState;
        secretPhrase = SecretPhrase;
        gameObjects0 = GameObjects0;
        gameObjects1 = GameObjects1;
        gameObjects2 = GameObjects2;
        gameObjects3 = GameObjects3;
        gameObjects4 = GameObjects4;
    }

}