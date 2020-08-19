using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base game piece class.
/// Knows:
///     It's parent tile
///     It's own team
/// </summary>
public class GamePiece : MonoBehaviour
{
    private Tile parentTile;
    private int teamNumber;


    /// <summary>
    /// Game piece constructor.
    /// </summary>
    public GamePiece()
    {
        parentTile = null;
        teamNumber = -1;
    }


    /// <summary>
    /// Changes game piece's team number.
    /// By convention numbers should be positive.
    /// </summary>
    /// <param name="number"></param>
    public void ChangeTeamNumber(int number)
    {
        teamNumber = number;
    }


    /// <summary>
    /// Changes game piece's parent tile
    /// </summary>
    /// <param name="newTile">New parent tile</param>
    public void ChangeParentTile(Tile newTile)
    {
        parentTile = newTile;
    }


    /// <summary>
    /// Return's game piece's parent tiles
    /// </summary>
    /// <returns>Game piece's parent tile.</returns>
    public Tile GetGamePieceParentTile()
    {
        //Debug.Log("Called GetGamePieceParentTile(), parent tile: " + parentTile.name);
        return parentTile;
    }


    /// <summary>
    /// Returns game piece's team number.
    /// </summary>
    /// <returns>Game piece's team number</returns>
    public int GetCurrentGameTeamNumber()
    {
        return teamNumber;
    }


    /// <summary>
    /// Change game piece's name
    /// </summary>
    /// <param name="newName">Piece's new name</param>
    public void ChangeName(string newName)
    {
        name = newName;
    }

    /// <summary>
    /// Return if wanted tile position is valid to move to.
    /// </summary>
    /// <returns>Always returns true</returns>
    public bool TilePositionIsValid(Tile wantedTile)
    {
        Debug.Log("Called function \"TilePositionIsValid()\" in class \"GamePiece\"");
        return true;
    }
}




enum PieceName
{
    Pawn,   // 0
    Rook,   // 1
    Knight, // 2
    Bishop, // 3
    Queen,  // 4
    King    // 5
}