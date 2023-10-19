using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEditor.Timeline.TimelinePlaybackControls;



public class Enemy : MonoBehaviour
{


    public PlayerController playerController;

    Animator animator;

    public float health = 3;

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

    public void UpdatePlayerCount(int newCount)
    {
        playerController.UpdateCount(newCount);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

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
            UpdatePlayerCount(1);
        }
    }

    public void Defeated()
    {
        animator.SetTrigger("Defeated");

    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }


}