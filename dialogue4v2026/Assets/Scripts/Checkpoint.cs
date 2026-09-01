using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnPoint;

    private bool activated;

    private void Start()
    {
        respawnPoint = this.transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (activated)
            return;

        activated = true;

        Debug.Log("Checkpoint ativado!");
    }
}