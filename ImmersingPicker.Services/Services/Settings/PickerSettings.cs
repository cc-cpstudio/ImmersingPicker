using ImmersingPicker.Core.Abstractions.Settings;

namespace ImmersingPicker.Services.Services.Settings;

public class MaxAvailableRange() : SettingsItemBase<int>("MaxAvailableRange", 7)
{
    public override bool Validate(int value) =>
        value > 0;
}

public class MinSelectionPoolAmount() : SettingsItemBase<int>("MinSelectionPoolAmount", 10)
{
    public override bool Validate(int value) =>
        value > 0;
}

public class WeighCoefficientOfSelectedAmount() : SettingsItemBase<double>("WeighCoefficientOfSelectedAmount", 1.1);

public class IntervalDays() : SettingsItemBase<int>("IntervalDays", 3)
{
    public override bool Validate(int value) =>
        value > 0;
}

public class WeighCoefficientOfSelectedTime() : SettingsItemBase<double>("WeighCoefficientOfSelectedTime", 1.2);

public class WeighCoefficientOfNullSelectedTime()
    : SettingsItemBase<double>("WeighCoefficientOfNullSelectedTime", 10.2);

public class RandomWeighCoefficientUpperBound() : SettingsItemBase<int>("RandomWeighCoefficientUpperBound", 8);

public class RandomWeighCoefficientLowerBound() : SettingsItemBase<int>("RandomWeighCoefficientLowerBound", 2);

public class PickerSettingsGroup : SettingsGroupBase
{
    public MaxAvailableRange MaxAvailableRange { get; } = new();
    public MinSelectionPoolAmount MinSelectionPoolAmount { get; } = new();
    public WeighCoefficientOfSelectedAmount WeighCoefficientOfSelectedAmount { get; } = new();
    public IntervalDays IntervalDays { get; } = new();
    public WeighCoefficientOfSelectedTime WeighCoefficientOfSelectedTime { get; } = new();
    public WeighCoefficientOfNullSelectedTime WeighCoefficientOfNullSelectedTime { get; } = new();
    public RandomWeighCoefficientUpperBound RandomWeighCoefficientUpperBound { get; } = new();
    public RandomWeighCoefficientLowerBound RandomWeighCoefficientLowerBound { get; } = new();

    public override void ResetToDefault()
    {
        MaxAvailableRange.Reset();
        MinSelectionPoolAmount.Reset();
        WeighCoefficientOfSelectedAmount.Reset();
        IntervalDays.Reset();
        WeighCoefficientOfSelectedTime.Reset();
        WeighCoefficientOfNullSelectedTime.Reset();
        RandomWeighCoefficientUpperBound.Reset();
        RandomWeighCoefficientLowerBound.Reset();
    }
}