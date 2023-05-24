using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum UnitState
{
    Idle,
    WalkToPoint,
    WalkToEnemy,
    Attack
}

public class Knight : Unit
{
    public UnitState CurrentUnitState;

    public Vector3 TargetPoint;
    public Enemy TargetEnemy;

    public float DistanseToFollow = 7f;
    public float DistanseToAttack = 1f;

    public float AttackPeriod = 1f;

    private float _timer;

    public override void Start()
    {
        base.Start();
        SetState(UnitState.WalkToPoint);
    }

    private void Update()
    {
        if (CurrentUnitState == UnitState.Idle)
        {
            FindClosestEnemy();
        }
        else if (CurrentUnitState == UnitState.WalkToPoint)
        {
            FindClosestEnemy();
        }
        else if (CurrentUnitState == UnitState.WalkToEnemy)
        {
            if (TargetEnemy)
            {
                NavMeshAgent.SetDestination(TargetEnemy.transform.position);

                float distance = Vector3.Distance(transform.position, TargetEnemy.transform.position);

                if (distance > DistanseToFollow)
                {
                    SetState(UnitState.WalkToPoint);
                }

                if (distance < DistanseToAttack)
                {
                    SetState(UnitState.Attack);
                }
            }
            else
            {
                SetState(UnitState.WalkToPoint);
            }


        }
        else if (CurrentUnitState == UnitState.Attack)
        {
            if (TargetEnemy)
            {
                NavMeshAgent.SetDestination(TargetEnemy.transform.position);
                float distance = Vector3.Distance(transform.position, TargetEnemy.transform.position);
                if (distance > DistanseToAttack)
                {
                    SetState(UnitState.WalkToEnemy);
                }

                _timer += Time.deltaTime;

                if (_timer > AttackPeriod)
                {
                    _timer = 0;
                    TargetEnemy.TakeDamege(1);
                }
            }
            else
            {
                SetState(UnitState.WalkToPoint);
            }

        }
    }

    public void SetState(UnitState enemyState)
    {
        CurrentUnitState = enemyState;

        if (CurrentUnitState == UnitState.Idle)
        {

        }
        else if (CurrentUnitState == UnitState.WalkToPoint)
        {

        }
        else if (CurrentUnitState == UnitState.WalkToEnemy)
        {

        }
        else if (CurrentUnitState == UnitState.Attack)
        {
            _timer = 0;
        }
    }

    public void FindClosestEnemy()
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        float minDistanse = Mathf.Infinity;
        Enemy closestEnemy = null;

        for (int i = 0; i < allEnemies.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allEnemies[i].transform.position);

            if (distance < minDistanse)
            {
                minDistanse = distance;
                closestEnemy = allEnemies[i];
            }
        }

        if (minDistanse < DistanseToFollow)
        {
            TargetEnemy = closestEnemy;

            SetState(UnitState.WalkToEnemy);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanseToAttack);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanseToFollow);
    }
#endif
}
