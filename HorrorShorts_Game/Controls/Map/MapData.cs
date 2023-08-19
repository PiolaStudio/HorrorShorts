using HorrorShorts_Game.Algorithms.AStar;
using HorrorShorts_Game.Controls.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorrorShorts_Game.Controls.Map
{
    public class MapData
    {
        private List<IMapLocation>[,] _entitiesLocation;
        private Node[,] _baseNodes;
        private Node[,] _overNodes;
        private Node[,] _finalNodes;
        private object lockObj;

        public MapData()
        {
            //_entitiesLocation = new List<IMapLocation>[2, 2];
            //for (int i = 0; i < 2; i++)
            //    for (int j = 0; j < 2; j++)
            //        _entitiesLocation[i, j] = new();
        }

        public void MoveAt(int fromX, int fromY, int toX, int toY, IMapLocation entity)
        {
            RemoveAt(fromX, fromY, entity);
            AddAt(toX, toY, entity);
        }
        public void RemoveAt(int x, int y, IMapLocation entity)
        {
            _entitiesLocation[x, y].Remove(entity);
            UpdateCost(x, y);
        }
        public void AddAt(int x, int y, IMapLocation entity)
        {
            _entitiesLocation[x, y].Add(entity);
            UpdateCost(x, y);
        }
        public void UpdateCost(int x, int y)
        {
            int newCost = 0;
            for (int i = 0; i < _entitiesLocation[x, y].Count; i++)
                newCost += _entitiesLocation[x, y][i].CostOverMap;
            _overNodes[x, y].Cost = newCost;
            _finalNodes[x, y].Cost = _baseNodes[x, y].Cost + _overNodes[x, y].Cost;
        }

        public void LoadMap(MapData map)
        {

        }

        public void UpdateAll()
        {
            //if (!_needRefreshMap) return;
            //_needRefreshMap = false;

            //Node[,] newFinalNodes = new[,];

            //for (int x = 0; x < 0; x++)
            //    for (int y = 0; y < 0; y++)
            //    {
            //        //int cost = 0;
            //        //for (int i = 0; i < _entitiesLocation[x, y].Count; i++)
            //        //    cost += _entitiesLocation[x, y][i].CostOverMap;

            //        int cost = _overNodes[x, y].Cost + _baseNodes[x, y].Cost;
            //        newFinalNodes[x, y] = new(x, y, cost);
            //    }

            //_finalNodes = newFinalNodes;
        }
    }
}
