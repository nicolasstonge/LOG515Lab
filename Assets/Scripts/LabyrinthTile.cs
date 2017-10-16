using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthTile : MonoBehaviour
{

    public int positionX;

    public int positionZ;

    public bool side1Init = false; // direction X+
    public bool side2Init = false; // direction X-
    public bool side3Init = false; // direction Z+
    public bool side4Init = false; // direction Z-

    public void InitSide(int sideNbr)
    {
        if (sideNbr == 1)
        {
            side1Init = true;
        }
        if (sideNbr == 2)
        {
            side2Init = true;
        }
        if (sideNbr == 3)
        {
            side3Init = true;
        }
        if (sideNbr == 4)
        {
            side4Init = true;
        }
    }

    public int GetRandomSide()
    {
        List<int> sideList = new List<int>();

        if (side1Init == false)
        {
            sideList.Add(1);
        }
        if (side2Init == false)
        {
            sideList.Add(2);
        }
        if (side3Init == false)
        {
            sideList.Add(3);
        }
        if (side4Init == false)
        {
            sideList.Add(4);
        }

        int Seed = (int)System.DateTime.Now.Ticks;
        System.Random randomListCell = new System.Random(Seed);

        int randomSide = sideList[randomListCell.Next(sideList.Count)];
        return randomSide;
    }

    public int GetOppositeSide(int side)
    {
        if (side == 1)
        {
            return 2;
        }
        if (side == 2)
        {
            return 1;
        }
        if (side == 3)
        {
            return 4;
        }
        else
        {
            return 3;
        }
    }

    public int GetOtherSidePositionX(int side)
    {
        if (side == 1)
        {
            return positionX + 1;
        }
        if (side == 2)
        {
            return positionX - 1;
        }
        else
        {
            return positionX;
        }
    }

    public int GetOtherSidePositionZ(int side)
    {
        if (side == 3)
        {
            return positionZ + 1;
        }
        if (side == 4)
        {
            return positionZ - 1;
        }
        else
        {
            return positionZ;
        }
    }

    public bool TileCompleted()
    {
        return side1Init && side2Init && side3Init && side4Init;
    }

    public void displayWall(int side)
    {
        if (side == 1)
        {
            transform.Find("Wall1").gameObject.SetActive(true);
        }
        if (side == 2)
        {
            transform.Find("Wall2").gameObject.SetActive(true);
        }
        if (side == 3)
        {
            transform.Find("Wall3").gameObject.SetActive(true);
        }
        if (side == 4)
        {
            transform.Find("Wall4").gameObject.SetActive(true);
        }
    }

}
