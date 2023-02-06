using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMap : MonoBehaviour
{
    [SerializeField] private GameObject mapNeedHide;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            mapNeedHide.SetActive(false);
        }
    }
}
