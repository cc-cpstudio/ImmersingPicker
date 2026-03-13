namespace ImmersingPicker.Core.Models;

public class AppSettings
{
    public enum LanguageEnums
    {
        System, SimplifiedChinese, TraditionalChinese, English
    }

    public enum ThemeEnums
    {
        System, Light, Dark
    }

    public enum SeatGridRowArrangementMode { T2B, B2T }
    public enum SeatGridColumnArrangementMode { L2R, R2L }

    public static AppSettings Instance { get; set;  } = new AppSettings();

    public AppSettings()
    {
        AppTheme = ThemeEnums.System;
        AppThemeColor = "#0078D4";

        WeightCalculationParam1 = 0.49;
        WeightCalculationParam2 = 1.23;
        WeightCalculationParam3 = 3;
        WeightCalculationParam4 = 1.14;
        WeightCalculationParam5 = 0.98;
        WeightCalculationParam6 = 5.14;
        WeightCalculationParam7 = 2;
        WeightCalculationParam8 = 6;
        WeightCalculationParam9 = 4;
        WeightCalculationParam10 = 9;

        HomeAnimationPlayAmount = 10;
        HomeAnimationPlayDelay = 100;
        SeatGridRowArrangement = SeatGridRowArrangementMode.T2B;
        SeatGridColumnArrangement = SeatGridColumnArrangementMode.L2R;
    }

    public event Action<bool> LaunchOnSystemStartChanged;
    public event Action<bool> OpenUrlAndIpcChanged;
    public event Action<ThemeEnums> AppThemeChanged;
    public event Action<string> AppThemeColorChanged;

    public event Action<double> WeightCalculationParam1Changed;
    public event Action<double> WeightCalculationParam2Changed;
    public event Action<int> WeightCalculationParam3Changed;
    public event Action<double> WeightCalculationParam4Changed;
    public event Action<double> WeightCalculationParam5Changed;
    public event Action<double> WeightCalculationParam6Changed;
    public event Action<int> WeightCalculationParam7Changed;
    public event Action<int> WeightCalculationParam8Changed;
    public event Action<int> WeightCalculationParam9Changed;
    public event Action<int> WeightCalculationParam10Changed;

    public event Action<int> HomeAnimationPlayAmountChanged;
    public event Action<int> HomeAnimationPlayDelayChanged;
    public event Action<SeatGridRowArrangementMode> SeatGridRowArrangementChanged;
    public event Action<SeatGridColumnArrangementMode> SeatGridColumnArrangementChanged;

    public event Action AnyChanged;

    private bool _launchOnSystemStart;
    private bool _openUrlAndIpc;
    private ThemeEnums _appTheme;
    private string _appThemeColor;

    private double _weightCalculationParam1;
    private double _weightCalculationParam2;
    private int _weightCalculationParam3;
    private double _weightCalculationParam4;
    private double _weightCalculationParam5;
    private double _weightCalculationParam6;
    private int _weightCalculationParam7;
    private int _weightCalculationParam8;
    private int _weightCalculationParam9;
    private int _weightCalculationParam10;

    private int _homeAnimationPlayAmount;
    private int _homeAnimationPlayDelay;
    private SeatGridRowArrangementMode _seatGridRowArrangement;
    private SeatGridColumnArrangementMode _seatGridColumnArrangement;

    public bool LaunchOnSystemStart
    {
        get => _launchOnSystemStart;
        set
        {
            _launchOnSystemStart = value;
            LaunchOnSystemStartChanged?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public bool OpenUrlAndIpc
    {
        get => _openUrlAndIpc;
        set
        {
            _openUrlAndIpc = value;
            OpenUrlAndIpcChanged?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public ThemeEnums AppTheme
    {
        get => _appTheme;
        set
        {
            _appTheme = value;
            AppThemeChanged?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public string AppThemeColor
    {
        get => _appThemeColor;
        set
        {
            _appThemeColor = value;
            AppThemeColorChanged?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public double WeightCalculationParam1
    {
        get => _weightCalculationParam1;
        set
        {
            _weightCalculationParam1 = value;
            WeightCalculationParam1Changed?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public double WeightCalculationParam2
    {
        get => _weightCalculationParam2;
        set
        {
            _weightCalculationParam2 = value;
            WeightCalculationParam2Changed?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public int WeightCalculationParam3
    {
        get => _weightCalculationParam3;
        set
        {
            _weightCalculationParam3 = value;
            WeightCalculationParam3Changed?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public double WeightCalculationParam4
    {
        get => _weightCalculationParam4;
        set
        {
            _weightCalculationParam4 = value;
            WeightCalculationParam4Changed?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public double WeightCalculationParam5
    {
        get => _weightCalculationParam5;
        set
        {
            _weightCalculationParam5 = value;
            WeightCalculationParam5Changed?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public double WeightCalculationParam6
    {
        get => _weightCalculationParam6;
        set
        {
            _weightCalculationParam6 = value;
            WeightCalculationParam6Changed?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public int WeightCalculationParam7
    {
        get => _weightCalculationParam7;
        set
        {
            _weightCalculationParam7 = value;
            WeightCalculationParam7Changed?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public int WeightCalculationParam8
    {
        get => _weightCalculationParam8;
        set
        {
            _weightCalculationParam8 = value;
            WeightCalculationParam8Changed?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public int WeightCalculationParam9
    {
        get => _weightCalculationParam9;
        set
        {
            _weightCalculationParam9 = value;
            WeightCalculationParam9Changed?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public int WeightCalculationParam10
    {
        get => _weightCalculationParam10;
        set
        {
            _weightCalculationParam10 = value;
            WeightCalculationParam10Changed?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public int HomeAnimationPlayAmount
    {
        get => _homeAnimationPlayAmount;
        set
        {
            _homeAnimationPlayAmount = value;
            HomeAnimationPlayAmountChanged?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public int HomeAnimationPlayDelay
    {
        get => _homeAnimationPlayDelay;
        set
        {
            _homeAnimationPlayDelay = value;
            HomeAnimationPlayDelayChanged?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public SeatGridRowArrangementMode SeatGridRowArrangement
    {
        get => _seatGridRowArrangement;
        set
        {
            _seatGridRowArrangement = value;
            SeatGridRowArrangementChanged?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }

    public SeatGridColumnArrangementMode SeatGridColumnArrangement
    {
        get => _seatGridColumnArrangement;
        set
        {
            _seatGridColumnArrangement = value;
            SeatGridColumnArrangementChanged?.Invoke(value);
            AnyChanged?.Invoke();
        }
    }
}