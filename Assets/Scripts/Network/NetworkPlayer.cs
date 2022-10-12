using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Cinemachine;
using TMPro;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    CinemachineFreeLook followCam;
    private GameObject camObj;
    public static NetworkPlayer Local { get; set; }

    public TextMeshProUGUI playerNickNameTM;

    [Networked(OnChanged = nameof(OnNickNameChanged))]
    public NetworkString<_16> nickName { get; set; }

    bool isPublicJoinMessageSent = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            camObj = GameObject.Find("CM ThirdPerson");
            followCam = camObj.GetComponent<CinemachineFreeLook>();
            followCam.Follow = transform.GetChild(0);
            followCam.LookAt = transform.GetChild(0);
            Debug.Log("Spawned local player");
            RPC_SetNickName(PlayerPrefs.GetString("PlayerNickname"));
        }
        else
        {
            //Disable the camera if we are not the local player
            // Camera localCamera = GetComponentInChildren<Camera>();
            // localCamera.enabled = false;


            // CinemachineFreeLook TPCM = GetComponentInChildren<CinemachineFreeLook>();
            // TPCM.enabled = false;

            //Only 1 audio listner is allowed in the scene so disable remote players audio listner
            // AudioListener audioListener = GetComponentInChildren<AudioListener>();
            // audioListener.enabled = false;
            Debug.Log("Spawned remote player");

        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
            Runner.Despawn(Object);
    }

    static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        Debug.Log($"{Time.time} OnHPChanged value {changed.Behaviour.nickName}");

        changed.Behaviour.OnNickNameChanged();
    }

    private void OnNickNameChanged()
    {
        Debug.Log($"Nickname changed for player to {nickName} for player {gameObject.name}");

        playerNickNameTM.text = nickName.ToString();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetNickName(string nickName, RpcInfo info = default)
    {
        Debug.Log($"[RPC] SetNickName {nickName}");
        this.nickName = nickName;

        if (!isPublicJoinMessageSent)
        {
            // networkInGameMessages.SendInGameRPCMessage(nickName, "joined");

            isPublicJoinMessageSent = true;
        }
    }
}
