using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Effect")]
public class EffectSO : ScriptableObject
{
    [SerializeField] private string effectName;
    [TextArea] [SerializeField] private string info;
    [SerializeField] private Sprite icon;

    [SerializeField] private List<KeyValueInfo> effects;
    [SerializeField] private int maxStack;
    [SerializeField] private float duration;

    public string EffectName => effectName;

    public string Info => info;

    public Sprite Icon => icon;

    public List<KeyValueInfo> Effects => effects;

    public int MaxStack => maxStack;

    public float Duration => duration;
}
