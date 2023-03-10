using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbility_CarryBodies : MonoBehaviour
{
    [SerializeField] List<BodyBag> _bodyBags = new();
    [SerializeField] int currEmptyBag = 0;
    public BodyBag _bodyBag;
    int valueControl = 0;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (valueControl == 0)
        {
            valueControl= 1;

            if (currEmptyBag >= _bodyBags.Count) return;

            if (hit.gameObject.CompareTag("Body"))
            {
                BoxCollider hitParentCollider = hit.transform.GetComponentInParent<PuppiesColliders>().transform.GetComponent<BoxCollider>();
                PuppiesColliders puppies = hitParentCollider.GetComponent<PuppiesColliders>();

                hit.transform.position = hitParentCollider.transform.position;
                hitParentCollider.transform.parent = _bodyBags[currEmptyBag].transform;
                hitParentCollider.transform.localPosition = new Vector3(0 - hitParentCollider.size.x / 2, 0, 0);
                hitParentCollider.transform.localRotation = Quaternion.Euler(90f, 0f, -90f);

                _bodyBags[currEmptyBag].ChangeBagFilledStatus();
                currEmptyBag++;

                puppies.DisablePuppyRagdoll(hit.transform);
            }
        }

        valueControl = 0;
    }

    public void AddBagBody(BodyBag bag, int index)
    {
        if (_bodyBags.Count == 0)
        {
            _bodyBags.Add(bag);
            bag.transform.SetParent(transform);
            bag.transform.localPosition = new Vector3(0, 1.5f, -0.6f);
            return;
        }

        BodyBag lastElement = _bodyBags[^1];
        _bodyBags.Add(bag);
        bag.transform.SetParent(lastElement.transform);
        bag.transform.localPosition = new Vector3(0, 0.5f, 0);
        bag.bagIndex = index;
        lastElement.moveEvent.AddListener(bag.Move);
    }

    public void DeleteBagBodies()
    {
        for (int i = 0; i < _bodyBags.Count; i++)
        {
            Destroy(_bodyBags[i].gameObject);
            _bodyBags.Remove(_bodyBags[i]);
        }

        _bodyBags.Clear();
    }

    public void StorageBodies(BodyStorage storage)
    {
        StartCoroutine(StorageBodiesCoroutine(storage));
    }

    IEnumerator StorageBodiesCoroutine(BodyStorage storage)
    {
        int lastIndex = _bodyBags.Count - 1;

        for (int i = lastIndex; i >= 0; i--)
        {
            if (!_bodyBags[i].bagFilled) continue;

            // fazer ir até a storage
            PuppiesColliders body = _bodyBags[i].GetComponentInChildren<PuppiesColliders>();
            body.transform.DOMove(storage.transform.position, .5f);
            body.transform.SetParent(null);
            storage.AddBodiesToList(body.transform);

            _bodyBags[i].ChangeBagFilledStatus();
            yield return new WaitForSeconds(.5f);
        }

        currEmptyBag = 0;

        CharacterLevelManager levelManager = GetComponent<CharacterLevelManager>();
        levelManager.InvokeLevelUp();
    }
}
