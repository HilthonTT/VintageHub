namespace Shared.Library.DataAccess.Helpers;
public static class ParameterHelper
{
    public static DynamicParameters GetIdParameters(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        return parameters;
    }

    public static DynamicParameters GetArtifactUpdateParameters(ArtifactModel artifact)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", artifact.Id);
        parameters.Add("Name", artifact.Name);
        parameters.Add("Description", artifact.Description);
        parameters.Add("ImageId", artifact.ImageId);
        parameters.Add("Quantity", artifact.Quantity);
        parameters.Add("Rating", artifact.Rating);
        parameters.Add("Price", artifact.Price);
        parameters.Add("DiscountAmount", artifact.DiscountAmount);
        parameters.Add("VendorId", artifact.VendorId);
        parameters.Add("CategoryId", artifact.CategoryId);
        parameters.Add("EraId", artifact.EraId);
        parameters.Add("Availability", artifact.Availability);

        return parameters;
    }

    public static DynamicParameters GetVendorUpdateParameters(VendorModel vendor)
    {
        var parameters = new DynamicParameters();

        parameters.Add("Id", vendor.Id);
        parameters.Add("OwnerUserId", vendor.OwnerUserId);
        parameters.Add("Name", vendor.Name);
        parameters.Add("Description", vendor.Description);
        parameters.Add("ImageId", vendor.ImageId);
        parameters.Add("DateFounded", vendor.DateFounded);

        return parameters;
    }

}
