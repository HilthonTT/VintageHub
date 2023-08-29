namespace Shared.Library.DataAccess.Interfaces;
public interface IVendorData
{
    Task<int> DeleteVendorAsync(int id);
    Task<List<VendorDisplayModel>> GetAllVendorsAsync();
    Task<VendorDisplayModel> GetVendorByIdAsync(int id);
    Task<List<VendorDisplayModel>> GetAllVendorByOwnerUserIdAsync(int ownerUserId);
    Task<int> InsertVendorAsync(VendorModel vendor);
    Task<int> UpdateVendorAsync(VendorModel vendor);
}