using Unity.Netcode;

public class LookOwnerOnlyCamera : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) gameObject.SetActive(false);
    }
}
