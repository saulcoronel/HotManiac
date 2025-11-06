using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float acceleration = 8f; // que tan rápido acelera
    public float rotationSpeed = 10f; // que tan rápido gira

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
        // Captura input (WASD o flechas)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(moveX, 0f, moveZ).normalized;

        // Actualizar Blend Tree
        animator.SetFloat("DirX", moveX);
        animator.SetFloat("DirY", moveZ);


        // Rotar suavemente hacia la dirección de movimiento
        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = moveInput * moveSpeed;
   
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }
}
