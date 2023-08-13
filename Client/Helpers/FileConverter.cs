using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace VintageHub.Client.Helpers;

public static class FileConverter
{
    public static IFormFile ToIFormFile(this IBrowserFile browserFile)
    {
        return new FormFile(
           browserFile.OpenReadStream(),
           0,
           browserFile.Size,
           browserFile.Name,
           browserFile.Name
       );
    }
}
