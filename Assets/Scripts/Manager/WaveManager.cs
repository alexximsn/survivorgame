using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(WaveManagerUI))]
public class WaveManager : MonoBehaviour,IGameStateListener
{
    [SerializeField]private Player player;//玩家
    private WaveManagerUI ui;//ui组件
    [SerializeField] private float waveDuration;//每波的持续时间
    private float timer;//计时器
    private bool isTimerOn;//计时器标记
    [SerializeField] private Wave[] wave;
    private List<float> localCounters = new List<float>();//当前波次索引

    private void Awake()
    {
        ui = GetComponent<WaveManagerUI>();
    }
    private int currentWaveIndex;
   
    void Update()
    {
        if (!isTimerOn)
            return;
        if (timer < waveDuration)//没到截至时间（进行中）
        {
            ManageCurrentWave();//敌人生成
            string timerString = ((int)(waveDuration - timer)).ToString();//更新倒计时
            ui.UpdateTimerText(timerString);

        }
            
        else
            StartWaveTransition();//结束，进入奖励
        
    }

    private bool CheckForAliveEnemies()//检查当前是否还有活着的敌人
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        return enemies.Length > 0;
    }
    private void StartWaveTransition()
    {
        isTimerOn = false;
        //检查是否还有存活的敌人
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
            GameManager.instance.SetGameState(GameState.STAGECOMPLETE);//所有波都完成，显示
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
        ui.UpdateWaveText("WAVE" + (currentWaveIndex + 1) + "/" + wave.Length);//更新这是第几波
        localCounters.Clear();
        foreach (WaveSegment segment in wave[waveIndex].segments)
            localCounters.Add(0
                );

        timer = 0;
        isTimerOn = true;
    }
    private void DefeatAllEnemies()//循环遍历所有敌人的对象
    {
        foreach (Enemy enemy in transform.GetComponentsInChildren<Enemy>())
            enemy.PassAwayAfterWave();//摧毁力子
    }
   
    private void ManageCurrentWave()//管理当前波
    {
        Wave currentWave = wave[currentWaveIndex];//获取一个关卡中所有的敌人生成数据
        for(int i=0;i<currentWave.segments.Count;i++)
        {
            WaveSegment segment = currentWave.segments[i];
            float tStart = segment.tStartEnd.x / 100 * waveDuration;
            float tEnd = segment.tStartEnd.y / 100 * waveDuration;//生成时段（百分比计算）
            if (timer < tStart || timer > tEnd)
                continue;//跳过未到激活时间或已过期的生成段
            float timeSinceSegmentStart = timer - tStart;//生成时间计数（已过去了几秒）
            float spawnDelay = 1f / segment.spawnFrequency;//生成间隔，例0.5两次，1次一次

            if (timeSinceSegmentStart / spawnDelay > localCounters[i])//算出理论的生成次数
            {
                Instantiate(segment.prefab, GetSpawnPosition(), Quaternion.identity, transform);
                localCounters[i]++;//生成数增加

                if (segment.spawnOnce)
                    localCounters[i] += Mathf.Infinity;
            }
        }
        timer += Time.deltaTime;
    }
    private Vector2 GetSpawnPosition()//计算敌人生成的位置
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
    public bool spawnOnce;//生成敌人仅一次
}