using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// This is a fast-moving ability, speed and duration can be set in the scriptable object
// What it does is forces the owner to move in the same direction as this
// In addition, while this skill is active player will become invisible to all attacks
public class ShadowStep : Projectile
{
    protected override void OnPhotonInstantiateComplete()
    {
        if (!photonView.IsMine) return;

        owner.Stats.SetInvisible(true);

        var characters = FindObjectsOfType<Character>();

        var nearestCharacter = characters.Where(i => i != owner).OrderByDescending(i => Vector3.Distance(owner.transform.position, i.transform.position)).FirstOrDefault();

        if (nearestCharacter)
        {
            owner.Stats.CacheAbilityDistanceNearEnemy = Vector3.Distance(owner.transform.position, nearestCharacter.transform.position);
        }

        
    }

    private void OnDestroy()
    {
        if (!photonView.IsMine) return;

        owner.Stats.SetInvisible(false);
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        var direction = new Vector2(transform.up.x, transform.up.y);

        rigidbody2d.MovePosition(rigidbody2d.position + direction * data.Speed * Time.fixedDeltaTime);

        var ownerRB = owner.GetComponent<Rigidbody2D>();

        ownerRB.MovePosition(ownerRB.position + direction * data.Speed * Time.fixedDeltaTime);
    }
}
