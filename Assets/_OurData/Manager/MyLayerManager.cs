using UnityEngine;

public class MyLayerManager : HieuMonoBehavior
{
    public static MyLayerManager instance;

    [Header("Layers")]
    public int layerHero;
    public int layerCeiling;
    public int layerGround;
    public int layerEnemy;
    public int layerBullet;


    private void Awake()
    {
        if (instance != null) return;
        instance = this;

        this.LoadComponents();

        Physics.IgnoreLayerCollision(this.layerHero, this.layerHero, true);
        Physics.IgnoreLayerCollision(this.layerHero, this.layerEnemy, true);
        Physics.IgnoreLayerCollision(this.layerHero, this.layerBullet, true);
        Physics.IgnoreLayerCollision(this.layerEnemy, this.layerEnemy, true);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.GetPlayers();
    }

    protected virtual void GetPlayers()
    {
        if (this.layerHero > 0 && this.layerGround > 0 && this.layerCeiling > 0 && this.layerEnemy > 0) return;
        this.layerHero = LayerMask.NameToLayer("Hero");
        this.layerGround = LayerMask.NameToLayer("Ground");
        this.layerCeiling = LayerMask.NameToLayer("Ceiling");
        this.layerEnemy = LayerMask.NameToLayer("Enemy");
        this.layerBullet = LayerMask.NameToLayer("Bullet");
    }

}
