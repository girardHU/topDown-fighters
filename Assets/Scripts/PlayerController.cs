using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 15.0f; // Control the speed of rotation
    public float movementSpeed = 250.0f;
    public float slowingFactor = 1.1f;

    private float attackDuration = 0.5f;
    private Quaternion targetRotation;
    private Vector3 movement;

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = gameObject.GetComponentInChildren<Animator>();
        targetRotation = transform.rotation; // Initialize with current rotation
    }

    void Update()
    {
        movement = Vector3.zero;

        // Detect key presses and update the target rotation
        if (Input.GetKey(KeyCode.W))
        {
            targetRotation = Quaternion.LookRotation(Vector3.forward);
            movement = Vector3.forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            targetRotation = Quaternion.LookRotation(Vector3.back);
            movement = Vector3.back;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            targetRotation = Quaternion.LookRotation(Vector3.left);
            movement = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            targetRotation = Quaternion.LookRotation(Vector3.right);
            movement = Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.End))
        {
            Kill();
        }

        HandleRotation();

        HandleMovement();


        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }

    }

    private void HandleRotation()
    {
        if (!CheckAngle(targetRotation, 5.0f))
        {
            if (rb.velocity == Vector3.zero)
            {
                rb.velocity /= slowingFactor;
            }

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void HandleMovement()
    {
        if (CheckAngle(targetRotation, 10.0f) && !animator.GetBool("IsSlashing_b"))
        {
            rb.velocity = movementSpeed * Time.deltaTime * movement;
        }

        // Change Boolean for running animation
        animator.SetBool("IsRunning_b", movement != Vector3.zero && CheckAngle(targetRotation, 5.0f));

    }

    private void Attack()
    {
        animator.SetBool("IsSlashing_b", true);
        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackDuration);
        animator.SetBool("IsSlashing_b", false);
    }

    private bool CheckAngle(Quaternion referenceAngle, float threshold)
    {
        return Quaternion.Angle(transform.rotation, referenceAngle) < threshold;

    }

    private void Kill()
    {
        animator.SetBool("IsDead_b", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (!other.transform.IsChildOf(transform) && other.CompareTag("Player Body"))
        {
            Kill();
        }
    }
}