using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D body;
    private new CircleCollider2D collider;

    public bool HitByWeapon { get; private set; }

    private SpriteRenderer spriteRenderer;

    public float SpeedModifier;
    private float _speed;

    public bool IsVisible
    {
        get { return spriteRenderer.isVisible; }
    }

    void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartMoving(float speed)
    {
        _speed = speed;
    }

    void Update()
    {
        if (GameManager.GameOver) return;

        if (!HitByWeapon)
        {
            body.velocity = Vector2.up * _speed * SpeedModifier;
        }
        if (!HitByWeapon && !IsVisible)
        {
            ScoreSystem.Instance.EnemyHitDefence(this);
        }

        if (!IsVisible) Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (!HitByWeapon && hit.gameObject.tag == "Weapon")
        {
            HitByWeapon = true;
            var weapon = hit.gameObject.GetComponentInParent<Weapon>();
            SoundManager.Instance.PlayCollisionSound();
            ScoreSystem.Instance.WeaponHitEnemy(this, weapon, hit);
            GetComponent<TrailRenderer>().enabled = true;
            //Destroy(collider);
            gameObject.layer = 9;
            body.gravityScale = 1;
            //Destroy(gameObject, 1f);
        }
    }


}
