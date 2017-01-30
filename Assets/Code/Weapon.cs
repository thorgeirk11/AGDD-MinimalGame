using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState
{
    Idle,
    GoingActive,
    Active,
    GoingIdle,
}

public class Weapon : MonoBehaviour
{

    public WeaponState State;

    public HingeJoint2D ropeJointPrefab;
    public HingeJoint2D endPrefab;

    private Vector2 origin;

    public bool CanBeDropped { get { return State == WeaponState.Idle; } }


    internal void SetOrigin(Vector2 originPoint)
    {
        transform.position = origin = originPoint;
    }

    internal void SetEnd(Vector2 end)
    {
        var distance = (origin - end).magnitude;
        var size = 0f;
        var last = GetComponent<Rigidbody2D>();
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
        }
        var weaponEnd = Instantiate(endPrefab);
        weaponEnd.connectedBody = last;
        weaponEnd.transform.SetParent(transform);
        weaponEnd.transform.position = end;
    }
}
