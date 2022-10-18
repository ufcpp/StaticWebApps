namespace SortVisualizer;

public partial class Sort
{
    public static IReadOnlyList<Algorithm> Algorithms => _algorithms;

    private static readonly Algorithm[] _algorithms = new Algorithm[]
    {
        I(QuickSort, "Quick", """
            比較・スワップに基づくソートの中で、だいたいのデータで最速。
            時々苦手なデータあり。
            """),

        I(ShellSort, "Shell", """
            要素数がそんなに多くないうちはむっちゃ速い。
            O(N lon N) ではないけども、数百要素超えても Heap とか Merge よりも速い。
            速いわりにコードが短い。
            """),

        I(HeapSort, "Heap", """
            O(N log N) ソートの中では遅めだけども、苦手なデータがない。
            Quick ソートと相補的に使ったりする
            (再帰が深くなった時に切り替える)。
            """),

        I(InsertSort, "Insert", """
            遅いやつ。
            ただ、おおむねソート済みのデータに対しては速い。
            Quick ソートと相補的に使ったりする
            (データが短くなってきたときに切り替える)。
            """),

        I(CycleSort, "Cycle", """
            Selection 系。
            スワップ回数が理論上最小らしい。
            """),

        I(SelectSort, "Selection", """
            遅いやつ。
            比較は多いけどもスワップは少ない。
            """),

        I(OddEvenSort, "Odd-Even", """
            Bubble 亜種。
            奇数・偶数に分けて Bubble ソート。
            奇数・偶数で並列処理可能なのがメリットらしい。
            正直、見た目が面白いから選んだ。
            """),

        I(ShakerSort, "Cocktail shaker", """
            Bubble 亜種。
            行ったり来たりすることでスキャン範囲を狭める工夫とのこと。
            正直、見た目が面白いから選んだ。
            """),

        I(BubbleSort, "Bubble", """
            入門記事によく出てくる遅いやつ。
            """),

        I(GnomeSort, "Gnome", """
            とにかくコードが短い。
            まあ、その代わり最遅
            (わざと遅さを競うようなものは除く)。
            """),
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
