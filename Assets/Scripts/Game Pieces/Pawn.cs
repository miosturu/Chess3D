using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : GamePiece
{
    private bool hasMoved = false;
    private int moveDirection = 0;
    private int promotionRow = -1;
    private GamePiece gamePieceData;
    private TileMap gameBoard;
    private GameManager manager;

    private readonly int[,] validAttack = new int[,] {{ -1,  1 },
                                                      {  1,  1 } };

    private void Start()
    {
        gamePieceData = this.GetComponent<GamePiece>();
        gameBoard = this.GetComponentInParent<TileMap>();
        int teamNum = gamePieceData.GetCurrentGameTeamNumber();

        DetermineDirection(teamNum);

        manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }


    /// <summary>
    /// Determines the movement direction and promotion row
    /// </summary>
    /// <param name="teamNumber">Piece's team number</param>
    public void DetermineDirection(int teamNumber)
    {
        if (teamNumber == 0)
        {
            moveDirection = 1;
            promotionRow = 7;
        }
        else if (teamNumber == 1)
        {
            moveDirection = -1;
            promotionRow = 0;
        }
        else
        {
            Debug.Log("Invalid team number");
        }
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

        int distance = Mathf.Abs(wantedY - currentY);

        if (wantedTile.GetGamepieceOnThisTile() == null)
        {
            if (distance == 1)
            {
                if (wantedX == currentX && wantedY == currentY + 1 * moveDirection)
                {
                    hasMoved = true;
                    PromotePawn(wantedY);
                    return true;
                }
            }
            else if (distance == 2 && !hasMoved)
            {
                for (int i = 1; i <= distance; i++)
                {
                    Tile currentTile = gameBoard.GetTileAt(currentX, currentY + (i * moveDirection));
                    if (currentTile.GetGamepieceOnThisTile() != null && currentTile != wantedTile)
                    {
                        hasMoved = true;
                        PromotePawn(wantedY);
                        return false;
                    }
                    if (currentTile == wantedTile)
                    {
                        hasMoved = true;
                        PromotePawn(wantedY);
                        return true;
                    }
                }
            }
        }
        else if (wantedTile.GetGamepieceOnThisTile() != null)
        {
            for (int i = 0; i <= 1; i++)
            {
                if (diffX == validAttack[i, 0] * moveDirection && diffY == validAttack[i, 1] * moveDirection)
                {
                    hasMoved = true;
                    PromotePawn(wantedY);
                    return true;
                }
            }
        }
        return false;
    }


    /// <summary>
    /// Promotes pawn if it's on correct row
    /// </summary>
    /// <param name="position">Pawn's position</param>
    public void PromotePawn(int position)
    {
        if (position == promotionRow)
        {
            manager.PromotePawn(transform.gameObject);
        }
    }
}
