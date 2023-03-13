using UnityEngine;


public class MobileOnly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(UnityEngine.Device.Application.isMobilePlatform);
    }
}
