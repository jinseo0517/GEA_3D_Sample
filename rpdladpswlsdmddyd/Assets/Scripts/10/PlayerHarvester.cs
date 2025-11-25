using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    public float rayDistance = 5f;      //채집 가능 거리
    public LayerMask hitMask = ~0;      // 가능 한 레이어 전부 다 (일단)
    public int toolDamage = 1;          // 타격 데미지
    public float hitCooldown = 0.15f;   // 연타 간격
    private float _nextHitTime;
    private Camera _cam;
    public Inventory inventory;         //// 플레이어 인벤(없으면 자동 부착)
    InventoryUI invenUI;
    public GameObject selectedBlock;

    void Awake()
    {
        _cam = Camera.main;
        if (inventory == null) inventory = gameObject.AddComponent<Inventory>();
        invenUI = FindObjectOfType<InventoryUI>();
    }
    void Update()
    {
        if (invenUI.selectedIndex < 0)
        {
            selectedBlock.transform.localScale = Vector3.zero;
            // 선택된 idx가 -1이면 수확 모드
            if (Input.GetMouseButton(0) && Time.time >= _nextHitTime)
            {
                _nextHitTime = Time.time + hitCooldown;

                Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 8
                if (Physics.Raycast(ray, out var hit, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
                {
                    var block = hit.collider.GetComponent<Block>();
                    if (block != null)
                    {
                        block.Hit(toolDamage, inventory);
                    }
                }
            }
        }
        else
        {
            Ray rayDebug = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //화면중앙
            if (Physics.Raycast(rayDebug, out var hitDebug, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
            {
                //Debug.DrawRay(hitDebug.point, hitDebug.normal, Color.red, 2f) ;
                Vector3Int placePos = AdjacentCellOnHitFace(hitDebug);
                selectedBlock.transform.localScale = Vector3.one;
                selectedBlock.transform.position = placePos;
                selectedBlock.transform.rotation = Quaternion.identity;
            }
            else
            {
                selectedBlock.transform.localScale = Vector3.zero;
            }

             if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("sdf");
                // 선택된 idx가 0 이상이면 설치 모드
                Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 8 38
                if (Physics.Raycast(ray, out var hit, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Vector3Int placePos = AdjacentCellOnHitFace(hit);
                    BlockType selected = invenUI.GetInventorySlot();
                    if (inventory.Consume(selected, 1))
                    {
                        FindObjectOfType<NoiseVoxeMap>().PlaceTile(placePos, selected);
                    }
                }
            }
        }
    }
    static Vector3Int AdjacentCellOnHitFace(in RaycastHit hit)
    {
        Vector3 baseCenter = hit.collider.transform.position; // 맞준 블록의 중심(정수 좌표(x,y,z))
        Vector3 adjCenter = baseCenter + hit.normal; // 그 면의 바깥쪽으로 정확히 한 칸 이등
        return Vector3Int.RoundToInt(adjCenter);
    }
}
