﻿namespace Server.Library.DataAccess.Interfaces;
public interface IVendorData
{
    Task<int> DeleteVendorAsync(int id);
    Task<List<VendorModel>> GetAllVendorsAsync();
    Task<VendorModel> GetVendorByIdAsync(int id);
    Task<List<VendorModel>> GetAllVendorByOwnerUserIdAsync(int ownerUserId);
    Task<int> InsertVendorAsync(VendorModel vendor);
    Task<int> UpdateVendorAsync(VendorModel vendor);
}