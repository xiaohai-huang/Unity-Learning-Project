using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GunsMenu : MonoBehaviour
{
    public GameObject Buttons;
    public GameObject[] Guns;
    int currentGun = 0;
    void Start()
    {
        Guns[0].SetActive(true);
    }

    public void NextGun()
    {
        Guns[currentGun ].SetActive(false);
        currentGun++;
        if (currentGun >= Guns.Length)
            currentGun = 0;
        Guns[currentGun].SetActive(true);
    }
    public void PreviousGun()
    {
        Guns[currentGun].SetActive(false);
        currentGun--;
        if (currentGun < 0)
            currentGun = Guns.Length - 1;
        Guns[currentGun].SetActive(true);
    }
    private void Update()
    {
     
    }
}
