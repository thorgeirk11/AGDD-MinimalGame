using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D body;
    [Range(1,100)]
    public float Speed;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("Up");
            body.AddForce(Vector2.up * Time.deltaTime * Speed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("Down");
            body.AddForce(Vector2.down * Time.deltaTime * Speed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            body.AddForce(Vector2.left * Time.deltaTime * Speed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            body.AddForce(Vector2.right * Time.deltaTime * Speed);
        }
    }
}
