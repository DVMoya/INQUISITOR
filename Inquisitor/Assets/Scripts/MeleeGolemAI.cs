using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeGolemAI : Character
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Attacking
    public float timeBetweenAttacks = .5f;
    bool alreadyAttacked;
    public float attackRange = 5f;
    public bool playerInRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        FullHeal();
    }

    private void Update()
    {
        playerInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        if(FindAnyObjectByType<Unity.AI.Navigation.NavMeshSurface>() != null)
        {
            if (!playerInRange) ChasePlayer();
            else AttackPlayer();
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        Debug.Log("attack");

        // make sure enemy doesnt move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // ATTACK CODE HERE

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public override void DealDamage(Collider col)
    {
        throw new System.NotImplementedException(); // Dummy's cant´deal damage
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
