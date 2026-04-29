namespace WareHouseApp.Bll.Exceptions;
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(int id)
    {
        Id = id;
    }

    public EntityNotFoundException(string message, int id)
        : base(message)
    {
        Id = id;
    }

    public EntityNotFoundException(string message, Exception innerException, int id)
        : base(message, innerException)
    {
        Id = id;
    }

    public int Id { get; }
}
