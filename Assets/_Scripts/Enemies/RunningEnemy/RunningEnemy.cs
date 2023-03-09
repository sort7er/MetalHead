using UnityEngine;

public class RunningEnemy : MonoBehaviour
{
    public EnemyRunState runState = new EnemyRunState();
    public EnemyStunnedState stunnedState = new EnemyStunnedState();
    public EnemyDieState dieState = new EnemyDieState();
    public EnemyAttackState attackState = new EnemyAttackState();
    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemySusState susState = new EnemySusState();
    public EnemyDodgeState dodgeState = new EnemyDodgeState();
    public EnemySearchingState searchingState = new EnemySearchingState();
    public EnemyKickState kickState = new EnemyKickState();

    private int state;

    private EnemyBaseState currentState;



    private void Start()
    {
        SwitchState(idleState);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void testState()
    {
        state++;
        if(state == 0)
        {
            SwitchState(idleState);
        }
        else if (state == 1)
        {
            SwitchState(susState);
        }
        else if (state == 2)
        {
            SwitchState(runState);
        }
        else if (state == 3)
        {
            SwitchState(dodgeState);
        }
        else if (state == 4)
        {
            SwitchState(kickState);
        }
        else if (state == 5)
        {
            SwitchState(attackState);
        }
        else if (state == 6)
        {
            SwitchState(stunnedState);
        }
        else if (state == 7)
        {
            SwitchState(searchingState);
        }
        else if (state == 8)
        {
            SwitchState(dieState);
        }
        else
        {
            Debug.Log(currentState);
        }
    }
}
