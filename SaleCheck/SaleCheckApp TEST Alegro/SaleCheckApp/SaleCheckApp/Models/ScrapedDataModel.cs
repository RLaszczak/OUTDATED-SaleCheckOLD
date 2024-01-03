using System.ComponentModel.DataAnnotations;

namespace PogodaApp.Models;

public class ScrapedDataModel
{
    [Required]
    public string Id { get; set; }
    public string AllegroName { get; set; }
    public string Price { get; set; }
    public string Link { get; set; }


}
