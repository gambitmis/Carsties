using System;

namespace AuctionService.DTOs;

public class UpdateAuctionDto
{
    public String Make { get; set; }
    public String Model { get; set; }
    public int? Year { get; set; }
    public string Color { get; set; }

    public int? Mileage { get; set; }

}
