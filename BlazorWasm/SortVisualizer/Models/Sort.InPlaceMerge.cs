namespace SortVisualizer;

public partial class Sort
{
    public static IEnumerable<Operation> InPlaceMergeSort(int[] arr) => InPlaceMergeSort(arr, 0, arr.Length);

    private static IEnumerable<Operation> InPlaceMergeSort(int[] arr, int l, int r)
    {
        if (l == r || l == r - 1) yield break;

        int m = (l + r) / 2;

        foreach (var x in InPlaceMergeSort(arr, l, m)) yield return x;
        foreach (var x in InPlaceMergeSort(arr, m, r)) yield return x;

        foreach (var x in InPlaceMerge(arr, l, r)) yield return x;
    }
    private static IEnumerable<Operation> InPlaceMerge(int[] a, int l, int r)
    {
        int gap = r - l;

        for (gap = nextGap(gap); gap > 0; gap = nextGap(gap))
        {
            for (int i = l; i + gap < r; i++)
            {
                int j = i + gap;
                yield return new(Kind.Compare, i, j);
                if (a[i] > a[j])
                {
                    yield return new(Kind.Swap, i, j);
                    Swap(a, i, j);
                }
            }
        }

        static int nextGap(int gap) => gap <= 1 ? 0 : (int)Math.Ceiling(gap / 2.0);
    }
}
