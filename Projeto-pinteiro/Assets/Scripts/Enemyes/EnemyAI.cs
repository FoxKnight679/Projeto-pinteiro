using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Combate")]
    public int damage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;

    [Header("Internos")]
    private float lastAttackTime;
    private bool isAttacking = false;
    private bool damageApplied = false;
    private int currentAttackTransition = 0;

    private Transform player;
    private Animator animator;
    private NavMeshAgent agent;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (player == null || animator == null || agent == null) return;

        if (isAttacking)
        {
            HandleAttackState();
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
                StartAttack();
            else
                SetIdle();
        }
        else
        {
            ChasePlayer();
        }
    }

    private void HandleAttackState()
    {
        agent.isStopped = true;
        agent.updateRotation = false;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (!damageApplied && state.normalizedTime >= 0.5f && state.IsTag("Attack"))
        {
            DealDamage();
            damageApplied = true;
        }

        if (state.normalizedTime >= 1.0f && state.IsTag("Attack"))
        {
            EndAttack();
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        damageApplied = false;
        lastAttackTime = Time.time;

        agent.isStopped = true;
        agent.updateRotation = false;

        currentAttackTransition = RandomAttack();
        animator.SetInteger("transition", currentAttackTransition);
    }

    private void EndAttack()
    {
        isAttacking = false;
        agent.updateRotation = true;

        SetIdle();
    }

    private void SetIdle()
    {
        agent.isStopped = true;
        animator.SetInteger("transition", 0);
    }

    private void ChasePlayer()
    {
        if (player == null) return;

        agent.isStopped = false;
        agent.updateRotation = true;

        agent.SetDestination(player.position);
        animator.SetInteger("transition", 1); // running
    }

    private void DealDamage()
    {
        player?.GetComponent<PlayerHealth>()?.TakeDamage(damage);
    }

    private int RandomAttack()
    {
        int[] attackTransitions = { 2, 3, 4 };
        return attackTransitions[Random.Range(0, attackTransitions.Length)];
    }

    public void Die()
    {
        isAttacking = false;
        agent.isStopped = true;
        agent.updateRotation = false;

        animator.SetInteger("transition", 5); // Death
        Destroy(gameObject, 3f);
    }
}
