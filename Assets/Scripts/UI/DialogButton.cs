using UnityEngine;
using UnityEngine.UI;

public class DialogButton : MonoBehaviour
{
    [SerializeField]
    private DialogButtonResult result = DialogButtonResult.Confirm;

    private Button button;

    public delegate void ButtonEvent(DialogButtonResult result);
    public event ButtonEvent OnClick;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnClick?.Invoke(result));
    }
}
