using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMaze : MonoBehaviour
{
    [SerializeField] private GameObject _tilePrefab;
    private GameObject[,] _gridTiles;

    [SerializeField] private Transform _resetPos;
    
    [SerializeField] private int _gridSizeX, _gridSizeZ;
    [SerializeField] private int[] _revealTilesIntX, _revealTilesIntZ;
    [SerializeField] private int _puzzleTries;
    [SerializeField] private int _puzzleTriesMax;

    public float _tileRevealTime;

    [SerializeField] private Material[] _tileMaterials;

    private void Start()
    {
        GenerateGrid();
        GenerateWalkableTilePath();
    }

    /// <summary>
    /// Generates a new tile grid
    /// </summary>
    private void GenerateGrid()
    {
        //If there is already a grid
        if (_gridTiles != null)
        {
            //Loop x and z
            for (int x = 0; x < _gridTiles.GetLength(0); x++)
            {
                for (int z = 0; z < _gridTiles.GetLength(1); z++)
                {
                    //Destroy tile
                    Destroy(_gridTiles[x, z].gameObject);
                }
            }
        }
        //Create new gridTile array
        _gridTiles = new GameObject[_gridSizeX, _gridSizeZ];
        //Loop x and z
        for (int x = 0; x < _gridSizeX; x++)
        {
            for (int z = 0; z < _gridSizeZ; z++)
            {
                //Instantiate new tile     
                _gridTiles[x, z] = Instantiate(_tilePrefab, new Vector3(transform.position.x + (x * 1.5f), transform.position.y, transform.position.z + (z * 1.5f)), Quaternion.identity);
                //Get the tile properties script
                TileProperties l_properties = _gridTiles[x, z].gameObject.GetComponent<TileProperties>();
                //Set needed variables
                l_properties.tileX = x;
                l_properties.tileZ = z;
                l_properties.tileMaze = this;
            }
        }
    }

    /// <summary>
    /// Reveals tiles around the set tile
    /// </summary>
    /// <param name="l_tileX">The x value of the tile</param>
    /// <param name="l_tileZ">The z value of the tile</param>
    /// <param name="l_reveal">Boolean to enable revealing tiles</param>
    /// <returns></returns>
    private IEnumerator RevealTiles(int l_tileX, int l_tileZ, bool l_reveal)
    {
        //Loop the length of the optionarray
        for (int i = 0; i < _revealTilesIntX.Length; i++)
        {
            int offsetX = _revealTilesIntX[i];
            int offsetZ = _revealTilesIntZ[i];
            //Check if the tile is inside the grid
            if (l_tileX + offsetX >= 0 && l_tileX + offsetX <= _gridSizeX &&
                l_tileZ + offsetZ >= 0 && l_tileZ + offsetZ <= _gridSizeZ)
            {
                //If reveal tiles bool true
                if (l_reveal)
                {
                    //Check if the tile is walkable
                    if (_gridTiles[l_tileX + offsetX, l_tileZ + offsetZ].GetComponent<TileProperties>().IsWalkable())
                        //Set material to positive material
                        _gridTiles[l_tileX + offsetX, l_tileZ + offsetZ].gameObject.GetComponent<MeshRenderer>().material = _tileMaterials[1];
                    else
                        //Set material to negative material
                        _gridTiles[l_tileX + offsetX, l_tileZ + offsetZ].gameObject.GetComponent<MeshRenderer>().material = _tileMaterials[2];
                }
                else
                    //Return tile material to normal
                    _gridTiles[l_tileX + offsetX, l_tileZ + offsetZ].gameObject.GetComponent<MeshRenderer>().material = _tileMaterials[0];
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    /// <summary>
    /// Coroutine to run reveal tiles function
    /// </summary>
    /// <param name="l_tileX">The x of the selected tile</param>
    /// <param name="l_tileZ">The z of the selected tile</param>
    /// <param name="l_revealTime">Time that the tiles will be revaled for</param>
    /// <returns></returns>
    public IEnumerator RevealTilesCoroutine(int l_tileX, int l_tileZ, float l_revealTime)
    {
        //Run coroutine to reveal the tiles
        StartCoroutine(RevealTiles(l_tileX, l_tileZ, true));
        //Wait set amount of time
        yield return new WaitForSeconds(l_revealTime);
        //Run the coroutine to hide the tiles
        StartCoroutine(RevealTiles(l_tileX, l_tileZ, false));
        yield return null;
    }

    /// <summary>
    /// Generates a walkable path on the grid
    /// </summary>
    private void GenerateWalkableTilePath()
    {
        //List for the walkable tiles
        List<GameObject> l_walkableTiles = new List<GameObject>();
        //Random start int
        int l_randomStart = UnityEngine.Random.Range(0, _gridSizeX - 1);
        //Set start tile
        GameObject l_startTile = _gridTiles[l_randomStart, 0];
        //Random end int
        int l_randomEnd = UnityEngine.Random.Range(l_randomStart, _gridSizeX);
        //Set start end
        GameObject l_endTile = _gridTiles[l_randomEnd, _gridSizeZ - 1];

        //Tiles before a turn is made
        int l_tilesBeforeTurn = 0;
        //Add start tile to list
        l_walkableTiles.Add(l_startTile);
        //If the start and end tile are not in a line
        if (l_randomStart < l_randomEnd)
        {
            //Set random tile amount before turn
            l_tilesBeforeTurn = UnityEngine.Random.Range(2, _gridSizeZ - 2);
            int l_tileZ = 0;
            int l_tileX = l_randomStart;
            //Loop till a turn has to be made
            for (int i = 0; i < l_tilesBeforeTurn; i++)
            {
                //Add tile to walkable tiles list
                l_walkableTiles.Add(_gridTiles[l_tileX, l_tileZ]);
                l_tileZ++;
            }
            //How many tiles to move in the turn
            int difference = l_randomEnd - l_randomStart;
            //Keep looping till new turn has to be made
            for (int i = 0; i < difference + 1; i++)
            {
                l_walkableTiles.Add(_gridTiles[l_tileX, l_tileZ]);
                l_tileX++;
            }
            //How many tiles left
            int l_tilesLeft = _gridSizeZ - l_tileZ;
            //Loop till at the end tile
            for (int i = 0; i < l_tilesLeft; i++)
            {
                //Add tile to walkable tiles list
                l_walkableTiles.Add(_gridTiles[l_randomEnd, (l_tileZ - 1) + i]) ;
            }
        }
        else
        {
            //Loop one line from start to end
            for (int i = 0; i < _gridSizeZ; i++)
            {
                //Add tile to walkable tiles list
                l_walkableTiles.Add(_gridTiles[l_randomStart, i]);
            }
        }

        //Add last tile to walkable tiles list
        l_walkableTiles.Add(l_endTile);
        for (int i = 0; i < l_walkableTiles.Count; i++)
        {
            //Set all tiles to walkable in the list
            l_walkableTiles[i].GetComponent<TileProperties>().SetWalkable(true);
        }
    }
    /// <summary>
    /// Not walkable tiles list
    /// </summary>
    /// <param name="l_player"></param>
    public void NotWalkableTile(GameObject l_player)
    {
        //If the puzzle tries is not the max yet
        if (_puzzleTries < _puzzleTriesMax)
        {
            //Increase tries int
            _puzzleTries++;
        }
        //Generate new grid and set new path
        else
        {
            GenerateGrid();
            GenerateWalkableTilePath();
            FindObjectOfType<RockThrowing>().thrownRocks = 0;
            _puzzleTries = 0;
        }
        //Get charactercontroller
        CharacterController l_controller = l_player.GetComponentInParent<CharacterController>();
        //Change charactercontroller state and update position
        l_controller.enabled = false;
        l_player.transform.position = _resetPos.position;
        l_controller.enabled = true;
    }
}
