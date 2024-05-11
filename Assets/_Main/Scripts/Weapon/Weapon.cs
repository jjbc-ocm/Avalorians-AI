using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Weapon : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    [SerializeField] private string layerSelf;
    [SerializeField] private string layerEnemy;

    protected Rigidbody2D rigidbody2d;

    protected AbilitySO data;
    protected Character owner;

    protected float cacheDistanceToTravel;
    protected float cacheDistanceTraveled;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        Utils.SetLayerRecursively(gameObject, LayerMask.NameToLayer(photonView.IsMine ? layerSelf : layerEnemy));
    }

    protected virtual void Update()
    {
        if (data.RangeType == RangeType.Melee)
        {
            transform.position = owner.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine) return;

        var collidedPhotonView = collision.GetComponent<PhotonView>();

        if (collidedPhotonView)
        {
            ExecuteDamage(collidedPhotonView);
        }
    }

    protected void ExecuteDamage(PhotonView photonView)
    {
        var eagleEye = owner.Stats.GetEffectValue("eagle_eye");

        var distanceTraveledRatio = cacheDistanceToTravel > 0 ? (cacheDistanceTraveled / cacheDistanceToTravel) : 0;

        var damage = (short)(owner.Stats.Attack * (1 + eagleEye * distanceTraveledRatio) * data.DamageRate);

        photonView.RPC("RPC_DamagePlayer", RpcTarget.All, damage, data.IsIgnoreDefense);

        if (!data.CanPassThrough)
        {
            Event_AutoDestroy();
        }
    }

    public void Event_AutoDestroy()
    {
        if (!photonView.IsMine) return; // Need to add this condition because this method can be access via animation

        PhotonNetwork.Destroy(gameObject);
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        var instantiationData = info.photonView.InstantiationData;

        owner = PhotonView.Find((int)instantiationData[0]).GetComponent<Character>();
        data = DataManager.Instance.GetAbility((string)instantiationData[1]);

        cacheDistanceToTravel = data.Speed * data.Duration;

        OnPhotonInstantiateComplete();
    }




    protected virtual void OnPhotonInstantiateComplete()
    {

    }
}