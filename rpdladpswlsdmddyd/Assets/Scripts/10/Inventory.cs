using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<BlockType, int> items = new();
    public List<BlockType> acquiredOrder = new(); // 획득 순서 저장

    public void Add(BlockType type, int count = 1)
    {
        if (!items.ContainsKey(type))
        {
            items[type] = 0;
            acquiredOrder.Add(type); // 처음 획득한 블록이면 순서에 추가
        }

        items[type] += count;
        Debug.Log($"[Inventory] +{count} {type} ( {items[type]})");
    }

    public bool Consume(BlockType type, int count = 1)
    {
        if (!items.TryGetValue(type, out var have) || have < count) return false;
        items[type] = have - count;
        Debug.Log($"[Inventory] -{count} {type} (= {items[type]})");
        return true;
    }

    public int Get(BlockType type) // 이거 추가!
    {
        if (items.TryGetValue(type, out int count))
            return count;
        return 0;
    }
}