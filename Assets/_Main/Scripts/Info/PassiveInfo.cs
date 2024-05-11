using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PassiveInfo
{
    [SerializeField] private string passiveName;
    [TextArea] [SerializeField] private string info;
    [SerializeField] private Sprite icon;

    [SerializeField] private string condition;
    [SerializeField] private EffectSO effect;

    public string PassiveName => passiveName;

    public string Info => info;

    public Sprite Icon => icon;

    public string Condition => condition;

    public EffectSO Effect => effect;
}
