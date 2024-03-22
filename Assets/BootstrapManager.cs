using FishNet.Managing;
using FishNet.Managing.Scened;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BootstrapManager : MonoBehaviour
{
    private static BootstrapManager instance;

    private void Awake() => instance = this;


    [SerializeField] private string menuName = "MenuSceneSteam";
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private FishySteamworks.FishySteamworks fishySteamworks;


    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;

    public static ulong CurrentLobbyID;


    private void Start()
    {
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(menuName, LoadSceneMode.Additive);


    }

    public static void CreateLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
    }


    private void OnLobbyCreated(LobbyCreated_t callback) //�κ� ���� ��
    {
        //Debug.Log("�κ� ���� ����" + callback.m_eResult.ToString());

        if (callback.m_eResult != EResult.k_EResultOK) return;

        CurrentLobbyID = callback.m_ulSteamIDLobby; // �κ� ���� �� ���� �κ� ���̵� ��������
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID), "HostAddress", SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID), "name", SteamFriends.GetPersonaName().ToString() + "'s lobby");
        fishySteamworks.SetClientAddress(SteamUser.GetSteamID().ToString());
        fishySteamworks.StartConnection(true);

        Debug.Log("�κ� ������ ���������� �Ϸ��");

    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback) //���� ��û
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback) //�κ� ����
    {
        CurrentLobbyID = callback.m_ulSteamIDLobby;

        MainMenuManager.LobbyEntered(SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "name"), networkManager.IsServer);

        fishySteamworks.SetClientAddress(SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "HostAddress"));
        fishySteamworks.StartConnection(false);
    }

    public static void JoinByID(CSteamID steamID)
    {
        Debug.Log("���� ID�� ������ �õ��մϴ�. " + steamID.m_SteamID);
        if(SteamMatchmaking.RequestLobbyData(steamID))
        {
            SteamMatchmaking.JoinLobby(steamID);
        }
        else
        {
            Debug.Log("���ӿ� ������ " + steamID.m_SteamID);
        }

    }
}
