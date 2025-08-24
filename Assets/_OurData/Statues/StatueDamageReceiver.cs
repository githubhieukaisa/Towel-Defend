using UnityEngine;

public class StatueDamageReceiver : DamageReceiver
{
    protected override void ResetValue()
    {
        this.hpMax = 10;
        this.hp = 10;
    }

    public override void Receive(int damage, DamageSender sender)
    {
        int senderLayer = sender.gameObject.layer;
        if (senderLayer != MyLayerManager.instance.layerEnemy) return;
        this.Receive(damage);
    }

    public virtual bool IsHpFull()
    {
        return this.hp >= this.hpMax;
    }

    public virtual void setHpMax(int hpMax)
    {
        this.hpMax = hpMax;
        //if (this.hp > this.hpMax) this.hp = this.hpMax;
    }

    protected override void Despawn()
    {
        //is game over
    }
}
