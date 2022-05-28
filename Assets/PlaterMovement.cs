using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaterMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;
     bool doubleJump = true;

    Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
            doubleJump = true;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded && doubleJump)
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        else if(Input.GetButtonDown("Jump") && doubleJump){
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            doubleJump = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) ){
            velocity.y = -2f;
            gravity = -9.81f / 2;
        }
        else 
            gravity = -9.81f * 2;


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
