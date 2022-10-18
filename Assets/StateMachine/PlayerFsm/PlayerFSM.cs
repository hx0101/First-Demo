using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum StateTypePlayer
{
    Idle, 
    Run, 
    Jump,
    JumpToFall,
    Fall,
    Crouch,
    Dash,
    Hurt,
    Death,
    Attack,
    Slide,
    EdgeIdle,
    EdgeGrab,
    ErrorFall
}

[Serializable]
public class ParameterPlayer
{
    public float dashDelay;

    public float invincibleTime;

    public Animator animator;

    public BoxCollider2D boxCollider2d;

    public float comboTime;
}

public class PlayerFSM : MonoBehaviour
{
    public float moveSpeed;

    public float verticalForce;

    float JumpHeight;

    public LayerMask ground;

    public Rigidbody2D rigidbody2d;

    public float invincibleTimePart;

    public float comboTimePart;

    public LayerMask enemy;

    public GameObject skeleton;

    public ParameterPlayer parameter;

    private Dictionary<StateTypePlayer, TState> states = new Dictionary<StateTypePlayer, TState>();

    private TState currentState;

    private float dashDelayPart;

    private float maxHealth = 10.0f;

    public float maxHealthh { get => maxHealth; }

    private float currentHealth;

    public float health { get => currentHealth; }

    public Vector2 colliderHead;

    public Vector2 colliderFoot;

    public bool infiniteJump;

    // Start is called before the first frame update
    void Start()
    {
        states.Add(StateTypePlayer.Idle,new IdlePlayerState(this));
        states.Add(StateTypePlayer.Run,new RunPlayerState(this));
        states.Add(StateTypePlayer.Jump, new JumpPlayerState(this));
        states.Add(StateTypePlayer.JumpToFall, new JumpToFallPlayerState(this));
        states.Add(StateTypePlayer.Fall, new FallPlayerState(this));
        states.Add(StateTypePlayer.Crouch, new CrouchPlayerState(this));
        states.Add(StateTypePlayer.Dash, new DashPlayerState(this));
        states.Add(StateTypePlayer.Hurt, new HurtPlayerState(this));
        states.Add(StateTypePlayer.Death, new DeathPlayerState(this));
        states.Add(StateTypePlayer.Attack, new AttackPlayerState(this));
        states.Add(StateTypePlayer.Slide, new SlidePlayerState(this));
        states.Add(StateTypePlayer.EdgeIdle, new EdgeIdlePlayerState(this));
        states.Add(StateTypePlayer.EdgeGrab, new EdgeGrabPlayerState(this));
        states.Add(StateTypePlayer.ErrorFall, new ErrorFallPlayerState(this));

        TransitionState(StateTypePlayer.Idle);

        dashDelayPart = parameter.dashDelay;

        currentHealth = maxHealth;

        //health = currentHealth;

        invincibleTimePart = parameter.invincibleTime;

        parameter.invincibleTime = 0;

        comboTimePart = parameter.comboTime;
        parameter.comboTime = 0;

        colliderHead = parameter.boxCollider2d.offset + parameter.boxCollider2d.size / 2;

        colliderFoot = parameter.boxCollider2d.size / 2 - parameter.boxCollider2d.offset;
    }

    // Update is called once per frame
    
    void Update()
    {
        currentState.OnUpdate();

        //if (currentHealth < health && currentHealth > 0)
        //{
        //    TransitionState(StateTypePlayer.Hurt);
        //    health = currentHealth;
        //}
        //else if (currentHealth <= 0)
        //{
        //    TransitionState(StateTypePlayer.Death);
        //}

        if (parameter.invincibleTime > 0)
        {
            parameter.invincibleTime -= Time.deltaTime;
        }

        if (parameter.comboTime > 0)
        {
            parameter.comboTime -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        currentState.OnFixedUpdate();
    }

    public void TransitionState(StateTypePlayer Type)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[Type];

        currentState.OnEnter();
    }

    public float MovementPlayer()
    {
        float horizontal;

        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            transform.localScale = new Vector3(horizontal, 1, 1);
        }

        rigidbody2d.velocity = new Vector2(horizontal * moveSpeed,rigidbody2d.velocity.y);

        return horizontal;
    }

    public void Toward()
    {
        float horizontal;

        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            transform.localScale = new Vector3(horizontal, 1, 1);
        }
    }

    public bool JumpPlayer()
    {
        bool inputJump = false;

        if (Input.GetButtonDown("Jump") && (OnTheGround() || infiniteJump || currentState == states[StateTypePlayer.EdgeIdle]))
        {
            rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, verticalForce);

            inputJump = true;
        }
        return inputJump;
    }

    public bool OnTheGround()
    {
        bool isGround = false;

        RaycastHit2D checkGround = Physics2D.Raycast(rigidbody2d.transform.position + new Vector3(parameter.boxCollider2d.size.x / 2 - 0.03f, 0f, 0f), Vector2.down, 0.1f, ground);
        RaycastHit2D checkGround1 = Physics2D.Raycast(rigidbody2d.transform.position - new Vector3(parameter.boxCollider2d.size.x / 2 - 0.03f, 0f, 0f), Vector2.down, 0.1f, ground);

        if (checkGround.collider != null || checkGround1.collider != null)
        {
            isGround = true;
            Debug.DrawRay(rigidbody2d.transform.position + new Vector3(parameter.boxCollider2d.size.x / 2 - 0.03f, 0f, 0f), Vector2.down * 0.1f, Color.red);
            Debug.DrawRay(rigidbody2d.transform.position - new Vector3(parameter.boxCollider2d.size.x / 2 - 0.03f, 0f, 0f), Vector2.down * 0.1f, Color.red);

        }

        return isGround;
    }

    public void Crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            TransitionState(StateTypePlayer.Crouch);
        }
    }

    public void Dash()
    {
        parameter.dashDelay -= Time.deltaTime;
        
        if (Input.GetButtonDown("Dash") && parameter.dashDelay <= 0)
        {
            TransitionState(StateTypePlayer.Dash);
            parameter.dashDelay = dashDelayPart;
            parameter.invincibleTime = invincibleTimePart;
        }
    }

    public float ThisStateTime()
    {
        AnimatorStateInfo info = parameter.animator.GetCurrentAnimatorStateInfo(0);

        return info.normalizedTime;
    }

    public void ChangeHealth()
    {
        if (parameter.invincibleTime <= 0f)
        {
            currentHealth--;

            rigidbody2d.transform.position = Vector2.MoveTowards(rigidbody2d.transform.position, 
                new Vector2(-transform.localScale.x * 0.1f + rigidbody2d.transform.position.x, rigidbody2d.transform.position.y), 0.5f);

            if (currentHealth > 0)
            {
                TransitionState(StateTypePlayer.Hurt);
            }
            else
            {
                TransitionState(StateTypePlayer.Death);
            }
            
        }
    }

    public bool Attack()
    {
        if (Input.GetButtonDown("Attack"))
        {
            TransitionState(StateTypePlayer.Attack);
            return true;
        }
        return false;
    }

    public void BeforeCheck()
    {
        colliderHead = parameter.boxCollider2d.offset + parameter.boxCollider2d.size / 2;
        colliderFoot = parameter.boxCollider2d.size / 2 - parameter.boxCollider2d.offset;

        float direction = transform.localScale.x;
        RaycastHit2D topBefore = Physics2D.Raycast(transform.position + new Vector3(colliderHead.x * direction,colliderHead.y,0f)
            ,new Vector2(direction,0f),0.15f,ground);
        RaycastHit2D faceBefore = Physics2D.Raycast(transform.position + new Vector3(colliderHead.x * direction, colliderHead.y - 0.13f, 0f)
            , new Vector2(direction, 0f), 0.15f, ground);
        RaycastHit2D ledgeCheck = Physics2D.Raycast(transform.position + new Vector3((colliderHead.x + 0.12f) * direction, colliderHead.y, 0f)
            , Vector2.down, 0.13f, ground);
        Debug.DrawRay(transform.position + new Vector3(colliderHead.x * direction, colliderHead.y, 0f), new Vector2(direction, 0f) * 0.15f, Color.red);
        Debug.DrawRay(transform.position + new Vector3(colliderHead.x * direction, colliderHead.y - 0.13f, 0f), new Vector2(direction, 0f) * 0.15f, Color.red);
        Debug.DrawRay(transform.position + new Vector3((colliderHead.x + 0.12f) * direction, colliderHead.y, 0f), Vector2.down * 0.13f, Color.red);

        if (!OnTheGround() && !topBefore && faceBefore && ledgeCheck && rigidbody2d.velocity.y < 0f && currentState != states[StateTypePlayer.EdgeIdle])
        {
            TransitionState(StateTypePlayer.EdgeIdle);
        }
        //else if (!OnTheGround() && topBefore && faceBefore && !ledgeCheck && currentState != states[StateTypePlayer.EdgeGrab])
        //{
        //    TransitionState(StateTypePlayer.EdgeGrab);
        //}

        if (currentState == states[StateTypePlayer.EdgeIdle] || currentState == states[StateTypePlayer.EdgeGrab])
        {
            transform.position = new Vector3(transform.position.x + (faceBefore.distance - 0.02f) * direction, transform.position.y - ledgeCheck.distance, transform.position.z);
        }
    }

    public void FootstepAudio()
    {
        AudioManager.PlayerFootstepAudio();
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            FSM fsm = other.gameObject.GetComponent<FSM>();

            Collider2D collider2D = other.gameObject.GetComponent<Collider2D>();

            if (collider2D != null)
            {
                fsm.ChangeHealth();
            }
        }
    }
}
