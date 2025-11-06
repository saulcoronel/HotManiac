using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float acceleration = 8f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 currentVelocity;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(moveX, 0f, moveZ).normalized;

        // Detectar si está corriendo
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // Actualizar Blend Tree
        animator.SetFloat("DirX", moveX);
        animator.SetFloat("DirY", moveZ);

        // Controlar parámetro Speed (para Blend Tree)
        float targetSpeed = moveInput.magnitude * (isRunning ? runSpeed : walkSpeed);
        animator.SetFloat("Speed", targetSpeed);

        // Rotar suavemente hacia la dirección de movimiento
        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        // Calcular velocidad dependiendo de si corre o camina
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 targetVelocity = moveInput * speed;
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }
}