using UnityEngine;
using UnityEngine.UIElements;

public class GrideGenerate : MonoBehaviour
{
    [SerializeField] private int gridWide;
    [SerializeField] private int gridHigh;
    [SerializeField] private Tile tilePerfab;
    [SerializeField] private GameObject tileParentObject;
    [SerializeField] private Transform cam;
    private Vector3 tilePosition;
    private void Start()
    {
        GenerateGrid();
        //SetCameraPosition();
    }
    private void GenerateGrid()
    {
        for (int i = 0; i < gridWide; i++)
        {
            for (int j = 0; j < gridHigh; j++)
            {
                var spawnTile = Instantiate(tilePerfab, new Vector3(i, j), Quaternion.identity);
                spawnTile.SetColorAlpha();
                spawnTile.name = $"Tile{i}{j}";
                spawnTile.transform.SetParent(tileParentObject.transform);
                var isOffset = (i + j) % 2 != 0;
                spawnTile.InitialGridColor(isOffset);
                //tilePosition =spawnTile.transform.position;
            }
        }
    }
    private void SetCameraPosition()
    {
        cam.transform.position = new Vector3((float)gridWide / 2 - 0.5f, (float)gridHigh / 2 - 0.5f, -10);
    }
    //private Vector3 GetTilePosition()
    //{
    //    return tilePosition;
    //}
}
