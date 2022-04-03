using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementAndCamera : MonoBehaviour
{
    [SerializeField]
    private InputActionReference leftClick;
    [SerializeField]
    private InputActionReference rightClick;
    [SerializeField]
    private InputActionReference movementControl;
    [SerializeField]
    private InputActionReference jumpControl;
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float gravityValue;
    [SerializeField]
    private float rotationSpeed = 4f;

    public GameObject cmFreeLookCamera;

    private CinemachineFreeLook cmFreeLook;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraMainTransform;
    private Vector2 movement;
    private Vector2 airMovement;
    private Vector3 airFacingForward;
    private Vector3 airFacingRight;
    private Vector3 move;

    private void OnEnable()
    {
        rightClick.action.Enable();
        movementControl.action.Enable();
        jumpControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
    }

    void Update()
    {
        if (cmFreeLook == null)
        {
            cmFreeLook = cmFreeLookCamera.GetComponent<CinemachineFreeLook>();
        }

        CheckForCameraMovement();

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (groundedPlayer)
        {
            movement = movementControl.action.ReadValue<Vector2>();

            if(Mathf.Approximately(leftClick.action.ReadValue<float>(), 1) && Mathf.Approximately(rightClick.action.ReadValue<float>(), 1))
            {
                movement.y = 1;
            }

            move = new Vector3(movement.x, 0, movement.y);
        }
        else
        {
             move = new Vector3(airMovement.x, 0, airMovement.y);
        }

        move.y = 0f;

        // Use Air facing if you are airborne to preserve momentum
        if (groundedPlayer)
            move = controller.transform.forward * move.z + controller.transform.right * move.x;
        else
            move = airFacingForward * move.z + airFacingRight * move.x;

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Mathf.Approximately(rightClick.action.ReadValue<float>(), 1))
        {
            Vector3 rotation = cameraMainTransform.forward;
            gameObject.transform.rotation = Quaternion.LookRotation(rotation);
        }

        // Changes the height position of the player..
        if (jumpControl.action.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            
            //Preserve air movement and facing to preserve momentum
            airMovement = movement;
            airFacingForward = controller.transform.forward;
            airFacingRight = controller.transform.right;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void CheckForCameraMovement()
    {
        if (Mathf.Approximately(leftClick.action.ReadValue<float>(), 1) || Mathf.Approximately(rightClick.action.ReadValue<float>(), 1))
        {
            cmFreeLook.m_XAxis.m_MaxSpeed = 0.20f;
        }
        else
        {
            cmFreeLook.m_XAxis.m_MaxSpeed = 0;
        }
    }
}
