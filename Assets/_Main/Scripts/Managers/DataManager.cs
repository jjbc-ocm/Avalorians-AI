using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    [SerializeField] private List<ClassSO> classes;
    [SerializeField] private List<AbilitySO> abilities;

    public List<ClassSO> Classes => classes;

    public List<AbilitySO> Abilities => abilities;

    public ClassSO GetClass(string id) => classes.FirstOrDefault(i => i.Id == id);

    public AbilitySO GetAbility(string id) => abilities.FirstOrDefault(i => i.Id == id);

}

