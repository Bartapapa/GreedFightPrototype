using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UNIVERSAL VARIABLES")]
    public UniversalVariables UniversalVariables;

    [Header("OBJECT REFERENCES")]
    public ActionSelect ActionSelectUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        CloseActionSelect();
    }

    public void ShowActionSelect(BattleCharacter character)
    {
        ActionSelectUI.gameObject.SetActive(true);
        ActionSelectUI.PopulateAbilities(character);
    }

    public void CloseActionSelect()
    {
        ActionSelectUI.gameObject.SetActive(false);
    }
}
