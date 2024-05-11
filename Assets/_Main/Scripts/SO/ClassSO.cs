using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Class")]
public class ClassSO : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string className;
    [SerializeField] private Sprite icon;

    [SerializeField] private string prefabName;
    [SerializeField] private short baseHealth;
    [SerializeField] private short baseMana;
    [SerializeField] private short baseAttack;
    [SerializeField] private short baseDefense;
    [SerializeField] private float baseSpeed;
    [SerializeField] private AbilityInputInfo[] abilityInputs;
    [SerializeField] private PassiveInfo[] passives;

    public string Id => id;

    public string ClassName => className;

    public Sprite Icon => icon;

    public string PrefabName => prefabName;

    public short BaseHealth => baseHealth;

    public short BaseMana => baseMana;

    public short BaseAttack => baseAttack;

    public short BaseDefense => baseDefense;

    public float BaseSpeed => baseSpeed;

    public AbilityInputInfo[] AbilityInputs => abilityInputs;

    public PassiveInfo[] Passives => passives;
}