using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 定义一组枚举变量，用于在字典中充当键
/// </summary>
public enum StateType
{
    Idle, Walk, ChaseToWalk, Chase, Attack, Attack1, Hurt, Death
}

/// <summary>
/// 序列化实际状态所需要使用的参数
/// </summary>
[Serializable]
public class Parameter
{
    public int health;                          //血量

    public float moveSpeed;                     //巡逻状态的移动速度

    public float chaseSpeed;                    //追击状态的移动速度

    public float idleTime;                      //在巡逻点上的停留时间，空闲状态的时间

    public Transform[] patrolPoints;            //巡逻点的变换组件

    public Transform[] chasePoints;             //最远追击点的变换组件

    public LayerMask layerMask;                 //检测玩家所在的层级，用于射线检测

    public Transform transformPlayer;           //获取玩家的变换组件，在追击与攻击状态时使用

    public Animator animator;                   //动画器

    public float AttackSpace;
}

public class FSM : MonoBehaviour
{
    public BoxCollider2D boxCollder2D;          //获取AttackCheck的碰撞体组件，攻击状态的挥刀时触发玩家血量降低

    public PolygonCollider2D polytonCollider2D;

    public Parameter parameter;                 //声明参数列表，在unity中显示出来

    private IState currentState;                //声明一个自定义类型的变量，存放当前的状态

    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();         //定义一个字典存放所有的状态

    public int maxHealth = 10;                     //最大血量

    public IState beforeState;                  //定义一个自定义类型的变量，存放上一个状态，在受伤动画结束后触发事件回到上一个状态

    public int currentHealth;                          //当前血量

    public float baseAttackTime;                       //攻击间隔时间

    [SerializeField]
    BoxCollider2D enemyBoxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Walk, new WalkState(this));
        states.Add(StateType.ChaseToWalk, new ChaseToWalkState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Attack1, new Attack1State(this));
        states.Add(StateType.Hurt, new HurtState(this));
        states.Add(StateType.Death, new DeathState(this));

        TransitionState(StateType.Idle);        //初始化状态为Idle

        parameter.animator = GetComponent<Animator>();      //获取动画组件

        currentHealth = maxHealth;              //初始化当前血量
    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate();

        if (currentHealth <= 0)
        {
            TransitionState(StateType.Death);
        }
    }

    /// <summary>
    /// 转换状态
    /// </summary>
    /// <param name="type">输入定义的枚举变量作为键</param>
    public void TransitionState(StateType type)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[type];
        currentState.OnEnter();
    }


    /// <summary>
    /// 控制朝向
    /// </summary>
    /// <param name="target">输入一个变换组件作为目标位置</param>
    public void FlipTO(Transform target)
    {
        if (target != null)
        {
            if (transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    /// <summary>
    /// 检测玩家
    /// </summary>
    /// <param name="frontLookLength">输入一个浮点数作为前方射线长度</param>
    /// <param name="behindLookLength">输入一个浮点数作为后方射线长度</param>
    public void CheckPlayer(float frontLookLength, float behindLookLength)
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position + new Vector3(enemyBoxCollider2D.offset.x, enemyBoxCollider2D.offset.y, 0f), new Vector2(transform.localScale.x, 0f), frontLookLength, parameter.layerMask);
        RaycastHit2D raycast1 = Physics2D.Raycast(transform.position + new Vector3(enemyBoxCollider2D.offset.x, enemyBoxCollider2D.offset.y, 0f), new Vector2(-transform.localScale.x, 0f), behindLookLength, parameter.layerMask);
        Debug.DrawRay(transform.position + new Vector3(enemyBoxCollider2D.offset.x, enemyBoxCollider2D.offset.y, 0f), new Vector2(transform.localScale.x, 0f) * frontLookLength, Color.red);
        Debug.DrawRay(transform.position + new Vector3(enemyBoxCollider2D.offset.x, enemyBoxCollider2D.offset.y, 0f), new Vector2(-transform.localScale.x, 0f) * behindLookLength, Color.red);
        if (raycast.collider != null || raycast1.collider != null)
        {
            TransitionState(StateType.Chase);
        }
        if (currentState == states[StateType.Chase] && raycast.collider == null && raycast1.collider == null)
        {
            TransitionState(StateType.ChaseToWalk);
        }
    }

    /// <summary>
    /// 启用攻击检测的触发器
    /// </summary>
    public void enableBoxCollider2D()
    {
        if (!boxCollder2D.enabled)
        {
            boxCollder2D.enabled = true; 
        }
    }

    /// <summary>
    /// 禁用攻击检测的触发器
    /// </summary>
    public void disableBoxCollider2D()
    {
        if (boxCollder2D.enabled)
        {
            boxCollder2D.enabled = false;
        }
    }

    /// <summary>
    /// 启用攻击1检测的触发器
    /// </summary>
    public void enable1BoxCollider2D()
    {
        if (!polytonCollider2D.enabled)
        {
            polytonCollider2D.enabled = true;
        }
    }

    /// <summary>
    /// 禁用攻击1检测的触发器
    /// </summary>
    public void disable1BoxCollider2D()
    {
        if (polytonCollider2D.enabled)
        {
            polytonCollider2D.enabled = false;
        }
    }

    /// <summary>
    /// 修改血量
    /// </summary>
    public void ChangeHealth()
    {
        TransitionState(StateType.Hurt);
        PlayerFSM playerFsm = GameObject.Find("Player").GetComponent<PlayerFSM>();

        Animator anim = playerFsm.parameter.animator;

        string animString = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        parameter.animator.SetTrigger("ComboHurt");

        if (parameter.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "hurt")
        {
            parameter.animator.SetTrigger("ComboHurt");
        }

        if (animString == "Attack")
        {
            currentHealth -= 2;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(-transform.localScale.x * 0.05f + transform.position.x, transform.position.y), 1.0f);
        }
        else if (animString == "Attack1")
        {
            currentHealth -= 1;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(-transform.localScale.x * 0.05f + transform.position.x, transform.position.y), 1.0f);
        }
        else
        {
            currentHealth -= 3;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(-transform.localScale.x * 0.4f + transform.position.x, transform.position.y), 0.5f);
        }

        
    }
}

    