using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.limphus.utilities;
using com.limphus.extraction_shooter;

public class AIAnimation : AnimationHandler
{
    [SerializeField] protected EnemyStats enemyStats;
    [SerializeField] protected AIBase ai;



    protected const string SPEED = "Speed";
    protected const string IS_MOVING = "isMoving";

    protected const string ATTACK = "Attack";
    protected const string ATTACK_BLEND = "AttackBlend";
    protected const string KILL = "Kill";



    protected override void Init()
    {
        if (!enemyStats) return;

        enemyStats.OnKill += EnemyStats_OnKill;

        if (!ai) return;

        ai.OnMoveChanged += Ai_OnMoveChanged;
        ai.OnStartAttack += Ai_OnStartAttack;
        ai.OnEndAttack += Ai_OnEndAttack;
    }

    private void Ai_OnStartAttack(object sender, System.EventArgs e)
    {
        SetTrigger(ATTACK, true);

        SetParamater(ATTACK_BLEND, (float)Random.Range(0, 3)); //can use this for randomized attacks via blend tree
    }

    private void Ai_OnEndAttack(object sender, System.EventArgs e)
    {
        SetTrigger(ATTACK, false);
    }

    private void Ai_OnMoveChanged(object sender, Events.OnBoolChangedEventArgs e)
    {
        SetParamater(IS_MOVING, e.i);
    }

    private void EnemyStats_OnKill(object sender, System.EventArgs e)
    {
        //trigger a death animation (potentially choose a random one)

        SetTrigger(KILL, true);
    }
}
