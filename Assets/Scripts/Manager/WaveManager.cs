using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(WaveManagerUI))]
public class WaveManager : MonoBehaviour,IGameStateListener
{
    [SerializeField]private Player player;//���
    private WaveManagerUI ui;//ui���
    [SerializeField] private float waveDuration;//ÿ���ĳ���ʱ��
    private float timer;//��ʱ��
    private bool isTimerOn;//��ʱ�����
    [SerializeField] private Wave[] wave;
    private List<float> localCounters = new List<float>();//��ǰ��������

    private void Awake()
    {
        ui = GetComponent<WaveManagerUI>();
    }
    private int currentWaveIndex;
   
    void Update()
    {
        if (!isTimerOn)
            return;
        if (timer < waveDuration)//û������ʱ�䣨�����У�
        {
            ManageCurrentWave();//��������
            string timerString = ((int)(waveDuration - timer)).ToString();//���µ���ʱ
            ui.UpdateTimerText(timerString);

        }
            
        else
            StartWaveTransition();//���������뽱��
        
    }

    private bool CheckForAliveEnemies()//��鵱ǰ�Ƿ��л��ŵĵ���
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        return enemies.Length > 0;
    }
    private void StartWaveTransition()
    {
        isTimerOn = false;
        //����Ƿ��д��ĵ���
        bool enemiesAlive = CheckForAliveEnemies();
        if (enemiesAlive)
        {
            GameManager.instance.SetGameState(GameState.GAMEOVER);
            return;
        }
        DefeatAllEnemies();
        currentWaveIndex++;
        if (currentWaveIndex >= wave.Length)
        {
            ui.UpdateTimerText("");
            ui.UpdateWaveText("Stage Completed");
            GameManager.instance.SetGameState(GameState.STAGECOMPLETE);//���в�����ɣ���ʾ
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
        ui.UpdateWaveText("WAVE" + (currentWaveIndex + 1) + "/" + wave.Length);//�������ǵڼ���
        localCounters.Clear();
        foreach (WaveSegment segment in wave[waveIndex].segments)
            localCounters.Add(0
                );

        timer = 0;
        isTimerOn = true;
    }
    private void DefeatAllEnemies()//ѭ���������е��˵Ķ���
    {
        foreach (Enemy enemy in transform.GetComponentsInChildren<Enemy>())
            enemy.PassAwayAfterWave();//�ݻ�����
    }
   
    private void ManageCurrentWave()//����ǰ��
    {
        Wave currentWave = wave[currentWaveIndex];//��ȡһ���ؿ������еĵ�����������
        for(int i=0;i<currentWave.segments.Count;i++)
        {
            WaveSegment segment = currentWave.segments[i];
            float tStart = segment.tStartEnd.x / 100 * waveDuration;
            float tEnd = segment.tStartEnd.y / 100 * waveDuration;//����ʱ�Σ��ٷֱȼ��㣩
            if (timer < tStart || timer > tEnd)
                continue;//����δ������ʱ����ѹ��ڵ����ɶ�
            float timeSinceSegmentStart = timer - tStart;//����ʱ��������ѹ�ȥ�˼��룩
            float spawnDelay = 1f / segment.spawnFrequency;//���ɼ������0.5���Σ�1��һ��

            if (timeSinceSegmentStart / spawnDelay > localCounters[i])//������۵����ɴ���
            {
                Instantiate(segment.prefab, GetSpawnPosition(), Quaternion.identity, transform);
                localCounters[i]++;//����������

                if (segment.spawnOnce)
                    localCounters[i] += Mathf.Infinity;
            }
        }
        timer += Time.deltaTime;
    }
    private Vector2 GetSpawnPosition()//����������ɵ�λ��
    {
        Vector2 direction = Random.onUnitSphere;
        Vector2 offset = direction.normalized *Random.Range(6, 10);
        Vector2 targetPosition = (Vector2)player.transform.position + offset;
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
    public bool spawnOnce;//���ɵ��˽�һ��
}