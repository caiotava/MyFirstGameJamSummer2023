using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private Image imageFill;
    [SerializeField] private Text timerText;

    private float duration;
    private float remainDuration;

    public UnityEvent OnTimerEnd;

    public void StartTimer(float seconds)
    {
        remainDuration = 0;
        duration = seconds;
        StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        var waitTime = new WaitForSeconds(1.0f);

        while (remainDuration <= duration)
        {
            yield return waitTime;

            var actualTime = duration - remainDuration;
            timerText.text = $"{Mathf.Floor(actualTime / 60):00}:{actualTime % 60:00}";
            imageFill.fillAmount = Mathf.Clamp01(remainDuration / duration);
            remainDuration++;
        }

        OnTimerEnd.Invoke();
    }
}