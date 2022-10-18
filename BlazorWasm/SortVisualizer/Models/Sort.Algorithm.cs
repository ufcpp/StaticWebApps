namespace SortVisualizer;

public partial class Sort
{
    public static IReadOnlyList<Algorithm> Algorithms => _algorithms;

    private static readonly Algorithm[] _algorithms = new Algorithm[]
    {
        I(QuickSort, "Quick Sort", "だいたいは最速。時々苦手なデータあり。"),
        I(ShellSort, "Shell Sort", "要素数が多くないうちはむっちゃ速い。"),
        I(HeapSort, "Heap Sort", "O(N log N) の中では遅めだけども、苦手なデータがない。Quick ソートと相補的に使ったりする。"),
        I(InsertSort, "Insert Sort", "入門によく出る遅いやつ。ただ、おおむねソート済みのデータに対しては速い。"),
        I(SelectSort, "Selection Sort", "入門によく出る遅いやつ。比較は多いけどもスワップは少ない。"),
        I(OddEvenSort, "Odd-Even Sort", "Bubble 亜種。奇数・偶数に分けてソート。奇遇で並列処理可能なのがメリットらしい。"),
        I(BubbleSort, "Bubble Sort", "入門によく出る遅いやつ。"),
        I(GnomeSort, "Gnome Sort", "コードが短い。まあ、遅い。"),
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
