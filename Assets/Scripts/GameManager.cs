using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This object is responsible for transfering commands to other objects, such as UI and map.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Global Variables

    [Header("Other game objects and components")]

    public GameObject UIManagerObject;
    private UIManager uiManager;

    [Header("Materials")]

    public Material[] materials;

    [Header("Game pieces")]

    public GameObject gamepieceObject;
    public Mesh[] pieceModels = new Mesh[] { };
    private readonly string[] pieceNames = new string[] { "Pawn", "Rook", "Knight", "Bishop", "Queen", "King" };
    private GamePiece gamePieceData;
    private GameObject currentGamePiece;

    [Header("Gameboard")]

    public GameObject tileMap;
    public GameObject baseTile;
    public int mapWidth; public int mapHeight;
    private TileMap newMap;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManagerObject.GetComponent<UIManager>();

        newMap = tileMap.GetComponent<TileMap>();

        newMap.DefineBaseTile(baseTile);
        newMap.DefineGameboardSize(mapWidth, mapHeight);
        newMap.GenerateMap(tileMap);

        int[] pawnPos = { 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] otherPos = { 1, 2, 3, 4, 5, 3, 2, 1 };

        PlacePieces(newMap.GetTileAt(0, 0), materials[0], otherPos, 0);
        PlacePieces(newMap.GetTileAt(0, 1), materials[0], pawnPos, 0);

        PlacePieces(newMap.GetTileAt(0, 7), materials[1], otherPos, 1);
        PlacePieces(newMap.GetTileAt(0, 6), materials[1], pawnPos, 1);
    }

    // Update is called once per frame
    void Update()
    {
        SelectPiece();
        MovePiece();
        UnselectGamePiece();
    }


    /// <summary>
    /// Select any game piece on the table with "Fire1"-button.
    /// Casts a ray that will hit any collider. If the collider contains "GamePiece"-script then it will be set as "currentGamePiece".
    /// </summary>
    public void SelectPiece()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform hitObject = hit.transform;
                GamePiece hitGamePiece= hitObject.parent.GetComponentInChildren<GamePiece>();

                if (hitGamePiece != null && currentGamePiece == null)
                {
                    currentGamePiece = hitObject.parent.gameObject;
                    uiManager.SetHeldPieceText(hitObject.parent.gameObject.name);
                }

                else if (hitGamePiece != null && currentGamePiece != null) // wanted game piece position is valid
                {
                    GamePiece currentPieceData = currentGamePiece.GetComponentInChildren<GamePiece>();

                    if (currentPieceData.GetCurrentGameTeamNumber() != hitGamePiece.GetCurrentGameTeamNumber())
                    {
                        Debug.Log("I wanna eat that " + hitObject.parent.gameObject);
                        newMap.EatOtherGamePiece(currentGamePiece, hitObject.parent.gameObject);
                        currentGamePiece = null;
                        uiManager.SetHeldPieceText("");
                    }
                    else
                    {
                        Debug.Log("It's on my own team, dummy!");
                    }
                }
            }
        }
    }


    /// <summary>
    /// Moves game piece to clicked tile. Uses raycasting.
    /// </summary>
    public void MovePiece()
    {
        if (Input.GetButtonDown("Fire1") && currentGamePiece != null)
        {
            Tile targetTile;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform hitObject = hit.transform;
                if (hitObject.parent.GetComponentInChildren<Tile>() != null)
                {
                    targetTile = hitObject.parent.GetComponentInChildren<Tile>();

                    newMap.MoveGamePiece(currentGamePiece, hitObject.parent.GetComponentInChildren<Tile>());
                    currentGamePiece = null;
                    uiManager.SetHeldPieceText("");
                }
            }
        }
    }


    /// <summary>
    /// Places game pieces on the gameboard from left to right.
    /// Placing starts from certain tile.
    /// </summary>
    /// <param name="startTile">First tile where placing starts</param>
    /// <param name="pieceColor">Piece's color</param>
    /// <param name="positions">What piece will be in position</param>
    public void PlacePieces(Tile startTile, Material pieceColor, int[] positions, int teamNumber)
    {
        int xPos = startTile.GetXPos();
        int yPos = startTile.GetYPos();

        for (int i = 0; i < positions.Length; i++)
        {
            PlacePiece(newMap.GetTileAt(xPos, yPos), pieceColor, pieceNames[positions[i]], teamNumber);
            xPos++;
        }
    }


    /// <summary>
    /// Places one piece to position with certain color.
    /// </summary>
    /// <param name="tile">Piece's position</param>
    /// <param name="pieceColor">Piece's color</param>
    public void PlacePiece(Tile tile, Material pieceMaterial, string pieceName, int teamNumber)
    {
        GameObject newGamePiece = Instantiate(gamepieceObject, tile.transform);
        GamePiece newGamePieceData = newGamePiece.GetComponent<GamePiece>();

        tile.ChangePieceOnTile(newGamePiece);

        newGamePieceData.ChangeParentTile(tile);
        newGamePieceData.ChangeTeamNumber(teamNumber);

        newGamePiece.GetComponentInChildren<MeshRenderer>().material = pieceMaterial;
        newGamePiece.name = pieceName;
    }


    /// <summary>
    /// Returns current piece
    /// </summary>
    /// <returns>Current piece</returns>
    public GameObject GetCurrentGamePiece()
    {
        return currentGamePiece;
    }


    /// <summary>
    /// Unselects game piece if Escape is pressed
    /// </summary>
    public void UnselectGamePiece()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentGamePiece = null;
            uiManager.SetHeldPieceText("");
        }
    }
}
