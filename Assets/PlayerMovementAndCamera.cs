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
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 4f;

    public GameObject cmFreeLookCamera;

    private CinemachineFreeLook cmFreeLook;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Quaternion playerFacing;
    private Vector3 playerFacingForward;
    private bool groundedPlayer;
    private Transform cameraMainTransform;

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

        playerFacingForward = controller.transform.forward;

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);

        // print($"output1: {move.x}, {move.z}, -- {controller.transform.forward.x}, {controller.transform.forward.z}");
        // print($"{controller.transform.localRotation.ToString()} -- {controller.transform.rotation.ToString()}");

        move.y = 0f;

        // Movement
        if (Mathf.Approximately(rightClick.action.ReadValue<float>(), 1))
        {
            //move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
            //controller.Move(move * Time.deltaTime * playerSpeed);

            //print($"output1: {controller.transform.forward.ToString()}, {controller.transform.right.ToString() }, -- {controller.transform.rotation.ToString()}, {controller.transform.localRotation.ToString()}");
            //move = controller.transform.forward * move.z + controller.transform.right * move.x;
            //controller.Move(move * Time.deltaTime * playerSpeed);
        }
        else
        {
            //print($"output1: {controller.transform.forward.ToString()}, {controller.transform.right.ToString() }, -- {controller.transform.rotation.ToString()}, {controller.transform.localRotation.ToString()}");
            //move = controller.transform.forward * move.z + controller.transform.right * move.x;
            //controller.Move(move * Time.deltaTime * playerSpeed);
        }

        print($"output1: {controller.transform.forward.ToString()}, {controller.transform.right.ToString() }, -- {controller.transform.rotation.ToString()}, {controller.transform.localRotation.ToString()}");
        move = controller.transform.forward * move.z + controller.transform.right * move.x;
        controller.Move(move * Time.deltaTime * playerSpeed);

        //// Facing
        //if (move != Vector3.zero)
        //{
            
        //    else
        //    {

        //    }
        //}

        if (Mathf.Approximately(rightClick.action.ReadValue<float>(), 1))
        {
            Vector3 rotation = cameraMainTransform.forward;
            gameObject.transform.rotation = Quaternion.LookRotation(rotation);
        }

        // Changes the height position of the player..
        if (jumpControl.action.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //if(movement != Vector2.zero)
        //{
        //    float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
        //    Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        //}
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
