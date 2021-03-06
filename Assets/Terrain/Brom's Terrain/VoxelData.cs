﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelData  {

    int[,] data = new int[,]
    {
        {0, 1, 1},
        {1, 1, 1},
        {1, 1, 0}
    };
    // properties and helper functions
    public int Width
    {
        get { return data.GetLength(0); }
    }
    public int Depth
    {
        get { return data.GetLength(1); }
    }
    public int GetCell(int x, int z)
    {
        return data[x, z];
    }

}
