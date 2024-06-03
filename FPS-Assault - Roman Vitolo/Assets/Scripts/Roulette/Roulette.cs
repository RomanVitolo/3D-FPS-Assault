using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Roulette
    {
        public T ExecuteRoulette<T>(Dictionary<T, int> dic)
        {
            float total = 0;
            foreach (var item in dic)
            {
                total += item.Value;
            }
            float random = Random.Range(0, total);

            foreach (var item in dic)
            {
                random -= item.Value;
                if (random < 0)
                {
                    return item.Key;
                }
            }
            return default(T);
        }
    }
}