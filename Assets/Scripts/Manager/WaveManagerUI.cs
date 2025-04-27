using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WaveManagerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;//ÎÄ×Ö
    [SerializeField] private TextMeshProUGUI timerText;//µ¹¼ÆÊ±
    public void UpdateWaveText(string waveString) => waveText.text = waveString;
    public void UpdateTimerText(string timerString) => timerText.text = timerString;

}
