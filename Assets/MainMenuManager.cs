using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager instance;

    public InputField lobbyInput;

    public InputField lobbyIdText;
    

    private void Awake() => instance = this;

    public void CreateLobby()
    {
        BootstrapManager.CreateLobby(); //로비 생성
    }

    public static void LobbyEntered(string lobbyName, bool isHost)
    {
        instance.lobbyIdText.text = BootstrapManager.CurrentLobbyID.ToString();
    }


    public void JoinLobby()
    {
        CSteamID steamID = new CSteamID(System.Convert.ToUInt64(lobbyInput.text));
        BootstrapManager.JoinByID(steamID);
    }
}
