using System.Collections;
using UnityEngine;

public class PokemonBehavior : MonoBehaviour
{
    public float jumpHeight = 2f; // Altura del salto
    public float attackDistance = 1f; // Distancia hacia el ataque en Z
    public float moveSpeed = 2f; // Velocidad de los movimientos de salto y ataque
    public float jumpChance = 0.3f; // Probabilidad de saltar (30%)
    public float attackChance = 0.2f; // Probabilidad de atacar (20%)

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(RandomBehavior());
    }

    IEnumerator RandomBehavior()
    {
        while (true)
        {
            float randomValue = Random.value;

            if (randomValue <= jumpChance)
            {
                StartCoroutine(Jump());
            }
            else if (randomValue <= jumpChance + attackChance)
            {
                StartCoroutine(Attack());
            }

            yield return new WaitForSeconds(Random.Range(2f, 5f)); // Espera entre comportamientos
        }
    }

    IEnumerator Jump()
    {
        // Subir el Pokémon en el eje Y
        Vector3 targetPosition = new Vector3(originalPosition.x, originalPosition.y + jumpHeight, originalPosition.z);
        while (transform.position.y < targetPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Bajar el Pokémon de vuelta a su posición original
        while (transform.position.y > originalPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Attack()
    {
        // Mover el Pokémon hacia el jugador en el eje Z
        Vector3 targetPosition = new Vector3(originalPosition.x, originalPosition.y, originalPosition.z - attackDistance);
        while (transform.position.z > targetPosition.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Regresar el Pokémon a su posición original
        while (transform.position.z < originalPosition.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}

