using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;                 //声明水平的运动速度
    public float jumpForce;             //声明垂直的跳跃力度
    public bool isGround;               //声明是否在地面上的变量
    public bool isJump;                 //声明是否跳跃的变量
    public Transform groundCheck;       //声明检测是否在地面的检测点变量
    public LayerMask ground;            //声明检测点所检测的图层
    public GameObject enemy;

    private Rigidbody2D rigidbody2d;    //声明刚体变量
    private Collider2D collider2d;      //声明碰撞体变量
    private Animator animator;          //声明动画变量

    float horizontal;                   //声明水平移动
    bool jumpPressed;                   //声明一个检测跳跃按键是否按下变量
    int jumpCount;                      //声明一个统计跳跃按键按下次数变量

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();      //获取刚体组件
        collider2d = GetComponent<Collider2D>();        //获取碰撞体组件
        animator = GetComponent<Animator>();            //获取动画器组件
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            FSM fsm = enemy.GetComponent<FSM>();
            fsm.ChangeHealth();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hitLeftFoot = Physics2D.Raycast(transform.position + new Vector3(-0.12f, -0.026f, 0f), Vector2.down, 0.01f, ground);
        RaycastHit2D hitRightFoot = Physics2D.Raycast(transform.position + new Vector3(0.1f,-0.026f,0f),Vector2.down, 0.01f, ground);     //以检测点为圆心，数值为半径，检测是否有ground图层包含在内
        if (hitLeftFoot.collider != null || hitRightFoot.collider != null)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }

        PlayerJump();
        PlayerMovement();
        PlayerAnimator();
        
    }

    void PlayerMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");   //获取水平输入
        rigidbody2d.velocity = new Vector2(horizontal * speed, rigidbody2d.velocity.y);     //更改刚体的线性速度

        //判断玩家输入以更改角色方向
        if (horizontal != 0)
        {
            transform.localScale = new Vector3(horizontal, 1, 1);
        }
    }

    void PlayerJump()
    {
        //判断是否在地面上
        if (isGround)
        {
            jumpCount = 2;      //在地面上，可跳跃次数为
            isJump = false;     //跳跃状态为
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            jumpCount--;
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }

    void PlayerAnimator()
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontal));      //设置站立与奔跑动画的转换

        animator.SetBool("Ground", isGround);
        
        if (!isGround && rigidbody2d.velocity.y > 0)
        {
            animator.SetBool("IsJump", true);
        }
        else if (rigidbody2d.velocity.y <= 0)
        {
            animator.SetBool("IsJump", false);
        }
    }
}
