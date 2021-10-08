using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MineSweeper
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField]
        int width = 6;
        [SerializeField]
        int height = 6;

        [SerializeField]
        GameObject tileMap;

        [SerializeField]
        Transform locationSpawn;

        private void Awake()
        {
            CreateMap();
        }

        public void CreateMap()
        {
            for(int i = 0; i < width * height; i++)
            {
                Instantiate(tileMap, locationSpawn);
            }
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }
    }
}