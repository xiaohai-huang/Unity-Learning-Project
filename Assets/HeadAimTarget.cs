using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAimTarget : MonoBehaviour
{
    [SerializeField] private CrossHairTarget _crossHairTarget;
    [SerializeField] private PlayerController _player;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        // Disable head aim of the player if the user is looking the back of the player while standing.
        Vector3 viewDir = Vector3.ProjectOnPlane(_crossHairTarget.transform.position, Vector3.up).normalized;
        var degrees = Vector3.Angle(_player.transform.forward, viewDir);
        bool isLookingAtBack = degrees > 120;
        if (isLookingAtBack)
        {
            transform.position = _crossHairTarget.transform.position;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _crossHairTarget.transform.position, 4f * Time.deltaTime);
        }

    }
}
