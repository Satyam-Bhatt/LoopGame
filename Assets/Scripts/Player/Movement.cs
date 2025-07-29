using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float Maxspeed = 0f;
    [SerializeField] private float accelerationConstant = 0f;
    [SerializeField] private float decelerationConstant = 0f;

    private Animator animator;
    private Vector2 velocity = Vector2.zero;
    public Vector2 lastDirection = Vector2.zero;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 horizontalInput = new Vector2(Input.GetAxis("Horizontal"), 0f);
        Vector2 verticalInput = new Vector2(0f, Input.GetAxis("Vertical"));

        // Acceleration Movement
        Vector2 movement = horizontalInput + verticalInput;
        Vector2 acceleration = movement.normalized;

        if (movement.x == 0) movement.x = 0;
        else if (movement.x < 0) movement.x = -1;
        else if (movement.x > 0) movement.x = 1;

        bool dirChanged = false;
        if (lastDirection.x != movement.x && velocity != Vector2.zero)
        {
            Debug.Log("RUN");
            dirChanged = true;
            velocity -= velocity * decelerationConstant * Time.deltaTime * 2;

            if (Approximate(velocity, Vector2.zero, 0.1f))
            {
               // lastDirection = acceleration;
                dirChanged = false;
            }
        }

        if (acceleration != Vector2.zero && dirChanged == false)
            velocity += acceleration * accelerationConstant * Time.deltaTime;
        else
        {
            velocity -= velocity * decelerationConstant * Time.deltaTime;
        }


        if (dirChanged == false) lastDirection = movement;
        velocity = Vector2.ClampMagnitude(velocity, Maxspeed);
        Debug.Log("Movement: " + movement + " || Last Direction: " + lastDirection);
        Debug.Log("Velocity: " + velocity + " || Acceleration: " + acceleration);
        transform.position += (Vector3)velocity * Time.deltaTime;

        // Tranform movement
        //Vector2 movement = horizontalInput + verticalInput;

        //transform.position += (Vector3)movement * Time.deltaTime * Maxspeed;

        AnimationHandle(movement.normalized);
    }

    bool Approximate(Vector2 a, Vector2 b, float epsilon)
    {
        return Mathf.Abs(a.x - b.x) < epsilon && Mathf.Abs(a.y - b.y) < epsilon;
    }

    void AnimationHandle(Vector2 direction)
    {
        animator.SetFloat("BlendX", direction.x);
        animator.SetFloat("BlendY", direction.y);
    }
}
