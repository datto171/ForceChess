using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MineSweeper
{
    public class GameContext : MonoBehaviour
    {
        // ~> bien nay nen tao thanh bien Gameside de check turn
        bool isRedTurn = true;

        // ~> 2 bien nay tao thanh bien end game
        bool isEnemyWin = false;
        bool isEndGame = false;

        int numbChessP1 = 7;
        int numbChessP2 = 1;

        [SerializeField]
        Text textChessP1;

        [SerializeField]
        Text textChessP2;

        List<TileComponent> listTileCheck = new List<TileComponent>();

        StateTile currentState; // state current player hit

        private void Awake()
        {
            TileComponent.onInteract = OnInteract;
        }

        public void ChangeTurn()
        {
            isRedTurn = !isRedTurn;
        }

        public void OnInteract(TileComponent target)
        {
            Debug.Log(target.GetPosX() + ", " + target.GetPosY());

            // Check win normal (3 tiles in a row)
            if (target.GetStateTile() == StateTile.None)
            {
                if (isRedTurn)
                {
                    numbChessP1 -= 1;
                    currentState = StateTile.Player1;
                }
                else
                {
                    numbChessP2 -= 1;
                    currentState = StateTile.Player2;
                }

                target.SetStateTile(currentState);

                listTileCheck.Add(target);

                // Đẩy các ô xung quanh ô vừa mới được đánh
                CheckTilesAround(target);

                // Show lại số quân cờ còn trong hộp mỗi người chơi
                ShowUI();

                // Check win in list Tile
                CheckWinListTile();

                // Check win special (1 side has 8 tiles on board)
                CountChessOneSide();

                if (isEndGame)
                {
                    if (isEnemyWin)
                    {
                        Debug.Log("ENEMY WIN");
                    }
                    else
                    {
                        Debug.Log("YOU WIN");
                    }
                }
                else
                {
                    ChangeTurn();
                }
            }
        }

        public void CountChessOneSide()
        {
            if (numbChessP1 == 0)
            {
                Debug.Log("P1 Win");
            }
            else if (numbChessP2 == 0)
            {
                Debug.Log("P2 Win");
            }
        }

        // Check Tile Around to Push this tile around to next tile with this direction
        public void CheckTilesAround(TileComponent tile)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i != 0 || j != 0) // không check ô được truyền vào
                    {
                        TileComponent tileAround = GameManager.Instance.GetTile(tile.GetPosX() + i, tile.GetPosY() + j);

                        if (tileAround != null) // Nếu ô tiếp theo này nằm trong bàn cờ thì mới check tiếp
                        {
                            if (tileAround.GetStateTile() != StateTile.None) // Nếu ô tiếp theo này không phải là ô rỗng
                            {
                                TileComponent nextTileAround = GameManager.Instance.GetTile(tileAround.GetPosX() + i, tileAround.GetPosY() + j);

                                if (nextTileAround != null) // Nếu ô tiếp theo nữa này nằm trong bàn cờ thì mới check tiếp
                                {
                                    Debug.Log("tile check" + nextTileAround.GetPosX() + ":" + nextTileAround.GetPosY());
                                    if (nextTileAround.GetStateTile() == StateTile.None) // Nếu ô tiếp theo nữa này là ô rỗng thì sẽ swap giá trị
                                    {
                                        // Set lại giá trị sau khi kích
                                        nextTileAround.SetStateTile(tileAround.GetStateTile());
                                        tileAround.SetStateTile(StateTile.None);

                                        listTileCheck.Add(nextTileAround);
                                    }
                                    else // Check win khi đánh ở giữa mà 2 ô tiếp theo nữa bị chặn 2 đầu 
                                    {
                                        listTileCheck.Add(tileAround);
                                    }
                                }
                                else // Nếu ô này nằm cạnh viền bàn cờ sẽ bị sút bay ra khỏi bàn cờ
                                {
                                    if (tileAround.GetStateTile() == StateTile.Player1)
                                    {
                                        numbChessP1 = numbChessP1 + 1;
                                    }
                                    else if (tileAround.GetStateTile() == StateTile.Player2)
                                    {
                                        numbChessP2 = numbChessP2 + 1;
                                    }

                                    tileAround.SetStateTile(StateTile.None);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Check win tính theo trường hợp từ ô target là 0 sau đó + 2 theo hướng (0, 1, 2)
        // Sẽ thiếu nếu check trường hợp win đánh vào giữa
        public void CheckWinDirection(TileComponent target, StateTile stateTarget)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i != 0 || j != 0)   // không check ô được truyền vào
                    {
                        TileComponent tileAround = GameManager.Instance.GetTile(target.GetPosX() + i, target.GetPosY() + j);

                        if (tileAround != null)
                        {
                            if (tileAround.GetStateTile() == target.GetStateTile())
                            {
                                //Debug.Log("target = " + target.GetPosX() + ":" + target.GetPosY() + ", tile around = " + (target.GetPosX() + i) + ":" + (target.GetPosY() + j));
                                TileComponent nextTileAround = GameManager.Instance.GetTile(tileAround.GetPosX() + i, tileAround.GetPosY() + j);

                                if (nextTileAround != null)
                                {
                                    if (nextTileAround.GetStateTile() == target.GetStateTile())
                                    {
                                        Debug.Log("target = " + target.GetPosX() + ":" + target.GetPosY()
                                            + ", tile around = " + (target.GetPosX() + i) + ":" + (target.GetPosY() + j)
                                            + ", next tile around = " + (tileAround.GetPosX() + i) + ":" + (tileAround.GetPosY() + j));

                                        Debug.Log("End Game");
                                        isEndGame = true;

                                        // Check Enemy Win
                                        if (nextTileAround.GetStateTile() != stateTarget && nextTileAround.GetStateTile() != StateTile.None)
                                        {
                                            Debug.Log("Enemy win");
                                            isEnemyWin = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CheckWinListTile()
        {
            foreach (TileComponent target in listTileCheck)
            {
                CheckWinDirection(target, currentState);
                if (isEnemyWin)
                {
                    Debug.Log("ENEMY WIN => Break");
                    break;
                }
            }
        }

        public void ShowUI()
        {
            textChessP1.text = numbChessP1 + "";
            textChessP2.text = numbChessP2 + "";
        }
    }
}