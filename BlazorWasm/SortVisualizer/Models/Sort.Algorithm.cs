namespace SortVisualizer;

public partial class Sort
{
    public static IReadOnlyList<Algorithm> Algorithms => _algorithms;

    private static readonly Algorithm[] _algorithms = new Algorithm[]
    {
        I(QuickSort, "Quick Sort", "比較・スワップに基づくソートの中で、だいたいは最速。時々苦手なデータあり。"),
        I(ShellSort, "Shell Sort", "要素数がそんなに多くないうちはむっちゃ速い。速いわりにコードが短い。"),
        I(HeapSort, "Heap Sort", "O(N log N) ソートの中では遅めだけども、苦手なデータがない。Quick ソートと相補的に使ったりする。"),
        I(InsertSort, "Insert Sort", "遅いやつ。ただ、おおむねソート済みのデータに対しては速い。"),
        I(CycleSort, "Cycle Sort", "Selection 系。スワップ回数が理論上最小らしい。"),
        I(SelectSort, "Selection Sort", "遅いやつ。比較は多いけどもスワップは少ない。"),
        I(OddEvenSort, "Odd-Even Sort", "Bubble 亜種。奇数・偶数に分けて Bubble ソート。奇遇で並列処理可能なのがメリットらしい。"),
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
