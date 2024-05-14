using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class MeleeGolemAI : Character
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Attacking
    public float timeBetweenAttacks = .5f;
    bool alreadyAttacked;
    public float attackRange = 1f;
    public float chaseRange = 5f;
    public bool playerInAttackRange = false;
    public bool playerInChaseRange  = false;

    [SerializeField] private float scale;
    [SerializeField] private float duration;
    [SerializeField] private GameObject damageBox;

    private Animator animator;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, duration);

        player   = GameObject.Find("Player").transform;
        agent    = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
        animator.SetFloat("atSpeed", _damageTick * 1.5f);

        damageBox.GetComponent<DamageTrigger>().SetTick(_damageTick); // Tells the damage trigger the damage tick rate (this was the simplest way I could think of)
        damageBox.SetActive(false);

        FullHeal();
    }

    private void Update()
    {
        if (!AlreadyDead)
        {
            playerInChaseRange  = Vector3.Distance(player.transform.position, transform.position) <= chaseRange;
            playerInAttackRange = Vector3.Distance(player.transform.position, transform.position) <= attackRange;

            if (FindAnyObjectByType<Unity.AI.Navigation.NavMeshSurface>() != null)
            {
                if (!playerInAttackRange && playerInChaseRange) ChasePlayer();
                else if (playerInAttackRange) AttackPlayer();
                else
                {
                    damageBox.SetActive(false);
                    animator.SetBool("isRunning", false);
                }
            }
        }
    }

    private void ChasePlayer()
    {
        damageBox.SetActive(false);
        animator.SetBool("isRunning", true);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", true);

        // make sure enemy doesnt move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            damageBox.SetActive(true);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public override void DealDamage(Collider col)
    {
        col.SendMessage("TakeDamage", _damage);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public override void Kill()
    {
        agent.enabled = false;

        base.Kill();
    }
}
