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

        public int size;
        private int[,] data;

        public override void Initialize(int newSeed)
        {
            seed = newSeed;
            Random.InitState(seed);
            data = new int[size, size];
            for (var i = 0; i <= iterations; i++)
            {
                if (i == 0)
                {
                    for (var x = 0; x < size; x++)
                    for (var y = 0; y < size; y++)
                    {
                        var value = Random.Range(0f, 1f);
                        data[x, y] = value > cutoff ? 1 : 0;
                    }
                    continue;
                }
                data = Iterate(data);
            }
        }

        public int[,] Iterate(int[,] old)
        {
            var newData = new int[size, size];
            for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
            {
                var count = CountNeighbors(i, j);
                var original = old[i, j];
                newData[i, j] = original;
                if (original == 1)
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

        private int CountNeighbors(int x, int y)
        {
            var count = 0;
            for (var i = -1; i <= 1; i++)
            for (var j = -1; j <= 1; j++)
            {
                var x1 = x + i;
                var y1 = y + j;
                if (i == 0 && y == 0) continue;
                if (x1 < 0 || x1 >= size || y1 < 0 || y1 >= size)
                {
                    count++;
                    continue;
                }

                count += data[x1, y1];
            }

            return count;
        }

        public override float GetValue(Vector3 pos)
        {
            if (data == null || pos.x >= size || pos.y >= size) return 0;
            return data[Mathf.FloorToInt(pos.x * size), Mathf.FloorToInt(pos.y * size)];
        }
    }
}