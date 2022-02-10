using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private Hazard initialHazard = null;
    [SerializeField]
    private Hazard[] hazards = null;

    #endregion

    #region Public Methods

    private void OnEnable()
    {
        GameManager.Instance.GameController.OnTimerDone += SpawnHazard;
        GameManager.Instance.OnGameStart += SpawnInitialHazard;
    }

    public void SpawnInitialHazard()
    {
        Instantiate(initialHazard, transform.position, Quaternion.identity);
    }

    public void SpawnHazard()
    {
        int hazardToSpawn = Random.Range(0, hazards.Length);

        Instantiate(hazards[hazardToSpawn], transform.position, Quaternion.identity);
    }

    private void OnDisable()
    {
        GameManager.Instance.GameController.OnTimerDone -= SpawnHazard;
        GameManager.Instance.OnGameStart -= SpawnInitialHazard;
    }

    #endregion
}