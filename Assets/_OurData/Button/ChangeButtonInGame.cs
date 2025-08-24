using TMPro;
using UnityEngine;

public class ChangeButtonInGame : HieuMonoBehavior
{
    [Header("Button In Game")]
    public HeroCtrl heroCtrl;
    [SerializeField] protected bool isActive = true;
    [Header("UI")]
    [SerializeField] protected TextMeshProUGUI buttonText;
    protected override void LoadComponents()
    {
        this.loadHeroCtrl();
        this.loadButtonText();
    }

    protected virtual void loadHeroCtrl()
    {
        if (this.heroCtrl != null) return;
        PlayerManager playerManager= PlayerManager.instance;
        if(playerManager == null) return;
        heroCtrl = playerManager.currentHero;
    }

    protected virtual void loadButtonText()
    {
        if (buttonText != null) return;
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public virtual void ChangeButton()
    {
        if (heroCtrl == null)
        {
            this.loadHeroCtrl();
        }
        //Override in child class
    }
}
