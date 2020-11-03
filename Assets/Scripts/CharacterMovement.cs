using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is copied over from the exercises I did but with a few improvements here and there.
public class CharacterMovement : MonoBehaviour
{
    // In case a variable name is ever changed in the animator, change it here rather than finding all the references in the body.
    private static class AnimationStrings
    {
        public const string walkingFloat = "LocomotionBlend";
        public const string groundedBool = "isGrounded";
    }

    public float speed = 4.0f;
    public float turnSpeed = 10.0f;
    public float jumpStrength = 4.0f;

    public bool grounded = true;

    private Animator anim;
    private Rigidbody rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        Vector3 input = new Vector3(0f, 0f, Input.GetAxis("Vertical"));
        Vector3 inputTurn = new Vector3(0f, Input.GetAxis("Horizontal"), 0f);

        gameObject.transform.Translate(input * speed * Time.deltaTime);
        gameObject.transform.Rotate(inputTurn * turnSpeed * Time.deltaTime);

        if (grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                grounded = false;
                rb.AddForce(new Vector3(0, jumpStrength, 0));

                return;
            }

            anim.SetFloat(AnimationStrings.walkingFloat, input.magnitude);
        }

        anim.SetBool(AnimationStrings.groundedBool, grounded);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
}
