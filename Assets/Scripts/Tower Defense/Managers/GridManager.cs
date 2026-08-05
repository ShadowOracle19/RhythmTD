using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region dont touch this
    private static GridManager _instance;
    public static GridManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("GridManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    #region Variables
    [Header("<b><size=15>Grid Data<b><size=15>")]
    [Line(255,255,255)]
    [Header("<b><size=15>Dimensions<b><size=15>")]
    //sets the width and height of the grid
    public int width;
    public int height;
    [Space(10)][Header("<b><size=15>Objects<b><size=15>")]
    public Transform gridParent; //grid parent where we will spawn tiles 
    public Tile tilePrefab; //tile prefab
    public Dictionary<Vector2, Tile> grid = new Dictionary<Vector2, Tile>();
    public List<Tile> tiles;
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        InitGrid();
    }
    #endregion

    //play this at the start
    public void InitGrid()
    {
        tiles.Clear();
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        if (grid.TryGetValue(position, out var tile))
            return tile;

        return null;
    }
}
