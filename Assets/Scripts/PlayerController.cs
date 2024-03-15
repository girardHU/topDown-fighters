using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 200.0f;
    public float turnSpeed = 1000.0f;
    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        // Get the Rigidbody component from the GameObject
        rb = GetComponent<Rigidbody>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    void Update()
    {

        // Get input from WASD keys
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // Create a Vector3 movement
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // rotate the player in the correct direction according to WASD
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }

        // Apply the movement to the Rigidbody
        rb.AddForce(movementSpeed * movement);
        rb.velocity = movementSpeed * Time.deltaTime * movement;

        animator.SetFloat("Speed_f", movementSpeed * Mathf.Abs(moveHorizontal) * Mathf.Abs(moveVertical) * 10.0f);
    }
}