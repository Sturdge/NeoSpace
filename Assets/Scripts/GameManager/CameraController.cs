using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Serialized Fields

    [Header("Camera Settings")]
    [SerializeField]
    private float distanceFromPlayer = 10;
    [SerializeField]
    private GameObject playerObject = null;

    [Header("Camera Shake")]
    [SerializeField]
    private float shakeDuration = 1;
    [SerializeField]
    private float shakeMagnitude = 1;
    [SerializeField]
    private float shakeRotationMultiplier = 0;

    [Header("Colours")]
    [SerializeField]
    private Color[] colours = null;

    #endregion

    #region Fields

    private int currentColour = 0;
    private Vector3 newPosition = new Vector3();

    #endregion

    #region Component References

    private GameManager gameManager = null;
    private Camera mainCam = null;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        mainCam = GetComponent<Camera>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnMainMenu += ResetBackground;
        gameManager.GameController.OnTimerDone += CameraEvents;
    }

    private void OnDisable()
    {
        gameManager.OnMainMenu -= ResetBackground;
        gameManager.GameController.OnTimerDone -= CameraEvents;
    }

    void LateUpdate()
    {
        MoveCamera();
    }

    #endregion

    #region Private Methods

    private void MoveCamera()
    {
        newPosition = mainCam.transform.position;
        newPosition.z = playerObject.transform.position.z - distanceFromPlayer;

        mainCam.transform.position = newPosition;
    }

    private void CameraEvents()
    {
        GetNewColour();
        ChangeBackground();
        StartCoroutine(ShakeCamera());
    }

    private void GetNewColour()
    {
        int newColour = Random.Range(0, colours.Length);
        if (newColour != currentColour)
            currentColour = newColour;
        else
            GetNewColour();
    }

    private void ChangeBackground()
    {
        mainCam.backgroundColor = colours[currentColour];
    }

    #endregion

    #region Public Methods


    public void ResetBackground()
    {
        mainCam.backgroundColor = Color.black;
    }

    #endregion

    #region Coroutines

    private IEnumerator ShakeCamera()
    {
        Vector3 originalPosition = mainCam.transform.localPosition;
        float shakePower = shakeMagnitude;
        float shakeRotation = shakeMagnitude * shakeRotationMultiplier;

        float timeElapsed = 0;
        while(timeElapsed < shakeDuration)
        {
            timeElapsed += Time.deltaTime;

            Vector3 shakeAmount = new Vector3
            {
                x = Random.Range(-1f, 1f) * shakePower,
                y = Random.Range(-1f, 1f) * shakePower,
                z = Random.Range(-1f, 1f) * shakeRotation
            };

            mainCam.transform.localPosition += new Vector3(shakeAmount.x, shakeAmount.y, originalPosition.z);

            shakePower = Mathf.MoveTowards(shakePower, 0, shakeMagnitude / shakeDuration * Time.deltaTime);
            shakeRotation = Mathf.MoveTowards(shakeRotation, 0, shakeMagnitude / shakeDuration * shakeRotationMultiplier * Time.deltaTime);

            mainCam.transform.rotation = Quaternion.Euler(0, 0, shakeAmount.z);

            yield return null;
        }

        mainCam.transform.localPosition = originalPosition;
    }

    #endregion
}