using WareHouseApp.Bll.Dtos;

namespace WareHouseApp.Bll.Interfaces;
public interface IWareHouseService
{
    Task<IEnumerable<WareHouseDto>> GetAllWareHousesAsync();
    Task<WareHouseDto> GetWareHouseByIdAsync(int id);
    Task<WareHouseDto> CreateWareHouseAsync(CreateWareHouseDto createWareHouse);
    Task<WareHouseDto> UpdateWareHouseAsync(int id, CreateWareHouseDto updateWareHouse);
    Task DeleteWareHouseAsync(int id);

}
