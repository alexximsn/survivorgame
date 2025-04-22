using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(WaveManagerUI))]
public class WaveManager : MonoBehaviour,IGameStateListener
{
    [SerializeField]private Player player;
    private WaveManagerUI ui;
    [SerializeField] private float waveDuration;//ÿ���ĳ���ʱ��
    private float timer;//��ʱ��
    private bool isTimerOn;
    [SerializeField] private Wave[] wave;
    private List<float> localCounters = new List<float>();

    private void Awake()
    {
        ui = GetComponent<WaveManagerUI>();
    }
    private int currentWaveIndex;
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTimerOn)
            return;
        if (timer < waveDuration)
        {
            ManageCurrentWave();
            string timerString = ((int)(waveDuration - timer)).ToString();
            ui.UpdateTimerText(timerString);

        }
            
        else
            StartWaveTransition();
        
    }
    private void StartWaveTransition()
    {
        isTimerOn = false;
        DefeatAllEnemies();
        currentWaveIndex++;
        if (currentWaveIndex >= wave.Length)
        {
            ui.UpdateTimerText("");
            ui.UpdateWaveText("Stage Completed");
            GameManager.instance.SetGameState(GameState.STAGECOMPLETE);

        }
           
        else
        {
          GameManager.instance.WaveCompletedCallback();
        }
    }
    private void StartNextWave()
    {
        StartWave(currentWaveIndex);
    }
    private void StartWave(int waveIndex)
    {
        ui.UpdateWaveText("WAVE" + (currentWaveIndex + 1) + "/" + wave.Length);
        localCounters.Clear();
        foreach (WaveSegment segment in wave[waveIndex].segments)
            localCounters.Add(0
                );

        timer = 0;
        isTimerOn = true;
    }
    private void DefeatAllEnemies()
    {
        foreach (Enemy enemy in transform.GetComponentsInChildren<Enemy>())
            enemy.PassAwayAfterWave();
    }
   
    private void ManageCurrentWave()
    {
        Wave currentWave = wave[currentWaveIndex];
        for(int i=0;i<currentWave.segments.Count;i++)
        {
            WaveSegment segment = currentWave.segments[i];
            float tStart = segment.tStartEnd.x / 100 * waveDuration;
            float tEnd = segment.tStartEnd.y / 100 * waveDuration;
            if (timer < tStart || timer > tEnd)
                continue;
            float timeSinceSegmentStart = timer - tStart;
            float spawnDelay = 1f / segment.spawnFrequency;

            if (timeSinceSegmentStart / spawnDelay > localCounters[i])
            {
                Instantiate(segment.prefab, GetSpawnPosition(), Quaternion.identity, transform);
                localCounters[i]++;

                if (segment.spawnOnce)
                    localCounters[i] += Mathf.Infinity;
            }
        }
        timer += Time.deltaTime;
    }
    private Vector2 GetSpawnPosition()
    {
        Vector2 direction = Random.onUnitSphere;
        Vector2 offset = direction.normalized *Random.Range(6, 10);
        Vector2 targetPosition = (Vector2)player.transform.position * offset;
        targetPosition.x = Mathf.Clamp(targetPosition.x, -10, 10);
        targetPosition.y = Mathf.Clamp(targetPosition.y,-6,6);
        return targetPosition;
    }
    public void GameStateChangedCallback(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.GAME:
                StartNextWave();
                break;
            case GameState.GAMEOVER:
                isTimerOn = false;
                break;
        }
    }
}
   [System.Serializable] 
   public struct Wave
    {
        public string name;
    public List<WaveSegment> segments;
    }
[System.Serializable]
public struct WaveSegment
{
    [MinMaxSlider(0, 100)] public Vector2 tStartEnd;
    public float spawnFrequency;
    public GameObject prefab;
    public bool spawnOnce;
}