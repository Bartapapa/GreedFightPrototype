using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTutorials : MonoBehaviour
{
    [Header("OBJECT REFS")]
    public GameObject BattleOverviewInputs;
    public GameObject ActionSelectInputs;
    public GameObject TargetingInputs;

    public void BattleOverview(bool show)
    {
        BattleOverviewInputs.SetActive(show);
    }

    public void ActionSelect(bool show)
    {
        ActionSelectInputs.SetActive(show);
    }

    public void Targeting(bool show)
    {
        TargetingInputs.SetActive(show);
    }
}
