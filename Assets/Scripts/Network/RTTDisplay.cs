using TMPro;
using Unity.Netcode;
using UnityEngine;

public class RTTDisplay : NetworkBehaviour
{
    private float m_RTT;
    private TextMeshProUGUI text;
    private Vector3 position;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        position = text.rectTransform.position;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsClient)
        {
            InvokeRepeating(nameof(SendTestMessage), 0f, 0.5f);
            text.rectTransform.position = position;
        }
    }

    public override void OnNetworkDespawn()
    {
        CancelInvoke(nameof(SendTestMessage));
        base.OnNetworkDespawn();
    }

    private void SendTestMessage()
    {
        ReceiveTestMessageServerRpc(Time.time);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ReceiveTestMessageServerRpc(float sentTime, ServerRpcParams serverParams = default)
    {
        //print($"sender id: {serverParams.Receive.SenderClientId}, sentTime: {sentTime}");
        ReceiveEchoMessageClientRpc(sentTime, new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { serverParams.Receive.SenderClientId }
            }
        });
    }

    [ClientRpc]
    private void ReceiveEchoMessageClientRpc(float sentTime, ClientRpcParams clientParams = default)
    {
        m_RTT = Time.time - sentTime;
        text.text = $"ping: {(m_RTT * 1000f):0.00} ms";
    }
}