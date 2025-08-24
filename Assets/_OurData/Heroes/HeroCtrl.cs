using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CharacterScripts.Firearms;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class HeroCtrl : HieuMonoBehavior
{
    [Header("Hero")]
    public Character character;
    public CharacterController characterCtrl;
    public Transform armL;
    public Transform armR;
    public Firearm firearm;
    public FirearmFire firearmFire;
    public HeroProfile heroProfile;
    [SerializeField] protected bool canAutomaticFire = false;
    [SerializeField] protected bool infinityBullet = false;
    [SerializeField] protected bool isThreeBullet = false;

    private void Start()
    {
        //Debug.Log(transform.name + ": "+ this.heroProfile.heroName);
        this.firearmFire.Firearm.Params.AutomaticFire = canAutomaticFire;
        if (infinityBullet)
        {
            this.firearmFire.Firearm.Params.MagazineCapacity = int.MaxValue;
        }
        if(isThreeBullet)
        {
            this.firearmFire.interations = 3;
        }
    }

    protected override void LoadComponents()
    {
        this.LoadCharacter();
        this.LoadCharCtrl();
        this.LoadCharBodyParts();
        this.LoadHeroProfile();
    }

    protected virtual void LoadHeroProfile()
    {
        if(this.heroProfile != null) return;
        this.heroProfile = GetComponent<HeroProfile>();
        Debug.Log(transform.name + ": LoadHeroProfile");
    }

    protected virtual void LoadCharacter()
    {
        if (this.character != null) return;
        this.character=GetComponent<Character>();

    }

    protected virtual void LoadCharCtrl()
    {
        if (this.characterCtrl != null) return;
        
        this.characterCtrl=GetComponent<CharacterController>();
        if(this.characterCtrl == null) this.characterCtrl=gameObject.AddComponent<CharacterController>();

        this.characterCtrl.center = new Vector3(0, 1.125f);
        this.characterCtrl.height = 2.5f;
        this.characterCtrl.radius = 0.75f;
        this.characterCtrl.minMoveDistance = 0;

    }

    protected virtual void LoadCharBodyParts()
    {
        if (this.firearm != null) return;
        Transform animation = transform.Find("Animation");
        Transform body = animation.Find("Body");
        Transform upper = body.Find("Upper");
        Transform armR1 = upper.Find("ArmR[1]");
        Transform forearmR = armR1.Find("ForearmR");
        Transform handR = forearmR.Find("HandR");
        Transform tranFirearm = handR.Find("Firearm");

        this.armL = upper.Find("ArmL");
        this.armR = armR1;
        this.firearm = tranFirearm.GetComponent<Firearm>();

        this.firearmFire= tranFirearm.GetComponent <FirearmFire>();
        this.firearmFire.CreateBullets = true;
    }
}
