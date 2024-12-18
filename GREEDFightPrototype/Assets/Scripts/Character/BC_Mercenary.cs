using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Mercenary : BattleCharacter
{
    [Header("CAMERAS")]
    public CinemachineVirtualCamera ActionSelectCamera;
    public CinemachineVirtualCamera TargetSelfCamera;

    public override void SelectCharacterForActionSelect(bool select)
    {
        ActionSelectCamera.enabled = select;
    }

    public override void TargetSelf(bool target)
    {
        TargetSelfCamera.enabled = target;
    }
}
