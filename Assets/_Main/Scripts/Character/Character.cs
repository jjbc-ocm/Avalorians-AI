using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Notes:
// Stats are using short to reduce the network latency
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CharacterStats))]
public class Character : MonoBehaviourPun
{
    [SerializeField] private string layerSelf;
    [SerializeField] private string layerEnemy;

    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private Collider2D collider2d;
    private SpriteRenderer spriteRenderer;
    private CharacterStats stats;

    private Vector2 moveDirection;

    public CharacterStats Stats => stats;

    #region Unity

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stats = GetComponent<CharacterStats>();

        Utils.SetLayerRecursively(gameObject, LayerMask.NameToLayer(photonView.IsMine ? layerSelf : layerEnemy));

        stats.OnDeadChanged += Stats_OnDeadChanged;
        stats.OnInvisibleChanged += Stats_OnInvisibleChanged;

        if (!photonView.IsMine) return;
    }

    private void Start()
    {
        GameUI.Instance.AddPlayerUI(this);

        if (!photonView.IsMine) return;

        GameManager.Instance.VirtualCamera.Follow = gameObject.transform;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        // Movement
        var inputX = Input.GetAxisRaw("Horizontal");
        var inputY = Input.GetAxisRaw("Vertical");

        if (!stats.IsDead())
        {
            moveDirection.x = inputX;
            moveDirection.y = inputY;
            moveDirection = moveDirection.normalized;
        }
        
        // Animation
        if ((inputX != 0 || inputY != 0) && !stats.IsDead())
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("dirX", moveDirection.x);
            animator.SetFloat("dirY", moveDirection.y);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        /*// Attacking
        foreach (var abilityInput in stats.Data.AbilityInputs)
        {
            if (Input.GetButtonDown(abilityInput.ButtonName) && !stats.IsDead() && abilityInput.Ability.Mana <= stats.Mana)
            {
                animator.SetBool("isAttacking", true);

                ExecuteAbility(abilityInput.Ability);

                stats.SetMana((short)(stats.Mana - abilityInput.Ability.Mana));
            }
        }*/
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        rigidbody2d.velocity = moveDirection * stats.Speed;
    }

    #endregion

    #region Event

    private void Stats_OnInvisibleChanged(bool value)
    {
        collider2d.enabled = !value;
        spriteRenderer.color = new Color(1, 1, 1, value ? 0.5f : 1);

    }

    private void Stats_OnDeadChanged(bool value)
    {
        collider2d.enabled = !value;
        spriteRenderer.color = new Color(1, 1, 1, value ? 0 : 1);
    }

    public void Event_StopAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    #endregion
}
