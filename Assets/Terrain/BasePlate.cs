using System;
using System.Collections.Generic;
using UnityEngine;

public class BasePlate : MonoBehaviour {
    public static int tileWidth = 50, tileHeight = 50; //goes with the 50x50 size of a baseplate
    public int gridX, gridY; //coords in the grid we are at
    public int[,] heightLayout = new int[tileWidth+1,tileHeight+1];  //map of terrain hights
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
     * sets up this tile's initial geography
     */
    public void setup()
    {
        designTerrain();
        generateTerrainFeatures();
    }

    /**
     * populates tile with weapons or possibly AI, after smoothing is done
     */
    public void populate()
    {
        populateWeapons();
    }
    /**
     * helper method for generateTerrainFEatures
     * sets the height of the terrain at the specified coordinates of this specific tile
     * @param x x-coord
     * @param y y-coord
     * @param h height
     */
    public void setTerrainColumn(int x, int y, int h)
    {
        if (h == 0) return; //dont render tiles in the floor
        GameObject piece = Instantiate(Resources.Load("TerrainBlock", typeof(GameObject))) as GameObject;
        piece.GetComponent<MeshRenderer>().receiveShadows = false; //disables shadows for a bit of fps
;        piece.transform.position = new Vector3((tileWidth * gridX) + x - tileWidth / 2 + .5f, 0, (tileHeight * gridY) + y - tileHeight / 2 + .5f); //making it so 0,0 is bottom corner of tile
        piece.transform.localScale += new Vector3(0, h, 0);
        piece.transform.parent = this.transform;
        heightLayout[x, y] = h; //record the height of this area
    }

    /*
     * realises and creates terrain based on heightlayout map instructions
     */
   public void  generateTerrainFeatures()
    {
        for(int w = 0; w < tileWidth; w++)
        {
            for(int h = 0; h < tileHeight; h++)
            {
                setTerrainColumn(w, h,heightLayout[w,h]);
            }
        }
    }

    /**
     *  determines the initial terrain features to generate by modifying the height layout map
     */
    public void designTerrain()
    {
        int numHills = UnityEngine.Random.Range(2, 4);
        for(int i =0; i < numHills; i++)
        {
            makeHill(UnityEngine.Random.Range(0, tileWidth), UnityEngine.Random.Range(0, tileHeight), UnityEngine.Random.Range(5, 20));
        }
 
    }

    /**
     * sets the layoutmap to schedule a hill at specified coordinates when terrain is generated
     */
    public void makeHill(int x, int y, int height)
    {
        print("making hill: " + x + "," + y + "," + height);
        print("tile size :" + heightLayout.GetLength(0) + "," + heightLayout.GetLength(1));
        if (height == 0) return; //no hill can be 0 tall
        heightLayout[x, y] = height; //the peak
        for(int i  = 0; i < heightLayout.GetLength(0);i++)
        {
            for(int j = 0; j < heightLayout.GetLength(1);j++)
            {
                try
                {
                    int distance = (int)Vector2.Distance(new Vector2(i, j), new Vector2(x, y));
                    if (distance > height || distance == 0) continue;
                    if (height - distance < 1) continue;
                    heightLayout[i, j] = (int)(height - distance);
                    heightLayout[i, j] += UnityEngine.Random.Range(-1, 1);
                }catch (IndexOutOfRangeException e) { print(e +"i:"+i+"j:"+j); }
            }
        }

    }

    public void populateWeapons()
    {
        int type = UnityEngine.Random.Range(1, 3); //0=nothing, 1=sword, 2 = bow
        switch (type)
        {
            case 0: return;
            case 1:
                GameObject sword = Instantiate(Resources.Load("Sword", typeof(GameObject))) as GameObject;
                int x = UnityEngine.Random.Range(0, tileWidth/2);
                int y = UnityEngine.Random.Range(0, tileHeight/2);
                int height = this.heightLayout[x*2, y*2] + 2;
                sword.transform.position = new Vector3((tileWidth * gridX) + x,height,(tileWidth * gridY)+ y);
                break;
            case 2:
                GameObject bow = Instantiate(Resources.Load("Bow", typeof(GameObject))) as GameObject;
                int x2 = UnityEngine.Random.Range(0, tileWidth/2);
                int y2 = UnityEngine.Random.Range(0, tileHeight/2);
                int height2 = this.heightLayout[x2*2, y2*2] + 2;
                bow.transform.position = new Vector3((tileWidth * gridX) + x2, height2, (tileWidth * gridY) + y2);
                break;
        }
    }
}
