using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    private const int AngularDragToSlowDown = 20;
    public HingeJoint2D ropeJointPrefab;
    public HingeJoint2D endPrefab;

    public Vector2 Origin { get; private set; }
    private Rigidbody2D body;
    private int chainCount;
    public float hitCount;

    public Color InActiveColor;
    public Color ActiveColor;

    private List<Rigidbody2D> chain = new List<Rigidbody2D>();
    private HingeJoint2D weaponEnd;
    private float scaleEndTime;
    private float defaultEndSize;

    public bool isDropping { get; private set; }

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    internal void SetOrigin(Vector2 originPoint)
    {
        transform.position = Origin = originPoint;
    }

    public void ScaleEnd(float modifyer, float modifyerTime)
    {
        weaponEnd.transform.localScale = Vector3.one * defaultEndSize * modifyer;
        scaleEndTime = modifyerTime;
    }

    private void Update()
    {
        if (Time.time > scaleEndTime)
        {
            ScaleEnd(1, Time.time + 10000);
        }
    }

    internal void SetEnd(Vector2 end)
    {
        if (isDropping) return;

        for (int i = 0; i < chain.Count; i++)
        {
            Destroy(chain[i].gameObject);
        }
        chain.Clear();

        var distance = Vector2.Distance(Origin, end);
        var size = 0f;
        var last = body;
        chainCount = 0;
        for (float i = 0; i < distance - size; i += size)
        {
            var joint = Instantiate(ropeJointPrefab);
            size = joint.GetComponent<Collider2D>().bounds.size.x;
            joint.connectedBody = last;
            joint.transform.SetParent(transform, false);
            joint.transform.position = Origin.x < end.x ?
                                           last.position + new Vector2(size, 0) :
                                           last.position - new Vector2(size, 0);
            last = joint.GetComponent<Rigidbody2D>();
            chain.Add(last);
            chainCount++;
        }
        weaponEnd = Instantiate(endPrefab);
        weaponEnd.connectedBody = last;
        weaponEnd.transform.SetParent(transform);
        weaponEnd.transform.position = end;
        weaponEnd.GetComponent<Collider2D>().enabled = false;
        defaultEndSize = weaponEnd.transform.localScale.x;
        chain.Add(weaponEnd.GetComponent<Rigidbody2D>());

        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
        {
            item.color = InActiveColor;
        }
        foreach (var r in chain)
            r.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    public void ReleaseWeapon()
    {
        weaponEnd.GetComponent<Collider2D>().enabled = true;
        foreach (var r in chain)
            r.constraints = RigidbodyConstraints2D.None;
        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
        {
            item.color = ActiveColor;
        }
        StartCoroutine(MakeStiff());
    }

    public IEnumerator MakeStiff()
    {
        var trans = weaponEnd.transform;
        //Wait for it to drop.
        if (trans.position.x > Origin.x)
            yield return new WaitUntil(() => isDropping || trans.position.x < Origin.x);
        else
            yield return new WaitUntil(() => isDropping || trans.position.x > Origin.x);

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
        weaponEnd.GetComponent<Collider2D>().enabled = false;

        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
        {
            item.color = InActiveColor;
            item.sortingOrder = -1;
        }
        isDropping = true;
        StartCoroutine(DropWeaponCor());
    }
    private IEnumerator DropWeaponCor()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        // Pull the rope up.
        var ropeJoints = GetComponentsInChildren<HingeJoint2D>();
        for (int i = 0; i < ropeJoints.Length - 1; i++)
        {
            //ropeJoints[i].connectedBody = body;
            Destroy(ropeJoints[i].gameObject);
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
