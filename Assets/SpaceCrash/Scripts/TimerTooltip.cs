using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerTooltip : MonoBehaviour
{
    private static TimerTooltip Instance;

    private Timer timer;

    [SerializeField] private Camera uiCamera;
    [SerializeField] private TextMeshProUGUI callerNameText;
    [SerializeField] private TextMeshProUGUI timeLeftText;
    [SerializeField] private Button skipButton;
    [SerializeField] private TextMeshProUGUI skipAmountText;
    [SerializeField] private Slider progressSlider;

    private bool countdown;

    private void Awake() 
    {
        Instance = this;
        transform.parent.gameObject.SetActive(false);
    }

    private void ShowTimer(GameObject caller)
    {
        timer = caller.GetComponent<Timer>();

        if(timer == null)
        {
            return;
        }

        callerNameText.text = timer.name;
        skipAmountText.text = timer.skipAmount.ToString();
        skipButton.gameObject.SetActive(true);

        Vector3 position = caller.transform.position - uiCamera.transform.position;
        position = uiCamera.WorldToScreenPoint(uiCamera.transform.TransformPoint(position));
        transform.position = position;

        countdown = true;
        FixedUpdate();

        transform.parent.gameObject.SetActive(true);
    }

    private void FixedUpdate() 
    {
        if(countdown)
        {
            progressSlider.value = (float) (1.0 - timer.seconsLeft / timer.timeToFinish.TotalSeconds);
            timeLeftText.text = timer.DisplayTime();
        }
    }

    public void SkipButton()
    {
        EventManager.Instance.AddListenerOnce<EnoughResourceGameEvent>(OnEnoughResource);
        EventManager.Instance.AddListenerOnce<NotEnoughResourceGameEvent>(OnNotEnoughResource);

        ResourceChangeGameEvent info = new ResourceChangeGameEvent(-timer.skipAmount, ResourceType.Gas);
        EventManager.Instance.QueueEvent(info);
    }

    private void OnEnoughResource(EnoughResourceGameEvent info)
    {
        timer.SkipTimer();
        skipButton.gameObject.SetActive(false);
        EventManager.Instance.RemoveListener<NotEnoughResourceGameEvent>(OnNotEnoughResource);
    }

    private void OnNotEnoughResource(NotEnoughResourceGameEvent info)
    {
        EventManager.Instance.RemoveListener<EnoughResourceGameEvent>(OnEnoughResource);
    }

    public void HideTimer()
    {
        transform.parent.gameObject.SetActive(false);
        timer = null;
        countdown = false;
    }

    public static void ShowTimer_Static(GameObject caller)
    {
        Instance.ShowTimer(caller);
    }

    public static void HideTimer_Static()
    {
        Instance.HideTimer();
    }
}
