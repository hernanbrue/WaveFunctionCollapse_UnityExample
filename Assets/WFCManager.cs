using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCManager : MonoBehaviour
{
    [SerializeField] GameObject[] _tiles = new GameObject[5]; // Ajusta el tamaño según el número de estados
    [SerializeField] int grid_xSize = 4, grid_ySize = 4;

    Dictionary<GridState, GameObject> _stateToTileMap = new Dictionary<GridState, GameObject>();

    Tile[,] _grid;

    void Start()
    {
        // Mapear cada estado del enumerador a su GameObject correspondiente
        _stateToTileMap[GridState.Blank] = _tiles[(int)GridState.Blank]; // O puedes asignar un GameObject específico para Blank si es necesario
        _stateToTileMap[GridState.Up] = _tiles[(int)GridState.Up];
        _stateToTileMap[GridState.Left] = _tiles[(int)GridState.Left];
        _stateToTileMap[GridState.Right] = _tiles[(int)GridState.Right];
        _stateToTileMap[GridState.Down] = _tiles[(int)GridState.Down];

        CreateRandomGrid(grid_xSize, grid_ySize);
        CheckNeighbours(grid_xSize, grid_ySize);
        WaveFunctionCollapse(grid_xSize, grid_ySize);

    }

    private void CreateRandomGrid(int xSize, int ySize)
    {
        _grid = new Tile[xSize, ySize];

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                // Asignar un estado inicialmente aleatorio
                GridState state = (GridState)Random.Range(1, System.Enum.GetValues(typeof(GridState)).Length);

                // Asignar un GameObject según el estado
                GameObject tileObj = _stateToTileMap[state];
               
                if (tileObj != null)
                {
                    _grid[x, y] = new Tile(Instantiate(tileObj, new Vector2(x, y), Quaternion.identity), state, false);
                    
                }

            }
        }
    }

    //private void CreateBlankGrid(int xSize, int ySize)
    //{
    //    _grid = new Tile[xSize, ySize];

    //    for (int x = 0; x < xSize; x++)
    //    {
    //        for (int y = 0; y < ySize; y++)
    //        {
    //            // Asignar un estado inicialmente aleatorio
    //            GridState state = (GridState)Random.Range(1, System.Enum.GetValues(typeof(GridState)).Length);

    //            // Asignar un GameObject según el estado
    //            GameObject tileObj = _stateToTileMap[state];

    //            if (tileObj != null)
    //            {
    //                _grid[x, y] = new Tile(Instantiate(tileObj, new Vector2(x, y), Quaternion.identity), state, false);

    //            }

    //        }
    //    }
    //}

    private void WaveFunctionCollapse(int xSize, int ySize)
    {
        _grid[0, 0].isCollapsed = true; //colapso la primera para tener un punto de partida

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (!_grid[x, y].isCollapsed)
                {
                    _grid[x, y].gameObject.SetActive(false);
                }

                else
                {
                    _grid[x, y].gameObject.SetActive(true);
                }

            }
        }
    }

    private void CheckNeighbours(int xSize, int ySize)
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                print(_grid[x, y].isCollapsed);// para probar que funcione  

            }
        }
    }

    void Update()
    {
           
    }
}
