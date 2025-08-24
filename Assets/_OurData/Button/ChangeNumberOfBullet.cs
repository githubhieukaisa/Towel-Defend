using UnityEngine;

public class ChangeNumberOfBullet : ChangeButtonInGame
{
    public override void ChangeButton()
    {
        base.ChangeButton();
        this.heroCtrl.firearmFire.interations = isActive ? 3 : 1;
        buttonText.text = isActive ? "Three" : "One";
        isActive = !isActive;
    }
}
