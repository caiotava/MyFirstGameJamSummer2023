using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image imageFill;
    [SerializeField] private Text timerText;
    [SerializeField] private float duration;

    private float remainDuration;

    // Start is called before the first frame update
    private void Start()
    {
        StartTimer(duration);
    }

    public void StartTimer(float seconds)
    {
        remainDuration = 0;
        StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        var waitTime = new WaitForSeconds(1.0f);

        while (remainDuration <= duration)
        {
            var actualTime = duration - remainDuration;
            timerText.text = $"{Mathf.Floor(actualTime / 60):00}:{actualTime % 60:00}";
            imageFill.fillAmount = Mathf.Clamp01(remainDuration / duration);
            remainDuration++;
            yield return waitTime;
        }

        OnTimerEnd();
    }

    private void OnTimerEnd()
    {
    }
}
