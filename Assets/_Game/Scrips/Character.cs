using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
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
    }

    public virtual void OnDespawn()
    {
  
    }

    protected virtual void OnDeath()
    {
        ChagneAnim("Die");
        Invoke(nameof(OnDespawn), 2f);
    }

    private void ChagneAnim(string v)
    {
        throw new NotImplementedException();
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
        if (hp >= damage) 
        {
            hp -= damage;
            
            if (hp <= damage)
            {
                OnDeath();
            }
        }
    }

}
