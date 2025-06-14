using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Combate")]
    public int damage = 10;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;

    [Header("Detecção")]
    public float detectionRange = 10f;
    public float closeDetectionRange = 3f;  // Área de detecção próxima (360°)
    [Range(0, 360)] public float fieldOfView = 120f;

    [Header("Animação")]
    public float rotationSpeedDuringAttack = 5f;

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

        float distance = Vector3.Distance(transform.position, player.position);

        // Se está atacando, processar estado de ataque
        if (isAttacking)
        {
            HandleAttackState();
            return;
        }

        // Se ainda está em animação de ataque, não fazer nada
        if (IsCurrentlyInAttackAnimation()) return;

        // Se jogador está no alcance de ataque
        if (distance <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
                StartAttack();
            else
                SetIdle();
        }
        // Se jogador está visível OU na área próxima
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
        agent.isStopped = true;
        agent.updateRotation = false;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        // Rotacionar durante o ataque para manter o alvo
        RotateTowardsPlayer(rotationSpeedDuringAttack);

        // Lógica de dano durante a animação
        if (state.IsTag("Attack"))
        {
            // Aplicar dano no ponto certo da animação
            if (!damageApplied && state.normalizedTime >= 0.5f)
            {
                DealDamage();
                damageApplied = true;
            }

            // Final do ataque - decidir próximo estado
            if (state.normalizedTime >= 0.95f)
            {
                EndAttack();
            }
        }
    }

    private void StartAttack()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // Se jogador saiu do alcance antes de iniciar o ataque
        if (distance > attackRange)
        {
            ChasePlayer();
            return;
        }

        // Orientar-se para o jogador antes de atacar
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
        isAttacking = false;
        damageApplied = false;
        agent.updateRotation = true;

        // Verificação de detecção após o ataque
        if (PlayerInSight() || PlayerInCloseRange())
        {
            // Se o jogador está na visão, perseguir
            ChasePlayer();
        }
        else
        {
            // Se não está na visão, ficar em idle
            SetIdle();
        }
    }

    private void SetIdle()
    {
        agent.isStopped = true;
        animator.SetInteger("transition", 0);
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;
        agent.updateRotation = true;
        agent.SetDestination(player.position);
        animator.SetInteger("transition", 1);
    }

    private bool PlayerInCloseRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return distance <= closeDetectionRange;
    }

    private void RotateTowardsPlayer(float speed)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }
    }

    private bool IsCurrentlyInAttackAnimation()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        return state.IsTag("Attack") && state.normalizedTime < 1.0f;
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

    private bool PlayerInSight()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectionRange) return false;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > fieldOfView / 2f) return false;

        // Verificação de obstáculos
        if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out RaycastHit hit, detectionRange))
        {
            if (!hit.transform.CompareTag("Player")) return false;
        }

        return true;
    }

    public void Die()
    {
        isAttacking = false;
        agent.isStopped = true;
        agent.updateRotation = false;

        animator.SetInteger("transition", 5);
        Destroy(gameObject, 3f);
    }

#if UNITY_EDITOR
    // Visualização dos campos de detecção
    private void OnDrawGizmosSelected()
    {
        // Área de detecção próxima (360°)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, closeDetectionRange);

        // Campo de visão principal
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