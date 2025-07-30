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
    public Vector2 lastAcceleration = Vector2.zero;
    public bool dirChanged = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 horizontalInput = new Vector2(Input.GetAxis("Horizontal"), 0f);
        Vector2 verticalInput = new Vector2(0f, Input.GetAxis("Vertical"));

        // Acceleration Movement
        Vector2 movement = horizontalInput;// + verticalInput;
        Vector2 acceleration = movement.normalized;

        if (acceleration != Vector2.zero && dirChanged == false)
            velocity += acceleration * accelerationConstant * Time.deltaTime;
        else
        {
            velocity -= velocity * decelerationConstant * Time.deltaTime;
        }

        if(lastAcceleration != acceleration && acceleration != Vector2.zero)
        {
            dirChanged = true;
            Debug.Log("RUN");
            velocity -= velocity * decelerationConstant * Time.deltaTime * 2f;

            if(Approximate(velocity, Vector2.zero, 0.1f))
            {
                dirChanged = false;
            }
        }

        if (dirChanged == false) lastAcceleration = acceleration;
        velocity = Vector2.ClampMagnitude(velocity, Maxspeed);
        Debug.Log("acceleration: " + acceleration + " || lastAcceleration: " + lastAcceleration);
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
