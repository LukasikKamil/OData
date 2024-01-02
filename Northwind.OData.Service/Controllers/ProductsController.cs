using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ALLinONE.Shared;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Northwind.OData.Service.Controllers;

public class ProductsController : ODataController
{
    protected readonly NorthwindContext db;

    public ProductsController(NorthwindContext db)
    {
        this.db = db;
    }
    
    [EnableQuery]
    public IActionResult Get(string version = "1")
    {
        Console.WriteLine($"*** ProductsController version {version}.");
        return Ok(db.Products);
    }

    [EnableQuery]
    public IActionResult Get(int key, string version = "1")
    {
        Console.WriteLine($"*** ProductsController version {version}.");

        IQueryable<Product> products = db.Products.Where(product => product.ProductId == key);

        Product? p = products.FirstOrDefault();

        if ((products is null) || (p is null)) 
        {
            return NotFound($"Product with id {key} not found.");
        }

        if (version == "2")
        {
            p.ProductName += " version 2.0";
        }

        return Ok(p);
    }

    public IActionResult Post([FromBody] Product product) 
    {
        db.Products.Add(product);
        db.SaveChanges();
        return Created(product);
    }


    public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Product product)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if(key != product.ProductId)
        {
            return BadRequest();
        }

        db.Entry(product).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return Updated(product);
    }




    /*
    public async Task<IActionResult> Put([FromBody] Product product)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingProduct = await db.Products.FindAsync(product.ProductId);
        if(existingProduct == null)
        {
            return NotFound();
        }

        db.Entry(existingProduct).CurrentValues.SetValues(product);

        try
        {
            await db.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException)
        {
            if(!ProductExists(product.ProductId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }
    */

    public async Task<IActionResult> Delete([FromODataUri] int key)
    {
        var product = await db.Products.FindAsync(key);
        if(product == null)
        {
            return NotFound();
        }

        db.Products.Remove(product);
        await db.SaveChangesAsync();

        return StatusCode((int)HttpStatusCode.NoContent);
    }

    private bool ProductExists(int key)
    {
        return db.Products.Any(p => p.ProductId == key);
    }
}
