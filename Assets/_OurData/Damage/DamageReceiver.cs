using UnityEngine;

public class DamageReceiver : HieuMonoBehavior
{
    [Header("Damage Receiver")]
    [SerializeField] protected int hpMax = 2;
    [SerializeField] protected int hp = 2;
    [SerializeField] protected string deadEffect = "EnemyDeath1";

    private void Start()
    {
        this.Revival();
    }

    private void OnEnable()
    {
        this.Revival();
    }

    public virtual int HP()
    {
        return this.hp;
    }

    public virtual void Revival()
    {
        this.hp = this.hpMax;
    }
    public virtual bool IsDead()
    {
        return this.hp <= 0;
    }

    public virtual void Receive(int damage)
    {
        this.hp -= damage;
        this.Dying();
    }

    public virtual void Receive(int damage, DamageSender sender)
    {
        this.Receive(damage);
    }

    protected virtual void Dying()
    {
        if (!this.IsDead()) return;

        this.ShowDeadEffect();
        this.Despawn();
    }

    protected virtual void ShowDeadEffect()
    {
        Transform effect = ObjPoolManager.instance.Spawn(this.deadEffect, this.transform.position, transform.rotation);
        effect.gameObject.SetActive(true);
    }
    
    protected virtual void Despawn()
    {
        ObjPoolManager.instance.Despawn(this.transform);
        ScoreManager.instance.Kill();
        ScoreManager.instance.GoldAdd(1);
    }

    public virtual void setHp(int hp)
    {
        this.hp = hp;
    }
    public virtual bool Heal()
    {
        int loseHp = this.hpMax - this.hp;
        int currentGold = ScoreManager.instance.GetGold();

        int cost = loseHp >= currentGold ? currentGold : loseHp;
        if (!ScoreManager.instance.GoldDeduct(cost)) return false;

        this.hp += cost;
        return true;
    }
}
