namespace WareHouseApp.Bll.Dtos.Encoding;

public interface IIdEncoder
{
    string Encode(int id);
    int Decode(string encodedId);
}
