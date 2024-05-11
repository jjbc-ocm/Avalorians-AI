using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyValueInfo
{
    [SerializeField] private string key;
    [SerializeField] private float value;

    public string Key => key;

    public float Value => value;
}
