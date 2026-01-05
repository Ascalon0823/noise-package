using UnityEngine;

namespace Noise
{
    [CreateAssetMenu(fileName = "New Cellular Automata Noise", menuName = "Noise/CellularAutomata", order = 0)]
    public class CellularAutomataProvider : BaseProvider
    {
        [Range(0, 5)] public int iterations;
        [Range(0, 1f)] public float cutoff;
        [Range(0, 8)] public int zeroLimit;

        [Range(0, 8)] public int oneLimit;
        public float[,] Iterate(float[,] old)
        {
            var sizeX = old.GetLength(0);
            var sizeY = old.GetLength(1);
            var newData = new float[sizeX, sizeY];
            for (var i = 0; i < sizeX; i++)
            for (var j = 0; j < sizeY; j++)
            {
                var count = CountNeighbors(i, j, old);
                var original = old[i, j];
                newData[i, j] = original;
                if (Mathf.Approximately(original, 1f))
                {
                    if (count < zeroLimit)
                    {
                        newData[i, j] = 0;
                    }
                }
                else
                {
                    if (count >= oneLimit)
                    {
                        newData[i, j] = 1;
                    }
                }
            }

            return newData;
        }

        private int CountNeighbors(int x, int y, float[,] data)
        {
            var count = 0;
            var sizeX = data.GetLength(0);
            var sizeY = data.GetLength(1);
            for (var i = -1; i <= 1; i++)
            for (var j = -1; j <= 1; j++)
            {
                var x1 = x + i;
                var y1 = y + j;
                if (i == 0 && y == 0) continue;
                if (x1 < 0 || x1 >= sizeX || y1 < 0 || y1 >= sizeY)
                {
                    count++;
                    continue;
                }

                count += data[x1, y1]!=0f?1:0;
            }

            return count;
        }
        public override float[,] GetData(int sizeX, int sizeY, int newSeed, float?[,] baseData = null)
        {
            seed = newSeed;
            Random.InitState(seed);
            var data = new float[sizeX, sizeY];
            var baseValid = baseData!=null && baseData.GetLength(0) == sizeX && baseData.GetLength(1) == sizeY;
            for (var i = 0; i <= iterations; i++)
            {
                if (i == 0)
                {
                    for (var x = 0; x < sizeX; x++)
                    for (var y = 0; y < sizeY; y++)
                    {
                        var value = Random.Range(0f, 1f);
                        data[x, y] = value > cutoff ? 1 : 0;
                        if (baseValid && baseData[x, y].HasValue)
                        {
                            data[x, y] = baseData[x, y].Value;
                        }
                    }
                    continue;
                }
                data = Iterate(data);
            }

            return data;
        }
    }
}