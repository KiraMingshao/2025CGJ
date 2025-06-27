public readonly struct Option<T> {
    private readonly T _value;
    public readonly bool HasValue;

    public T Value {
        get {
            if (!HasValue) {
                throw new System.InvalidOperationException("Option does not have a value.");
            }
            return _value;
        }
    }

    private Option(T value) {
        _value = value;
        HasValue = true;
    }

    public static Option<T> Some(T value) {
        return new Option<T>(value);
    }

    public static Option<T> None {
        get {
            return default;
        }
    }
}
