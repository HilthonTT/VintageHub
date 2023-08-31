namespace Shared.Library.Endpoints.Interfaces;
public interface IVendorEndpoint
{
    Task DeleteVendorAsync(VendorModel vendor);
    Task<List<VendorDisplayModel>> GetAllVendorsAsync();
    Task<List<VendorDisplayModel>> GetAllVendorsByOwnerUserIdAsync(int ownerUserId);
    Task<VendorDisplayModel> GetVendorByIdAsync(int id);
    Task<VendorDisplayModel> InsertVendorAsync(VendorModel vendor);
    Task UpdateVendorAsync(VendorModel vendor);
}