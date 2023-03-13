
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Multiplayer.Tools.MetricTypes;
using Unity.Netcode;
using Unity.Networking.Transport.Utilities;
using UnityEngine;

public class FPS : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private float time;
    private int frameCount;
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        frameCount++;
        if(time > 1)
        {
            int fps = Mathf.RoundToInt(frameCount / time);
            _text.text = $"{fps}/{Mathf.CeilToInt((float)Screen.currentResolution.refreshRateRatio.value)} FPS";
            frameCount = 0;
            time = 0;
        }
    }
}
