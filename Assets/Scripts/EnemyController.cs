using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float health = 10;
    public float speed = 1f;
    public GameObject player;
    public Animator chicken;
    public Rigidbody2D rb;

    private float nextAttack = 0.0f;
    private float attackCooldown = 0.75f;

    private void Update()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            if (gameObject.transform.position.x - player.transform.position.x <= 2)
                AttackPlayer();
            else if (gameObject.transform.position.x - player.transform.position.x <= 15)
                ChasePlayer();
        }
        
    }

    private void ChasePlayer()
    {
        chicken.SetBool("Run", true);
        chicken.SetBool("Eat", false);
        rb.MovePosition(rb.position - new Vector2(speed * Time.deltaTime, 0));
        
    }

    private void AttackPlayer()
    {
        chicken.SetBool("Run", false);
        chicken.SetBool("Eat", true);
        rb.MovePosition(new Vector2(rb.position.x, rb.position.y));
        nextAttack += Time.deltaTime;
        if (nextAttack >= attackCooldown)
        {
            GameController.instance.TakeDamage();
            nextAttack = 0.0f;
        }
            
    }

    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0) {
            gameObject.SetActive(false);
        }
    }
}
