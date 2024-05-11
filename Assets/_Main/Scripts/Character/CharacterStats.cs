using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterStats : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private ClassSO data;

    private Animator animator;

    private short serialHealth;
    private short serialMaxHealth;
    private short serialMana;
    private short serialMaxMana;
    private byte serialStates; // Using byte to represent state (e.g., idle, walking, running, etc.)

    private short attack;
    private short defense;
    private float speed;

    private List<AppliedEffectInfo> appliedEffects;
    private List<AbilityCooldownInfo> abilityCooldowns;

    private short cacheDamageReceived;
    private float cacheAbilityDistanceNearEnemy;
    private float cacheDeltaTime;
    private float cacheRespawnTime;

    public event Action<short, short> OnHealthChanged;
    public event Action<short, short> OnManaChanged;
    public event Action<bool> OnDeadChanged;
    public event Action<bool> OnInvisibleChanged;
    public event Action<AppliedEffectInfo> OnAddAppliedEffect;
    public event Action<AppliedEffectInfo> OnRemoveAppliedEffect;
    public event Action<List<AppliedEffectInfo>> OnAppliedEffectsUpdated;
    public event Action<List<AbilityCooldownInfo>> OnAbilityCooldownsUpdated;

    public ClassSO Data => data;

    public short Attack => (short)(attack * (1 + GetEffectValue("attack")));

    public short Defense => (short)(defense * (1 + GetEffectValue("defense")));

    public float Speed => speed * (1 + GetEffectValue("speed"));

    public short CacheDamageReceived 
    { 
        get => cacheDamageReceived; 
        set => cacheDamageReceived = value; 
    }

    public float CacheAbilityDistanceNearEnemy 
    { 
        get => cacheAbilityDistanceNearEnemy; 
        set => cacheAbilityDistanceNearEnemy = value; 
    }

    private void SetHealth(short value)
    {
        serialHealth = value;
        OnHealthChanged?.Invoke(serialHealth, serialMaxHealth);
    }

    private void SetMaxHealth(short value)
    {
        serialMaxHealth = value;
        OnHealthChanged?.Invoke(serialHealth, serialMaxHealth);
    }

    private void SetMana(short value)
    {
        serialMana = value;
        OnManaChanged?.Invoke(serialMana, serialMaxMana);
    }

    private void SetMaxMana(short value)
    {
        serialMaxMana = value;
        OnManaChanged?.Invoke(serialMana, serialMaxMana);
    }

    private void SetState(int stateId, bool value)
    {
        if (value)
        {
            serialStates = (byte)(serialStates | (1 << stateId));
        }
        else
        {
            serialStates = (byte)(serialStates & ~(1 << stateId));
        }
    }

    public bool IsDead()
    {
        return (serialStates & (1 << 0)) != 0;
    }

    public bool IsInvisible()
    {
        return (serialStates & (1 << 1)) != 0;
    }

    public void SetDead(bool value)
    {
        SetState(0, value);
        OnDeadChanged?.Invoke(value);
    }

    public void SetInvisible(bool value)
    {
        SetState(1, value);
        OnInvisibleChanged?.Invoke(value);
    }

    private void AddAppliedEffect(EffectSO effect)
    {
        var current = appliedEffects.FirstOrDefault(i => i.Effect == effect);

        if (current == null)
        {
            var newAppliedEffect = new AppliedEffectInfo
            {
                Effect = effect,
                Stack = 1,
                Duration = effect.Duration
            };

            appliedEffects.Add(newAppliedEffect);

            OnAddAppliedEffect?.Invoke(newAppliedEffect);
        }
        else
        {
            if (current.Stack < effect.MaxStack)
            {
                current.Stack += 1;
            }

            current.Duration = effect.Duration;
        }
    }





    #region Unity

    private void Awake()
    {
        animator = GetComponent<Animator>();

        appliedEffects = new List<AppliedEffectInfo>();
    }

    private void Start()
    {
        if (!photonView.IsMine) return;

        SetMaxHealth(data.BaseHealth);
        SetHealth(data.BaseHealth);
        SetMaxMana(data.BaseMana);
        SetMana(data.BaseMana);
        attack = data.BaseAttack;
        defense = data.BaseDefense;
        speed = data.BaseSpeed;

        abilityCooldowns = new List<AbilityCooldownInfo>();

        foreach (var abilityInput in data.AbilityInputs)
        {
            var abilityCooldown = new AbilityCooldownInfo
            {
                Ability = abilityInput.Ability,
                ButtonName = abilityInput.ButtonName,
                Cooldown = 0
            };

            abilityCooldowns.Add(abilityCooldown);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        // Automatically regenerate mana
        cacheDeltaTime += Time.deltaTime;

        if (cacheDeltaTime >= 1)
        {
            SetMana((short)Mathf.Min(serialMana + 4, serialMaxMana));

            cacheDeltaTime -= 1;
        }

        // If player is dead, respawn after respawn timer
        if (IsDead())
        {
            cacheRespawnTime += Time.deltaTime;

            if (cacheRespawnTime >= 5)
            {
                cacheRespawnTime = 0;

                SetDead(false);
            }
        }

        // Attacking and cooldown
        foreach (var abilityCooldown in abilityCooldowns)
        {
            abilityCooldown.Cooldown = Mathf.Min(abilityCooldown.Cooldown + Time.deltaTime, abilityCooldown.Ability.Cooldown);

            if (Input.GetButton(abilityCooldown.ButtonName) && !IsDead() && 
                abilityCooldown.Ability.Mana <= serialMana && 
                abilityCooldown.Cooldown >= abilityCooldown.Ability.Cooldown)
            {
                animator.SetBool("isAttacking", true);

                ExecuteAbility(abilityCooldown.Ability);

                SetMana((short)(serialMana - abilityCooldown.Ability.Mana));

                abilityCooldown.Cooldown = 0;
            }
        }

        OnAbilityCooldownsUpdated?.Invoke(abilityCooldowns);

        // Always check if passive can apply the effect
        foreach (var passive in data.Passives)
        {
            if (CallMethodByName(passive.Condition))
            {
                AddAppliedEffect(passive.Effect);
            }
        }

        // Reduce cooldowns of effects
        var removedEffects = new List<AppliedEffectInfo>();

        foreach (var effect in appliedEffects)
        {
            effect.Duration -= Time.deltaTime;

            if (effect.Duration <= 0)
            {
                removedEffects.Add(effect);
            }
        }

        OnAppliedEffectsUpdated?.Invoke(appliedEffects);

        // Remove effects that already expired
        foreach (var removedEffect in removedEffects)
        {
            appliedEffects.Remove(removedEffect);

            OnRemoveAppliedEffect?.Invoke(removedEffect);
        }
    }

    private void OnDestroy()
    {
        OnHealthChanged = null;
        OnDeadChanged = null;
        OnInvisibleChanged = null;
    }

    #endregion

    private void ExecuteAbility(AbilitySO ability)
    {
        for (var i = 0; i < ability.Count; i++)
        {
            var maxSpread = (ability.Count - 1) * ability.Spread;
            var spread = i * ability.Spread - (maxSpread / 2f);
            var dirX = animator.GetFloat("dirX");
            var dirY = animator.GetFloat("dirY");
            var angleRadians = Mathf.Atan2(dirY, dirX);
            var angleDegrees = angleRadians * Mathf.Rad2Deg;
            var angleFinal = angleDegrees + spread - 90;

            var data = new object[] { photonView.ViewID, ability.Id };

            PhotonNetwork.Instantiate(ability.PrefabAbility, transform.position, Quaternion.Euler(0, 0, angleFinal), 0, data);
        }
    }

    public float GetEffectValue(string key)
    {
        var accumulated = 0f;

        foreach (var appliedEffect in appliedEffects)
        {
            foreach (var effect in appliedEffect.Effect.Effects)
            {
                if (effect.Key == key)
                {
                    accumulated += effect.Value * appliedEffect.Stack;
                }
            }
        }
        return accumulated;
    }

    private void SetupRespawn()
    {
        GameManager.Instance.ResetPosition(photonView);
        SetHealth(data.BaseHealth);
    }

    #region Photon

    [PunRPC]
    private void RPC_DamagePlayer(short baseDamage, bool isIgnoreDefense)
    {
        if (!photonView.IsMine) return;

        var manaShield = Mathf.Min(GetEffectValue("mana_shield"), 1f);
        var defenseRatio = isIgnoreDefense ? 0 : 1;
        var damage = baseDamage - Defense * defenseRatio;
        var mpDamage = Math.Min(damage * manaShield, serialMana);
        var hpDamage = (short)(damage - mpDamage);
        
        SetHealth((short)(serialHealth - hpDamage));
        SetMana((short)(serialMana - mpDamage));

        if (serialHealth <= 0)
        {
            cacheDamageReceived = 0;

            SetDead(true);

            SetupRespawn();
        }
        else
        {
            cacheDamageReceived += hpDamage;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(serialHealth);
            stream.SendNext(serialMaxHealth);
            stream.SendNext(serialMana);
            stream.SendNext(serialMaxMana);
            stream.SendNext(serialStates);
        }
        else
        {
            SetHealth((short)stream.ReceiveNext());
            SetMaxHealth((short)stream.ReceiveNext());
            SetMana((short)stream.ReceiveNext());
            SetMaxMana((short)stream.ReceiveNext());
            serialStates = (byte)stream.ReceiveNext();

            // Call first the least priority
            // Reason: Some states affect same property, we want the state with heigher weight to dictate the property
            SetInvisible(IsInvisible());
            SetDead(IsDead());
        }
    }

    #endregion

    #region Conditions

    private bool CallMethodByName(string methodName)
    {
        var method = GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

        if (method != null)
        {
            return (bool)method.Invoke(this, null);
        }
        else
        {
            throw new ArgumentException("Method not found", nameof(methodName));
        }
    }

    private bool IsAlwaysActive()
    {
        return true;
    }

    private bool HasReceivedTenthDamage()
    {
        var tenthDamage = (short)(serialMaxHealth / 10);

        if (tenthDamage <= cacheDamageReceived)
        {
            cacheDamageReceived -= tenthDamage;

            return true;
        }

        return false;
    }

    private bool HasUsedAbilityNearEnemy()
    {
        if (cacheAbilityDistanceNearEnemy > 0 && cacheAbilityDistanceNearEnemy <= 2)
        {
            cacheAbilityDistanceNearEnemy = 0;

            return true;
        }

        return false;
    }

    #endregion
}
