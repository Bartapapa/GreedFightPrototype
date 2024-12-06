using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineIcon : MonoBehaviour
{
    [Header("OBJECT REFS")]
    public GameObject RevFlame;
    public GameObject firstGear;
    public GameObject secondGear;
    public GameObject thirdGear;
    public GameObject fourthGear;
    public GameObject fifthGear;

    private void Update()
    {
        HandleShowRevFlame();
        HandleGearText();
    }

    private void HandleGearText()
    {
        int currentGear = Player.instance.CurrentGear;
        if (currentGear == 1)
        {
            firstGear.SetActive(true);
            secondGear.SetActive(false);
            thirdGear.SetActive(false);
            fourthGear.SetActive(false);
            fifthGear.SetActive(false);
        }
        else if (currentGear == 2)
        {
            firstGear.SetActive(false);
            secondGear.SetActive(true);
            thirdGear.SetActive(false);
            fourthGear.SetActive(false);
            fifthGear.SetActive(false);
        }
        else if (currentGear == 3)
        {
            firstGear.SetActive(false);
            secondGear.SetActive(false);
            thirdGear.SetActive(true);
            fourthGear.SetActive(false);
            fifthGear.SetActive(false);
        }
        else if (currentGear == 4)
        {
            firstGear.SetActive(false);
            secondGear.SetActive(false);
            thirdGear.SetActive(false);
            fourthGear.SetActive(true);
            fifthGear.SetActive(false);
        }
        else
        {
            firstGear.SetActive(false);
            secondGear.SetActive(false);
            thirdGear.SetActive(false);
            fourthGear.SetActive(false);
            fifthGear.SetActive(true);
        }
    }

    private void HandleShowRevFlame()
    {
        RevFlame.SetActive(Player.instance.Revving);
    }
}
