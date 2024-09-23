using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//the new version of unity needs to call this AI to work
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : Lives
{
    public enum State {Idle, Chasing, Attacking};
    State currenState;
    public NavMeshAgent pathFinder;
    public Transform target;
    Lives LivingTarget;

    Material skinMaterial;
    Color originalColor;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;
    float damage = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadious;

    bool hasTarget;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {


            //defalt state chasing
            currenState = State.Chasing;
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("Player").transform;
            LivingTarget = target.GetComponent<Lives>();
            LivingTarget.OnDeath += OnTargetDeath;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadious = target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());
        }
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currenState = State.Idle;
    }
    // Update is called once per frame
    void Update()
    {
        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDsToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDsToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadious, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }
        
       

         //pathFinder.SetDestination(target.position);
    }

    IEnumerator Attack()
    {
        currenState = State.Attacking;
        pathFinder.enabled = false;
        Vector3 oringinalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while(percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                LivingTarget.TakeDamage(damage);
            }
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(oringinalPosition, attackPosition, interpolation);


            yield return null;
        }
        skinMaterial.color = originalColor;
        currenState = State.Chasing;
        pathFinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {
        float refereRate = .25f;

        while (hasTarget)
        {
            if(currenState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadious + attackDistanceThreshold/2);
                if (!dead)
                {
                    pathFinder.SetDestination(targetPosition);
                }
            }
            
            yield return new WaitForSeconds(refereRate);
        }
    }
}
