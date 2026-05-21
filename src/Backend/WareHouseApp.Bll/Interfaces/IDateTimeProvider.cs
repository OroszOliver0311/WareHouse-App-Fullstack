namespace WareHouseApp.Bll.Interfaces;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
