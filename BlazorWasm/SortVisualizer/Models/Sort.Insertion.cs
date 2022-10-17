namespace SortVisualizer;

public partial class Sort
{
    public static IEnumerable<Operation> InsertSort(int[] a)
    {
        int n = a.Length;
        for (int i = 1; i < n; i++)
            for (int j = i; j >= 1 && a[j - 1] > a[j]; --j)
            {
                yield return new(Kind.Compare, j - 1, j);
                Swap(ref a[j], ref a[j - 1]);
                yield return new(Kind.Swap, j - 1, j);
            }
    }
}
