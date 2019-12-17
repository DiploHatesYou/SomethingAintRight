using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats.health = 0;
        }

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().health = 0;
        }
    }
}
