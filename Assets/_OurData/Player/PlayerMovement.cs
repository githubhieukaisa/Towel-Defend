using Assets.HeroEditor.Common.CharacterScripts;
using UnityEngine;

public class PlayerMovement : HieuMonoBehavior
{
    [Header("Components")]
    public Character character;
    public CharacterController charCtrl;
    [SerializeField] protected Transform myGround;

    [Header("Movement")]
    [SerializeField] protected float walkSpeed = 7;
    [SerializeField] protected float jumpSpeed = 9;
    [SerializeField] protected float fallingSpeed = 25;
    [SerializeField] protected float jumbMax = 2;
    [SerializeField] protected float jumbCount = 0;
    [SerializeField] protected bool isGrounded = true;
    [SerializeField] protected bool canJumb = false;
    [SerializeField] protected bool jumbing = false;

    [Header("Input")]
    [SerializeField] protected float inputHorizontal = 0f;
    [SerializeField] public float inputHorizontalRaw = 0f;
    [SerializeField] protected float inputVertical = 0f;
    [SerializeField] public float inputVerticalRaw = 0f;
    [SerializeField] public float inputJumbRaw = 0f;
    [SerializeField] protected bool pressJump = false;

    [Header("Layers")]
    [SerializeField] protected int layerHero = 3;
    [SerializeField] protected int layerGround = 6;
    [SerializeField] protected int layerCeiling = 7;


    [Header("Vectors")]
    [SerializeField] protected Vector3 mouseToChar = Vector3.zero;
    [SerializeField] protected Vector3 speed = Vector3.zero;
    [SerializeField] protected Vector2 direction;

    //public void Start()
    //{
    //    this.character.Animator.SetBool("Ready", true);
    //}
    private void Awake()
    {
        this.jumbCount = this.jumbMax;
        Physics.IgnoreLayerCollision(this.layerHero, this.layerCeiling, true);
    }

    public void Update()
    {
        this.GroundFinding();
        //this.IsGrounded();
        this.InputToDirection();
        this.IsGoingDown();
        this.CharacterStateUpdate();
        this.Turning();
        this.Move();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.GetPlayrers();
    }

    protected virtual void GetPlayrers()
    {
        if (this.layerHero > 0 && this.layerGround > 0 && this.layerCeiling > 0) return;
        this.layerHero = LayerMask.NameToLayer("Hero");
        this.layerGround = LayerMask.NameToLayer("Ground");
        this.layerCeiling = LayerMask.NameToLayer("Ceiling");
    }

    protected virtual void GroundFinding()
    {
        Vector3 direction = Vector3.down;
        Vector3 position = this.character.transform.position;
        Physics.Raycast(position, direction, out RaycastHit hit);
        this.DebugRaycast(position, hit, direction);
        if (hit.transform == null) return;

        Ground ground = hit.transform.GetComponent<Ground>();
        if (ground == null) return;
        if (this.myGround == hit.transform) return;

        // Chỉ log khi đổi layer và myGround mới
        ground.ChangeLayer(this.layerGround);
        this.myGround = hit.transform;
    }

    protected virtual Vector2 InputToDirection() 
    {         
        Vector2 direction = Vector2.zero;

        //this.inputHorizontal = Input.GetAxis("Horizontal");
        //this.inputVertical = Input.GetAxis("Vertical");

        //this.inputHorizontalRaw = Input.GetAxisRaw("Horizontal");
        //this.inputVerticalRaw = Input.GetAxisRaw("Vertical");
        //this.inputJumbRaw = Input.GetAxisRaw("Jump");
        
        direction.x = this.inputHorizontalRaw;
        direction.y = this.inputVerticalRaw;
        if(this.inputVerticalRaw == 0) direction.y = this.inputJumbRaw;

        if(direction.y >0) this.pressJump = true;
        else this.pressJump = false;
        this.direction= direction;
        return direction;
    }

    protected virtual bool IsGrounded()
    {
        this.isGrounded = this.charCtrl.isGrounded;

        if (this.isGrounded)
        {
            this.jumbCount = this.jumbMax;
            this.canJumb = true;
            this.jumbing = false;
        }
        return this.isGrounded;
    }
    protected virtual void Turning()
    {
        Vector3 mouse = Input.mousePosition;
        Vector3 vec3 = new Vector3(mouse.x, mouse.y, this.character.transform.position.y);
        Vector3 mouseWorld=Camera.main.ScreenToWorldPoint(vec3);
        this.mouseToChar= mouseWorld-this.character.transform.position;
        character.transform.localScale = new Vector3(Mathf.Sign(this.mouseToChar.x), 1, 1);
    }

    protected virtual void CharacterStateUpdate()
    {
        if (this.direction != Vector2.zero) this.character.SetState(CharacterState.Run);
        else if(this.character.GetState() < CharacterState.DeathB) this.character.SetState(CharacterState.Idle);
        if(!this.IsGrounded()) this.character.SetState(CharacterState.Jump);
    }

    public void Move()
    {
        this.Walking();
        this.Jumbling();

        this.speed.y -= this.fallingSpeed * Time.deltaTime;
        this.charCtrl.Move(this.speed * Time.deltaTime);
    }

    protected virtual void Walking()
    {
        if(!this.isGrounded) return;
        this.speed.x = this.direction.x * this.walkSpeed;
    }

    protected virtual void Jumbling()
    {
        if(this.jumbing && !this.pressJump)
        {
            this.jumbing = false;
            this.canJumb = true;
            this.jumbCount--;
        }

        if(!this.pressJump || !this.canJumb || this.jumbCount <= 0) return;

        this.jumbing = true;
        this.canJumb = false;

        this.speed.y= this.jumpSpeed* this.direction.y;
    }

    protected virtual bool IsGoingDown()
    {
        bool isGoingDown = this.direction.y < 0;
        if (isGoingDown) this.ResetMyGround();
        return isGoingDown;
    }

    public virtual void ResetMyGround()
    {
        if (this.myGround == null) return;
        if (this.myGround.name == "GroundGrid0") return;
        this.myGround.GetComponent<Ground>().ChangeLayer(this.layerCeiling);
        this.myGround = null;
    }
}
