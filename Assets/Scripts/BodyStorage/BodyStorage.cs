using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BodyStorage : MonoBehaviour
{
    [SerializeField] List<Transform> storageBodies = new();

    [HideInInspector] public UnityEvent<int> bodiesCountEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterAbility_CarryBodies bodies = other.GetComponent<CharacterAbility_CarryBodies>();
            bodies.StorageBodies(this);
        }
    }

    public void AddBodiesToList(Transform body)
    {
        // montar lista pra armazenar e contar a qtdd de corpos
        storageBodies.Add(body);

        int bodiesCount = storageBodies.Count;
        bodiesCountEvent.Invoke(bodiesCount);
    }
}
