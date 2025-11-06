using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovementAndAttack : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 3f;
    public float detectionRadius = 10f;

    [Header("Ataque")]
    public int damage = 10;
    public float attackRange = 1.5f;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;

    private Rigidbody rb;
    private Transform player;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        float distance = direction.magnitude;

        // --- Movimiento hacia el jugador ---
        if (distance <= detectionRadius && distance > attackRange)
        {
            MoveTowardsPlayer(direction);
        }
        else
        {
            StopMoving();
        }

        // --- Ataque ---
        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
        }
    }

    private void MoveTowardsPlayer(Vector3 direction)
    {
        Vector3 moveVelocity = direction.normalized * moveSpeed;
        rb.MovePosition(rb.position + moveVelocity * Time.deltaTime);

        transform.forward = direction.normalized;

        // Actualizar Blend Tree del enemigo
        animator.SetFloat("DirX", 0f);
        animator.SetFloat("DirY", 1f); // 1 = avanzando al frente
    }

    private void StopMoving()
    {
        animator.SetFloat("DirX", 0f);
        animator.SetFloat("DirY", 0f);
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");

        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log($"[ENEMY ATTACK] {gameObject.name} atacó al Player causando {damage} de daño.");
        }

        nextAttackTime = Time.time + 1f / attackRate;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}