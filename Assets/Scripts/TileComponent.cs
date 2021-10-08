using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MineSweeper
{
    public enum StateTile { None, Player1, Player2}

    public class TileComponent : MonoBehaviour
    {
        int x;
        int y;
        int index;
        [SerializeField]
        Text text;

        StateTile currentState = StateTile.None;

        public static Action<TileComponent> onInteract;

        public void OnHitTile()
        {
            onInteract?.Invoke(this);
        }

        public StateTile GetStateTile()
        {
            return currentState;
        }

        public void SetStateTile(StateTile state)
        {
            currentState = state;

            if(state == StateTile.None)
            {
                SetColor(Color.yellow);
            }
            else if(state == StateTile.Player1)
            {
                SetColor(Color.red);
            }
            else if (state == StateTile.Player2)
            {
                SetColor(Color.blue);
            }
        }

        private void SetColor(Color colorSet)
        {
            var a = this.GetComponent<Image>();
            a.color = colorSet;
        }

        public int GetPosX()
        {
            return x;
        }

        public int GetPosY()
        {
            return y;
        }

        public void SetPosX(int posX)
        {
            x = posX;
        }

        public void SetPosY(int posY)
        {
            y = posY;
        }

        public void SetIndex(int indexTile)
        {
            index = indexTile;
        }

        public void SetTextTile(int x, int y)
        {
            text.text = x + ", " + y;
        }
    }
}
