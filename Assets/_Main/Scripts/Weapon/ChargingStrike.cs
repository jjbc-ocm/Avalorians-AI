using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a fast-moving ability, speed and duration can be set in the scriptable object
// What it does is forces the owner to move in the same direction as this
// In addition, if an enemy was hit during the process, this will make them follow this object as well
public class ChargingStrike : Projectile
{
    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        var direction = new Vector2(transform.up.x, transform.up.y);

        rigidbody2d.MovePosition(rigidbody2d.position + direction * data.Speed * Time.fixedDeltaTime);

        var ownerRB = owner.GetComponent<Rigidbody2D>();

        ownerRB.MovePosition(ownerRB.position + direction * data.Speed * Time.fixedDeltaTime);
    }



    
}
