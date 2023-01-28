using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnEnter(Enemy enemy);// Bat dau State
    void OnExecute(Enemy enemy); // Update State
    void OnExit(Enemy enemy); // Ket thuc State
}
