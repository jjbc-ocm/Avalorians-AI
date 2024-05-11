using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityInputInfo
{
    [SerializeField] private AbilitySO ability;
    [SerializeField] private string buttonName;

    public AbilitySO Ability => ability;

    public string ButtonName => buttonName;
}