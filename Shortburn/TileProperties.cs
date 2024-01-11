using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TileProperties : MonoBehaviour
{
    [SerializeField] private bool _walkableTile;
    private bool _isPressed;

    public TileMaze tileMaze;

    public int tileX, tileZ;

    private Vector3 _tilePos;
    private float _desiredTilePos = 0.05f;
    private float _neededTilePos = 0.005f;
    private void Start()
    {
        _tilePos = transform.position;
    }

    /// <summary>
    /// Checks collisions on tiles
    /// </summary>
    /// <param name="collision">Colliding object</param>
    private void OnCollisionEnter(Collision collision)
    {
        //If the colliding object is a player
        if (collision.gameObject.GetComponentInParent<SC_FPSController>() != null)
        {
            //If the tile is not pressed yet
            if (!_isPressed)
            {
                //If tile is walkable
                if (_walkableTile)
                {
                    //Make the tile look pressed
                    _tilePos.y -= _desiredTilePos;
                    //Set pressed variable to true
                    _isPressed = true;
                    //Move the tile to the desired height
                    StartCoroutine(LerpTile());
                }
                else
                {
                    //Run function that handles the wrong tile being pressed
                    tileMaze.NotWalkableTile(collision.transform.parent.gameObject);
                }
            }
        }
        //If colliding object is a rock
        else if (collision.gameObject.GetComponent<Rock>() != null)
        {
            //Reveal the tiles around the object
            StartCoroutine(tileMaze.RevealTilesCoroutine(tileX, tileZ, tileMaze._tileRevealTime));
            //Play a visual effect
            StartCoroutine(PlayVisualRockEffect(collision.gameObject, 1f));
        }
    }

    /// <summary>
    /// Lerps tile to the desired height
    /// </summary>
    /// <returns>Nothing</returns>
    private IEnumerator LerpTile()
    {
        //While the tile is not at the needed height, keep lerping
        while (Vector3.Distance(transform.position, _tilePos) > _neededTilePos)
        {
            //Run the lerp
            transform.position = Vector3.Lerp(transform.position, _tilePos, 1 * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// Handles playing the visual effect when rock collides with a tile
    /// </summary>
    /// <param name="l_rock">The rock that is colliding with a tile</param>
    /// <param name="l_waitTime">Time to wait before killing the effect</param>
    /// <returns></returns>
    private IEnumerator PlayVisualRockEffect(GameObject l_rock, float l_waitTime)
    {
        //Get meshrenderer and disable
        l_rock.GetComponent<MeshRenderer>().enabled = false;
        //Get visual effect component and enable
        l_rock.GetComponent<VisualEffect>().enabled = true;
        //Wait
        yield return new WaitForSeconds(l_waitTime);
        //Destroy rock
        Destroy(l_rock);
        yield return null;

    }

    /// <summary>
    /// Checks if the tile is walkable
    /// </summary>
    /// <returns>Boolean with the walkable state</returns>
    public bool IsWalkable()
    {
        return _walkableTile;
    }

    /// <summary>
    /// Change walkableTile state
    /// </summary>
    /// <param name="state"></param>
    public void SetWalkable(bool state)
    {
        _walkableTile = state;
    }
}
