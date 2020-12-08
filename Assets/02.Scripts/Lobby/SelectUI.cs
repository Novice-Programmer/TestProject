using UnityEngine;

public abstract class SelectUI : MonoBehaviour
{
    LobbyPlayer _lobbyPlayer;
    public virtual void Open(LobbyPlayer lobbyPlayer)
    {
        _lobbyPlayer = lobbyPlayer;
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        _lobbyPlayer.SelectEnd();
        _lobbyPlayer = null;
    }
}
