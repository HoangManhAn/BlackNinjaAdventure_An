using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameObject attackArea;

    private IState currentState;

    private bool isRight = true;

    private Character target;
    public Character Target => target;

    private void Update()
    {
        if(currentState != null && !IsDead)
        {
            currentState.OnExecute(this);
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        ChangeState(new IdleState());
        DeActiveAttack();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }

    public override void OnDeath()
    {
        base.OnDeath();
    }

    public void Moving()
    {
        ChangeAnim("run");
        rb.velocity = transform.right * moveSpeed;

    }

    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.velocity = Vector2.zero;
    }

    public void Attack()
    {
        ChangeAnim("attack");
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
        
    }

    public bool IsTargetInRange()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
            
    }

    public void ChangeState(IState newState)
    {
        if(currentState != null)
        {
            currentState.OnExit(this);
        }
        
        currentState = newState;

        if(currentState != null )
        {
            currentState.OnEnter(this);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyWall")
        {
            ChangeDirection(!isRight);
        }
       
    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;

        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }

    public void SetTarget(Character character)  
    {
        this.target = character;

        if(IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else if(Target != null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new IdleState());
        }
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
    }

}
