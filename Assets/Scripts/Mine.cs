using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private GameObject boomParticlePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var boom = Instantiate(boomParticlePrefab, transform.position + Vector3.up, Quaternion.identity);
        Destroy(boom, 4f);
    }
}
