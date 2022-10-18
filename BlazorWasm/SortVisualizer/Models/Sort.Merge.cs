using System.Security.Cryptography;

namespace SortVisualizer;

public partial class Sort
{
    public static IEnumerable<Operation> MergeSort(int[] arr) => MergeSort(arr, 0, arr.Length);

    private static IEnumerable<Operation> MergeSort(int[] arr, int l, int r)
    {
        if (l == r || l == r - 1) yield break;

        int m = (l + r) / 2;

        foreach (var x in MergeSort(arr, l, m)) yield return x;
        foreach (var x in MergeSort(arr, m, r)) yield return x;

        foreach (var x in Merge(arr, l, m, r)) yield return x;
    }

    private static IEnumerable<Operation> Merge(int[] a, int l, int m, int r)
    {
        // 普通は外からこのバッファーももらうけど、まあ、デモ用なので Clone。
        var b = a[l..r];

        var i = l;
        var j = m;
        var k = l;

        while (i < m && j < r)
        {
            yield return new(Kind.Compare, i, j);
            if (b[i - l] <= b[j - l])
            {
                a[k++] = b[i++ - l];
                yield return new(Kind.Swap, k, i);
            }
            else
            {
                a[k++] = b[j++ - l];
                yield return new(Kind.Swap, k, j);
            }
        }
        if (i == m)
        {
            while (j < r)
            {
                a[k++] = b[j++ - l];
                yield return new(Kind.Swap, k, j);
            }
        }
        else
        {
            while (i < m)
            {
                a[k++] = b[i++ - l];
                yield return new(Kind.Swap, k, i);
            }
        }
    }
}
