using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEditor.Timeline.TimelinePlaybackControls;



public class Enemy : MonoBehaviour
{


    public GameObject player;

    public float speedConst;

    public float speed;

    private float distance;

    public float tooFar;

    public float canSee;

    public float collisionOffset = 0.05f;

    public ContactFilter2D movementFilter;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public PlayerController playerController;

    Rigidbody2D rb;

    Animator animator;

    public float health = 3;

    bool dyeing = false;

    public float Health
    {
        set 
        {
            health = value;
            if (health <= 0)
            {
                Defeated();
            }
        }
        get 
        {
            return health;
        }
    }



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);

        //float currentSpeed = 1;


        if (distance < canSee)
        {
            speed = speedConst;

        }
        else if (distance > tooFar)
        {
            speed = 0;
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (dyeing == false)
        {

            if (other.tag == "Player")
            {
                PlayerController player = other.GetComponent<PlayerController>();

                if (player != null)
                {

                    player.Hit(1);
                    if (player.health <= 0)
                    {
                        print("Player dead");
                        
                    }
                }

            }

        }
   
    }






    public void setSpeed(float fspeed)
    {
        speed = fspeed;
    }

    public void UpdatePlayerCount(int newCount)
    {
        playerController.UpdateCount(newCount);
        if (playerController.count == 26)
        {
            //animator.SetTrigger("Defeated");
            Destroy(gameObject);
        }

    }



    public void backToIdle()
    {
        animator.SetTrigger("IdleNow");
    }

    public void Hit(int damage)
    {

        health -= damage;
        animator.SetTrigger("Hit");

        if (health <= 0)
        {
            Defeated();
            
        }
    }

    public void Defeated()
    {
        dyeing = true;

        animator.SetTrigger("Defeated");
        
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
        print("slime dead");
        UpdatePlayerCount(1);
    }


}