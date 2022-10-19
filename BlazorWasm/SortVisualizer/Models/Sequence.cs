namespace SortVisualizer;

public static partial class Sequence
{
    /// <summary>
    /// 生成条件。
    /// </summary>
    public class GenerationSettings
    {
        /// <summary>
        /// 生成する配列長。
        /// </summary>
        public uint Length { get; set; }

        /// <summary>
        /// 値が大きいほど同じ値を繰り返す。
        /// 0 のときすべての値がユニーク。
        /// 1のときに「2個ずつ」、2のときに「3個ずつ」、…。
        /// </summary>
        public uint Duplication { get; set; }

        /// <summary>
        /// ランダム度合い。
        /// </summary>
        /// <remarks>
        /// 現実装だと、 <see cref="Length"/> × <see cref="Randomness"/> 回シャッフルすることでランダム度合いを調整。
        /// </remarks>
        public float Randomness { get; set; }

        /// <summary>
        /// false なら昇順、true なら降順でデータを作った状態からシャッフルする。
        /// </summary>
        public bool IsDescending { get; set; }
    }

    public static int[] Generate(GenerationSettings settings) => Generate(Random.Shared, settings);

    public static int[] Generate(this Random random, GenerationSettings settings)
    {
        var array = new int[settings.Length];

        var step = (int)settings.Duplication + 1;
        var counter = 0;
        var value = step;
        int next()
        {
            if (counter == step)
            {
                value += step;
                counter = 0;
            }
            ++counter;
            return value;
        }

        for (int i = 0; i < array.Length; i++)
            array[i] = next();

        if (settings.IsDescending)
        {
            array.Reverse();
        }

        var shuffle = (int)(settings.Length * settings.Randomness);
        Shuffle(random, array, shuffle);
        return array;
    }

    public static void Shuffle(this Random random, int[] array, int shuffle)
    {
        for (int n = 0; n < shuffle; n++)
        {
            var i = random.Next(array.Length);
            var j = random.Next(array.Length);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}
