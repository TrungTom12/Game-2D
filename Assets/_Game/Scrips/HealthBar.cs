using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image imageFill;

   /* [SerializeField] Vector3 offset;
    private Transform target; p4.1 (35:44)
    (su dung khi thanh mau dat ben ngoai player & enemy)
    */

    float hp;
    float maxHp;

    // Update is called once per frame
    void Update()
    {
        // ham tinh luong mau tu fillAmount den luong mau con lai trong thoi gian nhat dinh
        imageFill.fillAmount= Mathf.Lerp(imageFill.fillAmount, hp / maxHp , Time.deltaTime * 5f);
           //transform.position = target.position + offset; p4.1
    }

    public void OnInit(float maxhp/*,Transform target(p4.1)*/)
    {
        //this.target = target; p4
        this.maxHp = maxhp;
        hp= maxhp;
        imageFill.fillAmount = 1;
    }

    public void SetNewHp(float hp)
    {
        this.hp = hp;

       // imageFill.fillAmount = hp / maxHp;
    }
}
