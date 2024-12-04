using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPMInitiative : MonoBehaviour
{
    [Header("SLIDER PARENT")]
    public Transform SliderParent;

    [Header("RPM Sliders")]
    public List<CharacterRPMSlider> RPMSliders = new List<CharacterRPMSlider>();

    private void Update()
    {
        HandleSliderLayer();
    }

    private int CompareSliderOrder(CharacterRPMSlider a, CharacterRPMSlider b)
    {
        if (a.CurrentValue < b.CurrentValue) return -1;
        else if (a.CurrentValue > b.CurrentValue) return 1;
        else return 0;
    }

    private void HandleSliderLayer()
    {
        RPMSliders.Sort(CompareSliderOrder);
        for (int i = 0; i < RPMSliders.Count; i++)
        {
            RPMSliders[i].CanvasComp.sortingOrder = i + 1;
        }
    }
}
