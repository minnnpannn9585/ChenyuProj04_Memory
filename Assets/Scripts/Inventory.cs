using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [Header("背包配置")]
    public List<FishType> fishTypesInInventory; // 背包中可容纳的鱼类型（必须设置4种）

    private Dictionary<FishType, int> fishCounts = new Dictionary<FishType, int>();
    public event Action OnInventoryUpdated; // 背包更新事件（用于UI同步）

    private void Awake()
    {
        // 单例初始化
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 初始化背包（4个格子对应4种鱼，初始数量0）
        if (fishTypesInInventory.Count != 4)
        {
            Debug.LogError("背包必须配置4种鱼类型！");
            return;
        }

        foreach (var fishType in fishTypesInInventory)
        {
            fishCounts[fishType] = 0;
        }
    }

    // 添加鱼到背包
    public void AddFish(FishType fishType)
    {
        if (fishCounts.ContainsKey(fishType))
        {
            fishCounts[fishType]++;
            OnInventoryUpdated?.Invoke(); // 通知UI更新
        }
        else
        {
            Debug.LogWarning($"背包不支持该鱼类型：{fishType.fishName}");
        }
    }

    // 获取指定鱼的数量
    public int GetFishCount(FishType fishType)
    {
        return fishCounts.TryGetValue(fishType, out int count) ? count : 0;
    }
}
