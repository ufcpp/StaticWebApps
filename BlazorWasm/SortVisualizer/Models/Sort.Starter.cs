namespace SortVisualizer;

public partial class Sort
{
    public static State Start(int[] array, Algorithm argorithm) => new(array, argorithm);

    public readonly struct State
    {
        public string Name { get; }
        private readonly int[] _array;
        private readonly int[]? _buffer;
        private readonly IEnumerator<Operation> _sortOperations;

        public State(int[] array, Algorithm algorithm)
        {
            Name = algorithm.Name;
            _array = array;
            _sortOperations = algorithm.Start(array).GetEnumerator();
        }

        public State(int[] array, int[] buffer, Algorithm algorithm)
        {
            Name = algorithm.Name;
            _array = array;
            _buffer = buffer;
            _sortOperations = algorithm.Start(array).GetEnumerator();
        }

        public ReadOnlySpan<int> Items => _array;
        public ReadOnlySpan<int> Buffers => _buffer;
        public Operation Current => _sortOperations.Current;
        public bool MoveNext() => _sortOperations.MoveNext();
    }
}
