﻿using Client.Library.Models;

namespace Client.Library.Endpoints.Interfaces;
public interface IVendorEndpoint
{
    Task DeleteVendorAsync(VendorModel vendor);
    Task<List<VendorModel>> GetAllVendorsAsync();
    Task<VendorModel> GetVendorByIdAsync(int id);
    Task<VendorModel> InsertVendorAsync(VendorModel vendor);
    Task UpdateVendorAsync(VendorModel vendor);
}