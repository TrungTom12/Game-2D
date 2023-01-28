using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    float timer;
    float randomTime;
    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        randomTime = Random.RandomRange(3f, 6f);
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;

        if (enemy.Target != null) 
        {
            enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);

            if (enemy.IsTargetInRanger())
            {
                enemy.ChangeState(new AttackState());
            }
            else
            {
                enemy.Moving();
            }
            // doi huong enemy  toi huong cua player
            
        }else
        {
            if (timer < randomTime)
            {
                enemy.Moving();
            }
            else
            {
                enemy.ChangeState(new IdleState());
            }
        }
    }

    public void OnExit(Enemy enemy)
    {
        
    }
   
}
