using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class CharacterMovements : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] InputActions inputActions;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] Transform cameraTransform;

    [SerializeField] float currSpeed;

#if UNITY_EDITOR
    private void OnValidate()
    {
        inputActions = new InputActions();
    }
#endif

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inputActions = new InputActions();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        inputActions.Gameplay.Run.performed += ctx => currSpeed = runSpeed;
        inputActions.Gameplay.Run.canceled += ctx => currSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        if (currSpeed != runSpeed)
            currSpeed = walkSpeed;

        Vector2 input = playerInput.actions["Movement"].ReadValue<Vector2>();
        Vector3 movement = new Vector3(input.x, 0.0f, input.y);

        movement = movement.x * cameraTransform.right + movement.z * cameraTransform.forward;
        movement.y = 0.0f;

        controller.Move(currSpeed * Time.deltaTime * movement);

        if (input != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        
        animator.SetBool("Walk", input != Vector2.zero);
        animator.SetBool("Run", input != Vector2.zero && currSpeed == runSpeed);
    }
}
