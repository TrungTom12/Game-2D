using Mono.Cecil.Cil;
using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 350;

    private bool isGrounded = true;  
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDeath = false;

    private float horizontal;
 
    private string currentAnimName;

    private int coin = 0;

    private Vector3 savePoint;
    // Start is called before the first frame update 
    void Start() //(part 2)
    {
        SavePoint();
        
        OnInit();
    }

    // Update is called once per frame   
    void Update()
    {

        if (isDeath)
        {
            return;
        }

        isGrounded = CheckGround();

        //-1 -> 0 -> 1 
        horizontal = Input.GetAxisRaw("Horizontal");
        //verticle = Input.GetAxisRaw("Verticle");
  
        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }

            //Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            //Change anim run 
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }

            //Attack
            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }

            //Throw
            if (Input.GetKeyDown(KeyCode.V) && isGrounded)
            {
                Throw();
            }
        }
        
        //check falling 
        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }

        //Moving (Tiến lùi + quay Anim)
        if (Mathf.Abs(horizontal) > 0.1f)
        {
   
            ChangeAnim("run");
            rb.velocity = new Vector2(horizontal * Time.deltaTime * speed, rb.velocity.y);
            //horizontal > 0 -> trả về 0, nếu horizontal <= 0 ->  trả về là 100
            transform.rotation= Quaternion.Euler(new Vector3(0,horizontal > 0 ? 0 : 180,0));
            //transform.localScale = new Vector3 (horizontal,1,1);
        }

        //Idle
        else if (isGrounded && !isJumping && ! isAttack)
        {
            ChangeAnim("idle");   
            rb.velocity = Vector2.zero; 
        }

    }

    public void OnInit() // reset các thông số đưa về các trạng thái đầu tiên (part 2)
    {
        isDeath = false;
        isAttack = false;

        transform.position = savePoint;
       
        ChangeAnim("idle");
    }

    private bool CheckGround()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
    /*
        if ( hit != null ) 
        {
            return true;
        }else
        {
            return false;
        }
    */
        return hit.collider != null;
    }

    private void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void Jump()
    {
        isJumping = true;

        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }  

    private void ResetAttack()
    {
        
        ChangeAnim("idle");
        isAttack = false;
    }

    //project nào cũng dùng 
    private void ChangeAnim(string animName)
    {
        Debug.Log(animName);
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    //Coin (part 2)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Debug.Log("Coin" + collision.gameObject.name);
            coin++;
            Destroy(collision.gameObject);
        }
        if (collision.tag.Equals("DeathZone"))
        {
            isDeath = true; 
            ChangeAnim("die");

            Invoke(nameof(OnInit), 1f);
        }
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }
}
 