using UnityEngine;

public class ChangeBulletToInfinity : ChangeButtonInGame
{
    public override void ChangeButton()
    {
        base.ChangeButton();
        this.heroCtrl.firearmFire.Firearm.Params.MagazineCapacity = isActive ? int.MaxValue : 1;
        if(!isActive) this.heroCtrl.firearmFire.Firearm.AmmoShooted=0;

        buttonText.text = isActive ? "Infinity" : "Normal";
        isActive = !isActive;
    }
}
