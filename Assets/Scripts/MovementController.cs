using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class MovementController : MonoBehaviour
{
    InputAction moveAction;
    Vector2 moveInput;

    [SerializeField]
    CharacterController controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(moveInput.x, 0, moveInput.y) * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}