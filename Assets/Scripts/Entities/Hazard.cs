using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;

    private GameManager gameManager;
    private ShipController playerObject;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        playerObject = GameManager.Instance.GameController.PlayerObject;
    }

    private void OnEnable()
    {
        gameManager.OnMainMenu += DestroySelf;
    }

    private void Start()
    {
        speed = playerObject.FakeSpeed / 2;
    }

    private void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    private void OnDisable()
    {
        gameManager.OnMainMenu -= DestroySelf;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
