using DG.Tweening;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppiesColliders : MonoBehaviour
{
    [SerializeField] Animator animator;
    public Collider mainCollider;
    public Rigidbody mainRB;
    public Rigidbody[] rbs;
    public Collider[] colls;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        colls = GetComponentsInChildren<Collider>();
        mainCollider = colls[0];
        mainRB = rbs[0];
    }

    public void AddPunchForce(float force, Vector3 direction)
    {
        Vector3 dir = -direction.normalized;
        transform.DOMove(transform.position - dir * force, 1);
        EnablePuppyRagdoll();
    }

    public void EnablePuppyRagdoll()
    {
        for (int i = 0; i < colls.Length; i++)
        {
            colls[i].enabled = true;
        }

        animator.enabled = false;
        mainCollider.isTrigger = true;
    }

    public void DisablePuppyRagdoll(Transform child)
    {
        child.transform.SetPositionAndRotation(transform.position, transform.rotation);

        for (int i = 0; i < colls.Length; i++)
        {
            colls[i].enabled = false;
        }
        //mainCollider.enabled = true;
        animator.enabled = true;
    }


}
