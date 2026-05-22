using AutoMapper;
using Sqids;
using System;
using System.Collections.Generic;
using System.Text;
using WareHouseApp.Bll.Exceptions;

namespace WareHouseApp.Bll.Dtos.Encoding;

public class IdEncoder : IIdEncoder
{

    private readonly SqidsEncoder<int> _sqids;
    public IdEncoder()
    {
        _sqids = new SqidsEncoder<int>(new SqidsOptions
        {
            Alphabet = "k3G7QAe51FCsPW92uEOyq4Bg6Sp8YzVTmnU0liwDdHXLajZrfxNhobJIRcMvKt",
            MinLength = 8
        });
    }

    public string Encode(int id)
    {
        return _sqids.Encode(id);
    }   
    public int Decode(string encodedId)
    {
        var decoded = _sqids.Decode(encodedId);
        return decoded.Count > 0 ? decoded[0] : throw new EntityNotFoundException("Invalid or malformed ID format", 0);
    }

}