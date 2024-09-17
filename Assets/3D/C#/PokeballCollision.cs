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
        Debug.Log("Pok�mon capturado: " + pokemon.name);
        // Aqu� puedes agregar l�gica para animaciones o eliminar el Pok�mon de la escena
        Destroy(pokemon);
    }
}
