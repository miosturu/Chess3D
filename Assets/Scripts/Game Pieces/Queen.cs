using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : GamePiece
{
    private GamePiece gamePieceData;
    private TileMap gameBoard;

    private void Start()
    {
        gamePieceData = this.GetComponent<GamePiece>();
        gameBoard = this.GetComponentInParent<TileMap>();
    }

    /// <summary>
    /// Return if wanted tile position is valid to move to.
    /// </summary>
    /// <returns>Returns true if position is valid</returns>
    public bool TilePositionIsValid(Tile wantedTile)
    {
        Tile parentTile = gamePieceData.GetGamePieceParentTile();
        int currentX = parentTile.GetXPos(); //Debug.Log(currentX);
        int currentY = parentTile.GetYPos(); //Debug.Log(currentY);

        int wantedX = wantedTile.GetXPos();
        int wantedY = wantedTile.GetYPos();

        int diffX = wantedX - currentX;
        int diffY = wantedY - currentY;

        int mulX = 0; int mulY = 0;
        int distance = 0;

        if ((diffX == 0 && diffY != 0) || (diffX != 0 && diffY == 0)) // Determine the direction
        {
            if (diffX == 0)
            {
                if (diffY > 0)
                {
                    mulY = 1;
                    distance = Mathf.Abs(diffY);
                }
                else
                {
                    mulY = -1;
                    distance = Mathf.Abs(diffY);
                }
            }
            else
            {
                if (diffX > 0)
                {
                    mulX = 1;
                    distance = Mathf.Abs(diffX);
                }
                else
                {
                    mulX = -1;
                    distance = Mathf.Abs(diffX);
                }
            }
        }
        else if ((diffX / diffY == 1) || (diffX / diffY == -1))
        {
            distance = Mathf.Abs(diffY);
            if (diffX > 0 && diffY > 0)
            {
                mulX = 1;
                mulY = 1;
            }
            else if (diffX > 0 && diffY < 0)
            {
                mulX = 1;
                mulY = -1;
            }
            else if (diffX < 0 && diffY < 0)
            {
                mulX = -1;
                mulY = -1;
            }
            else if (diffX < 0 && diffY > 0)
            {
                mulX = -1;
                mulY = 1;
            }
        }

        for (int i = 1; i <= distance; i++)
        {
            Tile currentTile = gameBoard.GetTileAt(currentX + (i * mulX), currentY + (i * mulY));
            if (currentTile.GetGamepieceOnThisTile() != null && currentTile != wantedTile)
            {
                return false;
            }
            if (currentTile == wantedTile)
            {
                return true;
            }
        }

        return false;
    }
}
