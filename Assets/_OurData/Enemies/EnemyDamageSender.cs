using UnityEngine;

public class EnemyDamageSender : DamageSender
{
    [Header("Enemy")]
    public EnemyCtrl enemyCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyCtrl();
    }

    protected virtual void LoadEnemyCtrl()
    {
        if (this.enemyCtrl != null) return;
        this.enemyCtrl = this.GetComponent<EnemyCtrl>();

        Debug.Log(transform.name + ": LoadEnemyCtril");
    }

    protected override void SendDamage(GameObject coliderObj)
    {
        DamageReceiver damageReceiver = coliderObj.GetComponent<DamageReceiver>();
        if (damageReceiver == null) return;

        int currentHP= this.enemyCtrl.damageReceiver.HP();
        damageReceiver.Receive(currentHP, this);
        this.enemyCtrl.damageReceiver.Receive(currentHP);
    }
}
