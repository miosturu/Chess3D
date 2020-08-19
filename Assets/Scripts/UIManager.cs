using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text currentPieceText;
    public Text currentPlayerTurnText;
    public Text eventText;

    public string gameOverText;


    /// <summary>
    /// Makes all the texts blank
    /// </summary>
    private void Start()
    {
        currentPieceText.text = "Currently selected: \n";
        SetCurrentPlayerTurn(0);
        eventText.text = "";
    }


    /// <summary>
    /// Sets current piece text to anything that game manager tells
    /// </summary>
    /// <param name="pieceName">Piece's name</param>
    public void SetHeldPieceText(string pieceName)
    {
        currentPieceText.text = "Currently selected: \n" + pieceName;
    }


    /// <summary>
    /// Sets event text if game piece was eaten
    /// </summary>
    /// <param name="attacker">Attacker's piece</param>
    /// <param name="defender">Defende's piece</param>
    public void SetEventText(GameObject attacker, GameObject defender)
    {
        eventText.text = attacker.name + " ate " + defender.name;
    }


    /// <summary>
    /// Sets new message to event log.
    /// </summary>
    public void SetEventText()
    {
        eventText.text = gameOverText;
    }


    /// <summary>
    /// Sets current player's turn text.
    /// This is just horrible way to do this bit I can't do better at the moment
    /// </summary>
    /// <param name="playerNumber">Player's number</param>
    public void SetCurrentPlayerTurn(int playerNumber)
    {
        if (playerNumber == 0)
        {
            currentPlayerTurnText.text = "White's turn";
        }
        else if (playerNumber == 1)
        {
            currentPlayerTurnText.text = "Black's turn";
        }
        else if (playerNumber == -1)
        {
            currentPlayerTurnText.text = "";
        }
        else
        {
            currentPlayerTurnText.text = "Something went wrong";
        }
    }
}
