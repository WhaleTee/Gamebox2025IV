using Input;
using UnityEngine;
using UnityEngine.UI;
using Reflex.Attributes;

public class EscToButton : MonoBehaviour
{
    [SerializeField] private Button firstButton;   // кнопка паузы
    [SerializeField] private Button secondButton;  // кнопка продолжить

    [Inject] private UserInput userInput;

    private void Start()
    {
        userInput.Pause += OnPause;
    }

    private void OnDestroy()
    {
        userInput.Pause -= OnPause;
    }

    private void OnPause()
    {
        // если первая кнопка видима и активна — нажимаем её
        if (firstButton != null && firstButton.gameObject.activeInHierarchy && firstButton.interactable)
        {
            firstButton.onClick.Invoke();
            return;
        }

        // если первая не подходит — нажимаем вторую
        if (secondButton != null && secondButton.gameObject.activeInHierarchy && secondButton.interactable)
        {
            secondButton.onClick.Invoke();
            return;
        }
    }
}
