using UnityEngine;

public class StatueLevel : Level
{
    [Header("Statue")]
    public StatueCtrl statueCtrl;

    protected override void ResetValue()
    {
        this.level = 1;
    }
    protected override void LoadComponents()
    {
        LoadStatueCtrl();
    }

    protected virtual void LoadStatueCtrl()
    {
        if (this.statueCtrl != null) return;
        this.statueCtrl = GetComponent<StatueCtrl>();

        Debug.Log(transform.name + ": LoadStatueCtrl");
    }

    private void OnTriggerStay(Collider other)
    {
        this.CheckActor(other.gameObject, true);
    }

    private void OnTriggerExit(Collider other)
    {
        this.CheckActor(other.gameObject, false);
    }

    protected virtual void CheckActor(GameObject gameObject, bool status)
    {
        if (this.canLevelUp == status) return;

        int layer= gameObject.layer;
        if(layer != MyLayerManager.instance.layerHero) return;

        this.canLevelUp = status;

        PlayerManager playerManager = PlayerManager.instance;
        if(status) playerManager.playerInput.interactable = this.statueCtrl.statueInteractable;
        else playerManager.playerInput.interactable = null;

        Debug.Log(transform.name + ": CheckActor " + gameObject.name + " status: " + status);
    }

    //protected override void afterUp()
    //{
    //    StatueDamageReceiver statueDamageReceiver = this.statueCtrl.statueDamageReceiver;
    //    statueDamageReceiver.setHpMax(20 + this.level * 10);
    //}

    public override int Up(int up)
    {
        base.Up(up);
        int newHpMax = this.level * this.levelCost;
        this.statueCtrl.statueDamageReceiver.setHpMax(newHpMax);
        this.statueCtrl.statueDamageReceiver.setHp(newHpMax);
        
        return this.level;
    }
}
