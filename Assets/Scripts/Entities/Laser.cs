using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private float timeAlive = 2;

    private GameManager gameManager = null;

    private void Awake() => gameManager = GameManager.Instance;

    private void OnEnable() => gameManager.OnMainMenu += DestroySelf;

    private void Start() => Destroy(gameObject, timeAlive);

    private void Update() => transform.Translate(Vector3.forward * speed * Time.deltaTime);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HazardObject"))
        {
            HazardObject obj = other.GetComponent<HazardObject>();
            obj.Die();
        }
        if (!other.CompareTag("Player"))
            Destroy(gameObject);
    }

    private void OnDisable() => gameManager.OnMainMenu += DestroySelf;

    private void DestroySelf()
    {
        if (this != null)
            Destroy(gameObject);
    }
}
