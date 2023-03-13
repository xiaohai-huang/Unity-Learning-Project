using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{
    void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() => 
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
