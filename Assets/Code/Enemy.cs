using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private Rigidbody2D body;
    private new CircleCollider2D collider;
    private Random rand;

    [Range(1, 1000)]
    public float speed;
    [Range(1, 10)]
    public float minWait;
    [Range(1, 10)]
    public float maxWait;
    public bool hasBeenHitByWeapon;


	void Start () {
        collider = GetComponent<CircleCollider2D>();
        body = GetComponent<Rigidbody2D>();
        rand = new Random(); 

        StartCoroutine(Clime());
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.tag == "Weapon")
        {
            hasBeenHitByWeapon = true;
        }
    }

    IEnumerator Clime() {
        while (!hasBeenHitByWeapon)
        {
            body.AddForce(Vector2.up * speed * Time.deltaTime, ForceMode2D.Force);
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
        }
    }
}
