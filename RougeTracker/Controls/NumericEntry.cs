using CommunityToolkit.Maui;
using System.Globalization;
using System.Text;

namespace RougeTracker.Controls;

public partial class NumericEntry : Entry
{
    private static readonly CultureInfo Culture = CultureInfo.CurrentCulture;
    private static readonly char FractionSeparator = Culture.NumberFormat.NumberDecimalSeparator[0];

    private bool _internalUpdate;

    [BindableProperty(PropertyChangedMethodName = nameof(OnValueChanged))]
    public partial decimal? Value { get; set; }

    public NumericEntry()
    {
        Keyboard = Keyboard.Numeric;

        Focused += OnFocused;
        Unfocused += OnUnfocused;
    }

    private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not NumericEntry entry) return;

        if (entry._internalUpdate)
            return;

        entry.UpdateTextFromValue();
    }


    private void InternalUpdateText(string? text) 
    {
        _internalUpdate = true;
        Text = text;
        _internalUpdate = false;
    }

    private void OnFocused(object? sender, FocusEventArgs e)
        => UpdateEditingText();

    private void OnUnfocused(object? sender, FocusEventArgs e)
    {
        ParseTextToValue();
        UpdateTextFromValue();
    }

    private string NormalizeText(string text) 
    {
        text = text.Replace('.', FractionSeparator).Replace(',', FractionSeparator).Trim();

        if (text.EndsWith(FractionSeparator))
            text += "0";

        return text;
    }

    protected override void OnTextChanged(string oldValue, string newValue)
    {
        if (_internalUpdate || newValue is null)
        {
            base.OnTextChanged(oldValue, newValue);
            return;
        }

        newValue = NormalizeText(newValue);

        var filtered = Filter(newValue);

        if (filtered != newValue)
        {
            InternalUpdateText(filtered);
        }

        ParseTextToValue();

        base.OnTextChanged(oldValue, Text);
    }

    private void ParseTextToValue()
    {
        var value = NormalizeText(Text);

        if (string.IsNullOrWhiteSpace(value))
        {
            InternalUpdateText(null);
            return;
        }

        if(value is "-" or "+") { return; }

        if (decimal.TryParse(
                value,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                Culture,
                out var number))
        {
            _internalUpdate = true;
            Value = number;
            _internalUpdate = false;
        }
    }



    private void UpdateTextFromValue()
    {
        var newText = Value switch
        {
            null => string.Empty,
            > 0 => $"+{Value:0.00}",
            < 0 => $"{Value:0.00}",
            _ => "0.00"
        };
        InternalUpdateText(newText);
    }

    private void UpdateEditingText()
    {
        InternalUpdateText(Value?.ToString("0.##", Culture) ?? string.Empty);
    }

    private static string Filter(string input)
    {
        var sb = new StringBuilder(input.Length);
        bool hasSeparator = false;
        bool hasSign = false;

        foreach (var c in input)
        {
            if (char.IsDigit(c))
            {
                sb.Append(c);
                continue;
            }

            if ((c is '+' or '-') && !hasSign && sb.Length == 0)
            {
                hasSign = true;
                sb.Append(c);
                continue;
            }

            if (c == FractionSeparator && !hasSeparator)
            {
                hasSeparator = true;
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}