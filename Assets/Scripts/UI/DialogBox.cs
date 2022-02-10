using UnityEngine;

public class DialogBox : MonoBehaviour
{
    [SerializeField]
    private DialogButton[] buttons = null;

    private Canvas canvas;

    public delegate void DialogBoxResult();
    public event DialogBoxResult OnConfirm;
    public event DialogBoxResult OnCancel;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    public void OnEnable()
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].OnClick += ReturnResult;
    }

    private void OnDisable()
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].OnClick -= ReturnResult;
    }

    private void ReturnResult(DialogButtonResult result)
    {
        Debug.Log(result);

        if (result == DialogButtonResult.Confirm)
            OnConfirm?.Invoke();
        else if (result == DialogButtonResult.Cancel)
            OnCancel?.Invoke();

        Destroy(gameObject);
    }
}