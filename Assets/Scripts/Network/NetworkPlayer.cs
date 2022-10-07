using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Cinemachine;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
          
            Camera.main.gameObject.SetActive(false); //Add MainCamera tag to the main camera

            CinemachineFreeLook localCamera =  GetComponentInChildren<CinemachineFreeLook>();
            localCamera.Priority = 200;

            Debug.Log("Spawned local player");
        }else{
            //Disable the camera if we are not the local player
            Camera localCamera =  GetComponentInChildren<Camera>();
            localCamera.enabled = false;


            // CinemachineFreeLook TPCM = GetComponentInChildren<CinemachineFreeLook>();
            // TPCM.enabled = false;

            //Only 1 audio listner is allowed in the scene so disable remote players audio listner
            AudioListener audioListener = GetComponentInChildren<AudioListener>();
            audioListener.enabled = false;
            Debug.Log("Spawned remote player");

        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
            Runner.Despawn(Object);
    }
}
