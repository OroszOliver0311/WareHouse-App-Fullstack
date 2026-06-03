using WareHouseApp.Bll.Interfaces;

namespace WareHouseApp.Bll.Services;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}