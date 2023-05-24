using Cinemachine;
using Unity.Netcode;

public class LookOwnerOnlyCamera : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        print("is owner: " + IsOwner);
        if (!IsOwner)
        {
            GetComponent<CinemachineVirtualCamera>().enabled = false;
            GetComponent<MyCameraInput>().enabled = false;
            GetComponent<CameraInputProvider>().enabled = false;
            gameObject.SetActive(true);
        }
    }
}
