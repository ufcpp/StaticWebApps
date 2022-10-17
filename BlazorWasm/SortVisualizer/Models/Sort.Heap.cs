namespace SortVisualizer;

public partial class Sort
{
    public static IEnumerable<Operation> HeapSort(int[] a)
    {
        for (int i = 1; i < a.Length; ++i)
            foreach (var x in MakeHeap(a, i)) yield return x;
        for (int i = a.Length - 1; i >= 0; --i)
        {
            var max = a[0];
            foreach (var x in PopHeap(a, i)) yield return x;
            a[i] = max;
        }
    }

    private static IEnumerable<Operation> MakeHeap(int[] a, int n)
    {
        while (n != 0)
        {
            int i = (n - 1) / 2;
            yield return new(Kind.Compare, n, i);
            if (a[n] > a[i])
            {
                Swap(ref a[n], ref a[i]);
                yield return new(Kind.Swap, n, i);
            }
            n = i;
        }
    }

    private static IEnumerable<Operation> PopHeap(int[] a, int n)
    {
        a[0] = a[n];

        for (int i = 0, j; (j = 2 * i + 1) < n;)
        {
            yield return new(Kind.Compare, j, j + 1);
            if ((j != n - 1) && (a[j] < a[j + 1])) j++;
            yield return new(Kind.Compare, i, j);
            if (a[i] < a[j])
            {
                Swap(ref a[i], ref a[j]);
                yield return new(Kind.Swap, i, j);
            }
            i = j;
        }
    }
}
