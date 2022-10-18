using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IdleState : IState
{
    private FSM manager;

    private Parameter parameter;

    private float timer;

    public IdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("idle");

        parameter.idleTime = Random.Range(1f, 3f);
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        
        if (timer >= parameter.idleTime)
        {
            manager.TransitionState(StateType.Walk);
        }

        manager.CheckPlayer(3.0f, 1.0f);
    }

    public void OnExit()
    {
        timer = 0;
    }
}

public class WalkState : IState
{
    private FSM manager;

    private Parameter parameter;

    private int patrolPosition;


    public WalkState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("run");

    }

    public void OnUpdate()
    {
        manager.FlipTO(parameter.patrolPoints[patrolPosition]);

        manager.CheckPlayer(3.0f, 1.0f);

        manager.transform.position = Vector2.MoveTowards(manager.transform.position,
            parameter.patrolPoints[patrolPosition].position, parameter.moveSpeed * Time.deltaTime);

        if (Vector2.Distance(manager.transform.position, parameter.patrolPoints[patrolPosition].position) < .1f)
        {
            manager.TransitionState(StateType.Idle);
        }
;
    }

    public void OnExit()
    {
        patrolPosition++;

        if (patrolPosition >= parameter.patrolPoints.Length)
        {
            patrolPosition = 0;
        }
    }
}

public class ChaseToWalkState : IState
{
    private FSM manager;

    private Parameter parameter;

    private int patrolPosition;

    public ChaseToWalkState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("run");
    }

    public void OnUpdate()
    {
        manager.FlipTO(parameter.patrolPoints[patrolPosition]);

        manager.CheckPlayer(0f, 0f);

        manager.transform.position = Vector2.MoveTowards(manager.transform.position,
            parameter.patrolPoints[patrolPosition].position, parameter.moveSpeed * Time.deltaTime);

        if (Vector2.Distance(manager.transform.position, parameter.patrolPoints[patrolPosition].position) < .1f)
        {
            manager.TransitionState(StateType.Idle);
        }
    }

    public void OnExit()
    {
        
    }
}

public class ChaseState : IState
{
    private FSM manager;

    private Parameter parameter;

    private float axisX;

    
    public ChaseState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("run");

        
    }

    public void OnUpdate()
    {

        manager.FlipTO(parameter.transformPlayer);

        manager.CheckPlayer(3.0f, 1.0f);

        if (Vector2.Distance(manager.transform.position, parameter.transformPlayer.position) > 0.7f )
        {
            axisX = Mathf.MoveTowards(manager.transform.position.x,
            parameter.transformPlayer.position.x, parameter.chaseSpeed * Time.deltaTime);

            manager.transform.position = new Vector3(axisX, manager.transform.position.y, manager.transform.position.z);
        }
        

        if (parameter.AttackSpace >= 0)
        {
            parameter.AttackSpace -= Time.deltaTime;
        }
        if (Vector2.Distance(manager.transform.position, parameter.transformPlayer.position) < 0.9f && parameter.AttackSpace <= 0)
        {
            manager.TransitionState(StateType.Attack);
        }

        
        if (Vector2.Distance(manager.transform.position, parameter.chasePoints[0].position) < .1f
            || Vector2.Distance(manager.transform.position, parameter.chasePoints[1].position) < .1f
            || Vector2.Distance(manager.transform.position, parameter.transformPlayer.position) > 3.3f)
        {
            manager.TransitionState(StateType.ChaseToWalk);
        }
;
    }

    public void OnExit()
    {
        
    }
}

public class AttackState : IState
{
    private FSM manager;

    private Parameter parameter;

    private float timer;

    private float axisX;

    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("attack");
        timer = manager.baseAttackTime;
    }

    public void OnUpdate()
    {
        manager.FlipTO(parameter.transformPlayer);

        timer = parameter.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;


        if (timer >= 1f)
        {
            manager.TransitionState(StateType.Attack1);
        }

        if (Vector2.Distance(manager.transform.position, parameter.transformPlayer.position) > 0.7f)
        {
            axisX = Mathf.MoveTowards(manager.transform.position.x, parameter.transformPlayer.position.x, 0.05f * Time.deltaTime);
            manager.transform.position = new Vector3(axisX, manager.transform.position.y, manager.transform.position.z);
        }

        if (Vector2.Distance(manager.transform.position, parameter.transformPlayer.position) > 1.5f)
        {
            manager.TransitionState(StateType.Chase);
        }
    }

    public void OnExit()
    {
        
    }
}

public class Attack1State : IState
{
    private FSM manager;

    private Parameter parameter;

    private float timer;

    private float axisX;
    public Attack1State(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("attack1");

        timer = manager.baseAttackTime;
    }

    public void OnUpdate()
    {
        manager.FlipTO(parameter.transformPlayer);

        timer = parameter.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (timer >= 1.0f)
        {
            manager.TransitionState(StateType.Chase);
        }

        if (Vector2.Distance(manager.transform.position, parameter.transformPlayer.position) > 0.7f)
        {
            axisX = Mathf.MoveTowards(manager.transform.position.x, parameter.transformPlayer.position.x, 0.05f * Time.deltaTime);
            manager.transform.position = new Vector3(axisX, manager.transform.position.y, manager.transform.position.z);
        }

        if (Vector2.Distance(manager.transform.position, parameter.transformPlayer.position) > 1.5f)
        {
            manager.TransitionState(StateType.Chase);
        }
    }

    public void OnExit()
    {
        parameter.AttackSpace = 0.5f;
    }
}

public class HurtState : IState
{
    private FSM manager;

    private Parameter parameter;

    private int patrolPosition;

    private float timer;

    public HurtState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        timer = parameter.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        if (timer >= 1.0f)
        {
            manager.TransitionState(StateType.Idle);
        }
    }

    public void OnExit()
    {
        Debug.Log(manager.currentHealth);
    }
}

public class DeathState : IState
{
    private FSM manager;

    private Parameter parameter;

    private float timer;

    public DeathState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }
    public void OnEnter()
    {
        parameter.animator.Play("death");
        manager.tag = "Death";
        manager.gameObject.layer = 17;
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}



