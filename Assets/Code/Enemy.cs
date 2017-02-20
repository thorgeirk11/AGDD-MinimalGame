using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D body;
    private new CircleCollider2D collider;

    private bool hitByWeapon;

    private SpriteRenderer spriteRenderer;

    public float SpeedModifier;

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
        body.velocity = Vector2.up * speed * SpeedModifier;
    }

    void Update()
    {
        if (!hitByWeapon && !IsVisible)
        {
            ScoreSystem.Instance.EnemyHitDefence(this);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.tag == "Weapon")
        {
            hitByWeapon = true;
            var weapon = hit.gameObject.GetComponentInParent<Weapon>();
            SoundManager.Instance.PlayCollisionSound();
            ScoreSystem.Instance.WeaponHitEnemy(this, weapon, hit);
            
            Destroy(collider);
            body.gravityScale = 1;
            Destroy(gameObject, 1f);
        }
    }


}
