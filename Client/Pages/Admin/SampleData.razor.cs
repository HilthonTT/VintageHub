namespace VintageHub.Client.Pages.Admin;

public partial class SampleData
{
    private UserModel loggedInUser;
    private string errorMessage = "";
    private bool isBusy = false;
    private bool categoriesCreated = false;
    private bool erasCreated = false;
    private bool artifactsCreated = false;
    private bool vendorsCreated = false;
    private int createdCategoriesCount = 0;
    private int createdErasCount = 0;
    private int createdArtifactsCount = 0;
    private int createdVendorsCount = 0;
    private List<CategoryModel> categories = new()
    {
        new CategoryModel
        {
            Name = "Jewelry and Accessories",
            Description = "Explore a stunning collection of vintage jewelry and accessories that never go out of style.",
        },
        new CategoryModel
        {
            Name = "Home Decor",
            Description = "Transform your living space with exquisite vintage home decor items that bring a touch of nostalgia.",
        },
        new CategoryModel
        {
            Name = "Collectibles",
            Description = "Discover unique vintage collectibles that tell stories of the past and enrich your collection.",
        },
        new CategoryModel
        {
            Name = "Fashion and Apparel",
            Description = "Step into the past with our curated selection of vintage fashion and apparel items.",
        },
        new CategoryModel
        {
            Name = "Art and Paintings",
            Description = "Experience the beauty of vintage art and paintings that capture the essence of different eras.",
        },
        new CategoryModel
        {
            Name = "Ceramics and Glassware",
            Description = "Enhance your living space with timeless ceramics and glassware pieces from bygone days.",
        },
        new CategoryModel
        {
            Name = "Watches and Clocks",
            Description = "Explore vintage timepieces that stand as a testament to the craftsmanship of yesteryears.",
        },
        new CategoryModel
        {
            Name = "Technology and Gadgets",
            Description = "Revisit the innovations of the past with our collection of vintage technology and gadgets.",
        },
        new CategoryModel
        {
            Name = "Automobile and Transportation",
            Description = "Experience the elegance of vintage automobiles and transportation modes that shaped history.",
        },
        new CategoryModel
        {
            Name = "Sports and Recreation",
            Description = "Rediscover the joy of classic sports and recreational activities through our vintage offerings.",
        },
    };
    private List<EraModel> eras = new()
    {
        new EraModel
        {
            Name = "Victorian Era",
            Description = "Experience the opulence and intricacy of the Victorian era artifacts.",
        },
        new EraModel
        {
            Name = "Roaring Twenties",
            Description = "Step back into the vibrant and glamorous Roaring Twenties with our curated artifacts.",
        },
        new EraModel
        {
            Name = "Art Deco Period",
            Description = "Discover the sleek and stylish artifacts from the iconic Art Deco period.",
        },
        new EraModel
        {
            Name = "Renaissance Era",
            Description = "Immerse yourself in the cultural revival of the Renaissance era with our collection.",
        },
        new EraModel
        {
            Name = "Ancient Civilizations",
            Description = "Explore artifacts from ancient civilizations that laid the foundation for our world.",
        },
        new EraModel
        {
            Name = "Industrial Revolution",
            Description = "Witness the transformative impact of the Industrial Revolution through our artifacts.",
        },
        new EraModel
        {
            Name = "Space Age",
            Description = "Journey through the Space Age with artifacts that capture the spirit of exploration.",
        },
        new EraModel
        {
            Name = "Medieval Period",
            Description = "Discover artifacts that evoke the essence of the medieval times and chivalry.",
        },
    };
    protected override async Task OnInitializedAsync()
    {
        loggedInUser = await AuthProvider.GetUserFromAuth(UserEndpoint);
    }

    private async Task GenerateCategoriesAsync()
    {
        if (isBusy)
        {
            return;
        }

        try
        {
            isBusy = true;
            foreach (var cat in categories)
            {
                await CategoryEndpoint.InsertCategoryAsync(cat);
                createdCategoriesCount++;
            }

            categoriesCreated = true;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
        finally
        {
            isBusy = false;
        }
    }

    private async Task GenerateErasAsync()
    {
        if (isBusy)
        {
            return;
        }

        errorMessage = "";
        isBusy = true;
        foreach (var era in eras)
        {
            await EraEndpoint.InsertEraAsync(era);
            createdErasCount++;
        }

        erasCreated = true;
        isBusy = false;
    }

    private async Task GenerateVendorsAsync()
    {
        if (isBusy)
        {
            return;
        }

        try
        {
            var vendors = new List<VendorModel>()
            {
                new VendorModel
                {
                    OwnerUserId = 1,
                    Name = "Vintage Treasures Emporium",
                    Description = "A haven for vintage enthusiasts, offering a wide range of artifacts from various eras.",
                    ImageId = "",
                    DateFounded = new DateTime(2005, 7, 15),
                },
                new VendorModel
                {
                    OwnerUserId = 1,
                    Name = "Nostalgia Collectibles Co.",
                    Description = "Curating collectible artifacts that capture the essence of times gone by.",
                    ImageId = "",
                    DateFounded = new DateTime(2010, 3, 22),
                },
            };
            errorMessage = "";
            isBusy = true;
            foreach (var v in vendors)
            {
                await VendorEndpoint.InsertVendorAsync(v);
                createdVendorsCount++;
            }

            vendorsCreated = true;
            isBusy = false;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
        finally
        {
            isBusy = false;
        }
    }

    private async Task GenerateArtifactAsync()
    {
        if (isBusy)
        {
            return;
        }

        try
        {
            var vendors = await VendorEndpoint.GetAllVendorsAsync();
            var eras = await EraEndpoint.GetAllErasAsync();
            var categories = await CategoryEndpoint.GetAllCategoriesAsync();
            var artifacts = new List<ArtifactModel>()
            {
                new ArtifactModel
                {
                    Name = "Vintage Pearl Necklace",
                    Description = "Elegant and timeless pearl necklace from the early 20th century.",
                    ImageId = "",
                    Quantity = 5,
                    Rating = 4.8,
                    Price = 250.99m,
                    VendorId = vendors[0].Id,
                    CategoryId = categories[0].Id,
                    EraId = eras[0].Id,
                    Availability = true,
                },
                new ArtifactModel
                {
                    Name = "Antique Wall Clock",
                    Description = "Charming wall clock with intricate woodwork and brass details.",
                    ImageId = "",
                    Quantity = 1,
                    Rating = 4.5,
                    Price = 120.50m,
                    VendorId = vendors[1].Id,
                    CategoryId = categories[1].Id,
                    EraId = eras[4].Id,
                    Availability = true,
                },
            };
            errorMessage = "";
            isBusy = true;
            foreach (var a in artifacts)
            {
                await ArtifactEndpoint.InsertArtifactAsync(a);
                createdArtifactsCount++;
            }

            artifactsCreated = true;
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }
        finally
        {
            isBusy = false;
        }
    }

    private bool DisableButton(bool isCreated)
    {
        if (isBusy || isCreated)
        {
            return true;
        }

        return false;
    }
}