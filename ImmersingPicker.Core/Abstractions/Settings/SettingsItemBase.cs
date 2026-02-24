namespace ImmersingPicker.Core.Abstractions.Settings;

public abstract class SettingsItemBase<T>
{
    private T _value;

    public string Key { get; init; }
    public T DefaultValue { get; }
    public T Value
    {
        get => _value;
        set => _value = Validate(value) ? value : DefaultValue;
    }

    protected SettingsItemBase(string key, T defaultValue)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key), "设置项键不能为空");
        DefaultValue = defaultValue;
        Value = defaultValue;
    }

    public virtual void Reset()
    {
        Value = DefaultValue;
    }

    public virtual bool Validate(T value) =>
        value != null;
}