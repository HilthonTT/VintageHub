namespace Server.Library.DataAccess.Interfaces;
public interface IVendorData
{
    Task<int> DeleteVendorAsync(VendorModel vendor);
    Task<List<VendorModel>> GetAllVendorsAsync();
    Task<VendorModel> GetVendorByIdAsync(int id);
    Task<int> InsertVendorAsync(VendorModel vendor);
    Task<int> UpdateVendorAsync(VendorModel vendor);
}