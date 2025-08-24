using Assets.HeroEditor.Common.ExampleScripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : HieuMonoBehavior
{
    [Header("Player")]
    public static PlayerManager instance;
    public HeroCtrl currentHero; 
    public PlayerInput playerInput;
    public PlayerAttacking playerAttacking;
    public EquipmentExample equipmentExample;
    public PlayerMovement playerMovement;
    public BowExample bowExample;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("There is already an instance of PlayerManager in the scene. Destroying this one.");
        }
    }
    public void Start()
    {
        this.LoadPlayers();
    }

    protected override void LoadComponents()
    {
        this.LoadPlayerComponents();
    }
    
    protected virtual void LoadPlayerComponents()
    {
        if (playerAttacking != null) return;
        this.playerAttacking=transform.GetComponentInChildren<PlayerAttacking>();
        this.playerMovement=transform.GetComponentInChildren<PlayerMovement>();
        this.bowExample=transform.GetComponentInChildren<BowExample>();
        this.equipmentExample=transform.GetComponentInChildren<EquipmentExample>();
        this.playerInput= transform.GetComponentInChildren<PlayerInput>();

        Debug.Log(transform.name + ": LoadPlayerComponents");

    }

    protected virtual void LoadPlayers()
    {
        GameObject hero;
        Vector3 vector3 = transform.position;
        vector3.x -= 7;
        foreach (HeroesManager heroManager in HeroManagers.instance.heroManagers)
        {
            vector3.x += 3;
            hero = heroManager.GetHero();
            hero.transform.position = vector3;
            hero.transform.SetParent(PlayersHolder.instance.transform);

            HeroCtrl heroCtrl = hero.GetComponent<HeroCtrl>();
            PlayersHolder.instance.heroCtrls.Add(heroCtrl);

            this.SetPlayerCtrl(hero);
        }
    }

    public virtual void SetPlayerCtrl(GameObject obj)
    {
        this.currentHero = obj.GetComponent<HeroCtrl>();

        this.equipmentExample.character = this.currentHero.character;
        this.bowExample.character = this.currentHero.character;

        this.playerMovement.character = this.currentHero.character;
        this.playerMovement.charCtrl = this.currentHero.characterCtrl;
        this.playerMovement.ResetMyGround();

        this.playerAttacking.character = this.currentHero.character;
        this.playerAttacking.bowExample = this.bowExample;
        this.playerAttacking.firearm = this.currentHero.firearm;
        this.playerAttacking.armL = this.currentHero.armL;
        this.playerAttacking.armR = this.currentHero.armR;
    }

    public virtual bool ChoosePlyer(string chooseClass)
    {
        string profileClass;
        foreach(HeroCtrl heroCtrl in PlayersHolder.instance.heroCtrls)
        {
            profileClass = heroCtrl.heroProfile.HeroClass();
            if(profileClass == chooseClass)
            {
                this.SetPlayerCtrl(heroCtrl.gameObject);
                return true;
            }
        }

        return false;
    }
}
