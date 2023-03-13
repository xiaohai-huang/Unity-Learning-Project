using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
#if UNITY_SERVER
        NetworkManager.Singleton.StartServer();
        print("run code in UNITY_SERVER scripting symbols");
#else
        //NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = "0.0.0.0";
        //NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.ServerListenAddress = "6.55.6.6";
        //NetworkManager.Singleton.StartClient();
        // Set the target frame rate to the max capability of the screen
        Application.targetFrameRate = Mathf.CeilToInt((float)Screen.currentResolution.refreshRateRatio.value);
#endif


        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;

    }
}
