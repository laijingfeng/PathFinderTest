using Jerry;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    private Image img;

    public GridData data;

    private bool awaked = false;
    private bool inited = false;

    void Awake()
    {
        img = this.transform.GetComponent<Image>();
        UGUIEventListener.Get(this.gameObject).onClick += (go) =>
        {
            if (data.state == GridState.Road)
            {
                data.state = GridState.Obstacle;
                RefreshState();
            }
            else if (data.state == GridState.Obstacle)
            {
                data.state = GridState.Road;
                RefreshState();
            }
        };
        awaked = true;
        TryWork();
    }

    public void Init(GridData tData)
    {
        data = tData;
        inited = true;
        TryWork();
    }

    private void TryWork()
    {
        if (!awaked || !inited)
        {
            return;
        }
        RefreshState();
    }

    public void RefreshState()
    {
        switch (data.state)
        {
            case GridState.Start:
                {
                    img.color = Color.green;
                }
                break;
            case GridState.Goal:
                {
                    img.color = Color.red;
                }
                break;
            case GridState.Obstacle:
                {
                    img.color = Color.black;
                }
                break;
            case GridState.Road:
                {
                    img.color = Color.grey;
                }
                break;
            case GridState.RoadClose:
                {
                    img.color = Color.blue;
                }
                break;
            case GridState.RoadOpen:
                {
                    img.color = Color.cyan;
                }
                break;
        }
    }

    public class GridData
    {
        public int x = 0;
        public int y = 0;
        public GridState state = GridState.None;

        public int ID
        {
            get
            {
                return x * 100 + y;
            }
        }
    }

    public enum GridState
    {
        None = 0,
        Road,
        RoadOpen,
        RoadClose,
        Obstacle,
        Start,
        Goal,
    }
}