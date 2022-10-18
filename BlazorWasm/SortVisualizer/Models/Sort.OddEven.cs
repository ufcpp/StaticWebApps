namespace SortVisualizer;

public partial class Sort
{
    public static IEnumerable<Operation> OddEvenSort(int[] a)
    {
        var sorted = false;
        while (!sorted)
        {
            sorted = true;
            for (var i = 1; i < a.Length - 1; i += 2)
            {
                yield return new(Kind.Compare, i, i + 1);
                if (a[i] > a[i + 1])
                {
                    yield return new(Kind.Swap, i, i + 1);
                    Swap(a, i, i + 1);
                    sorted = false;
                }
            }
            for (var i = 0; i < a.Length - 1; i += 2)
            {
                yield return new(Kind.Compare, i, i + 1);
                if (a[i] > a[i + 1])
                {
                    yield return new(Kind.Swap, i, i + 1);
                    Swap(a, i, i + 1);
                    sorted = false;
                }
            }
        }
    }
}
