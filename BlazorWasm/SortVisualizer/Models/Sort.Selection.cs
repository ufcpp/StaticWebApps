namespace SortVisualizer;

public partial class Sort
{
    public static IEnumerable<Operation> SelectSort(int[] a)
    {
        int n = a.Length;
        for (int i = 0; i < n; i++)
        {
            int min = i;
            for (int j = i + 1; j < n; j++)
            {
                yield return new(Kind.Compare, min, j);
                if (a[min] > a[j])
                    min = j;
            }
            Swap(ref a[i], ref a[min]);
            yield return new(Kind.Swap, i, min);
        }
    }
}
