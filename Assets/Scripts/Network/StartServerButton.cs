using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class StartServerButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => 
        {
            NetworkManager.Singleton.StartServer();
        });
    }

 
}
