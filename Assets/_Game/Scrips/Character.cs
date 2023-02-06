using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] protected CombatText CombatTextPrefab;

    private float hp;
    private string currentAnimName;
    public bool IsDead => hp <= 0;
    private void Start()
    {
        OnInit();  
    } 
    public virtual void OnInit()
    {
        hp = 100;
        healthBar.OnInit(100/*, transform(p4.1)*/);
    }
    public virtual void OnDespawn()
    {
  
    }
    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDespawn), 2f);
    }
    protected void ChangeAnim(string animName)
    {
        Debug.Log(animName);
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
    public void OnHit(float damage)
    {
        Debug.Log("hitttt");
        if (!IsDead) 
        {
            hp -= damage;
            
            if (IsDead)
            {
                hp = 0;
                OnDeath();
            }

            healthBar.SetNewHp(hp);
            Instantiate(CombatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        }
    }

}
/*
 if (hp <= damage)
            {
                OnDeath();
            }
 */