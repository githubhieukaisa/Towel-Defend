using UnityEngine;

public class ScarecrowDamageReceiver : DamageReceiver
{
    protected override void Despawn()
    {
        base.Despawn();
        ScarecrowSpawner.instance.ScarecrowDead();
    }
}
