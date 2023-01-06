using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    [SerializeField] Enemy Enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
            Enemy.TakeDamage();
    }
}
