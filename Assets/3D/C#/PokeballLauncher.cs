using UnityEngine;
using UnityEngine.InputSystem;

public class PokeballLauncher : MonoBehaviour
{
    public GameObject pokeballPrefab;
    public float forceMultiplier = 1f;
    public float curveTorque = 5f;
    public float curveThreshold = 100f;
    public ParticleSystem curveThrowEffect;

    private GameObject currentPokeball;
    private Rigidbody pokeballRigidbody;
    private bool isDragging = false;
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private Camera mainCamera;

    [SerializeField] private float rotationSpeed = 0f; // Velocidad de rotación
    [SerializeField] private float rotationAcceleration = 100f; // Aceleración de la rotación
    [SerializeField] private float maxRotationSpeed = 500f; // Velocidad máxima de rotación

    void Start()
    {
        mainCamera = Camera.main;
        SpawnPokeball();
        pokeballPrefab.GetComponent<Rigidbody>().useGravity = false;
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
                Vector3 screenPosition = new Vector3(touchPos.x, touchPos.y, mainCamera.WorldToScreenPoint(currentPokeball.transform.position).z);
                Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
                pokeballRigidbody.MovePosition(worldPosition);

                // Incrementar la velocidad de rotación mientras arrastramos
                rotationSpeed = Mathf.Min(rotationSpeed + rotationAcceleration * Time.deltaTime, maxRotationSpeed);

                // Aplicar rotación a la pokebola en el eje Z
                pokeballRigidbody.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            }
        }
        else if (Touchscreen.current.primaryTouch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended && isDragging)
        {
            endTouchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector2 swipeDirection = endTouchPos - startTouchPos;

            LaunchPokeball(swipeDirection);
            isDragging = false;
            rotationSpeed = 0f; // Resetear la velocidad de rotación al soltar
        }
    }

    void LaunchPokeball(Vector2 direction)
    {
        Vector3 force = new Vector3(0f, 1f, 1.0f) * forceMultiplier;
        pokeballRigidbody.AddForce(force, ForceMode.Impulse);

        bool isCurve = IsCurveThrow(startTouchPos, endTouchPos);
        if (isCurve)
        {
            pokeballRigidbody.AddTorque(Vector3.forward * curveTorque, ForceMode.Impulse);

            if (curveThrowEffect != null)
            {
                curveThrowEffect.gameObject.SetActive(true);
                curveThrowEffect.Play();
            }
        }
        else
        {
            if (curveThrowEffect != null && curveThrowEffect.isPlaying)
            {
                curveThrowEffect.Stop();
                curveThrowEffect.gameObject.SetActive(false);
            }
        }
        pokeballRigidbody.GetComponent<Rigidbody>().useGravity = true;

        Destroy(currentPokeball, 5f);
        Invoke(nameof(SpawnPokeball), 6f);
    }

    bool IsCurveThrow(Vector2 start, Vector2 end)
    {
        return Mathf.Abs(end.x - start.x) > curveThreshold;
    }

    void ReturnPokeballToPool()
    {
        pokeballRigidbody.velocity = Vector3.zero;
        pokeballRigidbody.angularVelocity = Vector3.zero;
        pokeballRigidbody.useGravity = false;
    }

    public void SpawnPokeball()
    {
        currentPokeball = Instantiate(pokeballPrefab, new Vector3(5.23f, 2.42f, 0.78f), Quaternion.identity);
    }
}
