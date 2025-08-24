using System.Collections.Generic;
using UnityEngine;

public class Bullet : HieuMonoBehavior
{
    [Header("References")]
    public List<Renderer> Renderers;
    [SerializeField] protected GameObject trail;
    [SerializeField] protected GameObject impact;
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected SpriteRenderer spriterenderer;
    [SerializeField] protected Collider _collider;

    [Header("Despawn")]
    [SerializeField] protected bool isDespawn = false;
    [SerializeField] protected float despawnTimer = 0;
    [SerializeField] protected float despawnDelay = 1f;

    [Header("Raycast")]
    [SerializeField] public Vector3 lastPosition;
    [SerializeField] protected Transform hitObject;
    [SerializeField] protected LayerMask nonBlockMask;

    private void OnEnable()
    {
        BulletRenew();
        CancelInvoke();
        Invoke(nameof(Despawn), despawnDelay);
        lastPosition = transform.position;
    }
    protected override void LoadComponents()
    {
        this._rigidbody = GetComponent<Rigidbody>();
        this.spriterenderer = GetComponent<SpriteRenderer>();
        this._collider = GetComponent<Collider>();
        if (this.trail == null)
        {
            this.trail = transform.Find("Trail").gameObject;
        }
        if (this.impact == null)
        {
            this.impact = transform.Find("Impact").gameObject;
        }
        if (this.Renderers == null || this.Renderers.Count == 0)
        {
            this.Renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
        }
        if (this.nonBlockMask == 0)
        {
            this.nonBlockMask = LayerMask.GetMask("Hero", "Ceiling");
        }
    }
    public void Update()
    {
        if (_rigidbody != null && _rigidbody.useGravity)
        {
            transform.right = _rigidbody.linearVelocity.normalized;
        }
    }

    private void FixedUpdate()
    {
        this.Raycasting();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (IsBlockLayer(other.gameObject.layer))
        {
            Bang(other.gameObject);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (IsBlockLayer(other.gameObject.layer))
        {
            Bang(other.gameObject);
        }
    }

    private void Bang(GameObject other)
    {
        this.ReplaceImpactSound(other);
        this.BulletHit();

        foreach (var ps in trail.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Stop();
        }

        this.TrailStatus(false);
    }

    protected virtual void TrailStatus(bool status)
    {
        foreach(var tr in trail.GetComponentsInChildren<TrailRenderer>())
        {
            tr.enabled = status;
        }
    }

    protected virtual void Raycasting()
    {
        Vector3 direction = (transform.position - this.lastPosition).normalized;
        Vector3 position = transform.position;
        Physics.Raycast(position, direction, out RaycastHit hit);
        this.DebugRaycast(position, hit, direction);
        this.lastPosition = transform.position;

        if (hit.transform == null) return;

        int hitLayer = hit.transform.gameObject.layer;
        //Debug.Log($"{transform.name} raycast hit {hit.transform.name} at {hit.point}, layer: {LayerMask.LayerToName(hitLayer)}");

        Collider hitCollider = hit.transform.GetComponent<Collider>();
        Physics.IgnoreCollision(hitCollider, this._collider, true);

        if (!IsBlockLayer(hitLayer)) return;

        Physics.IgnoreCollision(hitCollider, this._collider, false);
        this.hitObject = hit.transform;
        //Debug.Log($"{transform.name} hit {hitObject.name} at {hit.point}");

    }

    private void ReplaceImpactSound(GameObject other)
    {
        var sound = other.GetComponent<AudioSource>();

        if (sound != null && sound.clip != null)
        {
            impact.GetComponent<AudioSource>().clip = sound.clip;
        }
    }

    protected virtual void BulletHit()
    {
        this.impact.SetActive(true);
        this.spriterenderer.enabled = false;
        this._collider.enabled = false;
        this._rigidbody.isKinematic = true;
        this.isDespawn = true;
    }

    protected virtual void Despawn()
    {
        //if(!this.isDesapwn) return;
        //this.despawnTimer += Time.fixedDeltaTime;
        //if(this.despawnTimer < this.despawnDelay) return;

        ObjPoolManager.instance.Despawn(transform);
    }

    public virtual void BulletRenew()
    {
        this.impact.SetActive(false);
        this.spriterenderer.enabled = true;
        this._collider.enabled = true;
        this._rigidbody.isKinematic = false;
        this.isDespawn = false;
        this.despawnTimer = 0;

        this.TrailStatus(true);
    }

    private bool IsBlockLayer(int layer)
    {
        return (nonBlockMask.value & (1 << layer)) == 0;
    }
}
