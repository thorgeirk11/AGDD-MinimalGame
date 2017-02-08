using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D body;
    private new CircleCollider2D collider;

    [ReadOnly]
    public float speed;
    [Range(1, 50)]
    public float minSpeed = 1;
    [Range(50, 100)]
    public float maxSpeed = 10;
    [ReadOnly]
    public bool hasBeenHitByWeapon;

    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
        body = GetComponent<Rigidbody2D>();

        speed = Random.Range(minSpeed, maxSpeed) * 100;
        body.AddForce(Vector2.up * speed * Time.deltaTime, ForceMode2D.Force);
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.tag == "Weapon")
        {
            Destroy(collider);
            body.gravityScale = 1;
            Destroy(gameObject, 1f);
        }
    }

}
