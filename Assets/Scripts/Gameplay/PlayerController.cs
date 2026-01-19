using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public AudioClip[] footsteps;
    public AudioSource footstepSource;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool canMove = true;

    [HideInInspector] public Vector2 moveCommand;
    [HideInInspector] public float speed;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        speed = GetComponent<Player>().GetCurrentStat(Stat.MovementSpeed) / 10;
    }

    private void FixedUpdate() {
        Vector2 movement = speed * Time.fixedDeltaTime * moveCommand;
        UpdateAnimator(movement);
        if (canMove) {
            moveCommand = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            rb.velocity += movement;
            //rb.MovePosition((Vector2)transform.position + movement);

            spriteRenderer.flipX = moveCommand.x >= 0 ? true : false;

            if (moveCommand != Vector2.zero) {
                //Character is moving
                if (!footstepSource.isPlaying) {
                    footstepSource.clip = footsteps[Random.Range(0, footsteps.Length)];
                    footstepSource.Play();
                }
            }
        }
    }

    private void UpdateAnimator(Vector2 direction) {
        if (animator) {
            if (animator.GetInteger("WalkY")==0)
                animator.SetInteger("WalkX", direction.x < 0 ? -1 : direction.x > 0 ? 1 : 0);
            if(animator.GetInteger("WalkX") == 0)
                animator.SetInteger("WalkY", direction.y < 0 ? 1 : direction.y > 0 ? -1 : 0);
        }
    }

    public void BlockPlayer() {
        canMove = false;
    }
    public void UnlockPlayer() {
        canMove = true;
    }
}
