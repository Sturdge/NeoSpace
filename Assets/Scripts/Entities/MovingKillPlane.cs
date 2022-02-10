using UnityEngine;

public class MovingKillPlane : MonoBehaviour
{
    [SerializeField]
    private float speedMultiplier = 1.1f;
    [SerializeField]
    private Vector3 startPosition = new Vector3();

    private bool isMoving = true;

    private GameManager gameManager;
    private ShipController playerObject;

    public float FakeSpeed { get; private set; }

    private void Awake()
    {
        gameManager = GameManager.Instance;
        playerObject = GameManager.Instance.GameController.PlayerObject;
    }

    private void OnEnable()
    {
        Setup();
        gameManager.OnGameStart += Setup;
    }

    private void Update()
    {
        if(isMoving)
            Movement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.SetGameState(GameState.PlayerDeath);
            isMoving = false;
        }
    }

    private void OnDisable()
    {
        gameManager.OnGameStart -= Setup;
    }

    private void Setup()
    {
        isMoving = true;
        FakeSpeed = 30;
        transform.position = startPosition;
    }

    public float CheckDistance()
    {
        return FakeSpeed - playerObject.FakeSpeed;
    }

    private void Movement()
    {
        transform.Translate(Vector3.forward * CheckDistance() * Time.deltaTime);
        FakeSpeed += Time.deltaTime * speedMultiplier;
    }
}
