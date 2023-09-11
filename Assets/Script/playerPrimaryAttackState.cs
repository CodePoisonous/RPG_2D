using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 2f;

    public playerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);

        // 设置攻击时的速度和朝向
        float attackDir = player.facingDir;
        if(xInput != 0) attackDir = xInput;
        player.SetVelocity(
            player.attackMovement[comboCounter].x * attackDir, 
            player.attackMovement[comboCounter].y);

        stateTimer = 0.2f;   // 攻击之前保持一点点的惯性
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.15f);   // 调用BusyFor的协程

        ++comboCounter;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.ZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
