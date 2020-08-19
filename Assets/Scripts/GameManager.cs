using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
    public Material[] pieceMaterials;

    [Header("Audio")]

    public AudioSource audioSource;
    public AudioClip[] movementSounds;

    [Header("Game pieces")]

    public GameObject gamepieceObject;
    public Mesh[] pieceModels = new Mesh[] { };
    private readonly string[] pieceNames = new string[] { "Pawn", "Rook", "Knight", "Bishop", "Queen", "King" };
    private GamePiece gamePieceData;
    private GameObject currentGamePiece;

    // Pawn positions

    private readonly int[] pawnPos = { 0, 0, 0, 0, 0, 0, 0, 0 };
    private readonly int[] otherPos = { 1, 2, 3, 4, 5, 3, 2, 1 };

    [Header("Gameboard")]

    public GameObject tileMap;
    public GameObject baseTile;
    public int mapWidth; public int mapHeight;
    private TileMap newMap;

    [Header("Player turn")]

    public int[] turnOrder = new int[] { 0, 1 };
    public int playerTurn;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Determines player's turn
        playerTurn = turnOrder[0];

        // Get the game ui manager from the scene
        uiManager = UIManagerObject.GetComponent<UIManager>();

        // Creating the game board
        newMap = tileMap.GetComponent<TileMap>();
        newMap.DefineBaseTile(baseTile);
        newMap.DefineGameboardSize(mapWidth, mapHeight);
        newMap.GenerateMap(tileMap);

        // Place white (team number 0) pieces
        PlacePieces(newMap.GetTileAt(0, 0), pieceMaterials[0], otherPos, 0);
        PlacePieces(newMap.GetTileAt(0, 1), pieceMaterials[0], pawnPos, 0);

        // Place black (team number 1) pieces
        PlacePieces(newMap.GetTileAt(0, 7), pieceMaterials[1], otherPos, 1);
        PlacePieces(newMap.GetTileAt(0, 6), pieceMaterials[1], pawnPos, 1);
    }

    // Update is called once per frame
    void Update()
    {
        ManipulateGamePieces();
        UnselectGamePiece();
    }


    /// <summary>
    /// Manipulates game piece in general. 
    /// Uses raycasting to select tile from which it tries to get game piece.
    /// This function can: 
    ///     select game piece,
    ///     move game piece,
    ///     and devour oposing game piece
    /// </summary>
    public void ManipulateGamePieces()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int teamNum = -1;

            if (Physics.Raycast(ray, out hit))
            {
                Transform hitObject = hit.transform;
                Tile hitTile = hitObject.parent.GetComponentInChildren<Tile>();

                try
                {
                    teamNum = hitObject.parent.GetComponentInChildren<GamePiece>().GetCurrentGameTeamNumber();
                }
                catch { /* No need to do anything else */ }

                if (hitTile.GetGamepieceOnThisTile() != null && currentGamePiece == null && teamNum == playerTurn) // And is correct player's turn
                {
                    // Select game piece on tile if there's no selected game pice

                    currentGamePiece = hitTile.GetGamepieceOnThisTile();
                    gamePieceData = currentGamePiece.GetComponent<GamePiece>();
                    uiManager.SetHeldPieceText(currentGamePiece.name);
                }
                else if (hitTile.GetGamepieceOnThisTile() == null && currentGamePiece != null && GetValidFromAny(hitTile, currentGamePiece))
                {
                    // Move to tile if move is valid

                    Tile targetTile = hitObject.parent.GetComponentInChildren<Tile>();

                    newMap.MoveGamePiece(currentGamePiece, hitObject.parent.GetComponentInChildren<Tile>());
                    currentGamePiece = null;
                    uiManager.SetHeldPieceText("");
                    ChangePlayerTurn();
                    audioSource.PlayOneShot(RandomAudioClip());
                }
                else if (hitTile.GetGamepieceOnThisTile() != null && currentGamePiece != null && GetValidFromAny(hitTile, currentGamePiece) && (gamePieceData.GetCurrentGameTeamNumber() != teamNum)) // Way to compress this?
                {
                    // Eat other game piece if move is valid and is on other team

                    GameObject defender = hitObject.parent.GetComponentInChildren<GamePiece>().gameObject;

                    newMap.EatOtherGamePiece(currentGamePiece, defender);
                    uiManager.SetEventText(currentGamePiece, defender);
                    currentGamePiece = null;
                    uiManager.SetHeldPieceText("");
                    ChangePlayerTurn();
                    CheckKingIsDead(defender);
                    audioSource.PlayOneShot(RandomAudioClip());
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
			if (positions[i] != -1)
			{
				PlacePiece(newMap.GetTileAt(xPos, yPos), pieceColor, pieceNames[positions[i]], teamNumber);
			}
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
        MeshFilter mesh = newGamePiece.GetComponentInChildren<MeshFilter>();
        GamePiece newGamePieceData = newGamePiece.GetComponent<GamePiece>();

        switch (pieceName)
        {
            case "Pawn":
                Pawn pawn = newGamePiece.AddComponent(typeof(Pawn)) as Pawn;
                mesh.mesh = pieceModels[0];
                break;
            case "Rook":
                Rook rook = newGamePiece.AddComponent(typeof(Rook)) as Rook;
                mesh.mesh = pieceModels[1];
                break;
            case "Knight":
                Knight knight = newGamePiece.AddComponent(typeof(Knight)) as Knight;
                mesh.mesh = pieceModels[2];
                break;
            case "Bishop":
                Bishop bishop = newGamePiece.AddComponent(typeof(Bishop)) as Bishop;
                mesh.mesh = pieceModels[3];
                break;
            case "Queen":
                Queen queen = newGamePiece.AddComponent(typeof(Queen)) as Queen;
                mesh.mesh = pieceModels[4];
                break;
            case "King":
                King king = newGamePiece.AddComponent(typeof(King)) as King;
                mesh.mesh = pieceModels[5];
                break;
            default:
                Debug.Log("Error: no such thing as " + pieceName);
                break;
        }

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
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Fire2"))
        {
            currentGamePiece = null;
            uiManager.SetHeldPieceText("");
        }
    }


    /// <summary>
    /// Keeps track of player turns.
    /// Should propably make ring buffer for this puropose.
    /// At the moment custom ring list didn't work thus using this solution.
    /// </summary>
    public void ChangePlayerTurn()
    {
        if (playerTurn == 0)
        {
            playerTurn++;
            uiManager.SetCurrentPlayerTurn(playerTurn);
        }
        else if (playerTurn == 1)
        {
            playerTurn--;
            uiManager.SetCurrentPlayerTurn(playerTurn);
        }
        else
        {
            Debug.Log("Tried to change player turn but failed");
        }
    }


    /// <summary>
    /// Returns if tile position is valid for any piece
    /// </summary>
    /// <param name="tilePos">Wanted tile position</param>
    /// <param name="gamePiece">Game piece data</param>
    /// <returns></returns>
    public bool GetValidFromAny(Tile tilePos, GameObject gamePiece)
    {
        string pieceName = gamePiece.name;

        switch (pieceName)
        {
            case "Pawn":
                // Get Pawn-script and get result
                return gamePiece.GetComponent<Pawn>().TilePositionIsValid(tilePos);
            case "Rook":
                // Get Rook-script and get result
                return gamePiece.GetComponent<Rook>().TilePositionIsValid(tilePos);
            case "Knight":
                // Get Knight-script and get result
                return gamePiece.GetComponent<Knight>().TilePositionIsValid(tilePos);
            case "Bishop":
                // Get Bishop-script and get result
                return gamePiece.GetComponent<Bishop>().TilePositionIsValid(tilePos);
            case "Queen":
                // Get Queen-script and get result
                return gamePiece.GetComponent<Queen>().TilePositionIsValid(tilePos);
            case "King":
                // Get King-script and get result
                return gamePiece.GetComponent<King>().TilePositionIsValid(tilePos);
            default:
                Debug.Log("Error: no such thing as " + pieceName);
                break;
        }

        return false;
    }


    /// <summary>
    /// Promotes pawn to queen
    /// </summary>
    /// <param name="pawn">To be new queen</param>
    public void PromotePawn(GameObject pawn)
    {
        GamePiece pieceData = pawn.GetComponent<GamePiece>();
        Tile tile = pieceData.GetGamePieceParentTile();
        int teamNumber = pieceData.GetCurrentGameTeamNumber();

        Destroy(GetComponent<Pawn>());
        Queen queen = pawn.AddComponent(typeof(Queen)) as Queen;

        pawn.name = "Queen";
    }


    /// <summary>
    /// Check if the king has died. Restarts the scene after 5 seconds
    /// </summary>
    /// <param name="defender">Defender object that could be king</param>
    public void CheckKingIsDead(GameObject defender)
    {
        if (defender.name == "King")
        {
            uiManager.SetEventText();
            playerTurn = -1;
            uiManager.SetCurrentPlayerTurn(-1);
            Invoke("RestartLevel", 5f);
        }
    }


    /// <summary>
    /// Loads scene. Used only for "CheckKingIsDead"-method
    /// </summary>
    public void RestartLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }


    /// <summary>
    /// Returns random audio clip
    /// </summary>
    /// <returns>Random audio clip</returns>
    AudioClip RandomAudioClip()
    {
        return movementSounds[Random.Range(0, movementSounds.Length - 1)];
    }
}