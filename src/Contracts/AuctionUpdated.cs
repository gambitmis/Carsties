using System;

namespace Contracts;

public class AuctionUpdated
{
    public String Id { get; set; }
    public String Make { get; set; }
    public String Model { get; set; }
    public int? Year { get; set; }
    public string Color { get; set; }
    public int? Mileage { get; set; }

}
