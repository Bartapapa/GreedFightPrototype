using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefineFill : MonoBehaviour
{
    [Header("FILL")]
    public Image Fill;

    [Header("OVERLAYS")]
    public List<GameObject> Overlays = new List<GameObject>();

    private void Update()
    {
        float fuelThreshold = 1f;
        if (Player.instance.CurrentGear == 5)
        {
            Fill.enabled = false;
        }
        else
        {
            if (Player.instance.CurrentGear == 1)
            {
                fuelThreshold = GameManager.instance.UniversalVariables.Gear2FuelThreshold;
            }
            else if (Player.instance.CurrentGear == 2)
            {
                fuelThreshold = GameManager.instance.UniversalVariables.Gear3FuelThreshold;
            }
            else if (Player.instance.CurrentGear == 3)
            {
                fuelThreshold = GameManager.instance.UniversalVariables.Gear4FuelThreshold;
            }
            else if (Player.instance.CurrentGear == 4)
            {
                fuelThreshold = GameManager.instance.UniversalVariables.Gear5FuelThreshold;
            }

            Fill.fillAmount = Player.instance.CurrentRefinedFuel / (fuelThreshold / .34f);
        }

        for (int i = 0; i < Overlays.Count; i++)
        {
            if (i == Player.instance.CurrentGear - 1)
            {
                Overlays[i].SetActive(true);
            }
            else
            {
                Overlays[i].SetActive(false);
            }
        }
    }
}
