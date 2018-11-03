using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {
    public static BasePlate[,] grid; //list of all plates,x and y are first and second param
	// Use this for initialization
	void Start () {
        createWorld(3, 3);
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void createWorld(int width, int height)
    {
        grid = new BasePlate[width, height];
        for (int i = 0; i < width; i++)
        {
           for(int j = 0; j < height; j++)
            {
                UnityEngine.Object toCreate = Resources.Load("baseplate");
                GameObject tile = Instantiate(Resources.Load("baseplate", typeof(GameObject))) as GameObject;
                tile.transform.position = new Vector3(i * BasePlate.tileWidth, 0 , j*BasePlate.tileHeight);
                BasePlate bp = tile.GetComponent<BasePlate>();
                bp.gridX = i;
                bp.gridY = j;
                bp.setup();
                grid[i, j] = bp;
            }
        }
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                grid[i, j].generateTerrainFeatures();
            }
        }
        //twice for silky smoothe
        smoothEdges();
        smoothEdges();
        for(int i = 0; i< grid.GetLength(0); i++)
        {
            for(int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j].populate();
            }
        }
    }



    public void smoothEdges()
    {
        print("smoothing...");
        for (int q = 0; q < grid.GetLength(0); q++)
        {
            for (int w = 0; w < grid.GetLength(1); w++)
            {
                BasePlate orgin = grid[q, w];

                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        BasePlate test = grid[orgin.gridX + 1, orgin.gridY];
                    }catch(IndexOutOfRangeException e)
                    {
                        continue;
                    }
                    //smoothes this tile with the one to the right
                    int toRaiseTo = grid[orgin.gridX + 1, orgin.gridY].heightLayout[0, BasePlate.tileHeight * i / 5];
                    orgin.makeHill(BasePlate.tileWidth - 1, BasePlate.tileHeight * i / 5, toRaiseTo);
                }

                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        BasePlate test = grid[orgin.gridX - 1, orgin.gridY];
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        continue;
                    }
                    //smoothes this tile with the one to the left
                    int toRaiseTo = grid[orgin.gridX - 1, orgin.gridY].heightLayout[BasePlate.tileWidth, BasePlate.tileHeight * i / 5];
                    orgin.makeHill(0, BasePlate.tileHeight * i / 5, toRaiseTo);
                }

                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        BasePlate test = grid[orgin.gridX, orgin.gridY+1];
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        continue;
                    }
                    //smoothes this tile with the one on top
                    int toRaiseTo = grid[orgin.gridX, orgin.gridY + 1].heightLayout[BasePlate.tileWidth * i / 5, 0];
                    orgin.makeHill(BasePlate.tileWidth * i / 5, BasePlate.tileWidth, toRaiseTo);
                }


                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        BasePlate test = grid[orgin.gridX, orgin.gridY - 1];
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        continue;
                    }
                    //smoothes this tile with the one below
                    int toRaiseTo = grid[orgin.gridX, orgin.gridY - 1].heightLayout[BasePlate.tileHeight, BasePlate.tileWidth * i / 5];
                    orgin.makeHill(0, BasePlate.tileWidth * i / 5, toRaiseTo);
                }

                orgin.generateTerrainFeatures();
            }
        }
    }
}
