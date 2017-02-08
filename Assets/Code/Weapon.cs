using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    private const int AngularDragToSlowDown = 20;
    public HingeJoint2D ropeJointPrefab;
    public HingeJoint2D endPrefab;

    private Vector2 origin;
    private Rigidbody2D body;
    private int chainCount;

    public bool isDropping { get; private set; }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    internal void SetOrigin(Vector2 originPoint)
    {
        transform.position = origin = originPoint;
    }

    internal void SetEnd(Vector2 end)
    {
        var distance = (origin - end).magnitude;
        var size = 0f;
        var last = GetComponent<Rigidbody2D>();
        chainCount = 0;
        for (float i = 0; i < distance - size; i += size)
        {
            var joint = Instantiate(ropeJointPrefab);
            size = joint.GetComponent<Collider2D>().bounds.size.x;
            joint.connectedBody = last;
            joint.transform.SetParent(transform, false);
            joint.transform.position = origin.x < end.x ?
                                           last.position + new Vector2(size, 0) :
                                           last.position - new Vector2(size, 0);
            last = joint.GetComponent<Rigidbody2D>();
            chainCount++;
        }
        var weaponEnd = Instantiate(endPrefab);
        weaponEnd.connectedBody = last;
        weaponEnd.transform.SetParent(transform);
        weaponEnd.transform.position = end;

        if (chainCount < 2)
        {
            DropWeapon();
        }
        else
        {
            StartCoroutine(MakeStiff(weaponEnd));
        }
    }

    private IEnumerator MakeStiff(HingeJoint2D weaponEnd)
    {
        var trans = weaponEnd.transform;
        //Wait for it to drop.
        if (trans.position.x > origin.x)
            yield return new WaitUntil(() => isDropping || trans.position.x < origin.x);
        else
            yield return new WaitUntil(() => isDropping || trans.position.x > origin.x);

        if (isDropping) yield break;
        // Slow it down.
        var cur = weaponEnd;
        while (cur != null)
        {
            cur.connectedBody.angularDrag = AngularDragToSlowDown;
            cur = cur.connectedBody.GetComponent<HingeJoint2D>();
        }
        var endVelocity = weaponEnd.GetComponent<Rigidbody2D>();

        for (int i = 0; i < 2; i++)
        {
            yield return new WaitUntil(() => isDropping || endVelocity.velocity.magnitude < 0.4);
            yield return new WaitUntil(() => isDropping || endVelocity.velocity.magnitude > 0.4);
        }
        DropWeapon();
    }
    public void DropWeapon()
    {
        if (isDropping) return;
        isDropping = true;
        StartCoroutine(DropWeaponCor());
    }
    private IEnumerator DropWeaponCor()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        // Pull the rope up.
        var ropeJoints = GetComponentsInChildren<HingeJoint2D>();
        for (int i = 1; i < ropeJoints.Length - 1; i++)
        {
            //ropeJoints[i].connectedBody = body;
            Destroy(ropeJoints[i - 1].gameObject);
            yield return new WaitForSeconds(.06f);
        }
        if (ropeJoints.Length > 1)
        {
            var endRenderer = ropeJoints[ropeJoints.Length - 1].GetComponent<SpriteRenderer>();
            yield return new WaitWhile(() => endRenderer.isVisible);
        }
        Destroy(gameObject);
    }
}
