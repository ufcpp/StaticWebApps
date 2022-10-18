namespace SortVisualizer;

public partial class Sort
{
    public static IReadOnlyList<Algorithm> Algorithms => _algorithms;

    private static readonly Algorithm[] _algorithms = new Algorithm[]
    {
        new(QuickSort)
        {
            Name = "Quick",
            Description = """
            比較・スワップに基づくソートの中で、だいたいのデータで最速。
            時々苦手なデータあり。
            """
        },
        new(ShellSort)
        {
            Name = "Shell",
            Description = """
            Insertion 改良版。
            要素数がそんなに多くないうちはむっちゃ速い。
            O(N lon N) ではないけども、数百要素超えても Heap とか Merge よりも速い。
            速いわりにコードが短い。
            """
        },
        new(CombSort)
        {
            Name = "Comb",
            Description = """
            Bubble 改良版。
            Insertion に対する Shell と似たような発想。
            """
        },
        new(HeapSort)
        {
            Name = "Heap",
            Description = """
            O(N log N) ソートの中では遅めだけども、苦手なデータがない。
            Quick ソートと相補的に使ったりする
            (再帰が深くなった時に切り替える)。
            """
        },
        new(MergeSort)
        {
            Name = "Merge",
            Description = """
            O(N log N) の中では遅い部類。
            in-place じゃない(外部バッファーが必要)。
            ただ、安定ソートだったり並列化しやすいという利点あり。
            安定ソートの中では速い方。
            """
        },
        new(InPlaceMergeSort)
        {
            Name = "In-place Merge",
            Description = """
            Merge 亜種。
            in-place (ソート対象の配列内で完結して、追加のメモリが不要)になるように変更したもの。
            その分だいぶ遅い。
            """
        },
        new(InsertSort)
        {
            Name = "Insertion",
            Description = """
            遅いやつ。
            ただ、おおむねソート済みのデータに対しては速い。
            Quick ソートと相補的に使ったりする
            (データが短くなってきたときに切り替える)。
            """
        },
        new(CycleSort)
        {
            Name = "Cycle",
            Description = """
            Selection 系。
            スワップ回数が理論上最小らしい。
            """
        },
        new(SelectSort)
        {
            Name = "Selection",
            Description = """
            遅いやつ。
            比較は多いけどもスワップは少ない。
            """
        },
        new(OddEvenSort)
        {
            Name = "Odd-Even",
            Description = """
            Bubble 亜種。
            奇数・偶数に分けて Bubble ソート。
            奇数・偶数で並列処理可能なのがメリットらしい。
            正直、見た目が面白いから選んだ。
            """
        },
        new(ShakerSort)
        {
            Name = "Cocktail shaker",
            Description = """
            Bubble 亜種。
            行ったり来たりすることでスキャン範囲を狭める工夫とのこと。
            正直、見た目が面白いから選んだ。
            """
        },
        new(BubbleSort)
        {
            Name = "Bubble",
            Description = """
            入門記事によく出てくる遅いやつ。
            """
        },
        new(GnomeSort)
        {
            Name = "Gnome",
            Description = """
            とにかくコードが短い。
            まあ、その代わり最遅
            (わざと遅さを競うようなものは除く)。
            """
        },
        new(BucketSort)
        {
            Name = "Bucket",
            Description = """
            適用できる条件がかなり限られている代わりに爆速。
            上限・下限が既知の整数配列とかに対して使うと本当に速い。 O(N)。
            """
        },
    };

    public record Algorithm(Func<int[], IEnumerable<Operation>> Sorter)
    {
        public required string Name { get; init; }
        public string? Description { get; init; }
        public IEnumerable<Operation> Start(int[] array) => Sorter(array);
    }
}
