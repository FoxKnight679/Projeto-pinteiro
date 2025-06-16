using UnityEngine;

public class PlayerNovo : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;

    public float speed = 5f;
    public float jumpForce = 8f;
    public float gravity = -9.81f;
    private float verticalVelocity;

    public int currentWeaponIndex = 1; // 1 = arma padrão

    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    private float xRotation = 0f;

    private bool isAttacking = false;
    private bool canMove = true;


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

        // Aplica movimento horizontal
        Vector3 movement = move * speed;

        // Aplica velocidade vertical (gravidade ou pulo)
        verticalVelocity += gravity * Time.deltaTime;

        // Aplica salto
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
            anim.SetBool("isJumping", false); // Reset no chão
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            verticalVelocity = jumpForce;
            anim.SetBool("isJumping", true); // Ativa animação de pulo
        }

        movement.y = verticalVelocity;

        controller.Move(movement * Time.deltaTime);

        // Animação de caminhada (apenas se estiver no chão)
        bool isWalking = controller.isGrounded && (moveX != 0 || moveZ != 0);
        anim.SetBool("isWalking", isWalking);
    }

    private void HandleJump()
    {
        // Essa função agora só controla gravidade e animação de pulo
        // A lógica já está centralizada em HandleMovement()
    }

    public void SetWeaponIndex(int index)
    {
        currentWeaponIndex = index;
        anim.SetInteger("WeaponIndex", index);
    }


    private void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;

            if (currentWeaponIndex == 0) // Arma de fogo
            {
                anim.SetTrigger("attack"); // Trigger para atirar
            }
            else if (currentWeaponIndex == 1) // Espada
            {
                anim.SetTrigger("attack"); // Trigger para ataque corpo-a-corpo
            }
        }
    }


    public void EndAttack()
    {
        Debug.Log("Fim do ataque");
        isAttacking = false;
        anim.ResetTrigger("attack");
    }


}
