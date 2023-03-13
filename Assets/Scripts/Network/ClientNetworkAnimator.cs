using Unity.Netcode.Components;
using UnityEngine;

public class ClientNetworkAnimator : NetworkAnimator
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }

    public new void ResetTrigger(int hash)
    {
        base.ResetTrigger(hash);
        InternalSetTrigger(hash, false);
    }

    private void InternalSetTrigger(int hash, bool isSet = true)
    {
        Animator.SetBool(hash, isSet);
    }
}