using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : GamePiece
{
    private GamePiece gamePieceData;

    private readonly int[,] validPositions = new int[,] { { -1, -2 }, 
                                                          { -2, -1 },
                                                          { -2,  1 },
                                                          { -1,  2 },
                                                          {  1,  2 },
                                                          {  2,  1 },
                                                          {  2, -1 },
                                                          {  1, -2 },};

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
        Tile parentTile = gamePieceData.GetGamePieceParentTile();
        int currentX = parentTile.GetXPos();
        int currentY = parentTile.GetYPos();

        int wantedX = wantedTile.GetXPos();
        int wantedY = wantedTile.GetYPos();

        int diffX = currentX - wantedX; 
        int diffY = currentY - wantedY;

        for (int i = 0; i < 8; i++)
        {
            //Debug.Log(validPositions[i, 0] + " " + validPositions[i, 1]);
            if (validPositions[i, 0] == diffX && validPositions[i, 1] == diffY) return true;
        }

        return false;
    }
}
