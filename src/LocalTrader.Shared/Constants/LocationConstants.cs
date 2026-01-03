using LocalTrader.Shared.Data.Account;

namespace LocalTrader.Shared.Constants;

public static class LocationConstants
{
    public static Meter MaximumRadius { get; } = new(300 * 1_000);
    
    public static Longitute MaximumLongitute { get; } = new(180);
    public static Longitute MinimumLongitute { get; } = new(-180);
    
    /// <summary>
    /// North Pole
    /// </summary>
    public static Latitude MaximumLatitude { get; } = new(90);
    /// <summary>
    /// South Pole
    /// </summary>
    public static Latitude MinimumLatitude { get; } = new(-90);
}