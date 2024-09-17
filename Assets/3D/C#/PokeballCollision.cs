using UnityEngine;

public class PokeballCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Pokemon"))
        {
            CapturePokemon(other.gameObject);
        }
    }

    void CapturePokemon(GameObject pokemon)
    {
        Debug.Log("Pokémon capturado: " + pokemon.name);
        // Aquí puedes agregar lógica para animaciones o eliminar el Pokémon de la escena
        Destroy(pokemon);
    }
}
