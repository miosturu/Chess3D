using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text currentPieceText;
    public Text currentPlayerTurnText;


    /// <summary>
    /// Sets currentPieceText as blank
    /// </summary>
    private void Start()
    {
        currentPieceText.text = "";
        currentPlayerTurnText.text = "";
    }


    /// <summary>
    /// Sets current piece text to anything that game manager tells
    /// </summary>
    /// <param name="pieceName"></param>
    public void SetHeldPieceText(string pieceName)
    {
        currentPieceText.text = pieceName;
    }


    /// <summary>
    /// Sets current player's turn text
    /// </summary>
    /// <param name="playerNumber"></param>
    public void SetCurrentPlayerTurn(int playerNumber)
    {

    }
}
