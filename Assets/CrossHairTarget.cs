using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    private Camera cam;
    Ray ray;

    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = cam.transform.position;
        ray.direction = cam.transform.forward;

        Physics.Raycast(ray, out RaycastHit hitInfo);
        transform.position = hitInfo.point;
    }
}
