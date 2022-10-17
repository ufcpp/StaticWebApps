namespace SortVisualizer;

public partial class Sort
{
    public static IEnumerable<Operation> ShellSort(int[] a)
    {
        int n = a.Length;
        int h;
        for (h = 1; h < n / 9; h = h * 3 + 1) ;
        for (; h > 0; h /= 3)
            for (int i = h; i < n; i++)
                for (int j = i; j >= h && a[j - h].CompareTo(a[j]) > 0; j -= h)
                {
                    yield return new(Kind.Compare, j, j - h);
                    Swap(ref a[j], ref a[j - h]);
                    yield return new(Kind.Swap, j, j - h);
                }
    }
}
