using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCooldownInfo
{
    private AbilitySO ability;
    private string buttonName;
    private float cooldown;

    public AbilitySO Ability { get => ability; set => ability = value; }

    public string ButtonName { get => buttonName; set => buttonName = value; }

    public float Cooldown { get => cooldown; set => cooldown = value; }
}
