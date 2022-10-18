namespace SortVisualizer;

public partial class Sort
{
    public static IEnumerable<Operation> BucketSort(int[] a)
    {
        // 値の範囲が既知ならここから、
        var min = 0;
        var max = 0;
        for (int i = 0; i < a.Length; i++)
        {
            yield return new(Kind.Compare, min, i);
            yield return new(Kind.Compare, max, i);
            if (a[min] > a[i]) min = i;
            if (a[max] < a[i]) max = i;
        }

        min = a[min];
        max = a[max];
        // ここまで不要。

        var len = max - min + 1;
        var count = new int[len];

        for (int i = 0; i < a.Length; i++)
        {
            count[a[i] - min]++;
        }

        var j = 0;
        for (int v = 0; v < count.Length; v++)
        {
            for (int c = 0; c < count[v]; c++)
            {
                a[j++] = v + min;

                // 別に Swap してるわけじゃないんだけど… 形式的に、上書き先だけ。
                yield return new(Kind.Swap, j, -1);
            }
        }
    }
}
