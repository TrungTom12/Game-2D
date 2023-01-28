using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    float timer;
    float randomTime;

    public void OnEnter(Enemy enemy)
    {
        enemy.StopMoving();
        timer = 0;
        randomTime = Random.RandomRange(2f, 4f);
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;

        if (timer > randomTime) 
        {
            enemy.ChangeState(new PatrolState());
        }
    }
     
    public void OnExit(Enemy enemy)
    {

    }


}
