namespace SortVisualizer;

public partial class Sort
{
    public static IEnumerable<Operation> GnomeSort(int[] a)
    {
        var i = 0;
        while (i < a.Length)
        {
            yield return new(Kind.Compare, i, i - 1);
            if (i == 0 || a[i] >= a[i - 1])
            {
                i++;
            }
            else
            {
                yield return new(Kind.Swap, i, i - 1);
                Swap(a, i, i - 1);
                i--;
            }
        }
    }
}
