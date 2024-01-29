using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject prefab;
    public int SizeX = 0;
    public int SizeY = 0;
    public int SizeZ = 0;
    Automata[,,] map;
    Automata[,,] map2;
    private GameObject[,,] gameObjects;
    public int Survival = 0;
    public int Spawn = 0;
    public int States = 0;
    public int iterations;
    
    public void Start()
    {
        CrateBoard(SizeX,SizeY,SizeZ,Survival,Spawn);
    }

    public void CrateBoard(int _sizeX, int _sizeY, int _sizeZ, int _WC, int _EC)
    {
        SizeX = _sizeX;
        SizeY = _sizeY;
        SizeZ = _sizeZ;
        Survival = _WC;
        Spawn = _EC;
        //Clean();
        map = new Automata[SizeX, SizeY, SizeZ];
        map2 = new Automata[SizeX, SizeY, SizeZ];
        gameObjects = new GameObject[SizeX, SizeY, SizeZ];
        InitializeRandom();

        // CheckAll();

        // if (iterations > 1)
        // {
        //     for(int i = 0; i < iterations; i++)
        //     {
        //         map = map2;
        //         CheckAll();
        //     }   
        // }
        InstantiateBoard();
        StartCoroutine(AdvanceNextGen());
    }

    private void InitializeRandom()
    {
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                for (int k = 0; k < SizeZ; k++)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > .5f)
                    {   
                       
                        Automata a = new Automata(true, States);
                        map[i, j, k] = a;
                    }
                    else
                    {
                        Automata a = new Automata(false, States);
                        map[i, j, k] = a;
                    }
                }
            }
        }
    }

    private void CheckAll()
    {
        for (int i = 1; i < SizeX - 1; i++)
        {
            for (int j = 1; j < SizeY - 1; j++)
            {
                for (int k = 1; k < SizeZ - 1; k++)
                {
                    Check(i,j,k);
                }
            }
        }
    }

    private void RandomEdge()
    {
        for (int i = 0; i < SizeY; i++)
        {
            for (int j = 0; j < SizeZ; j++)
            {
                map2[0, i, j] = new Automata(false, States);
                map2[SizeX - 1, i, j] = new Automata(false, States);
            }
        }
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeZ; j++)
            {
                map2[i, 0, j] = new Automata(false, States);
                map2[i, SizeY - 1, j] = new Automata(false, States);
            }
        }
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                map2[i, j, 0] = new Automata(false, States);
                map2[i, j, SizeZ - 1] = new Automata(false, States);
            }
        }
    }

    void InstantiateBoard()
    {
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                for (int k = 0; k < SizeZ; k++)
                {
                    gameObjects[i, j, k] = Instantiate(prefab, new Vector3(i, -j, k), Quaternion.identity) as GameObject;
                    gameObjects[i, j, k].SetActive(true);
                    if (map[i, j, k].GetIsAlive() == false)
                    {
                        gameObjects[i, j, k].SetActive(false);
                    }
                }
            }
        }
    }

    void UpdateBoard()
    {
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                for (int k = 0; k < SizeZ; k++)
                {
                    if (map2[i, j, k].GetIsAlive() == false)
                    {
                        gameObjects[i, j, k].SetActive(false);
                    }
                    else
                    {
                        gameObjects[i, j, k].SetActive(true);
                    }
                }
            }
        }
    }

    void Check(int x, int y, int z)
    {
        int neighbors = 0;
        
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                for (int k = -1; k < 2; k++)
                {
                    if (map[x + i, y + j, z + k].GetIsAlive())
                    {
                        neighbors++;
                    }
                }
                
            }
        }


        if (map[x,y,z].GetIsAlive())
        {
            if(neighbors >= Survival && !map[x,y,z].GetIsDying())
            {
                //map2[x, y, z] = null;
                map2[x, y, z] = new Automata(true, States);
            }
            else if(neighbors >= Survival && map[x,y,z].GetIsDying())
            {
                //map2[x, y, z] = null;
                map2[x, y, z].AdvanceState();
            }
            else if(neighbors < Survival && !map[x,y,z].GetIsDying())
            {
                //map2[x, y, z] = null;
                map2[x, y, z] = new Automata(true, States);
                map2[x, y, z].SetIsDying(true);
                map2[x, y, z].AdvanceState();
            }
        }
        else
        {
            if (neighbors >= Spawn)
            {
                map2[x, y, z] = null;
                map2[x, y, z] = new Automata(true, States);
            }
            else
            {
                map2[x, y, z] = null;
                map2[x, y, z] = new Automata(false, States);
            }
        }
    }

    public IEnumerator AdvanceNextGen()
    {
        yield return new WaitForSeconds(.5f);
        RandomEdge();
        CheckAll();
        UpdateBoard();
        map = map2;
        StartCoroutine(AdvanceNextGen());
    }
}
