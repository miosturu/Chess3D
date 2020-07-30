using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 2D-grid of the gameboard.
/// This class can:
///     Generate new map
///     Change the position of the gamepiece
///     Check if tile has neighbors
/// </summary>
public class TileMap : MonoBehaviour
{
    private int mapW, mapH;             // Map's width and height
    private Tile[,] tileArray;          // Abstract map of tiles
    private GameObject baseTile;        // Tile prefabs

    [Space(10)]

    public Material[] materials;        // Materials for the checkerboard


    /// <summary>
    /// Generates checkerboard patterned map.
    /// </summary>
    /// <param name="parentObject">Parent game object</param>
    public void GenerateMap(GameObject parentObject)
    {
        tileArray = new Tile[mapW, mapH];

        for (int i = 0; i < mapH; i++)
        {
            for (int j = 0; j < mapW; j++)
            {
                GameObject newTile = Instantiate(baseTile, parentObject.transform);
                Tile tileData = newTile.GetComponent<Tile>();
                MeshRenderer tileRenderer = newTile.GetComponentInChildren<MeshRenderer>();

                tileArray[j, i] = tileData;
                tileRenderer.material = materials[(j + i) % 2];
                newTile.transform.position = new Vector3(j, 0, i);
                tileData.ChangeTilePositionData(j, i);
                newTile.name = "(" + j + ", " + i + ")";
            }
        }
    }


    /// <summary>
    /// Defines how big the map will be.
    /// </summary>
    /// <param name="width">Map width</param>
    /// <param name="height">Map height</param>
    public void DefineGameboardSize(int width, int height)
    {
        mapW = width;
        mapH = height;
    }


    /// <summary>
    /// Returns tile at position (x, y)
    /// </summary>
    /// <param name="x">X-position</param>
    /// <param name="y">Y-position</param>
    /// <returns>Tile at (x, y) position. Can be null.</returns>
    public Tile GetTileAt(int x, int y)
    {
        try
        {
            return tileArray[x, y];
        }
        catch
        {
            Debug.Log("No such tile at position (" + x +", " + y + ")");
            return null;
        }

    }


    /// <summary>
    /// Returns base tile prefab
    /// </summary>
    /// <returns>Tile prefab</returns>
    public GameObject GetTilePrefab()
    {
        return baseTile;
    }


    /// <summary>
    /// Defines base tile
    /// </summary>
    /// <param name="tile">New base tile</param>
    public void DefineBaseTile(GameObject tile)
    {
        baseTile = tile;
    }


    /// <summary>
    /// Teturns current piece on the tile.
    /// </summary>
    /// <param name="tile">Wanted tile.</param>
    /// <returns>Current piece on the tile.</returns>
    public GameObject GetGamePieceAt(Tile tile)
    {
        return tile.GetGamepieceOnThisTile();
    }


    /// <summary>
    /// Assaigns game piece to new tile
    /// </summary>
    /// <param name="gamepiece">Game piece that will be assigned</param>
    /// <param name="tile">Game piece's new parent tile</param>
    public void AssignGamepieceTo(GameObject gamepiece, Tile tile)
    {
        tile.ChangePieceOnTile(gamepiece);
    }


    /// <summary>
    /// Moves game piece to new tile
    /// </summary>
    /// <param name="gamePiece">Selected game piece</param>
    /// <param name="tile">New tile</param>
    public void MoveGamePiece(GameObject gamePiece, Tile tile)
    {
        GamePiece gamePieceData = gamePiece.GetComponent<GamePiece>();
        gamePieceData.GetGamePieceParentTile().GetComponent<Tile>().ChangePieceOnTile(null);
        tile.ChangePieceOnTile(gamePiece);

        gamePieceData.ChangeParentTile(tile);
        gamePiece.transform.position = tile.transform.position;
        gamePiece.transform.parent = tile.transform;
    }


    /// <summary>
    /// Attacker eats defender game piece
    /// </summary>
    /// <param name="attacker">Attacker game object</param>
    /// <param name="defender">Defender game object</param>
    public void EatOtherGamePiece(GameObject attacker, GameObject defender)
    {
        Tile tile = defender.GetComponentInChildren<GamePiece>().GetGamePieceParentTile();
        Destroy(defender);
        MoveGamePiece(attacker, tile);
    }
}
