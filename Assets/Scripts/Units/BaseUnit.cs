using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Pathfinding;
using System;

public abstract class BaseUnit : Damageable
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float attackRange, sightRange;
    private float sqrAttackRange;
    [SerializeField] public float attackDelay;
    [SerializeField] public LayerMask enemyMask;

    public int damage;

    public Transform target;
    public bool overrideTarget = false;
    public bool isDamagingEnemy;
    AIDestinationSetter destinationSetter;
    AIPath _AIPath;

    [SerializeField] private GameObject selectionSpriteObject; // this is an object that has selection image and collider for selection.
    public SpriteRenderer selectionSprite;

    #region enums
    public enum UnitState
    {
        Idle,
        Moving,
        Approaching,
        Fighting
    }
    public enum UnitControl
    {
        Player,
        AI
    }

    [SerializeField] public UnitState unitState;
    [SerializeField] public UnitControl unitControl;
    #endregion

    #region events
    public delegate void VoidDelegate();
    public VoidDelegate OnTargetReach;
    public VoidDelegate OnDeath;
    #endregion


    public virtual void Start()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        _AIPath = GetComponent<AIPath>();
        unitState = UnitState.Idle;
        sqrAttackRange = Mathf.Sqrt(attackRange);

        OnDeath += UnitDeath;
    }

    public virtual void Update()
    {

        switch (unitState)
        {
            case UnitState.Idle:
                if (!overrideTarget)
                {
                    target = CheckForEnemies();

                    if (target != null)
                    {
                        Attack(target);
                    }

                }
                break;
            case UnitState.Moving:
                //nothing happends because player makes it move
                if (_AIPath.reachedDestination )
                {
                    unitState = UnitState.Idle;
                }

                break;
            case UnitState.Approaching:
                if (target != null && target.GetComponent<BaseUnit>()?.unitControl != unitControl)
                {
                    Attack(target);
                }
                if (target == null)
                {
                    unitState = UnitState.Idle;
                }

                break;
            case UnitState.Fighting:
                if (target != null && target.GetComponent<BaseUnit>()?.unitControl != unitControl)
                {
                    Attack(target);
                }

                if (target == null)
                {
                    unitState = UnitState.Idle;
                }

                break;
            default:
                break;
        }

        UpdateTarget();

        if (health<=0)
        {
            OnDeath.Invoke();
        }
    }

    public virtual Transform CheckForEnemies()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, sightRange, enemyMask);
        List<GameObject> enemies = new List<GameObject>();

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject?.transform != transform)
            {
                enemies.Add(collider.gameObject);
            }
        }

        Transform temptarget = transform;
        if (enemies.Count > 0)
        {
            List<float> distancesList = new List<float>();
            foreach (GameObject enemy in enemies)
            {
                distancesList.Add(Vector3.Distance(transform.position, enemy.transform.position));
            }

            var target = enemies?[distancesList.IndexOf(distancesList.Min())].GetComponentInParent<Damageable>().transform;
            return target;
        }
        else return null;
    }

    public virtual void Attack(Transform target)
    {
        if (SqrWithinDistance(transform.position, target.position, sqrAttackRange))
        {

            unitState = UnitState.Fighting;

            if (!isDamagingEnemy)
            {
                StartCoroutine(Damage(target, damage, attackDelay));
            }

        }
        else
        {
            ApproachEnemy(target);
        }
    }

    //public virtual void MoveUnitToPosition

    public virtual void ApproachEnemy(Transform targetToAttack)
    {
        if (overrideTarget)
        {
            unitState = UnitState.Approaching;
            target = targetToAttack;
        }
    }

    public virtual IEnumerator Damage(Transform target, int damage, float attackDelay)
    {
        isDamagingEnemy = true;
        target.GetComponent<Damageable>().health -= damage;

        Debug.Log("target is damaged");

        yield return new WaitForSeconds(attackDelay);
        isDamagingEnemy = false;
    }


    private bool SqrWithinDistance(Vector3 original, Vector3 target, float sqrAttackRange)
    {
        return (original - target).sqrMagnitude <= sqrAttackRange;
    }

    private void UpdateTarget()
    {
        destinationSetter.target = target;

    }

    public virtual void UnitDeath()
    {
        Destroy(gameObject);
    }
}
