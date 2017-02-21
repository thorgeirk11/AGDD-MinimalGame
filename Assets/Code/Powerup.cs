using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    public float speed = 1;

    private const float FadeDuration = 0.3f;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    private new CircleCollider2D collider;
    private bool Collected;
    private float CollectedTime;

    public bool IsVisible
    {
        get { return spriteRenderer.isVisible; }
    }

    protected virtual void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (GameManager.GameOver) return;

        body.velocity = Vector2.up * speed;
        if (Collected)
        {
            var alpha = Mathf.Lerp(0, FadeDuration, Time.time - CollectedTime);
            if (Time.time - CollectedTime > FadeDuration)
                Destroy(gameObject);
            else
            {
                spriteRenderer.color = new Color(1, 1, 1, 1 - alpha);
            }
        }
        if (!IsVisible) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collidier)
    {
        if (collidier.gameObject.tag == "Weapon")
        {
            OnPickup();
            var weapon = collidier.gameObject.GetComponentInParent<Weapon>();
            SoundManager.Instance.PlayCollisionSound();
            body.constraints = RigidbodyConstraints2D.FreezeAll;
            CollectedTime = Time.time;
            Collected = true;
        }
    }

    protected abstract void OnPickup();
}
