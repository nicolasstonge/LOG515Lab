using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labyrinth : MonoBehaviour
{

    // labyrinth prefab
    public LabyrinthTile labyrinthTilePref;
    public LabyrinthTileStart labyrinthTileStartPref;
    public LabyrinthTileFinish labyrinthTileFinishPref;

    private LabyrinthTileStart labyrinthStartInstance;
    private LabyrinthTileFinish labyrinthTileFinishInstance;

    // labyrinth size
    public int labyrinthSizeX;
    public int labyrinthSizeZ;

    // labyrinth map
    private LabyrinthTile[,] labyrinthTiles;

    /// <summary>
    /// This method will create the labyrinth.
    /// </summary>
    public void CreateLabyrinth()
    {
        // labyrinth map
        labyrinthTiles = new LabyrinthTile[labyrinthSizeX, labyrinthSizeZ];

        // list of non-completed tiles
        List<LabyrinthTile> activeTiles = new List<LabyrinthTile>();

        // add the first tile
        activeTiles.Add(NewTile(0, 0));

        while (activeTiles.Count > 0)
        {
            ModifyActiveTiles(activeTiles);
        }

        // start tiles
        labyrinthStartInstance = Instantiate(labyrinthTileStartPref) as LabyrinthTileStart;
        labyrinthStartInstance.transform.Translate(-2, 0, 0);
        labyrinthTiles[0, 0].HideWall(2);
        labyrinthTiles[0, 1].HideWall(2);

        // finish tiles
        labyrinthTileFinishInstance = Instantiate(labyrinthTileFinishPref) as LabyrinthTileFinish;
        labyrinthTileFinishInstance.transform.Translate(-labyrinthSizeX -1, 0, -labyrinthSizeZ + 1);
        labyrinthTiles[labyrinthSizeX -1, labyrinthSizeZ - 1].HideWall(1);
        labyrinthTiles[labyrinthSizeX -1, labyrinthSizeZ - 2].HideWall(1);

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
}
