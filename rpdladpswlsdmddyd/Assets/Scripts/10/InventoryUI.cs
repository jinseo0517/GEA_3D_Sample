using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    #region //각 큐브 별 스프라이트
    public Sprite dirtSprite;
    public Sprite diamondSprite;
    public Sprite grassSprite;
    public Sprite waterSprite;
    public Sprite cloudSprite;
    #endregion
    public List<Transform> Slot = new List<Transform>(); // 내 UI의 각 슬롯들의 리스트
    public GameObject SlotItem; // 슬롯 내부에 들어가는 아이템
    List<GameObject> items = new List<GameObject>(); // 아이템 삭제용 전체 리스트
    // 인벤토리 업데이트 시 호출

    public int selectedIndex = -1;
    public void UpdateInventory(Inventory myInven)
    {
        // 1. 기존 슬롯 초기화
        foreach (var slotItems in items)
        {
            Destroy(slotItems); // 시작할때 슬롯 아이템들의 GameObject 삭제
        }
        items.Clear(); // 시작할때 아이템 리스트 클리어
                       // 2. 내 인벤토리 데이터를 전체 탐색
        int idx = 0; // 접근할 슬롯의 인덱스
        foreach (var item in myInven.items)
        {
            #region 슬롯아이템 생성 로직 (게임오브젝트 인스턴스 생성, 위치 조정, SlotItemPrefab 컴포넌트 가져오기, 그 후 아이템 세팅
            var go = Instantiate(SlotItem, Slot[idx].transform);
            go.transform.localPosition = Vector3.zero;
            SlotItemPrefab sItem = go.GetComponent<SlotItemPrefab>();
            items.Add(go); // 아이템 리스트에 하나 추가
            #endregion
            switch (item.Key) // 각 케이스별로 아이템 추가
            {
                case BlockType.Dirt:
                    sItem.ItemSetting(dirtSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case BlockType.Grass:
                    sItem.ItemSetting(grassSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case BlockType.Water:
                    sItem.ItemSetting(waterSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case BlockType.Diamond:
                    sItem.ItemSetting(diamondSprite, "x" + item.Value.ToString(), item.Key);
                    break;
            }
            idx++;  //인덱스한칸 추가
        }
    }
    private void Update()
    {
        for (int i = 0; i < Mathf.Min(9, Slot.Count); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetSelectedIndex(i);
            }
        }
    }
    public void SetSelectedIndex(int idx)
    {
        ResetSelection();
        if (selectedIndex == idx)
        {
            selectedIndex = -1; // 같은 인덱스 선택 시 선택 해제
        }
        else
        {
            if (idx >= items.Count)
            {
                selectedIndex = -1; // 아이템이 없는 슬롯 선택 시 선택 해제
            }
            else
            {
                SetSelection(idx);
                selectedIndex = idx;
            }
        }
    }
    public void ResetSelection()
    {
        foreach (var slot in Slot)
        {
            slot.GetComponent<Image>().color = Color.white;
        }
    }
    void SetSelection(int _idx)
    {
        Slot[_idx].GetComponent<Image>().color = Color.yellow;
    }
    public BlockType GetInventorySlot()
    {
        return items[selectedIndex].GetComponent<SlotItemPrefab>().blockType;
    }
}