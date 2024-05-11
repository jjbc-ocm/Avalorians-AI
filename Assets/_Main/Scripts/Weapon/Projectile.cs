using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : Weapon
{
    

    private void Start()
    {
        if (!photonView.IsMine) return;

        StartCoroutine(YieldDestroy());
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        var direction = new Vector2(transform.up.x, transform.up.y);

        rigidbody2d.velocity = direction * data.Speed;

        cacheDistanceTraveled += data.Speed * Time.fixedDeltaTime;
    }

    private IEnumerator YieldDestroy()
    {
        yield return new WaitForSeconds(data.Duration);

        Event_AutoDestroy();
    }
}
