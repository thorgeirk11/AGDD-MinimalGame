using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D body;
    private new CircleCollider2D collider;

    private bool hitByWeapon;

    private SpriteRenderer spriteRenderer;
    public bool IsVisible
    {
        get { return spriteRenderer.isVisible; }
    }

    public float speed;
    public float minSpeed = 5;
    public float maxSpeed = 10;

    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        speed = Random.Range(minSpeed, maxSpeed);
        body.AddForce(Vector2.up * speed, ForceMode2D.Force);
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
            ScoreSystem.Instance.WeaponHitEnemy(this, weapon, hit);

            Destroy(collider);
            body.gravityScale = 1;
            Destroy(gameObject, 1f);
        }
    }


}
