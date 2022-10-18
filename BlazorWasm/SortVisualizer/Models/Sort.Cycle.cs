namespace SortVisualizer;

public partial class Sort
{
    public static IEnumerable<Operation> CycleSort(int[] a)
    {
        for (int cycle_start = 0; cycle_start < a.Length - 1; cycle_start++)
        {
            var item = a[cycle_start];
            int pos = cycle_start;
            for (int i = cycle_start + 1; i < a.Length; i++)
                if (a[i] < item)
                {
                    yield return new(Kind.Compare, i, cycle_start);
                    pos++;
                }

            yield return new(Kind.Compare, pos, cycle_start);
            if (pos == cycle_start)
                continue;

            while (item == a[pos])
            {
                yield return new(Kind.Compare, pos, cycle_start);
                pos++;
            }

            Swap(ref a[pos], ref item);
            yield return new(Kind.Swap, pos, cycle_start);

            while (pos != cycle_start)
            {
                pos = cycle_start;
                for (int i = cycle_start + 1; i < a.Length; i++)
                    if (a[i] < item)
                    {
                        yield return new(Kind.Compare, i, cycle_start);
                        pos++;
                    }

                while (item == a[pos])
                {
                    yield return new(Kind.Compare, pos, cycle_start);
                    pos++;
                }

                Swap(ref a[pos], ref item);
                yield return new(Kind.Swap, pos, cycle_start);
            }
        }
    }
}
