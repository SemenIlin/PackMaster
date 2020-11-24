using System;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private GameObject buttons;

    public static event Action ButtonClicks;
    public static event Action ButtonClickOk;
    public static void ResetButtonClickOk()
    {
        ButtonClickOk = null;
    }

    private void Start()
    {
        FollowerController.ShowButton += SetVisibleButtons;
    }
    public void ClickOk()
    {
        ButtonClickOk?.Invoke();
    }

    public void ClickClose()
    {
        Application.Quit();
    }
    public void ClickSkip()
    {
        buttons.SetActive(false);
        ButtonClicks?.Invoke();
    }
    private void SetVisibleButtons(bool isVisible)
    {
        buttons.SetActive(isVisible);
    }
}
