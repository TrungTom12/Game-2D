using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   public static UIManager instance;
    //C1:
    /* public static UIManager Instance
     {
         get 
         { 
             if (instance == null)
             {
                 instance = FindObjectOfType<UIManager>();
             }

             return instance;
         }
     }    
    */
    //C2:
    private void Awake()
    {
        instance = this; 
    }

    [SerializeField] Text coinText;

    public void SetCoin(int coin)
    {
        coinText.text = coin.ToString();
    }

}
