namespace Server.Library.DataAccess.Helpers;
public static class ParameterHelper
{
    public static DynamicParameters GetIdParameters(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        return parameters;
    }
}
