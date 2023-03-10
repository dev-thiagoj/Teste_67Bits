using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class CharacterAbility_Punch : CharacterInputsBase
{
    [Header("Punch Setup")]
    [SerializeField] float punchForce;
    [SerializeField] float punchCooldown;
    [SerializeField] LayerMask enemyLayer;
    float _punchTimer;

    [Header("Punch Effect")]
    [SerializeField] float animScale;
    [SerializeField] float AnimDuration;
    [SerializeField] Ease easeMode;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Trigger punch
        inputActions.Gameplay.Punch.performed += ctx => Punch();
    }

    // Update is called once per frame
    void Update()
    {
        //Punch Cooldown timer
        if (_punchTimer > 0)
            _punchTimer -= Time.deltaTime;

        //Debug
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1,
            transform.position.z), transform.forward * 2, Color.magenta);
    }

    void Punch()
    {
        StartCoroutine(Punch_Coroutine());
    }

    IEnumerator Punch_Coroutine()
    {
        // Start punch cooldown timer
        _punchTimer = punchCooldown;

        CharacterMovements.Instance.animator.SetTrigger("Punch");
        yield return new WaitForSeconds(.2f);
        transform.DOScale(animScale, AnimDuration).SetLoops(2, LoopType.Yoyo).SetEase(easeMode);
        yield return new WaitForSeconds(.3f);

        //Ray to find target
        if(Physics.Raycast(transform.position, transform.forward * 2, out RaycastHit hit, enemyLayer) &&
        hit.transform.CompareTag("Puppies"))
        {
            PuppiesColliders puppies = hit.transform.GetComponentInParent<PuppiesColliders>();
            Vector3 direction = hit.transform.position - transform.position; 
            puppies.AddPunchForce(punchForce, direction);
        }
    }
}
