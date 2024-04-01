using System.Linq;
using UnityEngine;

public enum GridState
{
    Blank,
    Up,
    Left,
    Right,
    Down,
    // Otros estados si es necesario
}

public class Tile
{
    public GameObject gameObject;
    public GridState state;
    public bool isCollapsed;

    public GridState[] _posibleStates = new GridState[]
    {
        GridState.Blank,
        GridState.Left,
        GridState.Down,
        GridState.Right,
        GridState.Up
    };

    public Tile(GameObject obj, GridState s, bool collapse)
    {
        gameObject = obj;
        state = s;
        isCollapsed = collapse;


    }
}
