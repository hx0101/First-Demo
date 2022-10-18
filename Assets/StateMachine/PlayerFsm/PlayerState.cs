using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlayerState : TState
{
    private PlayerFSM manager;

    private ParameterPlayer parameter;

    private float horizontal;

    public IdlePlayerState(PlayerFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Idle");
    }

    public void OnUpdate()
    {
        bool inputJump = manager.JumpPlayer();

        if (inputJump)
        {
            manager.TransitionState(StateTypePlayer.Jump);
        }

        manager.Crouch();

        manager.Dash();

        manager.Attack();

    }

    public void OnFixedUpdate()
    {
        horizontal = manager.MovementPlayer();

        if (horizontal != 0)
        {
            manager.TransitionState(StateTypePlayer.Run);
        }
    }

    public void OnExit()
    {

    }
}

public class RunPlayerState : TState
{
    private PlayerFSM manager;

    private ParameterPlayer parameter;

    public RunPlayerState(PlayerFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Run");
    }

    public void OnUpdate()
    {
        if (manager.JumpPlayer())
        {
            manager.TransitionState(StateTypePlayer.Jump);
        }

        manager.Dash();

        manager.Attack();
    }

    public void OnFixedUpdate()
    {
        float horizontal = manager.MovementPlayer();

        if (horizontal == 0)
        {
            manager.TransitionState(StateTypePlayer.Idle);
        }

        if (!manager.OnTheGround() && manager.rigidbody2d.velocity.y < 0)
        {
            manager.TransitionState(StateTypePlayer.Fall);
        }
    }

    public void OnExit()
    {

    }
}

public class JumpPlayerState : TState
{
    private PlayerFSM manager;

    private ParameterPlayer parameter;

    public JumpPlayerState(PlayerFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Jump");
        AudioManager.PlayerJumpAudio();
    }

    public void OnUpdate()
    {
        
        
        if (manager.rigidbody2d.velocity.y < 0)
        {
            manager.TransitionState(StateTypePlayer.JumpToFall);
        }
        else if(manager.rigidbody2d.velocity.y < 0)
        {
            manager.TransitionState(StateTypePlayer.Fall);
        }
    }

    public void OnFixedUpdate()
    {
        float horizontal = manager.MovementPlayer();
    }

    public void OnExit()
    {
        
    }
}

public class JumpToFallPlayerState : TState
{
    private PlayerFSM manager;

    private ParameterPlayer parameter;

    private float timer;

    public JumpToFallPlayerState(PlayerFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("JumpToFall");
        
    }

    public void OnUpdate()
    {
        if (manager.JumpPlayer())
        {
            manager.TransitionState(StateTypePlayer.Jump);
        }

        manager.BeforeCheck();
    }

    public void OnFixedUpdate()
    {
        timer = manager.ThisStateTime();
        if (timer >= 1.0f)
        {
            manager.TransitionState(StateTypePlayer.Fall);
        }

        float horizontal = manager.MovementPlayer();
    }

    public void OnExit()
    {
        
    }
}

public class FallPlayerState : TState
{
    private PlayerFSM manager;

    private ParameterPlayer parameter;

    public FallPlayerState(PlayerFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Fall");
    }

    public void OnUpdate()
    {
        bool isGround = manager.OnTheGround();
        if (isGround)
        {
            manager.TransitionState(StateTypePlayer.Idle);
        }

        if (manager.JumpPlayer())
        {
            manager.TransitionState(StateTypePlayer.Jump);
        }

        manager.BeforeCheck();
    }

    public void OnFixedUpdate()
    {
        float horizontal = manager.MovementPlayer();
    }

    public void OnExit()
    {
        AudioManager.PlayerFallAudio();
    }
}

public class CrouchPlayerState : TState
{
    private PlayerFSM manager;

    private ParameterPlayer parameter;

    private float timer;

    public CrouchPlayerState(PlayerFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Crouch");

        parameter.boxCollider2d.size = new Vector2(parameter.boxCollider2d.size.x, parameter.boxCollider2d.size.y / 2);
        parameter.boxCollider2d.offset = new Vector2(parameter.boxCollider2d.offset.x, parameter.boxCollider2d.offset.y / 2);

        manager.verticalForce += 1.0f;
    }

    public void OnUpdate()
    {
        bool inputJump = manager.JumpPlayer();

        if (inputJump)
        {
            manager.TransitionState(StateTypePlayer.Jump);
        }

        if (Input.GetButtonDown("Dash") && parameter.dashDelay <= 0)
        {
            manager.TransitionState(StateTypePlayer.Slide);
        }
    }

    public void OnFixedUpdate()
    {
        timer = manager.ThisStateTime();
        if (timer >= 1)
        {
            manager.TransitionState(StateTypePlayer.Idle);
        }
    }

    public void OnExit()
    {
        parameter.boxCollider2d.size = new Vector2(parameter.boxCollider2d.size.x, parameter.boxCollider2d.size.y * 2);
        parameter.boxCollider2d.offset = new Vector2(parameter.boxCollider2d.offset.x, parameter.boxCollider2d.offset.y * 2);

        manager.verticalForce -= 1.0f;
    }
}

public class DashPlayerState : TState
{
    private PlayerFSM manager;

    private ParameterPlayer parameter;

    private float timer;

    public DashPlayerState(PlayerFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Dash");
        parameter.animator.speed = 2.0f;

        manager.rigidbody2d.gravityScale = 0;

        AudioManager.PlayerDashAudio();
    }

    public void OnUpdate()
    {
        bool inputJump = manager.JumpPlayer();

        if (inputJump)
        {
            manager.TransitionState(StateTypePlayer.Jump);
        }

        manager.Attack();
    }

    public void OnFixedUpdate()
    {
        manager.Toward();

        manager.rigidbody2d.velocity = new Vector2(manager.rigidbody2d.transform.localScale.x * 5.0f, manager.rigidbody2d.velocity.y);

        timer = manager.ThisStateTime();

        if (timer >= 1.0)
        {
            manager.TransitionState(StateTypePlayer.Idle);
        }

        if (!manager.OnTheGround())
        {
            manager.TransitionState(StateTypePlayer.Fall);
        }
    }

    public void OnExit()
    {
        parameter.animator.speed = 1.0f;

        manager.rigidbody2d.gravityScale = 1;

        parameter.invincibleTime = 0;
    }
}

public class HurtPlayerState : TState
{
    private PlayerFSM manager;

    private ParameterPlayer parameter;

    private float timer;

    public HurtPlayerState(PlayerFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Hurt");

        manager.rigidbody2d.velocity = new Vector2(0, 0);
    }

    public void OnUpdate()
    {
        
    }

    public void OnFixedUpdate()
    {
        timer = manager.ThisStateTime();
        if (timer >= 1)
        {
            manager.TransitionState(StateTypePlayer.Idle);
        }

        if (!manager.OnTheGround() && manager.rigidbody2d.velocity.y < 0)
        {
            manager.TransitionState(StateTypePlayer.Fall);
        }
    }

    public void OnExit()
    {
        parameter.invincibleTime = manager.invincibleTimePart;
    }
}

public class DeathPlayerState : TState
{
    private PlayerFSM manager;

    private ParameterPlayer parameter;

    private bool bol;

    private float timer;

    public DeathPlayerState(PlayerFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Death");

        manager.rigidbody2d.velocity = new Vector2(0, 0);

        
    }

    public void OnUpdate()
    {

        bol = manager.OnTheGround();
        if (bol)
        {
            manager.rigidbody2d.gravityScale = 0;
            manager.parameter.boxCollider2d.enabled = false;
        }

        timer += Time.deltaTime;
        if (timer >= 1.5)
        {
            MainScene mainScene = new MainScene();
            SceneControl.GetInstance().LoadScene(mainScene.SceneName,mainScene);
            manager.TransitionState(StateTypePlayer.Idle);
        }
    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }
}

public class AttackPlayerState : TState
{
    private PlayerFSM manager;

    private ParameterPlayer parameter;

    private string[] attackStates;

    public int arr;

    private float timer;

    private bool attackCheck;

    public AttackPlayerState(PlayerFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        string[] attackStates = { "Attack","Attack1","Dash-Attack"};
        if (parameter.comboTime > 0)
        {
            arr++;
            parameter.comboTime = 0;
            if (arr > 2)
            {
                arr = 0;
            }
        }
        else
        {
            arr = 0;
        }
        
        parameter.animator.Play(attackStates[arr]);
        AudioManager.PlayerAttackAudio(arr);

        manager.rigidbody2d.velocity = new Vector2(0, manager.rigidbody2d.velocity.y);
    }

    public void OnUpdate()
    {

        bool inputJump = manager.JumpPlayer();

        if (inputJump)
        {
            manager.TransitionState(StateTypePlayer.Jump);
        }

        if (Input.GetButtonDown("Attack"))
        {
            attackCheck = true;
        }

        timer = manager.ThisStateTime();
        if (timer >= 1.0f)
        {
            if (attackCheck)
            {
                manager.TransitionState(StateTypePlayer.Attack);
            }
            else
            {
                manager.TransitionState(StateTypePlayer.Idle);
            }
        }

        manager.Dash();

        manager.Toward();
    }

    public void OnFixedUpdate()
    {
        manager.rigidbody2d.velocity = new Vector2(manager.transform.localScale.x * 0.01f, manager.rigidbody2d.velocity.y);

        if (!manager.OnTheGround() && manager.rigidbody2d.velocity.y < 0)
        {
            manager.TransitionState(StateTypePlayer.Fall);
        }
    }

    public void OnExit()
    {
        attackCheck = false;

        parameter.comboTime = manager.comboTimePart;
    }
}

public class SlidePlayerState : TState
{
    private PlayerFSM manager;

    private ParameterPlayer parameter;

    private float timer;

    public SlidePlayerState(PlayerFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Slide");

        parameter.boxCollider2d.size = new Vector2(parameter.boxCollider2d.size.x, parameter.boxCollider2d.size.y / 2);
        parameter.boxCollider2d.offset = new Vector2(parameter.boxCollider2d.offset.x, parameter.boxCollider2d.offset.y / 2);

        parameter.invincibleTime = 0.18f;

        AudioManager.PlayerDashAudio();
    }

    public void OnUpdate()
    {
        bool inputJump = manager.JumpPlayer();

        if (inputJump)
        {
            manager.TransitionState(StateTypePlayer.Jump);
        }

        timer = manager.ThisStateTime();
        if (timer >= 1.0)
        {
            manager.TransitionState(StateTypePlayer.Idle);
        }
    }

    public void OnFixedUpdate()
    {
        manager.rigidbody2d.velocity = new Vector2(manager.rigidbody2d.transform.localScale.x * 6.0f, manager.rigidbody2d.velocity.y);

        if (!manager.OnTheGround() && manager.rigidbody2d.velocity.y < 0)
        {
            manager.TransitionState(StateTypePlayer.Fall);
        }
    }

    public void OnExit()
    {
        parameter.boxCollider2d.size = new Vector2(parameter.boxCollider2d.size.x, parameter.boxCollider2d.size.y * 2);
        parameter.boxCollider2d.offset = new Vector2(parameter.boxCollider2d.offset.x, parameter.boxCollider2d.offset.y * 2);

        parameter.invincibleTime = 0;
    }
}

public class EdgeIdlePlayerState : TState
{
    private PlayerFSM manager;

    private ParameterPlayer parameter;

    private bool isGround;

    private Vector2 offset;
    private Vector2 size;

    public EdgeIdlePlayerState(PlayerFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Edge-Idle");
        manager.rigidbody2d.bodyType = RigidbodyType2D.Static;

        offset = parameter.boxCollider2d.offset;
        size = parameter.boxCollider2d.size;

        parameter.boxCollider2d.offset = new Vector2(-0.04f, 0.53f);
        parameter.boxCollider2d.size = new Vector2(0.35f, parameter.boxCollider2d.size.y);
    }

    public void OnUpdate()
    {
        bool inputJump = manager.JumpPlayer();

        if (inputJump)
        {
            manager.TransitionState(StateTypePlayer.Jump);
        }

        isGround = manager.OnTheGround();

        if (isGround)
        {
            manager.TransitionState(StateTypePlayer.Idle);
        }

        if (Input.GetKeyDown("s"))
        {
            manager.TransitionState(StateTypePlayer.ErrorFall);
        }

        manager.BeforeCheck();
    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnExit()
    {
        manager.rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        manager.transform.position = new Vector3(manager.transform.position.x - manager.transform.localScale.x * 0.2f
            , manager.transform.position.y, manager.transform.position.z);

        parameter.boxCollider2d.offset = offset;
        parameter.boxCollider2d.size = size;
    }
}

public class EdgeGrabPlayerState : TState
{
    private PlayerFSM manager;

    private ParameterPlayer parameter;

    private bool isGround;

    private float timer;

    private Vector2 offset;
    private Vector2 size;

    public EdgeGrabPlayerState(PlayerFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Edge-Grab");

        offset = parameter.boxCollider2d.offset;
        size = parameter.boxCollider2d.size;

        parameter.boxCollider2d.offset = new Vector2(-0.04f, 0.53f);
        parameter.boxCollider2d.size = new Vector2(0.35f, parameter.boxCollider2d.size.y);

        manager.rigidbody2d.bodyType = RigidbodyType2D.Static;

    }

    public void OnUpdate()
    {
        bool inputJump = manager.JumpPlayer();

        if (inputJump)
        {
            manager.TransitionState(StateTypePlayer.Jump);
        }

        isGround = manager.OnTheGround();

        if (isGround)
        {
            manager.TransitionState(StateTypePlayer.Idle);
        }

        timer = manager.ThisStateTime();

        if (timer >= 1.0f)
        {
            manager.TransitionState(StateTypePlayer.ErrorFall);
        }
        else if (Input.GetKeyDown("s"))
        {
            manager.TransitionState(StateTypePlayer.ErrorFall);
        }

        manager.BeforeCheck();
    }

    public void OnFixedUpdate()
    {
        
    }

    public void OnExit()
    {
        manager.rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        manager.transform.position = new Vector3(manager.transform.position.x - manager.transform.localScale.x * 0.2f
            , manager.transform.position.y, manager.transform.position.z);

        parameter.boxCollider2d.offset = offset;
        parameter.boxCollider2d.size = size;
    }
}

public class ErrorFallPlayerState : TState
{
    private PlayerFSM manager;

    private ParameterPlayer parameter;

    private bool isGround;

    public ErrorFallPlayerState(PlayerFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("Fall");
    }

    public void OnUpdate()
    {
        if (manager.OnTheGround())
        {
            manager.TransitionState(StateTypePlayer.Idle);
        }
    }

    public void OnFixedUpdate()
    {
        manager.MovementPlayer();
    }

    public void OnExit()
    {
        
    }
}