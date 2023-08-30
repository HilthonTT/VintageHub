namespace VintageHub.Client.Pages;

public partial class Listing
{
    private CategoryModel selectedCategory;
    private EraModel selectedEra;
    private List<CategoryModel> categories;
    private List<ArtifactDisplayModel> artifacts;
    private List<EraModel> eras;
    private Rating selectedRating = Rating.ZeroStar;
    private List<Rating> ratings = new()
    {
        Rating.ZeroStar,
        Rating.OneStar,
        Rating.TwoStar,
        Rating.ThreeStar,
        Rating.FourStar,
        Rating.FiveStar,
    };
    private string searchText = "";
    private bool showAllCategories = false;
    private bool showAllEras = false;
    private bool sortByPrice = false;
    private bool isLoading = true;
    private bool filterDiscounts = false;
    protected override async Task OnInitializedAsync()
    {
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
        var output = await ArtifactEndpoint.GetAllArtifactsAsync();
        return output.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Name);
    }

    private async Task LoadFilterStateAsync()
    {
        var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
        var queryParameters = HttpUtility.ParseQueryString(uri.Query);
        searchText = queryParameters["searchText"] is not null 
            ? searchText = queryParameters["searchText"] 
            : searchText = await SessionStorage.GetItemAsStringAsync(nameof(searchText)) ?? "";

        selectedRating = queryParameters["rating"] is not null 
            ? Enum.TryParse(queryParameters["rating"], out Rating parsedRating) 
                ? parsedRating : Rating.ZeroStar 
                : await SessionStorage.GetItemAsync<Rating>(nameof(selectedRating));

        selectedEra = queryParameters["eraId"] is not null 
            ? selectedEra = await EraEndpoint.GetEraByIdAsync(int.Parse(queryParameters["eraId"])) 
            : selectedEra = await SessionStorage.GetItemAsync<EraModel>(nameof(selectedEra)) ?? null;

        selectedCategory = queryParameters["categoryId"] is not null 
            ? selectedCategory = await CategoryEndpoint.GetCategoryByIdAsync(int.Parse(queryParameters["categoryId"])) 
            : selectedCategory = await SessionStorage.GetItemAsync<CategoryModel>(nameof(selectedCategory)) ?? null;

        sortByPrice = queryParameters["sortByPrice"] is not null 
            ? sortByPrice = bool.Parse(queryParameters["sortByPrice"]) 
            : sortByPrice = await SessionStorage.GetItemAsync<bool>(nameof(sortByPrice));

        filterDiscounts = queryParameters["filterDiscounts"] is not null 
            ? filterDiscounts = bool.Parse(queryParameters["filterDiscounts"]) 
            : filterDiscounts = await SessionStorage.GetItemAsync<bool>(nameof(filterDiscounts));
    }

    private async Task SaveFilterStateAsync()
    {
        await SessionStorage.SetItemAsStringAsync(nameof(searchText), searchText ?? "");
        await SessionStorage.SetItemAsync(nameof(selectedEra), selectedEra);
        await SessionStorage.SetItemAsync(nameof(selectedCategory), selectedCategory);
        await SessionStorage.SetItemAsync(nameof(selectedRating), selectedRating);
        await SessionStorage.SetItemAsync(nameof(sortByPrice), sortByPrice);
    }

    private async Task FilterArtifactsAsync()
    {
        var output = await ArtifactEndpoint.GetAllArtifactsAsync();
        if (string.IsNullOrWhiteSpace(searchText)is false)
        {
            output = output.Where(a => a.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        if (filterDiscounts)
        {
            output = output.Where(a => a.DiscountAmount > 0).ToList();
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
            ? output.OrderByDescending(a => a.Price).ThenByDescending(a => a.Rating).ToList() 
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

    private async Task OnFilterButtonClickAsync(bool isPrice)
    {
        sortByPrice = isPrice;
        await FilterArtifactsAsync();
    }

    private async Task OnRatingClickAsync(Rating rating)
    {
        selectedRating = rating;
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