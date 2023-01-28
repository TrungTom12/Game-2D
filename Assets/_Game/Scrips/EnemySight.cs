using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public Enemy enemy;

    private void OnTriggerEnter2D(Collider2D collision) // Hai thg va cham
    {
        if (collision.tag == "Player")
        {
            enemy.setTarget(collision.GetComponent<Character>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //Hai thg thoat khoi nhau
    {
        if (collision.tag == "Player")
        {
            enemy.setTarget(null);
        }
    }
}
