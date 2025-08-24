using System;
using UnityEngine;

public class DamageSender : HieuMonoBehavior
{
    [Header("Damage Sender")]
    [SerializeField] protected int damage = 1;

    void OnCollisionEnter(Collision collision)
    {
        this.SendDamage(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        this.SendDamage(other.gameObject);
    }


    protected virtual void SendDamage(Collision collision)
    {
        DamageReceiver damageReceiver = collision.gameObject.GetComponent<DamageReceiver>();
        if (damageReceiver == null) return;

        damageReceiver.Receive(this.damage);
    }
    protected virtual void SendDamage(GameObject gameObject)
    {
        DamageReceiver damageReceiver = gameObject.GetComponent<DamageReceiver>();
        if (damageReceiver == null) return;

        damageReceiver.Receive(this.damage, this);
    }
}
