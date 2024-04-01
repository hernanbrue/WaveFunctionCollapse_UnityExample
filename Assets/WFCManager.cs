using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class WFCManager : MonoBehaviour
{
    [SerializeField] GameObject[] _tiles = new GameObject[5]; // Ajusta el tamaño según el número de estados
    [SerializeField] int grid_xSize = 20, grid_ySize = 20;
    [SerializeField] float delayBetweenCollapses = 0.1f; // Tiempo de espera entre colapsos (en segundos)

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
        StartCoroutine("CollapseTiles");
    }

    // Función para crear una cuadrícula aleatoria
    private void CreateRandomGrid(int xSize, int ySize)
    {
        _grid = new Tile[xSize, ySize];

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                // Asignar un estado inicialmente aleatorio
                GridState state = (GridState)UnityEngine.Random.Range(1, Enum.GetValues(typeof(GridState)).Length);

                // Asignar un GameObject según el estado
                GameObject tileObj = _stateToTileMap[state];

                if (tileObj != null)
                {
                    _grid[x, y] = new Tile(Instantiate(tileObj, new Vector2(x, y), Quaternion.identity), state, false);
                }
            }
        }
    }

    // Función para colapsar las celdas de manera secuencial con un retraso
    private IEnumerator CollapseTiles()
    {
        int xSize = _grid.GetLength(0);
        int ySize = _grid.GetLength(1);

        // Colapsar la primera celda (0, 0)
        _grid[0, 0].isCollapsed = true;

        // Iterar sobre las celdas restantes
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                // Si la celda ya está colapsada, continuar con la siguiente
                if (_grid[x, y].isCollapsed)
                    continue;

                // Obtener los estados de las celdas vecinas
                var neighbors = GetNeighborStates(x, y);

                // Filtrar los posibles estados excluyendo los estados vecinos
                var possibleStates = _grid[x, y]._posibleStates.Except(neighbors).ToArray();

                // Si no hay estados posibles, retroceder y reevaluar celdas colapsadas
                if (possibleStates.Length == 0)
                {
                    x = Mathf.Max(x - 2, 0);
                    y = Mathf.Max(y - 1, 0);
                    continue;
                }

                // Asignar un estado aleatorio de los posibles
                GridState newState = possibleStates[UnityEngine.Random.Range(0, possibleStates.Length)];

                // Colapsar la celda con el nuevo estado
                _grid[x, y].state = newState;
                _grid[x, y].isCollapsed = true;

                // Mostrar la celda colapsada durante un breve período de tiempo
                yield return new WaitForSeconds(delayBetweenCollapses);
            }
        }
    }

    // Función para obtener los estados de las celdas vecinas teniendo en cuenta las restricciones
    private GridState[] GetNeighborStates(int x, int y)
    {
        List<GridState> neighborStates = new List<GridState>();

        // Obtener los estados de las celdas vecinas dentro de los límites de la cuadrícula
        if (x > 0)
            neighborStates.Add(_grid[x - 1, y].state); // Izquierda
        if (x < _grid.GetLength(0) - 1)
            neighborStates.Add(_grid[x + 1, y].state); // Derecha
        if (y > 0)
            neighborStates.Add(_grid[x, y - 1].state); // Abajo
        if (y < _grid.GetLength(1) - 1)
            neighborStates.Add(_grid[x, y + 1].state); // Arriba

        // Aplicar restricciones adicionales para cada tipo de celda
        switch (_grid[x, y].state)
        {
            case GridState.Up:
                neighborStates.RemoveAll(state => state != GridState.Blank && state != GridState.Up);
                break;
            case GridState.Down:
                neighborStates.RemoveAll(state => state != GridState.Blank && state != GridState.Down);
                break;
            case GridState.Left:
                neighborStates.RemoveAll(state => state != GridState.Right && state != GridState.Down && state != GridState.Left);
                break;
            case GridState.Right:
                neighborStates.RemoveAll(state => state != GridState.Left && state != GridState.Blank && state != GridState.Right);
                break;
            case GridState.Blank:
                neighborStates.RemoveAll(state => state != GridState.Up && state != GridState.Right && state != GridState.Left && state != GridState.Down);
                break;
        }

        return neighborStates.ToArray();
    }


    void Update()
    {

    }
}
