using UnityEngine;

public class EnemyMovement : HieuMonoBehavior
{
    [Header("Enemy")]
    [SerializeField] protected EnemyCtrl enemyCtrl;
    [SerializeField] protected Transform target;
    [SerializeField] protected float speed= 2f;
    [SerializeField] protected Vector3 direction= new Vector3(0,0,0);

    private void Update()
    {
        this.Moving();        
    }
    private void FixedUpdate()
    {
        this.Turning();
    }

    private void OnEnable()
    {
        this.OnReview();
    }
    
    protected virtual void OnReview()
    {
        this.enemyCtrl._rigidbody.linearVelocity= Vector3.zero;
        this.enemyCtrl._rigidbody.angularVelocity= Vector3.zero;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyCtrl();
    }

    protected virtual void LoadEnemyCtrl()
    {
        if (this.enemyCtrl != null) return;
        this.enemyCtrl = transform.parent.GetComponent<EnemyCtrl>();
        Debug.Log($"{this.name}: LoadEnemyCtrl");
    }

    protected virtual void Moving()
    {
        if (!this.IstargetActive()) return;

        Vector3 tempVec= this.GetDirection() * Time.deltaTime * this.speed;
        this.enemyCtrl._rigidbody.MovePosition(transform.position + tempVec);
    }

    protected virtual Vector3 GetDirection()
    {
        this.direction.x = 0;
        if (this.target.position.x > transform.position.x) this.direction.x = 1;
        else if(this.target.position.x< transform.position.x) this.direction.x = -1;
        return this.direction;
    }

    protected virtual bool IstargetActive()
    {
        if(this.target == null) return false;
        return true;
    }

    protected virtual void Turning()
    {
        if(this.direction.x == 0) return;
        Vector3 newScale= this.enemyCtrl.enemy.transform.localScale;
        newScale.x = Mathf.Abs(newScale.x);
        newScale.x *= this.direction.x * -1; //Todo need optimize

        this.enemyCtrl.enemy.transform.localScale = newScale;
    }

    public virtual void SetTarget(Transform target)
    {
        this.target = target;
    }
}
