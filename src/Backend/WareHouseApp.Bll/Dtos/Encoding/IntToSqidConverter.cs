using AutoMapper;


namespace WareHouseApp.Bll.Dtos.Encoding;

public class IntToSqidConverter(IIdEncoder idEncoder) : IValueConverter<int, string>
{
    public string Convert(int sourceMember, ResolutionContext context)
    {
        return idEncoder.Encode(sourceMember);
    }
}