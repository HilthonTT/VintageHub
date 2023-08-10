﻿using System.ComponentModel.DataAnnotations;

namespace VintageHub.Server.Library.Models;
public class VendorModel
{
    public int Id { get; set; }

    [Required]
    public int OwnerUserId { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(256)]
    public string Description { get; set; }

    [Required]
    public DateTime DateFounded { get; set; }
}
