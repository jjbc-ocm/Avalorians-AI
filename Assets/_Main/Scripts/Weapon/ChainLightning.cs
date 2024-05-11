using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainLightning : Projectile
{
    [SerializeField] private LineRenderer lineRenderer;

    protected override void OnPhotonInstantiateComplete()
    {
        if (!photonView.IsMine) return;

        var maxChains = 3;
        var distance = 4f;

        var targets = new List<Character>();

        for (var i = 0; i < maxChains; i++)
        {
            var layerMask = 1 << LayerMask.NameToLayer("Character (Enemy)");

            var relativePosition = targets.Count > 0 ? targets.Last().transform.position : transform.position;

            var colliders = Physics2D.OverlapCircleAll(relativePosition, distance, layerMask);

            var sortedCollider = colliders.OrderBy(i => Vector3.Distance(transform.position, i.transform.position));

            var characters = sortedCollider
                .Select(i => i.GetComponent<Character>())
                .Where(i => !targets.Contains(i));

            var nearestCharacter = characters.FirstOrDefault();

            if (nearestCharacter)
            {
                targets.Add(nearestCharacter);
            }
            else
            {
                break;
            }
        }

        if (targets.Count > 0)
        {
            var viewId1 = targets.Count > 0 ? targets[0].photonView.ViewID : -1;
            var viewId2 = targets.Count > 1 ? targets[1].photonView.ViewID : -1;
            var viewId3 = targets.Count > 2 ? targets[2].photonView.ViewID : -1;

            photonView.RPC("RPC_RenderLightning", RpcTarget.All, viewId1, viewId2, viewId3);
        }
        

        foreach (var target in targets)
        {
            //var damage = (short)owner.Stats.Attack;

            //target.photonView.RPC("RPC_DamagePlayer", RpcTarget.All, damage);

            ExecuteDamage(target.photonView);
        }
    }

    [PunRPC]
    private void RPC_RenderLightning(int viewId1, int viewId2, int viewId3)
    {
        var positions = new List<Vector3> { photonView.transform.position };
        var view1 = PhotonNetwork.GetPhotonView(viewId1);
        var view2 = PhotonNetwork.GetPhotonView(viewId2);
        var view3 = PhotonNetwork.GetPhotonView(viewId3);
        if (view1) positions.Add(view1.transform.position);
        if (view2) positions.Add(view2.transform.position);
        if (view3) positions.Add(view3.transform.position);

        lineRenderer.positionCount = positions.Count;

        for (int i = 0; i < positions.Count; i++)
        {
            lineRenderer.SetPosition(i, positions[i]);
        }
    }
}