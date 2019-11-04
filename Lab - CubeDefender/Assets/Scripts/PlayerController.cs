﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject head;
    [SerializeField] private float maxVelocity = 0;
    [SerializeField] private float jumpForce = 0;
    [SerializeField] private float mouseSensitivity = 0;

    private Rigidbody rb;
    private bool canMove = true;
    private bool canJump = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && rb)
        {
            ProcessPlayerInput();
        }
    }

    void ProcessPlayerInput()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        bool jump = Input.GetButtonDown("Jump");

        // horizontal movement
        Vector2 hInput = (new Vector2(transform.forward.x, transform.forward.z) * zInput +
            new Vector2(transform.right.x, transform.right.z) * xInput);
        Vector2 hVelocity = (hInput.magnitude > 1 ? hInput.normalized : hInput) * maxVelocity * Time.deltaTime;

        rb.velocity = new Vector3(hVelocity.x, rb.velocity.y, hVelocity.y);

        // rotate player & camera
        rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0, mouseX * mouseSensitivity * Time.deltaTime, 0));

        if (head)
        {
            //TODO: contraint head movement
            head.transform.Rotate(-mouseY * mouseSensitivity * Time.deltaTime, 0, 0, Space.Self);
        }

        // jumping
        if (jump && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            canJump = true;
        }
    }
}
