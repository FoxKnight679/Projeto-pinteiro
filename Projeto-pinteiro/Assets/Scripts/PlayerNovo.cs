using UnityEngine;

public class PlayerNovo : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;

    [Header("Movimento")]
    public float speed = 7f;
    public float jumpForce = 8f;
    public float gravity = -9.81f;
    private float verticalVelocity;

    [Header("Armas e Combate")]
    public int currentWeaponIndex = 1; 
    public int attackDamage = 25;
    public float attackRange = 2f;
    public LayerMask enemyLayer;
    public Transform attackPoint;

    [Header("Arma de Fogo")]
    public float fireRange = 100f;
    public Transform firePoint;

    [Header("Câmera e Mouse Look")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    private float xRotation = 0f;

    private bool isAttacking = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (GameOverManager.isGameOver) return;

        HandleMouseLook();
        HandleMovement();
        HandleAttack();

        if (firePoint == null)
            UpdateFirePointReference();

#if UNITY_EDITOR
        DebugFirePoint();
#endif
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        Vector3 movement = move * speed;
        verticalVelocity += gravity * Time.deltaTime;

        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
            anim.SetBool("isJumping", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            verticalVelocity = jumpForce;
            anim.SetBool("isJumping", true);
        }

        movement.y = verticalVelocity;
        controller.Move(movement * Time.deltaTime);

        bool isWalking = controller.isGrounded && (moveX != 0 || moveZ != 0);
        anim.SetBool("isWalking", isWalking);
    }

    private void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1") && !isAttacking && controller.isGrounded)
        {
            isAttacking = true;
            anim.SetTrigger("attack");
        }
    }

    public void DealDamage()
    {
        if (attackPoint == null) return;

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        Debug.Log($"Inimigos atingidos: {hitEnemies.Length}");

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out EnemyAI enemyAI))
            {
                Debug.Log($"Dano ao inimigo: {enemy.name}");
                enemyAI.TakeDamage(attackDamage);
            }
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
        anim.ResetTrigger("attack");
    }

    public void SetWeaponIndex(int index)
    {
        currentWeaponIndex = index;
        anim.SetInteger("WeaponIndex", index);

        firePoint = null;
        UpdateFirePointReference();
    }

    public void Shoot()
    {
        if (currentWeaponIndex != 0 || firePoint == null) return;

        Ray ray = new Ray(firePoint.position, firePoint.forward);
        Debug.DrawRay(ray.origin, ray.direction * fireRange, Color.yellow, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit, fireRange, enemyLayer))
        {
            Debug.Log($"Tiro acertou: {hit.collider.name}");

            if (hit.collider.TryGetComponent(out EnemyAI enemy))
            {
                enemy.TakeDamage(attackDamage);
            }
        }
        else
        {
            Debug.Log("Tiro não acertou nada.");
        }
    }

    private void UpdateFirePointReference()
    {
        GameObject found = GameObject.FindGameObjectWithTag("firePointTag");
        if (found != null)
        {
            firePoint = found.transform;
            Debug.Log("Novo firePoint encontrado por tag.");
        }
    }

    private void DebugFirePoint()
    {
        if (firePoint == null)
        {
            Debug.LogWarning("firePoint está nulo!");
            return;
        }

        Debug.DrawRay(firePoint.position, firePoint.forward * fireRange, Color.green);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        if (cameraTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * attackRange);
        }

        if (firePoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(firePoint.position, firePoint.forward * fireRange);
        }
    }
}