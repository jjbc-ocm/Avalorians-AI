    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Ability")]
public class AbilitySO : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string abilityName;
    [TextArea] [SerializeField] private string info;
    [SerializeField] private Sprite icon;

    [SerializeField] private string prefabAbility;
    [SerializeField] private RangeType rangeType;
    [SerializeField] private float damageRate;
    [SerializeField] private float speed;
    [SerializeField] private float duration;
    [SerializeField] private int count;
    [SerializeField] private float spread;
    [SerializeField] private bool isIgnoreDefense;
    [SerializeField] private bool canPassThrough;
    [SerializeField] private float cooldown;
    [SerializeField] private int mana;

    public string Id => id;

    public string AbilityName => abilityName;

    public string Info => info;

    public Sprite Icon => icon;

    public string PrefabAbility => prefabAbility;

    public RangeType RangeType => rangeType;

    public float DamageRate => damageRate;

    public float Speed => speed;

    public float Duration => duration;

    public int Count => count;

    public float Spread => spread;

    public bool IsIgnoreDefense => isIgnoreDefense;

    public bool CanPassThrough => canPassThrough;

    public float Cooldown => cooldown;

    public int Mana => mana;
}