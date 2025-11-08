using UnityEngine;
using static GameManager;

public class FishSpawnManager : MonoBehaviour
{
    [SerializeField] private FishSpawnConfig spawnConfig;
    [SerializeField] private GameObject fishPrefab;
    
    //private float nextSpawnTime;
    private float currentMinInterval;
    private float currentMaxInterval;

    float intervalTimer;

    private void Start()
    {
        // 初始化间隔时间
        currentMinInterval = spawnConfig.minSpawnInterval;
        currentMaxInterval = spawnConfig.maxSpawnInterval;
        intervalTimer = 2f;
    }

    private void Update()
    {
        if(GameManager.Instance.currentState == GameState.Fishing)
        {
            intervalTimer -= Time.deltaTime;
        }
        if (intervalTimer <= 0)
        {
            SpawnFish();
            SetNextSpawnTime();
            //UpdateSpawnIntervals();
        }
    }

    private void SpawnFish()
    {
        // 随机选择鱼的种类
        int randomIndex = Random.Range(0, spawnConfig.availableFishTypes.Count);
        FishType selectedFish = spawnConfig.availableFishTypes[randomIndex];
        //print(randomIndex);
        //print(spawnConfig.availableFishTypes.Count);
        
        // 决定生成方向
        bool spawnLeft = Random.value < spawnConfig.leftSpawnChance;
        
        // 计算生成位置
        Vector3 spawnPosition = CalculateSpawnPosition(spawnLeft);
        
        // 生成鱼并初始化
        GameObject fishObject = Instantiate(fishPrefab, spawnPosition, Quaternion.identity);
        Fish fish = fishObject.GetComponent<Fish>();
        fish.Initialize(selectedFish, !spawnLeft); // 左侧生成则向右移动，反之亦然
        //print(fish.type);
    }

    private Vector3 CalculateSpawnPosition(bool spawnLeft)
    {
        // 计算X轴位置（屏幕外）
        float xPos;
        if (spawnLeft)
        {
            // 左侧生成点
            xPos = Camera.main.ViewportToWorldPoint(Vector3.zero).x - spawnConfig.spawnPadding;
        }
        else
        {
            // 右侧生成点
            xPos = Camera.main.ViewportToWorldPoint(Vector3.right).x + spawnConfig.spawnPadding;
        }
        
        // 随机Y轴位置
        float yPos = Random.Range(spawnConfig.yMin, spawnConfig.yMax);
        
        return new Vector3(xPos, yPos, 0);
    }

    private void SetNextSpawnTime()
    {
        intervalTimer = Random.Range(currentMinInterval, currentMaxInterval);
    }

    //private void UpdateSpawnIntervals()
    //{
    //    // 逐渐减小生成间隔，增加游戏难度
    //    currentMinInterval = Mathf.Max(spawnConfig.minPossibleInterval, 
    //        currentMinInterval - spawnConfig.spawnIntervalDecrement * Time.deltaTime);
            
    //    currentMaxInterval = Mathf.Max(spawnConfig.minPossibleInterval + 0.1f, 
    //        currentMaxInterval - spawnConfig.spawnIntervalDecrement * Time.deltaTime);
    //}
}