using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{

    /*bool IsMoving
    {
        set
        {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }*/

    public float health = 3f;

    public float moveSpeed = 700f;

    public float maxSpeed = 2.2f;

    public float collisionOffset = 0.05f;

    public ContactFilter2D movementFilter;

    public SwordAttack swordAttack;

    public float idleFriction = 0.9f;

    Vector2 movementInput;

    SpriteRenderer spriteRenderer;

    Rigidbody2D rb;

    Animator animator;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    bool dead = false;

    bool win = false;

    public TextMeshProUGUI countText;

    public int count;





    // bool isMoving = false;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        count = 0;

        SetCountText();
    }

    public void UpdateCount(int newCount)
    {
        count += newCount;

        SetCountText();
    }


    public void Hit(int damage)
    {

        health -= damage;

        if (health <= 0)
        {
            Defeated();

        }
    }

    public void Defeated()
    {
        if (win == false)
        {

            animator.SetBool("isMoving", false);
            dead = true;
            LockMovement();
            animator.SetTrigger("dead");
            countText.text = "You've been defeated. Please use pause menu to retry!";
            //Time.timeScale = 0;

        }
    }

    void SetCountText()
    {

        countText.text = "Count: " + count.ToString(); 

        if (count == 12)
        {
            win = true;

            countText.text = "All Slime Destroyed. Mission Accomplished";
        }
    }



    /* void FixedUpdate()
     {
         if (canMove == true && movementInput != Vector2.zero)
         {
             rb.velocity = Vector2.ClampMagnitude(rb.velocity + (movementInput * moveSpeed * Time.deltaTime), maxSpeed);

             if (movementInput.x > 0)
             {
                 spriteRenderer.flipX = false;
             }
             else if (movementInput.x < 0)
             {
                 spriteRenderer.flipX = true;
             }

             IsMoving = true;


         }
         else
         {
             rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);

             IsMoving = false;
         }

         UpdateAnimatorParameter();

     }

     void UpdateAnimatorParameter()
     {
         animator.SetBool("isMoving", isMoving);
     }*/




    // Update is called once per frame
    private void FixedUpdate()
    {

        if (dead == false)
        {

            if (canMove)
            {

                if (movementInput != Vector2.zero)
                {
                    bool success = TryMove(movementInput);

                    if (!success)
                    {
                        success = TryMove(new Vector2(movementInput.x, 0));
                    }

                    if (!success)
                    {
                        success = TryMove(new Vector2(0, movementInput.y));
                    }


                    if (dead == false)
                    { 
                        animator.SetBool("isMoving", success);
                    }
                }
                else
                {
                    animator.SetBool("isMoving", false);
                }

                if (movementInput.x < 0)
                {
                    spriteRenderer.flipX = true;
                }
                else if (movementInput.x > 0)
                {
                    spriteRenderer.flipX = false;
                }


            }

        }

    }

     private bool TryMove(Vector2 direction)
     {
        if (dead == false)
        {

            if (direction != Vector2.zero)
            {
                int count = rb.Cast(
                    direction,
                    movementFilter,
                    castCollisions,
                    moveSpeed * Time.fixedDeltaTime + collisionOffset);


                if (count == 0)
                {
                    rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
     }


    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
        canMove = true;
    }

    void OnFire()
    {
        if (dead == false)
        {
            animator.SetTrigger("swordAttack");
            canMove = false;
        }
    }

    public void SwordAttack()
    {

            LockMovement();

            if (spriteRenderer.flipX == true)
            {
                swordAttack.AttackLeft();
            }

            else
            {
                swordAttack.AttackRight();
            }
        

    }

    public void EndSwordAttack()
    {
        UnlockMovement();

        swordAttack.StopAttack();
    }

    public void LockMovement() 
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        if (dead == false)
        {
            canMove = true;
        }
    }



}
