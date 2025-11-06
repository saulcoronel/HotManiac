using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Ataque")]
    public int damage = 15;
    public float attackRadius = 2f;
    public LayerMask enemyLayer;

    private Animator animator;
    private bool isAttacking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Evita que el jugador pueda atacar mientras la animación anterior no termina
        if ((Mouse.current.leftButton.wasPressedThisFrame || Input.GetMouseButtonDown(0)) && !isAttacking)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (animator != null)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
        }

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Health enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log($"Golpeaste a {enemy.name} causando {damage} de daño.");
            }
        }

        // Espera el fin de la animación antes de permitir otro ataque
        Invoke(nameof(ResetAttack), 0.8f); // Ajusta 0.8f al tiempo real de tu animación
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}