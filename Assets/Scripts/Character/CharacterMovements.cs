using System.Collections;
using UnityEngine;

public class CharacterMovements : CharacterInputsBase
{
    public static CharacterMovements Instance { get; private set; }

    public Vector3 movement;
    public Animator animator { get; private set; }
    CharacterController _controller;

    [Header("Movement Setup")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float rotationSpeed;
    float _currSpeed;

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
        _controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        // Trigger the run
        inputActions.Gameplay.Run.performed += ctx => _currSpeed = runSpeed;
        inputActions.Gameplay.Run.canceled += ctx => _currSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }


    void PlayerMovement()
    {
        //Set walk speed as default speed
        if (_currSpeed != runSpeed)
            _currSpeed = walkSpeed;

        // Character Movement
        Vector2 input = inputActions.Gameplay.Movement.ReadValue<Vector2>();
        movement = new Vector3(input.x, 0.0f, input.y);

        _controller.SimpleMove(_currSpeed * movement);

        //Character Rotation
        if (input != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Set animator state
        animator.SetBool("Walk", input != Vector2.zero);
        animator.SetBool("Run", input != Vector2.zero && _currSpeed == runSpeed);
    }

    
}
