using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField]
    private GameState state = GameState.Null;

    private GameManager gameManager;
    private Button button;

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(() => ChangeState());
    }

    private void ChangeState() => GameManager.Instance.SetGameState(state);
}
