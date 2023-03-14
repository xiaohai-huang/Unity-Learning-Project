using System;
using Unity.Netcode;
using UnityEngine;

public class BasicRigidBodyPush : NetworkBehaviour
{
    public LayerMask pushLayers;
    public bool canPush;
    [Range(0.5f, 5f)] public float strength = 1.1f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (canPush) {
            // make sure we only push desired layer(s)
            if(hit.collider.attachedRigidbody)
            {
                var bodyLayerMask = 1 << hit.collider.attachedRigidbody.gameObject.layer;
                if ((bodyLayerMask & pushLayers.value) == 0) return;
                PushItemServerRpc(hit.gameObject.GetComponent<NetworkObject>(), hit.point - transform.position);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PushItemServerRpc(NetworkObjectReference objToPush, Vector3 pushDirection)
    {
        var obj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[objToPush.NetworkObjectId].gameObject;
        // https://docs.unity3d.com/ScriptReference/CharacterController.OnControllerColliderHit.html
        // make sure we hit a non kinematic rigidbody
        Rigidbody body = obj.GetComponent<Rigidbody>();
        if (body == null || body.isKinematic) return;

        // make sure we only push desired layer(s)
        var bodyLayerMask = 1 << body.gameObject.layer;
        if ((bodyLayerMask & pushLayers.value) == 0) return;

        pushDirection.y = 0;
        body.AddForce(pushDirection.normalized * strength, ForceMode.Impulse);
    }
 
}