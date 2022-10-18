namespace SortVisualizer;

public partial class Sort
{
    public static IEnumerable<Operation> CombSort(int[] a)
    {
        int h = a.Length * 10 / 13;

        while (true)
        {
            int swaps = 0;
            for (int i = 0; i + h < a.Length; ++i)
            {
                yield return new(Kind.Compare, i, i + h);
                if (a[i] > a[i + h])
                {
                    yield return new(Kind.Swap, i, i + h);
                    Swap(a, i, i + h);
                    ++swaps;
                }
            }
            if (h == 1)
            {
                if (swaps == 0) break;
            }
            else
            {
                h = h * 10 / 13;
            }
        }
    }
}
