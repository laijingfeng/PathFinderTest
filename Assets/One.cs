using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class One
{
    private Grid[,] map;
    private Grid start;
    private Grid goal;
    private OneData lastFirst;
    private List<OneData> open = new List<OneData>();
    private List<OneData> close = new List<OneData>();

    public void Init(Grid[,] tMap)
    {
        map = tMap;
        for (int i = 0; i < GridMgr.Width; i++)
        {
            for (int j = 0; j < GridMgr.Height; j++)
            {
                if (map[i, j].data.state == Grid.GridState.Start)
                {
                    start = map[i, j];
                }
                else if (map[i, j].data.state == Grid.GridState.Goal)
                {
                    goal = map[i, j];
                }
            }
        }
    }

    public void InitFind()
    {
        open.Clear();
        close.Clear();

        open.Add(new OneData()
        {
            grid = start,
            dis2Goal = GetDis(start, goal),
        });
    }

    public bool OneFind()
    {
        if (open.Count == 0)
        {
            return true;
        }

        OneData first = open[0];
        open.Remove(first);
        close.Add(first);
        if (lastFirst != null)
        {
            lastFirst.grid.data.state = Grid.GridState.RoadClose;
            lastFirst.grid.RefreshState();
        }
        first.grid.data.state = Grid.GridState.Start;
        first.grid.RefreshState();
        lastFirst = first;

        if (first.grid.data.ID == goal.data.ID)
        {
            return true;
        }

        Check(first.grid, -1, 0);
        Check(first.grid, 1, 0);
        Check(first.grid, 0, -1);
        Check(first.grid, 0, 1);
        open.Sort((x, y) => x.dis2Goal.CompareTo(y.dis2Goal));

        return false;
    }

    private void Check(Grid grid, int dx, int dy)
    {
        Grid nData = GetMapData(grid.data.x + dx, grid.data.y + dy);
        if (nData.data.state == Grid.GridState.Obstacle)
        {
            return;
        }
        if (open.Exists((x) => x.grid.data.ID == nData.data.ID))
        {
            return;
        }
        if (close.Exists((x) => x.grid.data.ID == nData.data.ID))
        {
            return;
        }
        nData.data.state = Grid.GridState.RoadOpen;
        nData.RefreshState();
        open.Add(new OneData()
        {
            grid = nData,
            dis2Goal = GetDis(nData, goal),
        });
    }

    private Grid GetMapData(int x, int y)
    {
        if (x < 0)
        {
            x = 0;
        }
        if (x >= GridMgr.Width)
        {
            x = GridMgr.Width - 1;
        }
        if (y < 0)
        {
            y = 0;
        }
        if (y >= GridMgr.Height)
        {
            y = GridMgr.Height - 1;
        }
        return map[x, y];
    }

    private float GetDis(Grid a, Grid b)
    {
        return Mathf.Sqrt(Mathf.Pow(1.0f * a.data.x - b.data.x, 2) + Mathf.Pow(1.0f * a.data.y - b.data.y, 2));
    }

    public class OneData
    {
        public Grid grid;
        public float dis2Goal;
    }
}