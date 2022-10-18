namespace SortVisualizer;

public partial class Sort
{
    public static IReadOnlyList<Algorithm> Algorithms => _algorithms;

    private static readonly Algorithm[] _algorithms = new Algorithm[]
    {
        I(QuickSort, "Quick Sort"),
        I(ShellSort, "Shell Sort"),
        I(HeapSort, "Heap Sort"),
        I(InsertSort, "Insert Sort"),
        I(SelectSort, "Selection Sort"),
        I(OddEvenSort, "Odd-Even Sort"),
        I(BubbleSort, "Bubble Sort"),
        I(GnomeSort, "Gnome Sort"),
    };

    private static InPlaceAlgorithm I(Func<int[], IEnumerable<Operation>> sorter, string name, string? description = null)
        => new(sorter)
        {
            Name = name,
            Description = description,
        };

    public abstract record Algorithm
    {
        public required string Name { get; init; }
        public string? Description { get; init; }
        public abstract IEnumerable<Operation> Start(int[] array);
    }

    public record InPlaceAlgorithm(Func<int[], IEnumerable<Operation>> Sorter) : Algorithm
    {
        public override IEnumerable<Operation> Start(int[] array) => Sorter(array);
    }

    public record OutOfPlaceAlgorithm(Func<int[], int[], IEnumerable<Operation>> Sorter) : Algorithm
    {
        public override IEnumerable<Operation> Start(int[] array) => Sorter(array, new int[array.Length]);
    }
}
