namespace VintageHub.Client.Pages;

public partial class Vendor
{
    [Parameter]
    public int Id { get; set; }

    private UserModel loggedInUser;
    private VendorDisplayModel vendor;
    private List<ArtifactDisplayModel> artifacts;
    private List<CategoryModel> categories;
    private List<EraModel> eras;
    private CategoryModel selectedCategory;
    private EraModel selectedEra;
    private Rating selectedRating = Rating.ZeroStar;
    private string imageSource = "";
    private string searchText = "";
    private bool sortByPrice = false;
    private bool showAllCategories = false;
    private bool showAllEras = false;
    private bool isLoading = true;
    private List<Rating> ratings = new()
    {
        Rating.ZeroStar,
        Rating.OneStar,
        Rating.TwoStar,
        Rating.ThreeStar,
        Rating.FourStar,
        Rating.FiveStar,
    };

    protected override async Task OnInitializedAsync()
    {
        loggedInUser = await AuthProvider.GetUserFromAuth(UserEndpoint);
        vendor = await VendorEndpoint.GetVendorByIdAsync(Id);
        if (vendor is not null)
        {
            imageSource = ImageEndpoint.GetImage(vendor.ImageId);
        }

        await LoadFilterStateAsync();
        await FilterArtifactsAsync();
        await LoadCategoriesAsync();
        await LoadErasAsync();
        isLoading = false;
    }

    private async Task LoadCategoriesAsync()
    {
        var allCategories = await CategoryEndpoint.GetAllCategoriesAsync();
        categories = showAllCategories ? allCategories : allCategories.Take(5).ToList();
    }

    private async Task LoadErasAsync()
    {
        var allEras = await EraEndpoint.GetAllErasAsync();
        eras = showAllEras ? allEras : allEras.Take(5).ToList();
    }

    private async Task<IEnumerable<string>> SearchArtifactsAsync(string value)
    {
        searchText = value;
        var output = await ArtifactEndpoint.GetArtifactByVendorIdAsync(Id);

        return output.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Name);
    }

    private async Task LoadFilterStateAsync()
    {
        searchText = await SessionStorage.GetItemAsStringAsync(nameof(searchText)) ?? "";
        selectedRating = await SessionStorage.GetItemAsync<Rating>(nameof(selectedRating));
        var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
        var queryParameters = HttpUtility.ParseQueryString(uri.Query);

        selectedEra = queryParameters["eraId"] is not null
            ? await EraEndpoint.GetEraByIdAsync(int.Parse(queryParameters["eraId"]))
            : await SessionStorage.GetItemAsync<EraModel>(nameof(selectedEra)) ?? null;

        selectedCategory = queryParameters["categoryId"] is not null
            ? await CategoryEndpoint.GetCategoryByIdAsync(int.Parse(queryParameters["categoryId"]))
            : await SessionStorage.GetItemAsync<CategoryModel>(nameof(selectedCategory)) ?? null;
    }

    private async Task SaveFilterStateAsync()
    {
        await SessionStorage.SetItemAsStringAsync(nameof(searchText), searchText);
        await SessionStorage.SetItemAsync(nameof(selectedEra), selectedEra);
        await SessionStorage.SetItemAsync(nameof(selectedCategory), selectedCategory);
        await SessionStorage.SetItemAsync(nameof(selectedRating), selectedRating);
    }

    private async Task FilterArtifactsAsync()
    {
        var output = await ArtifactEndpoint.GetArtifactByVendorIdAsync(Id);
        if (string.IsNullOrWhiteSpace(searchText)is false)
        {
            output = output.Where(a => a.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        if (selectedEra is not null)
        {
            output = output.Where(a => a.Era.Id == selectedEra.Id).ToList();
        }

        if (selectedCategory is not null)
        {
            output = output.Where(a => a.Category.Id == selectedCategory.Id).ToList();
        }

        output = selectedRating switch
        {
            Rating.ZeroStar => output.Where(a => a.Rating >= 0).ToList(),
            Rating.OneStar => output.Where(a => a.Rating >= 1).ToList(),
            Rating.TwoStar => output.Where(a => a.Rating >= 2).ToList(),
            Rating.ThreeStar => output.Where(a => a.Rating >= 3).ToList(),
            Rating.FourStar => output.Where(a => a.Rating >= 4).ToList(),
            Rating.FiveStar => output.Where(a => a.Rating >= 5).ToList(),
            _ => output
        };

        output = sortByPrice 
            ?  output.OrderByDescending(a => a.Price).ThenByDescending(a => a.Rating).ToList() 
            : output.OrderByDescending(a => a.Rating).ThenByDescending(a => a.Price).ToList();

        artifacts = output;
        await SaveFilterStateAsync();
    }

    private async Task OnSearchInputAsync(string searchInput)
    {
        searchText = searchInput;
        if (string.IsNullOrWhiteSpace(searchInput))
        {
            searchText = "";
        }

        await FilterArtifactsAsync();
    }

    private async Task OnEraClickAsync(EraModel era)
    {
        selectedEra = era;
        await FilterArtifactsAsync();
    }

    private async Task OnCategoryClickAsync(CategoryModel category)
    {
        selectedCategory = category;
        await FilterArtifactsAsync();
    }

    private async Task OnRatingClickAsync(Rating rating)
    {
        selectedRating = rating;
        await FilterArtifactsAsync();
    }

    private async Task OnFilterButtonClickAsync(bool isPrice)
    {
        sortByPrice = isPrice;
        await FilterArtifactsAsync();
    }

    private async Task ToggleShowAllCategoriesAsync(bool showAll)
    {
        showAllCategories = showAll;
        await LoadCategoriesAsync();
    }

    private async Task ToggleShowAllErasAsync(bool showAll)
    {
        showAllEras = showAll;
        await LoadErasAsync();
    }

    private async Task SendDeleteRequestAsync()
    {
        if (IsOwner() is false)
        {
            Snackbar.Add("You do not have permission to delete the vendor.");
        }
        else
        {
            var parameters = new DialogParameters<DeleteVendor>
            {
                { x => x.Vendor, vendor },
                { x => x.LoggedInUser, loggedInUser }
            };

            await DialogService.ShowAsync<DeleteVendor>($"Delete Vendor {vendor?.Name}", parameters);
        }
    }

    private async Task SendEditRequestAsync()
    {
        if (IsOwner() is false)
        {
            Snackbar.Add("You do not have permission to delete the vendor.");
        }
        else
        {
            var parameters = new DialogParameters<EditVendor>
            {
                { x => x.Vendor, vendor },
                { x => x.LoggedInUser, loggedInUser }
            };

            await DialogService.ShowAsync<EditVendor>($"Delete Vendor {vendor?.Name}", parameters);
        }
    }

    private bool IsOwner()
    {
        if (loggedInUser?.Id == vendor?.Owner?.Id)
        {
            return true;
        }

        return false;
    }

    private void ClosePage()
    {
        Navigation.NavigateTo("/");
    }

    private void LoadListingPage()
    {
        Navigation.NavigateTo("/Listing");
    }

    private Color GetSelectedEraClass(EraModel era = null)
    {
        if (selectedEra?.Id == era?.Id)
        {
            return Color.Success;
        }

        return Color.Default;
    }

    private Color GetSelectedCategory(CategoryModel category = null)
    {
        if (selectedCategory?.Id == category?.Id)
        {
            return Color.Success;
        }

        return Color.Default;
    }

    private Color GetSelectedFiltering(bool isPrice)
    {
        if (sortByPrice == isPrice)
        {
            return Color.Success;
        }

        return Color.Primary;
    }
    private static int GetRatingValue(Rating rating)
    {
        return rating switch
        {
            Rating.ZeroStar => 0,
            Rating.OneStar => 1,
            Rating.TwoStar => 2,
            Rating.ThreeStar => 3,
            Rating.FourStar => 4,
            Rating.FiveStar => 5,
            _ => 0,
        };
    }

    private Color GetSelectedRating(Rating rating)
    {
        if (selectedRating == rating)
        {
            return Color.Success;
        }

        return Color.Default;
    }
}