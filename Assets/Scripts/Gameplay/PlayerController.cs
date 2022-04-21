using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public AudioClip[] footsteps;
    public AudioSource footstepSource;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector3 moveCommand;
    private float horInput;
    private float verInput;
    private float speed;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        speed = GetComponent<Player>().GetCurrentStats().ms / 100;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            speed *= 1.5f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            speed /= 1.5f;
        }
    }

    private void FixedUpdate() {
        horInput = Input.GetAxis("Horizontal");
        verInput = Input.GetAxis("Vertical");

        float moveX = horInput;
        float moveY = verInput;
        moveCommand = new Vector3(moveX, moveY, 0);
        UpdateAnimator(moveCommand);
        Vector3 movement = moveCommand * speed * Time.deltaTime;
        
        rb.MovePosition(transform.position + movement);
        spriteRenderer.flipX = moveCommand.x >= 0 ? true : false;

        if(moveCommand != Vector3.zero) {
            //Character is moving
            if (!footstepSource.isPlaying) {
                footstepSource.clip = footsteps[Random.Range(0, footsteps.Length)];
                footstepSource.Play();
            }
        }
    }

    private void UpdateAnimator(Vector3 direction) {
        if (animator) {
            if (animator.GetInteger("WalkY")==0)
                animator.SetInteger("WalkX", direction.x < 0 ? -1 : direction.x > 0 ? 1 : 0);
            if(animator.GetInteger("WalkX") == 0)
                animator.SetInteger("WalkY", direction.y < 0 ? 1 : direction.y > 0 ? -1 : 0);
        }
    }
}
