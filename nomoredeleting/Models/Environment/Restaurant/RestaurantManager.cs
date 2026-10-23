using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using nomoredeleting.Models.Environment.Restaurant;

namespace nomoredeleting.Managers
{
    public class RestaurantManager
    {
        private List<TableInfo> _tables = new List<TableInfo>();
        public IReadOnlyList<TableInfo> Tables => _tables;

        public void GenerateTables()
        {
            _tables.Clear();
            Random rand = new Random();
            float minDistance = 3.0f;
            int maxTries = 500;

            while (_tables.Count < 10 && maxTries > 0)
            {
                float x = (float)(rand.NextDouble() * 10.0 - 5.0);
                float z = (float)(rand.NextDouble() * 8.0 - 3.0);
                Vector3 newPos = new Vector3(x, 0f, z);

                bool tooClose = false;
                foreach (var table in _tables)
                {
                    if (Vector3.Distance(newPos, table.Position) < minDistance)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    TableInfo info = new TableInfo
                    {
                        Position = newPos,
                        HasPlate = rand.Next(2) == 1,
                        HasMug = rand.Next(2) == 1,
                        HasCutlery = rand.Next(2) == 1
                    };
                    _tables.Add(info);
                }

                maxTries--;
            }
        }
    }
}

