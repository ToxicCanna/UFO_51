using UnityEngine;
using UnityEngine.Tilemaps;

public class GrideGenerate : MonoBehaviour
{
    [SerializeField] private int gridWide;
    [SerializeField] private int gridHigh;
    [SerializeField] private Tile tilePerfab;
    [SerializeField] private GameObject tileParentObject;
    [SerializeField] private Transform cam;
    private void Start()
    {
        GenerateGrid();
        SetCameraPosition();
    }
    private void GenerateGrid()
    {
        for (int i = 1; i < gridWide; i++)
        {
            for (int j = 1; j < gridHigh; j++)
            {
                var spawnTile = Instantiate(tilePerfab, new Vector3(i, j), Quaternion.identity);
                spawnTile.name = $"Tile{i}{j}";
                spawnTile.transform.SetParent(tileParentObject.transform);
            }
        }
    }
    private void SetCameraPosition()
    {
        cam.transform.position = new Vector3((float) gridWide / 2-0.5f, (float)gridHigh / 2-0.5f,-10);
    }
}
