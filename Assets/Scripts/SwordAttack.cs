using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{

    public Collider2D swordCollider;

    // public AttackDirection attackDirection;

    public float damage = 1;

    Vector2 rightAttackOffset;

   

    private void Start()
    {
        rightAttackOffset = transform.position;
    }


    public void AttackRight()
    {
        print("Attack Right");
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft()
    {
        print("Attack Left");
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null) 
            {
                print("Enemy Hit");
                enemy.Hit(3);
            }
        }
    }



}