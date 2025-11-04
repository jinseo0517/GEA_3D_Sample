using UnityEngine;

public class NoiseVoxeMap : MonoBehaviour
{
    public GameObject blockPrefab;

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
    }
}