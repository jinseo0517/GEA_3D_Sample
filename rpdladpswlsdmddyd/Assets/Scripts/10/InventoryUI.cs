using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;           // 플레이어 인벤토리 연결
    public GameObject slotPrefab;         // 슬롯 프리팹 (Image + Text)
    public Transform slotParent;          // 슬롯들이 들어갈 부모 오브젝트 (GridLayoutGroup)

    private Dictionary<BlockType, GameObject> slotMap = new(); // 블록 타입별 슬롯 저장

    void Update()
    {
        if (inventory == null) return;

        foreach (BlockType type in inventory.acquiredOrder)
        {
            // 슬롯이 없으면 새로 생성
            if (!slotMap.ContainsKey(type))
            {
                GameObject slot = Instantiate(slotPrefab, slotParent);
                slotMap[type] = slot;

                // 아이콘 설정
                Image icon = slot.GetComponentInChildren<Image>();
                icon.sprite = GetIconForType(type);
            }

            // 수량 업데이트
            int count = inventory.Get(type);

            // 텍스트 설정
            Text countText = slotMap[type].GetComponentInChildren<Text>();
            countText.text = count.ToString();

            // 아이콘 표시 여부
            Image image = slotMap[type].GetComponentInChildren<Image>();
            image.enabled = count > 0;
        }
    }

    // 블록 타입에 따라 아이콘 불러오기
    Sprite GetIconForType(BlockType type)
    {
        return Resources.Load<Sprite>($"Icons/{type}");
    }
}