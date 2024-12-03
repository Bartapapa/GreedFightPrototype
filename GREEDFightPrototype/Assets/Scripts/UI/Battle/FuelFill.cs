using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelFill : MonoBehaviour
{
    [Header("FILL")]
    public Image Fill;

    [Header("FUEL GEMS")]
    public List<GameObject> EmptyGems = new List<GameObject>();
    public List<GameObject> FilledGems = new List<GameObject>();

    private void Update()
    {
        Fill.fillAmount = Player.instance.CurrentFuel / GameManager.instance.UniversalVariables.MaxFuelAmount;
        if (Fill.fillAmount >= .25f)
        {
            EmptyGems[0].SetActive(false);
            FilledGems[0].SetActive(true);
        }
        else
        {
            EmptyGems[0].SetActive(true);
            FilledGems[0].SetActive(false);
        }
        if (Fill.fillAmount >= .5f)
        {
            EmptyGems[1].SetActive(false);
            FilledGems[1].SetActive(true);
        }
        else
        {
            EmptyGems[1].SetActive(true);
            FilledGems[1].SetActive(false);
        }
        if (Fill.fillAmount >= .75f)
        {
            EmptyGems[2].SetActive(false);
            FilledGems[2].SetActive(true);
        }
        else
        {
            EmptyGems[2].SetActive(true);
            FilledGems[2].SetActive(false);
        }
        if (Fill.fillAmount >= 1f)
        {
            EmptyGems[3].SetActive(false);
            FilledGems[3].SetActive(true);
        }
        else
        {
            EmptyGems[3].SetActive(true);
            FilledGems[3].SetActive(false);
        }
    }
}
