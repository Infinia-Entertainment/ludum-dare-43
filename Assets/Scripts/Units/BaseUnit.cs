using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Pathfinding;
using System;

public class BaseUnit : Damageable
{
    [SerializeField] public float moveSpeed;
    [SerializeField] public float attackRange, sightRange;
    private float AttackRange;
    [SerializeField] public float attackDelay;
    [SerializeField] public LayerMask enemyMask;

    private Animator animator;

    public int damage;

    public Transform gateTarget;
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
        animator = GetComponent<Animator>();
        gateTarget = FindObjectOfType < Gate>().transform;
        destinationSetter = GetComponent<AIDestinationSetter>();
        _AIPath = GetComponent<AIPath>();
        unitState = UnitState.Idle;
        AttackRange = Mathf.Sqrt(attackRange);

        _AIPath.maxSpeed = moveSpeed;

        OnDeath += UnitDeath;
    }

    public virtual void Update()
    {

        switch (unitState)
        {
            case UnitState.Idle:
                switch (unitControl)
                {
                    case UnitControl.Player:
                        if (!overrideTarget)
                        {
                            target = CheckForEnemies();

                            if (target != null) Attack(target);

                        }
                        break;

                    case UnitControl.AI:

                        if (!overrideTarget)
                        {
                            Transform tempTarget = CheckForEnemies();
                            target = (tempTarget == null) ? gateTarget : tempTarget;

                            if (target != null) Attack(target);

                        }

                        break;
                    default:
                        break;
                }
                break;
            case UnitState.Moving:
                switch (unitControl)
                {
                    case UnitControl.Player:
                        if (_AIPath.reachedDestination)
                        {
                            unitState = UnitState.Idle;
                        }
                        break;
                    case UnitControl.AI:
                        if (_AIPath.reachedDestination)
                        {
                            unitState = UnitState.Idle;
                        }
                        break;
                    default:
                        break;
                }
                break;
            case UnitState.Approaching:
                switch (unitControl)
                {
                    case UnitControl.Player:
                        if (target != null && target.GetComponent<BaseUnit>()?.unitControl != unitControl)
                        {
                            Attack(target);
                        }
                        else if (target == null)
                        {
                            unitState = UnitState.Idle;
                        }
                        break;
                    case UnitControl.AI:
                        if (target != null && target.GetComponent<BaseUnit>()?.unitControl != unitControl)
                        {
                            Attack(target);
                        }
                        else if (target == null)
                        {
                            unitState = UnitState.Idle;
                        }
                        break;
                    default:
                        break;
                }


                break;
            case UnitState.Fighting:
                switch (unitControl)
                {
                    case UnitControl.Player:
                        if (target != null && target.GetComponent<BaseUnit>()?.unitControl != unitControl)
                        {
                            Attack(target);
                        }
                        else if (target == null)
                        {
                            unitState = UnitState.Idle;
                        }
                        break;
                    case UnitControl.AI:
                        if (target != null && target.GetComponent<BaseUnit>()?.unitControl != unitControl)
                        {
                            Attack(target);
                        }
                        else if (target == null)
                        {
                            unitState = UnitState.Idle;
                        }
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }


        if (health<=0)
        {
            OnDeath.Invoke();
        }

        UpdateTarget();
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
            Debug.Log(enemies.Count);
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
        Debug.Log(WithinDistance(transform.position, target.position, AttackRange));
        if (WithinDistance(transform.position, target.position, AttackRange))
        {
            Debug.Log("attacking");
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


    private bool WithinDistance(Vector2 original, Vector2 target, float AttackRange)
    {
        return (original - target).magnitude <= AttackRange;
    }

    private void UpdateTarget()
    {
        //if (unitControl == UnitControl.AI &&target ==null && gateTarget != null)
        //{
        //    target = gateTarget;
        //}
        destinationSetter.target = target;

    }

    public virtual void UnitDeath()
    {
        PlayerUnitContoller.RemoveUnitFromLists(selectionSprite.GetComponent<Collider2D>());
        Destroy(gameObject);
    }

    public void UnselectUnit()
    {
        animator.SetBool("isSelected", false);
    }

    public void SelectUnit()
    {
        animator.SetBool("isSelected",true);
    }
}
