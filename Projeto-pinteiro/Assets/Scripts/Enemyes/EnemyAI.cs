using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Combate")]
    public int damage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;

    [Header("Detec��o")]
    public float detectionRange = 10f;
    public float closeDetectionRange = 3f;
    [Range(0, 360)] public float fieldOfView = 120f;

    [Header("Anima��o")]
    public float rotationSpeedDuringAttack = 5f;

    private float lastAttackTime;
    private bool isAttacking = false;
    private bool damageApplied = false;
    private int currentAttackTransition = 0;

    private Transform player;
    private Animator animator;
    private NavMeshAgent agent;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isDead) return;
        if (player == null || animator == null || agent == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (isAttacking)
        {
            HandleAttackState();
            return;
        }

        if (IsCurrentlyInAttackAnimation()) return;

        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            StartAttack();
        }
        else if (PlayerInSight() || PlayerInCloseRange())
        {
            ChasePlayer();
        }
        else
        {
            SetIdle();
        }
    }

    private void HandleAttackState()
    {
        if (isDead) return;

        agent.isStopped = true;
        agent.updateRotation = false;
        RotateTowardsPlayer(rotationSpeedDuringAttack);

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsTag("Attack"))
        {
            if (!damageApplied && state.normalizedTime >= 0.5f)
            {
                float distToPlayer = Vector3.Distance(transform.position, player.position);
                if (distToPlayer <= attackRange)
                {
                    Vector3 direction = (player.position - transform.position).normalized;
                    Vector3 rayOrigin = transform.position + Vector3.up * 1.5f; // ajuste de altura
                    if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, attackRange))
                    {
                        if (hit.transform.CompareTag("Player"))
                        {
                            player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
                        }
                    }
                }
                damageApplied = true;
            }

            if (state.normalizedTime >= 0.95f)
            {
                EndAttack();
            }
        }
    }

    private void StartAttack()
    {
        if (isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackRange)
        {
            ChasePlayer();
            return;
        }

        RotateTowardsPlayer(rotationSpeedDuringAttack * 2f);

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
        if (isDead) return;

        isAttacking = false;
        damageApplied = false;
        agent.updateRotation = true;

        if (PlayerInSight() || PlayerInCloseRange())
        {
            ChasePlayer();
        }
        else
        {
            SetIdle();
        }
    }

    private void SetIdle()
    {
        if (isDead) return;

        agent.isStopped = true;
        animator.SetInteger("transition", 0);
    }

    private void ChasePlayer()
    {
        if (isDead) return;

        agent.isStopped = false;
        agent.updateRotation = true;

        if (!agent.SetDestination(player.position))
            Debug.LogWarning("Falha ao definir destino do inimigo!");

        animator.SetInteger("transition", 1);
    }

    private bool PlayerInCloseRange()
    {
        if (isDead) return false;
        return Vector3.Distance(transform.position, player.position) <= closeDetectionRange;
    }

    private void RotateTowardsPlayer(float speed)
    {
        if (isDead) return;

        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }
    }

    private bool IsCurrentlyInAttackAnimation()
    {
        if (isDead) return false;
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        return state.IsTag("Attack") && state.normalizedTime < 1.0f;
    }

    private int RandomAttack()
    {
        return new int[] { 2, 3, 4 }[Random.Range(0, 3)];
    }

    private bool PlayerInSight()
    {
        if (isDead) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectionRange) return false;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > fieldOfView / 2f) return false;

        if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out RaycastHit hit, detectionRange))
        {
            if (!hit.transform.CompareTag("Player")) return false;
        }

        return true;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        Debug.Log("Inimigo recebeu dano: " + amount);
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        isAttacking = false;

        agent.isStopped = true;
        agent.updateRotation = false;
        agent.velocity = Vector3.zero;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        animator.SetInteger("transition", 5);

        Destroy(gameObject, 4.5f);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, closeDetectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Vector3 leftLimit = Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward;
        Vector3 rightLimit = Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftLimit * detectionRange);
        Gizmos.DrawRay(transform.position, rightLimit * detectionRange);
    }
#endif
}
