using UnityEngine;
using System.Collections;
using Jerry;

public class GridMgr : MonoBehaviour
{
    private Transform prefab;
    private Transform grid;

    public const int Width = 12;
    public const int Height = 10;
    public Grid[,] grids;
    private One one;

    void Awake()
    {
        grid = this.transform.FindChild("Grid");
        prefab = this.transform.FindChild("Prefab");
        prefab.gameObject.SetActive(false);

        grids = new Grid[Width, Height];
        for (int j = 0; j < Height; j++)
        {
            for (int i = 0; i < Width; i++)
            {
                grids[i, j] = JerryUtil.CloneGo<Grid>(new JerryUtil.CloneGoData()
                {
                    parant = grid,
                    prefab = prefab.gameObject,
                    clean = false,
                    active = true,
                    name = string.Format("{0}_{1}", j, i),
                });
                grids[i, j].Init(new Grid.GridData()
                {
                    x = i,
                    y = j,
                    state = GetState(i, j),
                });
            }
        }
    }

    private void ReInit()
    {
        for (int j = 0; j < Height; j++)
        {
            for (int i = 0; i < Width; i++)
            {
                grids[i, j].Init(new Grid.GridData()
                {
                    x = i,
                    y = j,
                    state = GetState(i, j),
                });
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            this.StartCoroutine(DoFind());
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            ReInit();
        }
    }

    private IEnumerator DoFind()
    {
        one = new One();
        one.Init(grids);
        one.InitFind();
        while (true)
        {
            if (one.OneFind())
            {
                Debug.LogWarning("over");
                break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private Grid.GridState GetState(int x, int y)
    {
        if (x == 0 && y == 3)
        {
            return Grid.GridState.Start;
        }
        else if (x == 10 && y == 5)
        {
            return Grid.GridState.Goal;
        }
        return Grid.GridState.Road;
    }
}