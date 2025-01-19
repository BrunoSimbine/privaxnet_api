using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace privaxnet_api.Models;


public class Product : BaseEntity
{
    public Guid Id { get; set; }
    [NotMapped]
    public bool IsAvaliable
    {
        get
        {
            return !DateDeleted.HasValue;
        }
    }
    public string Name { get; set; }
    public string ImageUrl { get; set; } = "https://statics.privaxnet.com/img/8273498325795.png";
    public decimal Price { get; set; } 
    public int DurationDays { get; set; }
    public long DataAmount { get; set; } = 10;
}
