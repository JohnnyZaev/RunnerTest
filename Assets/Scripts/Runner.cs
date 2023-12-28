using UnityEngine;

public class Runner : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private GameObject deathExplosionPrefab;

    [HideInInspector] public CharactersAligner charactersAligner;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Kill")) return;
        charactersAligner.RemoveCharacter(this);
        var expObj = Instantiate(deathExplosionPrefab, transform.position, UnityEngine.Quaternion.identity);
        Destroy(expObj, 1f);
        Destroy(gameObject, 0.5f);
    }
}
