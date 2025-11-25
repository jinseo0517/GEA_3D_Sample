using UnityEngine;

public class NoiseVoxeMap : MonoBehaviour
{
    /*public GameObject blockPrefab;

    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16;

    [SerializeField] float noiseScale = 20f;

    public Material dirtMaterial;
    public Material grassMaterial;
    public Material waterMaterial;
    public Material oreMaterial;

    public float oreChance = 0.05f; // 광물 등장 확률
    public int waterLevel = 4;

    void Start()
    {
        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;

                float noise = Mathf.PerlinNoise(nx, nz);
                int h = Mathf.FloorToInt(noise * maxHeight);

                // 지형 생성
                for (int y = 0; y <= h; y++)
                {
                    GameObject block = Place(x, y, z);

                    if (y == h)
                    {
                        // 최상단은 잔디
                        block.GetComponent<MeshRenderer>().material = grassMaterial;
                    }
                    else if (Random.value < oreChance)
                    {
                        // 확률적으로 광물
                        block.GetComponent<MeshRenderer>().material = oreMaterial;
                    }
                    else
                    {
                        // 나머지는 흙
                        block.GetComponent<MeshRenderer>().material = dirtMaterial;
                    }
                }

                // 물 채우기
                for (int y = h + 1; y <= waterLevel; y++)
                {
                    GameObject water = Place(x, y, z);
                    water.GetComponent<MeshRenderer>().material = waterMaterial;
                }
            }
        }
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(width / 2, maxHeight + 2, depth / 2);
        }

    }

    private GameObject Place(int x, int y, int z)
    {
        var go = Instantiate(blockPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"B_{x}_{y}_{z}";
        return go;
    }*/
    public GameObject blockPrefabDirt;
    public GameObject blockPrefabGrass;
    public GameObject blockPrefabWater;
    // 다이아몬드 프리팹 추가
    public GameObject blockPrefabDiamond;

    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16; // Y
    public int waterLevel = 4;
    [SerializeField] float noiseScale = 20f;

    void Start()
    {
        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;
                float noise = Mathf.PerlinNoise(nx, nz);
                int h = Mathf.FloorToInt(noise * maxHeight);
                if (h <= 0) h = 1;

                for (int y = 0; y <= h; y++)
                {
                    // 다이아몬드 생성 로직 (가장 아래층, 1% 확률)
                    if (y == 1 && Random.Range(0, 100) < 1)
                    {
                        PlaceDiamond(x, y, z);
                    }
                    else if (y == h)
                    {
                        PlaceGrass(x, y, z);
                    }
                    else
                    {
                        PlaceDirt(x, y, z);
                    }
                }

                for (int y = h + 1; y <= waterLevel; y++)
                {
                    PlaceWater(x, y, z);
                }
            }
        }
    }

    private void PlaceWater(int x, int y, int z)
    {
        var go = Instantiate(blockPrefabWater, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Water_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = BlockType.Water;
        b.maxHP = 3;
        b.dropCount = 1;
        b.mineable = false;
    }

    private void PlaceDirt(int x, int y, int z)
    {
        var go = Instantiate(blockPrefabDirt, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Dirt_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = BlockType.Dirt;
        b.maxHP = 3;
        b.dropCount = 1;
        b.mineable = true;
    }

    private void PlaceGrass(int x, int y, int z)
    {
        var go = Instantiate(blockPrefabGrass, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Grass_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = BlockType.Grass;
        b.maxHP = 3;
        b.dropCount = 1;
        b.mineable = true;
    }

    // Diamond 블록 생성 메서드 추가
    private void PlaceDiamond(int x, int y, int z)
    {
        var go = Instantiate(blockPrefabDiamond, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Diamond_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = BlockType.Diamond; // BlockType.Diamond 사용
        b.maxHP = 10;                // 다이아몬드 체력 설정 (예시)
        b.dropCount = 1;
        b.mineable = true;
    }
    public void PlaceTile(Vector3Int pos, BlockType type)
    {
        switch (type)
        {
            case BlockType.Dirt:
                PlaceDirt(pos.x, pos.y, pos.z);
                break;
            case BlockType.Grass:
                PlaceGrass(pos.x, pos.y, pos.z);
                break;
            case BlockType.Water:
                PlaceWater(pos.x, pos.y, pos.z);
                break;
            case BlockType.Diamond:
                PlaceDiamond(pos.x, pos.y, pos.z);
                break;
        }
    }
}