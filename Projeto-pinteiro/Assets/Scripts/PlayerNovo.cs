using UnityEngine;

public class PlayerNovo : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;

    public float speed = 5f;
    public float jumpForce = 8f;
    public float gravity = -9.81f;
    private float verticalVelocity;

    public int currentWeaponIndex = 1;
    public int attackDamage = 25;
    public float attackRange = 2f;
    public LayerMask enemyLayer;
    public Transform attackPoint;

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
        HandleMouseLook();
        HandleMovement();
        HandleJump();
        HandleAttack();
        anim.SetInteger("WeaponIndex", currentWeaponIndex);
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

    private void HandleJump() { }

    public void SetWeaponIndex(int index)
    {
        currentWeaponIndex = index;
        anim.SetInteger("WeaponIndex", index);
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
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        Debug.Log($"Inimigos atingidos: {hitEnemies.Length}");
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out EnemyAI enemyAI))
            {
                Debug.Log($"Dando dano ao inimigo: {enemy.name}");
                enemyAI.TakeDamage(attackDamage);
            }
        }
    }

    public void EndAttack()
    {
        Debug.Log("Fim do ataque");
        isAttacking = false;
        anim.ResetTrigger("attack");
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        if (cameraTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * attackRange);
        }
    }
}