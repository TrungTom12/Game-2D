using Mono.Cecil.Cil;
using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.TextCore.Text;

public class Player : Character
{

    private float moveSpeed, dirX, dirY;
    public bool ClimbingAllowed { get; set; }

    [SerializeField] private Rigidbody2D rb;
    //[SerializeField] private Animator anim; p3
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 350;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;

    private GameObject currentTeleporter;
    private bool isGrounded = true;  
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDeath = false;
    private float horizontal;
    //private string currentAnimName; p3
    private int coin = 0;
    private Vector3 savePoint;

    
    /* void Start() //(part 3)
     {
         //SavePoint();     
     }
     */
    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin",0);

        rb = GetComponent<Rigidbody2D>();
        moveSpeed = 5f;

    }
    void Update()
    {
        if (isDeath)
        {
            return;
        }
        isGrounded = CheckGround();
        //-1 -> 0 -> 1 
        //horizontal = Input.GetAxisRaw("Horizontal"); p4
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

        //Teleporter
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentTeleporter != null)
            {
                transform.position = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
            }
        }

        //Ladder
        dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if (ClimbingAllowed)
        {
            dirY = Input.GetAxisRaw("Vertical") * moveSpeed;
        }
    }

    private void FixedUpdate()
    {
        
        if (ClimbingAllowed)
        {
            //Debug.Log("TTTTTTTT");
            rb.isKinematic = true;
            rb.velocity = new Vector2(dirX, dirY);
        }
        else
        {
            rb.isKinematic = false;
            rb.velocity = new Vector2(dirX, rb.velocity.y);
        }
    }

    public override void OnInit() 
    //reset các thông số đưa về các trạng thái đầu tiên p2
    {
        base.OnInit(); 
        //isDeath = false; p4
        isAttack = false;

        transform.position = savePoint;
        
        DeActiveAttack();
        ChangeAnim("idle");

        SavePoint();
        UIManager.instance.SetCoin(coin);
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }
    protected override void OnDeath()
    {
        base.OnDeath();

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
        } p1
    */ 
        return hit.collider != null;
    }
    public void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }
    public void Throw()
    {  
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefab, throwPoint.position ,throwPoint.rotation);   
    }
    public void Jump()
    {
        isJumping = true;

        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }  
    public void Climb()
    {
        ChangeAnim("climb");
        
    }
    private void ResetAttack()
    {
        
        ChangeAnim("idle");
        isAttack = false;
    }

    //project nào cũng dùng (da cho vao character) 
  /*  private void ChangeAnim(string animName)
    {
        Debug.Log(animName);
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
  */
    //Coin p2
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            //Debug.Log("Coin" + collision.gameObject.name);
            coin++;
            PlayerPrefs.SetInt("coin", coin); //Luu data coin 
            UIManager.instance.SetCoin(coin);
            Destroy(collision.gameObject);
        }
        if (collision.tag.Equals("DeathZone"))
        {
            isDeath = true; 
            ChangeAnim("die");

            Invoke(nameof(OnInit), 1f);
        }

        //teleporter
        if (collision.CompareTag("Teleporter"))
        {
            currentTeleporter = collision.gameObject;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //teleporter
        if (collision.CompareTag("Teleporter"))
        {
            if (collision.gameObject == currentTeleporter)
            {
                currentTeleporter = null;
            }
        }
    }



    internal void SavePoint()
    {
        savePoint = transform.position;
    }
    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }
    private void DeActiveAttack()
    {
        attackArea.SetActive(false);   
    }
    public void SetMove(float hozizontal)
    {
        this.horizontal = hozizontal;
    } 
}
 