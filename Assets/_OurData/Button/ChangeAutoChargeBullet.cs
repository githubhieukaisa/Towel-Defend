using TMPro;
using UnityEngine;

public class ChangeAutoChargeBullet : ChangeButtonInGame
{
    public override void ChangeButton()
    {
        base.ChangeButton();
        this.heroCtrl.firearmFire.Firearm.Params.AutomaticFire = isActive;
        buttonText.text = isActive ? "Auto" : "Not Auto";
        isActive = !isActive;
    }
}
