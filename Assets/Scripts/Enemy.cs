using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    WalkToBulding,
    WalkToUnit,
    Attack
} 

public class Enemy : MonoBehaviour
{
    public EnemyState CurrentEnemyState;

    public int Health;
    public Bilding TargetBulding;
    public Unit TargetUnit;

    public float DistanseToFollow = 7f;
    public float DistanseToAttack = 1f;

    public NavMeshAgent NavMeshAgent;

    public float AttackPeriod = 1f;

    public GameObject HealthBarPrefab;
    private HealthBar _healthBar;
    private int _maxHealth;

    private float _timer;

    private void Start()
    {
        SetState(EnemyState.WalkToBulding);

        _maxHealth = Health;
        GameObject healthBar = Instantiate(HealthBarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
    }

    private void Update()
    {
        if(CurrentEnemyState == EnemyState.Idle)
        {
            FindClosestBuilding();

            if (TargetBulding)
            {
                SetState(EnemyState.WalkToBulding);
            }

            FindClosestUnit();

            if(TargetBulding == null)
            {
                SetState(EnemyState.Idle);
            }
        }
        else if(CurrentEnemyState == EnemyState.WalkToBulding)
        {
            FindClosestUnit();
        }
        else if(CurrentEnemyState == EnemyState.WalkToUnit)
        {
            if (TargetUnit)
            {
                NavMeshAgent.SetDestination(TargetUnit.transform.position);
                float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distance > DistanseToFollow)
                {
                    SetState(EnemyState.WalkToBulding);
                }

                if (distance < DistanseToAttack)
                {
                    SetState(EnemyState.Attack);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBulding);
            }


        }else if( CurrentEnemyState == EnemyState.Attack)
        {
            if(TargetUnit)
            {
                NavMeshAgent.SetDestination(TargetUnit.transform.position);

                float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distance > DistanseToAttack)
                {
                    SetState(EnemyState.WalkToUnit);
                }

                _timer += Time.deltaTime;

                if (_timer > AttackPeriod)
                {
                    _timer = 0;
                    TargetUnit.TakeDamege(1);
                }
            }
            else
            {
                SetState(EnemyState.WalkToBulding);
            }
                
        }
    }

    public void SetState(EnemyState enemyState)
    {
        CurrentEnemyState = enemyState;

        if (CurrentEnemyState == EnemyState.Idle)
        {

        }
        else if (CurrentEnemyState == EnemyState.WalkToBulding)
        {
            FindClosestBuilding();

            if(TargetBulding)
            {
                NavMeshAgent.SetDestination(TargetBulding.transform.position);
            }
            else
            {
                SetState(EnemyState.Idle);
            }
        }
        else if (CurrentEnemyState == EnemyState.WalkToUnit)
        {

        }
        else if (CurrentEnemyState == EnemyState.Attack)
        {
            _timer = 0;
        }
    }

    public void FindClosestBuilding()
    {
        Bilding[] allBuilding = FindObjectsOfType<Bilding>();

        float minDistanse = Mathf.Infinity;
        Bilding closestBuilding = null;

        for (int i = 0; i < allBuilding.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allBuilding[i].transform.position);

            if(distance < minDistanse)
            {
                minDistanse = distance;
                closestBuilding = allBuilding[i];
            }
        }

        TargetBulding = closestBuilding;
    }

    public void FindClosestUnit()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();

        float minDistanse = Mathf.Infinity;
        Unit closestUnit = null;

        for (int i = 0; i < allUnits.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);

            if (distance < minDistanse)
            {
                minDistanse = distance;
                closestUnit = allUnits[i];
            }
        }

        if(minDistanse <DistanseToFollow)
        {
            TargetUnit = closestUnit;

            SetState(EnemyState.WalkToUnit);
        }
    }

    public void TakeDamege(int damageValue)
    {
        Health -= damageValue;
        _healthBar.SetHealth(Health, _maxHealth);

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_healthBar)
        {
            Destroy(_healthBar.gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up,DistanseToAttack);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanseToFollow);
    }
#endif
}
