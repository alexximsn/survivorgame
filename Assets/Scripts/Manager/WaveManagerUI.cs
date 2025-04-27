using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WaveManagerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;//����
    [SerializeField] private TextMeshProUGUI timerText;//����ʱ
    public void UpdateWaveText(string waveString) => waveText.text = waveString;
    public void UpdateTimerText(string timerString) => timerText.text = timerString;

}
