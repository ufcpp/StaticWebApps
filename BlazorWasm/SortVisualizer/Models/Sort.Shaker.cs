namespace SortVisualizer;

public partial class Sort
{
    public static IEnumerable<Operation> ShakerSort(int[] a)
    {
        var top = 0;
        var bottom = a.Length - 1;

        while (true)
        {
            var lastSwap = top;

            for (int i = top; i < bottom; i++)
            {
                yield return new(Kind.Compare, i, i + 1);
                if (a[i] > a[i + 1])
                {
                    Swap(a, i, i + 1);
                    yield return new(Kind.Swap, i, i + 1);
                    lastSwap = i;
                }
            }
            bottom = lastSwap;

            if (top == bottom) break;

            lastSwap = bottom;

            for (int i = bottom; i > top; i--)
            {
                yield return new(Kind.Compare, i, i - 1);
                if (a[i] < a[i - 1])
                {
                    Swap(a, i, i - 1);
                    yield return new(Kind.Swap, i, i - 1);
                    lastSwap = i;
                }
            }
            top = lastSwap;

            if (top == bottom) break;
        }
    }
}
