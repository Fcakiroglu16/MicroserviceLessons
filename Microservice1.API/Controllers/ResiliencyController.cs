using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Microservice1.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResiliencyController : ControllerBase
{
    private readonly AppDbContext _context;

    public ResiliencyController(AppDbContext context)
    {
        _context = context;
    }

    public async  Task<IActionResult> Get()
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        strategy.Execute(
            () =>
            {
                using var transaction = _context.Database.BeginTransaction();
                _context.Products.Add(new Product { Name = "Pen 1" });
                _context.SaveChanges();
                _context.Products.Add(new Product { Name = "Pen 2" });
                _context.SaveChanges();
                transaction.Commit();
            });


        //this is better than above
       await  ResilientTransaction.Create(_context).Execute(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            _context.Products.Add(new Product { Name = "Pen 1" });
            await _context.SaveChangesAsync();
            _context.Products.Add(new Product { Name = "Pen 2" });
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();


        });


        return Ok();
    }
}