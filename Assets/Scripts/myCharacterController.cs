using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myCharacterController : MonoBehaviour
{
    [SerializeField] private ScreenTouchController input;
    [SerializeField] private Rigidbody myRigidbody;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerManager playerManager;
    
    void FixedUpdate()
    {
        var direction = new Vector3(input.Direction.x, 0f, input.Direction.y);
        Move(direction);
        
    }

    private void Move(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            
            if (playerManager.papers.Count > 1)
            {
                anim.SetBool("carry", false);
                anim.SetBool("RunWithPapers", true);
            }
            else
            {
                anim.SetBool("run", true);
                anim.SetBool("carry", false);
            }
            
            // Calculate the rotation to look in the move direction
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);

            // Smoothly rotate towards the calculated rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }
        else
        {
            if (playerManager.papers.Count > 1)
            {
                anim.SetBool("carry", true);
                anim.SetBool("RunWithPapers", false);
            }
            else
            {
                anim.SetBool("run", false);
                anim.SetBool("carry", false);

            }

        }
        myRigidbody.velocity = direction * moveSpeed * Time.deltaTime;
    }
}
