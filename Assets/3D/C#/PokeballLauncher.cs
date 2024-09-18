using UnityEngine;
using UnityEngine.InputSystem;

public class PokeballLauncher : MonoBehaviour
{
    public ObjectPool objectPool;  
    public float forceMultiplier = 1f;
    public float curveTorque = 5f;
    public float curveThreshold = 100f;
    public ParticleSystem curveThrowEffect;

    private GameObject currentPokeball;
    public Rigidbody pokeballRigidbody;
    private bool isDragging = false;
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        pokeballRigidbody.useGravity = false;
    }

    void Update()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(touchPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Pokeball") && !isDragging)
                {
                    isDragging = true;
                    startTouchPos = touchPos;
                    currentPokeball = hit.collider.gameObject;
                    pokeballRigidbody = currentPokeball.GetComponent<Rigidbody>();
                }
            }

            if (isDragging)
            {
                // Mover la pokebola mientras el jugador arrastra
                Vector3 screenPosition = new Vector3(touchPos.x, touchPos.y, mainCamera.WorldToScreenPoint(currentPokeball.transform.position).z);
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
                pokeballRigidbody.MovePosition(worldPosition);
            }
        }
        else if (Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended && isDragging)
        {
            // Soltar la pokebola y lanzarla
            endTouchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector2 swipeDirection = endTouchPos - startTouchPos;

            LaunchPokeball(swipeDirection);
            isDragging = false;
        }
    }

    void LaunchPokeball(Vector2 direction)
    {
        // Aplicar fuerza para lanzar la pokebola
        Vector3 force = new Vector3(0f, 1f, 1.0f) * forceMultiplier;
        pokeballRigidbody.AddForce(force, ForceMode.Impulse);

        // Si es un tiro con curva
        if (IsCurveThrow(startTouchPos, endTouchPos))
        {
            pokeballRigidbody.AddTorque(Vector3.forward * curveTorque, ForceMode.Impulse);
            if (curveThrowEffect != null)
            {
                curveThrowEffect.Play();
            }
        }
        pokeballRigidbody.useGravity = true;
        // Regresar la pokebola al pool después de 5 segundos
        Invoke(nameof(ReturnPokeballToPool), 5f);
    }

    bool IsCurveThrow(Vector2 start, Vector2 end)
    {
        return Mathf.Abs(end.x - start.x) > curveThreshold;
    }

    void ReturnPokeballToPool()
    {
        pokeballRigidbody.velocity = Vector3.zero; // Detener cualquier movimiento de la pokebola
        pokeballRigidbody.angularVelocity = Vector3.zero; // Detener la rotación
        objectPool.ReturnObjectToPool(currentPokeball); // Regresar al poolw
        print("qwedwda");
    }

    public void SpawnPokeball()
    {
        // Obtener una nueva pokebola del pool
        currentPokeball = objectPool.GetPooledObject();
        pokeballRigidbody = currentPokeball.GetComponent<Rigidbody>();
        transform.position = new Vector3(5.230001f, 2.42f, 0.7799997f);
    }
}



