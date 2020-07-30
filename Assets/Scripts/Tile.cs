using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Tile object will be know where it is and what gamepiece is on it.
/// </summary>
public class Tile : MonoBehaviour
{
    private int xPos, yPos;
    private GameObject gamepieceOnThisTile;


    /// <summary>
    /// Tile constructor.
    /// </summary>
    public Tile()
    {
        xPos = 0;
        yPos = 0;
    }


    /// <summary>
    /// This method allows for tile to change it's own coordinate position.
    /// </summary>
    /// <param name="x">X-position</param>
    /// <param name="y">Y-position</param>
    public void ChangeTilePositionData(int x, int y)
    {
        xPos = x;
        yPos = y;
    }


    /// <summary>
    /// Sets the gamepiece on this tile.
    /// </summary>
    /// <param name="piece">Gamepiece that will be on this tile. Can be bull.</param>
    public void ChangePieceOnTile(GameObject piece)
    {
        gamepieceOnThisTile = piece;
    }


    /// <summary>
    /// Returns gamepiece on this tile.
    /// </summary>
    /// <returns>Current gamepiece on this tile. Can be null.</returns>
    public GameObject GetGamepieceOnThisTile()
    {
        return gamepieceOnThisTile;
    }


    /// <summary>
    /// Returns tile's x-position
    /// </summary>
    /// <returns>X-position</returns>
    public int GetXPos()
    {
        return xPos;
    }


    /// <summary>
    /// Returns tile's y-position
    /// </summary>
    /// <returns>Y-position</returns>
    public int GetYPos()
    {
        return yPos;
    }
}
