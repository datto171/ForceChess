using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MineSweeper
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        int size = 6;

        [SerializeField]
        GameObject gridTile;

        TileComponent[] cells;

        [SerializeField]
        GameContext gameContext;

        private void Start()
        {
            Instance = this;
            SetIndexMap();
            SetupMap();
            gameContext.ShowUI();
        }

        public void SetIndexMap()
        {
            cells = gridTile.GetComponentsInChildren<TileComponent>();

            for(int i = 0; i < cells.Length; i++)
            {
                cells[i].SetPosX(i % size);
                cells[i].SetPosY(i / size);
                cells[i].SetIndex(i);
                cells[i].SetTextTile(i % size, i / size);

                cells[i].SetStateTile(StateTile.None);
            }
        }

        public TileComponent GetTile(int x, int y)
        {
            if(x > (size - 1) || x < 0 || y > (size - 1) || y < 0)
            {
                return null;
            }
            else
            {
                return cells[y * size + x];
            }
        }

        public void SetupMap()
        {
            GetTile(0, 0).SetStateTile(StateTile.Player2);
            GetTile(1, 1).SetStateTile(StateTile.Player2);
            GetTile(3, 2).SetStateTile(StateTile.Player2);
            GetTile(3, 4).SetStateTile(StateTile.Player2);
            GetTile(4, 4).SetStateTile(StateTile.Player2);


            //GetTile(0, 2).SetStateTile(StateTile.Player1);
            //GetTile(2, 2).SetStateTile(StateTile.Player1);
            GetTile(1, 3).SetStateTile(StateTile.Player1);


            //GetTile(3, 3).SetStateTile(StateTile.Player2);
            GetTile(2, 4).SetStateTile(StateTile.Player2);
            GetTile(3, 5).SetStateTile(StateTile.Player2);

        }
    }
}
