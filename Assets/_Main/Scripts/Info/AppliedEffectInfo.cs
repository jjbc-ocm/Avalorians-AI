using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AppliedEffectInfo
{
    [SerializeField] private EffectSO effect;
    [SerializeField] private int stack;
    [SerializeField] private float duration;

    public EffectSO Effect { get => effect; set => effect = value; }

    public int Stack { get => stack; set => stack = value; }

    public float Duration { get => duration; set => duration = value; }
}
