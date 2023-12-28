using System;
using UnityEngine;

public class NewRunnerCollectable : MonoBehaviour
{
    private CharactersAligner _charactersAligner;

    private void Awake()
    {
        _charactersAligner = FindObjectOfType<CharactersAligner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _charactersAligner.AddCharacter(transform.position);
        Destroy(gameObject);
    }
}
