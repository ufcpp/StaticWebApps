namespace SortVisualizer;

public partial class Sort
{
    public static IReadOnlyList<Algorithm> Algorithms => _algorithms;

    private static readonly Algorithm[] _algorithms = new Algorithm[]
    {
        new InPlaceAlgorithm("Bubble Sort", BubbleSort),
        new InPlaceAlgorithm("Quick Sort", QuickSort),
    };

    public abstract record Algorithm(string Name)
    {
        public abstract IEnumerable<Operation> Start(int[] array);
    }

    public record InPlaceAlgorithm(string Name, Func<int[], IEnumerable<Operation>> Sorter)
        : Algorithm(Name)
    {
        public override IEnumerable<Operation> Start(int[] array) => Sorter(array);
    }

    public record OutOfPlaceAlgorithm(string Name, Func<int[], int[], IEnumerable<Operation>> Sorter)
        : Algorithm(Name)
    {
        public override IEnumerable<Operation> Start(int[] array) => Sorter(array, new int[array.Length]);
    }
}
