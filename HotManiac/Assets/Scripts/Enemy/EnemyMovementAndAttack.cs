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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance <= detectionRadius && distance > attackRange)
        {
            Vector3 moveVelocity = direction.normalized * moveSpeed;
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

            if (moveVelocity != Vector3.zero)
                transform.forward = moveVelocity.normalized;
        }

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"[ENEMY ATTACK] {gameObject.name} atacó al Player causando {damage} de daño. Vida del Player: {playerHealth.currentHealth}");
            }
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}