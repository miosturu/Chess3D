using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : GamePiece
{
    /*int[,] validPositions = new int[,] { { 1, 1, 1 } ,
                                         { 1, 0, 1 } ,
                                         { 1, 1, 1 } };*/
    private GamePiece gamePieceData;

    private void Start()
    {
        gamePieceData = this.GetComponent<GamePiece>();
    }


    /// <summary>
    /// Return if wanted tile position is valid to move to.
    /// </summary>
    /// <returns>Returns true if position is valid</returns>
    public bool TilePositionIsValid(Tile wantedTile)
    {
        //Debug.Log("Called fucntion \"TilePositionIsValid()\" in \"king\" class");
        Tile parentTile = gamePieceData.GetGamePieceParentTile();
        int currentX = parentTile.GetXPos();
        int currentY = parentTile.GetYPos();

        int wantedX = wantedTile.GetXPos();
        int wantedY = wantedTile.GetYPos();

        if ((wantedX - currentX >= -1 && wantedX - currentX <= 1) && (wantedY - currentY >= -1 && wantedY - currentY <= 1))
        {
            return true;
        }

        return false;
    }
}
