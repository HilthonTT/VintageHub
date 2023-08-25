﻿namespace VintageHub.Client.Models;

public class CreateArtifactModel
{
    [Required(ErrorMessage = "Please provide the artifact's name.")]
    [StringLength(100, ErrorMessage = "The artifact's name must not exceed 100 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please provide the artifact's description")]
    [StringLength(1000, ErrorMessage = "The artifact's description must not exceed 1000 characters")]
    public string Description { get; set; }

    [Display(Name = "Image")]
    public string ImageId { get; set; } = "";

    [Required(ErrorMessage = "Please provide the artifact's quantity")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Please provide the artifact's price.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Please provide the discount amount")]
    [Display(Name = "Discount Amount")]
    public decimal DiscountAmount { get; set; }

    [Required(ErrorMessage = "Please provide the vendor")]
    [Display(Name = "Vendor")]
    public int VendorId { get; set; }

    [Required(ErrorMessage = "Please provide the category")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Please provide the era")]
    [Display(Name = "Era")]
    public int EraId { get; set; }

    [Required(ErrorMessage = "Please specify availability")]
    public bool Availability { get; set; }
}
