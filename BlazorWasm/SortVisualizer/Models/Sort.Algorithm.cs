namespace SortVisualizer;

public partial class Sort
{
    public static IReadOnlyList<Algorithm> Algorithms => _algorithms;

    private static readonly Algorithm[] _algorithms = new Algorithm[]
    {
        new InPlaceAlgorithm("Quick Sort", QuickSort),
        new InPlaceAlgorithm("Shell Sort", ShellSort),
        new InPlaceAlgorithm("Heap Sort", HeapSort),
        new InPlaceAlgorithm("Insert Sort", InsertSort),
        new InPlaceAlgorithm("Selection Sort", SelectSort),
        new InPlaceAlgorithm("Odd-Even Sort", OddEvenSort),
        new InPlaceAlgorithm("Bubble Sort", BubbleSort),
        new InPlaceAlgorithm("Gnome Sort", GnomeSort),
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
