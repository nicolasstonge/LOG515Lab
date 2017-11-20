using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Labyrinth : MonoBehaviour
{

    // labyrinth prefab
    public LabyrinthTile labyrinthTilePref;
    public LabyrinthTileStart labyrinthTileStartPref;
    public LabyrinthTileFinish labyrinthTileFinishPref;
    public Character characterPrefab;
    public KeyTile keyPrefab;
    public Treasure treasurePrefab;
    public Monster monsterPrefab;
    private Treasure treasureInstance;
    private KeyTile keyInstance;
    private Character characterInstance;
    private LabyrinthTileStart labyrinthStartInstance;
    private LabyrinthTileFinish labyrinthTileFinishInstance;

    private int labyrinthSizeX;
    private int labyrinthSizeZ;

    public NavMeshSurface navSurface;

    // labyrinth map
    private LabyrinthTile[,] labyrinthTiles;

    /// <summary>
    /// This method will create the labyrinth.
    /// </summary>
    public void CreateLabyrinth(int sizeX, int sizeZ)
    {
        labyrinthSizeX = sizeX;
        labyrinthSizeZ = sizeZ;

        // labyrinth map
        labyrinthTiles = new LabyrinthTile[labyrinthSizeX, labyrinthSizeZ];

        // list of non-completed tiles
        List<LabyrinthTile> activeTiles = new List<LabyrinthTile>();

        // add the first tile
        activeTiles.Add(NewTile(0, 0));

        // add other tiles
        while (activeTiles.Count > 0)
        {
            ModifyActiveTiles(activeTiles);
        }

        // start tiles
        labyrinthStartInstance = Instantiate(labyrinthTileStartPref, transform) as LabyrinthTileStart;
        labyrinthStartInstance.transform.Translate(-2, 0, 0);
        labyrinthTiles[0, 0].HideWall(2);
        labyrinthTiles[0, 1].HideWall(2);

        // finish tiles
        labyrinthTileFinishInstance = Instantiate(labyrinthTileFinishPref, transform) as LabyrinthTileFinish;
        labyrinthTileFinishInstance.transform.Translate(-labyrinthSizeX -1, 0, -labyrinthSizeZ + 1);
        labyrinthTiles[labyrinthSizeX -1, labyrinthSizeZ - 1].HideWall(1);
        labyrinthTiles[labyrinthSizeX -1, labyrinthSizeZ - 2].HideWall(1);

        // Scale labyrinth
        transform.localScale = new Vector3(10,5,10);
        navSurface.BuildNavMesh();

        // Generate key
        keyInstance = Instantiate(keyPrefab, GameObject.Find("LabyrinthObjects").transform) as KeyTile;
        keyInstance.name = "Key";
        GameObject[] spawnPoints;
        spawnPoints = GameObject.FindGameObjectsWithTag("Tile");
        int index = Random.Range(0, spawnPoints.Length);
        keyInstance.transform.Translate(spawnPoints[index].transform.position);
        keyInstance.ActivateKeyCollider();

        // Generate treasure
        treasureInstance = Instantiate(treasurePrefab, GameObject.Find("LabyrinthObjects").transform) as Treasure;
        treasureInstance.name = "Treasure";
        treasureInstance.transform.Translate(GameObject.Find("LabyrinthTileFinish(Clone)").transform.position);

        // Spawn character
        characterInstance = Instantiate(characterPrefab, GameObject.Find("LabyrinthObjects").transform) as Character;
        characterInstance.name = "Character";
        characterInstance.transform.Translate(GameObject.Find("LabyrinthTileStart(Clone)").transform.position);

        // Spawn monsters
        int spawnedMonsters = 0;

        GameObject[] WayPoints;
        WayPoints = GameObject.FindGameObjectsWithTag("Waypoint");

        while (spawnedMonsters < sizeX)
        {
            int indexM = GameObject.Find("GameController").GetComponent<GameController>().GetRandomInt(0, WayPoints.Length);
            if (WayPoints[indexM].transform.parent.GetComponent<LabyrinthTile>().hasMonster == false)
            {
                Monster monster;
                monster = Instantiate(monsterPrefab, GameObject.Find("LabyrinthObjects").transform) as Monster;
                monster.GetComponent<NavMeshAgent>().enabled = false;
                monster.transform.Translate(WayPoints[indexM].transform.position);
                WayPoints[indexM].transform.parent.GetComponent<LabyrinthTile>().hasMonster = true;
                monster.GetComponent<NavMeshAgent>().enabled = true;
                spawnedMonsters++;
            }
            

        }
    }

    /// <summary>
    /// This method will add a new tile and map it in the array.
    /// </summary>
    /// <param name="positionX"></param>
    /// <param name="positionZ"></param>
    /// <returns></returns>
    private LabyrinthTile NewTile(int positionX, int positionZ)
    {
        LabyrinthTile tile = Instantiate(labyrinthTilePref, transform) as LabyrinthTile;
        tile.positionX = positionX;
        tile.positionZ = positionZ;
        tile.transform.Translate(positionX, 0, positionZ);
        labyrinthTiles[positionX, positionZ] = tile;

        return tile;
    }

    private void ModifyActiveTiles(List<LabyrinthTile> activeTiles)
    {
        LabyrinthTile currentTile = activeTiles[activeTiles.Count - 1];

        if (currentTile.TileCompleted())
        {
            activeTiles.RemoveAt(activeTiles.Count - 1);
            return;
        }

        int randomSide = currentTile.GetRandomSide();

        // verify if tile on the other side is out of bound
        if (currentTile.GetOtherSidePositionX(randomSide) >= 0 &&
            currentTile.GetOtherSidePositionX(randomSide) < labyrinthSizeX &&
            currentTile.GetOtherSidePositionZ(randomSide) >= 0 &&
            currentTile.GetOtherSidePositionZ(randomSide) < labyrinthSizeZ
            )
        {
            // get the tile on the other side
            LabyrinthTile otherTile = labyrinthTiles[currentTile.GetOtherSidePositionX(randomSide),
                                                        currentTile.GetOtherSidePositionZ(randomSide)];

            if (otherTile == null)
            {
                // if the tile on the other side is empty, create it and make a passage between the tiles
                otherTile = NewTile(currentTile.GetOtherSidePositionX(randomSide), 
                                        currentTile.GetOtherSidePositionZ(randomSide));

                activeTiles.Add(otherTile);
                currentTile.InitSide(randomSide);
                otherTile.InitSide(currentTile.GetOppositeSide(randomSide));
            }
            else
            {
                // if the tile on the other side is already created, put a wall on the current tile.
                currentTile.InitSide(randomSide);
                currentTile.displayWall(randomSide);
            }
        }
        else
        {
            // if out of bound, display wall on the side of the current tile
            currentTile.InitSide(randomSide);
            currentTile.displayWall(randomSide);
        }
    }

    private GameObject[] RandomizeArray(GameObject[] arr)
    {
        for (var i = arr.Length - 1; i > 0; i--)
        {
            var r = Random.Range(0, i);
            var tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }

        return arr;
    }
}
