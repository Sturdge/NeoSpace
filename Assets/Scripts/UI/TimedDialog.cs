using System.Collections;
using UnityEngine;

public class TimedDialog : MonoBehaviour
{
    [SerializeField]
    private float duration = 2;
    [SerializeField]
    private TimedDialogLogic logicHandler = null;

    private Canvas canvas;

    public delegate void DialogResult(TimedDialogResult result);
    public event DialogResult OnFinished;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    private void OnEnable()
    {
        logicHandler.OnFinished += FinishProcessing;
        StartCoroutine(Delay());
    }

    private void OnDisable()
    {
        logicHandler.OnFinished -= FinishProcessing;
    }

    private void FinishProcessing(TimedDialogResult result)
    {
        OnFinished?.Invoke(result);

        Destroy(gameObject);
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(duration);
        logicHandler.PerformLogic();
    }
}
