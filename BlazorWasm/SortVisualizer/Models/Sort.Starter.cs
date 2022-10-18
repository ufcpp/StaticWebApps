namespace SortVisualizer;

public partial class Sort
{
    public static State Start(int[] array, Algorithm argorithm) => new(array, argorithm);

    public readonly struct State
    {
        private readonly Algorithm _algorithm;
        private readonly int[] _array;
        private readonly int[]? _buffer;
        private readonly IEnumerator<Operation> _sortOperations;

        public State(int[] array, Algorithm algorithm)
        {
            _algorithm = algorithm;
            _array = array;
            _sortOperations = algorithm.Start(array)
                .Append(new(Kind.Done, -1, -1)) // 最後に Compare/Swap 表示が残らないように。
                .GetEnumerator();
        }

        public State(int[] array, int[] buffer, Algorithm algorithm)
        {
            _algorithm = algorithm;
            _array = array;
            _buffer = buffer;
            _sortOperations = algorithm.Start(array).GetEnumerator();
        }

        public string Name => _algorithm.Name;
        public string? Description => _algorithm.Description;

        public ReadOnlySpan<int> Items => _array;
        public ReadOnlySpan<int> Buffers => _buffer;
        public Operation Current => _sortOperations.Current;
        public bool MoveNext() => _sortOperations.MoveNext();
    }
}
