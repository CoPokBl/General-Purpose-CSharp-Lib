namespace GeneralPurposeLib; 

public class Property {
    public string Text { get {
        if (_value == null) {
            throw new InvalidOperationException("Value is null");
        }
        if (_value is not string str) {
            throw new InvalidOperationException("Value is not a string");
        }
        return str;
    } }
    public int Integer { get {
        if (_value == null) {
            throw new InvalidOperationException("Value is null");
        }
        if (_value is not int i) {
            throw new InvalidOperationException("Value is not a integer");
        }
        return i;
    } }
    public double Decimal { get {
        if (_value == null) {
            throw new InvalidOperationException("Value is null");
        }
        if (_value is not double i) {
            throw new InvalidOperationException("Value is not a double");
        }
        return i;
    } }
    public float Float { get {
        if (_value == null) {
            throw new InvalidOperationException("Value is null");
        }
        if (_value is not float i) {
            throw new InvalidOperationException("Value is not a float");
        }
        return i;
    } }
    public bool Boolean { get {
        if (_value == null) {
            throw new InvalidOperationException("Value is null");
        }
        if (_value is not bool boolean) {
            throw new InvalidOperationException("Value is not a boolean");
        }
        return boolean;
    } }
    public DateTime Date {
        get {
            if (_value == null) {
                throw new InvalidOperationException("Value is null");
            }
            if (_value is not DateTime time) {
                throw new InvalidOperationException("Value is not a DateTime");
            }
            return time;
        }
    }

    public override string ToString() => Text;

    private readonly object? _value;

    // Empty Constructor for JSON Deserialization
    public Property() { }
    public Property(string text) { _value = text; }
    public Property(int number) { _value = number; }
    public Property(bool boolean) { _value = boolean; }
    public Property(double number) { _value = number; }
    public Property(float number) { _value = number; }
    public Property(DateTime date) { _value = date; }

    public Property(object obj) {
        _value = obj switch {
            string text => text,
            int number => number,
            bool boolean => boolean,
            double dub => dub,
            float flo => flo,
            DateTime date => date,
            _ => throw new ArgumentException("Object is not a valid type")
        };
    }
    
    public PropertyType Type {
        get {
            return _value switch {
                string => PropertyType.String,
                int => PropertyType.Integer,
                bool => PropertyType.Boolean,
                double => PropertyType.Decimal,
                float => PropertyType.Float,
                DateTime => PropertyType.Date,
                null => PropertyType.Null,
                _ => throw new InvalidOperationException("Value is not a valid type")
            };
        }
    }
    
    public enum PropertyType {
        String,
        Integer,
        Decimal,
        Float,
        Boolean,
        Date,
        Null
    }
    
    // To Property
    public static implicit operator Property(string text) => new(text);
    public static implicit operator Property(int number) => new(number);
    public static implicit operator Property(double number) => new(number);
    public static implicit operator Property(float number) => new(number);
    public static implicit operator Property(bool boolean) => new(boolean);
    public static implicit operator Property(DateTime date) => new(date);
    
    // From Property
    public static implicit operator string(Property property) => property.Text;
    public static implicit operator int(Property property) => property.Integer;
    public static implicit operator double(Property property) => property.Decimal;
    public static implicit operator float(Property property) => property.Float;
    public static implicit operator bool(Property property) => property.Boolean;
    public static implicit operator DateTime(Property property) => property.Date;
    
}