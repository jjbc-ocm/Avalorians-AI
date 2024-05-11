using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private BarUI uiHealth;
    [SerializeField] private BarUI uiMana;

    private RectTransform rectTransform;

    private Character character;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (character != null)
        {
            var screenPoint = Camera.main.WorldToScreenPoint(character.transform.position); // Convert the world position of the target to screen coordinates

            rectTransform.position = screenPoint; // Set the position of the UI element
        }
    }

    public void Initialize(Character character)
    {
        this.character = character;

        character.Stats.OnHealthChanged += Character_OnHealthChanged;
        character.Stats.OnManaChanged += Stats_OnManaChanged;
    }

    private void Character_OnHealthChanged(short health, short maxHealth)
    {
        uiHealth.Refresh(state =>
        {
            state.Value = (float)health / maxHealth;
        });
    }

    private void Stats_OnManaChanged(short mana, short maxMana)
    {
        uiMana.Refresh(state =>
        {
            state.Value = (float)mana / maxMana;
        });
    }
}
