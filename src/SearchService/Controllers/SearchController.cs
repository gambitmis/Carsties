using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    //public async Task<ActionResult<List<Item>>> SearchItems(string searchTerm, int pageNumber = 1, int pageSize = 4) //before migrate to searchparam
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery]SearchParams searchParams) //after migrate to searchparam
    {
        //var query = DB.Find<Item>(); // This is the old way of querying
        var query = DB.PagedSearch<Item, Item>(); // add second Item on searchparam

        // query.Sort( x => x.Ascending( a => a.Make));

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        query = searchParams.OrderBy switch
        {
            "make" => query.Sort(x=> x.Ascending(a => a.Make)),
            "new" => query.Sort(x=> x.Descending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
        };

        query = searchParams.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6) 
                && x.AuctionEnd > DateTime.UtcNow),
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
        };

        if (!string.IsNullOrEmpty(searchParams.Seller))
        {
            query.Match(x => x.Seller == searchParams.Seller);
        }

        if (!string.IsNullOrEmpty(searchParams.Winner))
        {
            query.Match(x => x.Winner == searchParams.Winner);
        }

        query.PageNumber(searchParams.PageNumber); // adding for query page
        query.PageSize(searchParams.PageSize); // adding for query page

        var result = await query.ExecuteAsync();

        //return result; // This is the old way of returning
        return Ok(new 
        { 
            result = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount     
        });
    }

}