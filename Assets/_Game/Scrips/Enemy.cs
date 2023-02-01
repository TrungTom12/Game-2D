using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float attackRanger;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject attackArea;
    private IState currenState;
    private bool isRight = true;

    private Character target;
    public Character Target => target;


    private void Update()
    {
        if (currenState != null && !IsDead)
        {
            currenState.OnExecute(this);
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
        Destroy(gameObject);
    }

    protected override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
    }
    

    public void ChangeState(IState newState)
    // khi đổi sang state mới , ktra state cũ != null , thoát state cũ và gán state mới = currenState
    // , check currenState != null => bắt đầu truy cập enter state mới 
    {
        if (currenState != null)
        {
            currenState.OnExit(this);
        }

        currenState= newState;
        if (currenState != null)
        {
            currenState.OnEnter(this);
        }
    }

    internal void setTarget(Character character)
    {
        this.target= character;

        if (IsTargetInRanger())
        {
            ChangeState(new AttackState());
        }else
            if (Target != null )
            {
                ChangeState(new PatrolState());
            }
            else
            {
                ChangeState(new IdleState());
            }
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

    public bool IsTargetInRanger()
    {
        if (target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRanger)
        {
            return true;
        }else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyWall")
        {
            ChangeDirection(!isRight);
        }
    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ?
            Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
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
