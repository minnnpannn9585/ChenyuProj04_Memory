using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishSpawnConfig", menuName = "FishGame/FishSpawnConfig")]
public class FishSpawnConfig : ScriptableObject
{
    [Header("鱼的种类配置")]
    public List<FishType> availableFishTypes; // 可用的鱼种类（至少3种）
    
    [Header("生成时间设置")]
    public float minSpawnInterval = 1f; // 最小生成间隔
    public float maxSpawnInterval = 3f; // 最大生成间隔
    public float spawnIntervalDecrement = 0.1f; // 随时间减少的间隔值
    public float minPossibleInterval = 0.5f; // 最小可能的间隔
    
    [Header("生成概率设置")]
    [Range(0, 1)] public float leftSpawnChance = 0.5f; // 从左侧生成的概率
    
    [Header("边界设置")]
    public float yMin = -3f; // Y轴最小位置
    public float yMax = 3f; // Y轴最大位置
    public float spawnPadding = 0.2f; // 生成位置的屏幕外偏移
}