using AutoMapper;

namespace WareHouseApp.Bll.Dtos.Encoding;

public class SqidToIntConverter(IIdEncoder idEncoder) : IValueConverter<string, int>
{
    public int Convert(string sourceMember, ResolutionContext context)
    {
        return idEncoder.Decode(sourceMember);
    }
}