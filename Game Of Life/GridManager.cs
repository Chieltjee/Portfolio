using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridSizeY;
    public int gridSizeX;

    public GameObject[,] gridArray;
    public int[,] stateArray;
    public int[,] stateArray2;
    public int[] optionArrayX;
    public int[] optionArrayY;

    public GameObject TilePrefab;

    private SpriteRenderer spriteRenderer;
    public Sprite aliveTile;
    public Sprite deadTile;

    public Transform startPos;

    private bool gameActive;

    private float timer;
    public float timerMax = 2.5f;

    private void Start()
    {
        if (gridSizeX == 0 || gridSizeY == 0)
        {
            Debug.LogError("Grid Width or Height is not set.");
        }   
        //Define all arrays
        gridArray = new GameObject[gridSizeX, gridSizeY];
        stateArray = new int[gridSizeX, gridSizeY];
        stateArray2 = new int[gridSizeX, gridSizeY];

        //Create grid
        CalculateGrid();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Converts mouseposition to useable data
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            //Send raycast
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                //Loop x and y values
                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < gridArray.GetLength(0); y++)
                    {
                        //Check if the hit gameobject is the same as the looped one
                        if (hit.collider.gameObject == gridArray[x,y].gameObject)
                        {
                            //If the tile is dead, make it alive. Else kill it
                            if (stateArray[x,y] == 0)
                            {
                                stateArray[x,y] = 1;
                                CalculateGrid();
                            }
                            else
                            {
                                stateArray[x, y] = 0;
                                CalculateGrid();
                            }
                        }
                    }
                }
            }
        }
        //Timer to update the grid every .. time
        if (gameActive)
            timer += Time.deltaTime;

        while (timer > timerMax)
        {
            timer = 0;
            ApplyRules();
            CalculateGrid();
        }

    }
    /// <summary>
    /// Update game state
    /// </summary>
    /// <param name="state"></param>
    public void GameState(bool state)
    {
        gameActive = state;
    }

    /// <summary>
    /// Calculates the x and y for the grid
    /// </summary>
    private void CalculateGrid()
    {
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                DrawTile(x, y);
            }
        }
    }

    /// <summary>
    /// Draws the tiles in the grid and changes the spriterenderer
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void DrawTile(int x, int y)
    {
        //if the grid is not empty
        if (gridArray[x,y] != null)
        {
            //Get the spriterenderer
            spriteRenderer = gridArray[x,y].GetComponent<SpriteRenderer>();

            //If the tile is alive update the sprite to alive
            if (stateArray[x,y] == 1)
            {
                spriteRenderer.sprite = aliveTile;
            }
            else
            {
                spriteRenderer.sprite = deadTile;
            }
        }
        else
        {
            //Get the spriterenderer, instantiate the gameobject and update its name
            spriteRenderer = TilePrefab.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = deadTile;
            gridArray[x,y] = Instantiate(TilePrefab, new Vector3(startPos.transform.position.x + 0.5f, 
            startPos.transform.position.y + 0.5f) + new Vector3(x * 1.0f, y * 1.0f), Quaternion.identity);
            gridArray[x,y].gameObject.name = x + "," + y;
        }
    }

    /// <summary>
    /// Applies the rules of game of life to the tiles in the grid
    /// </summary>
    private void ApplyRules()
    {
        //Loop x and y of the grid
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                int nearTiles = 0;
                //Loop the length of the optionarray
                for (int i = 0; i < optionArrayX.Length; i++)
                {
                    int offsetX = optionArrayX[i];
                    int offsetY = optionArrayY[i];
                    //Check if the tile is not the middle one
                    if (offsetX == 0 && offsetY == 0)
                        continue;
                    //Check if the tile is inside the grid
                    if (x + offsetX > 0 && x + offsetX < gridSizeX &&
                        y + offsetY > 0 && y + offsetY < gridSizeY)
                    {
                        //Check if the tile is alive
                        int v = stateArray[x + offsetX, y + offsetY];
                        if (v == 1) nearTiles++;
                    }
                }

                stateArray2[x, y] = stateArray[x, y];
                //Less then 2 and more then 3 tiles means it dies
                if (nearTiles < 2 || nearTiles > 3)
                {
                    stateArray2[x, y] = 0;
                }
                //If the tile is dead and theres 3 tiles around it it becomes alive
                else if (nearTiles == 3 && stateArray[x, y] == 0)
                {
                    stateArray2[x, y] = 1;
                }
            }
        }
        //Swap the array values
        (stateArray, stateArray2) = (stateArray2, stateArray);
    }



    public void RandomStart()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                int random = Random.Range(0, 100);

                if (random < 20)
                {
                    stateArray[x, y] = 1;
                }
                else
                {
                    stateArray[x, y] = 0;
                }
            }
        }
        CalculateGrid();
    }
}
