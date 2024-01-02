using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ALLinONE.Shared;

namespace Northwind.OData.Service.Controllers;

public class CustomersController : ODataController
{
    protected readonly NorthwindContext db;

    public CustomersController(NorthwindContext db)
    {
        this.db = db;
    }
    
    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(db.Customers);
    }

    [EnableQuery]
    public IActionResult Get(string key)
    {
        return Ok(db.Customers.Where(customer => customer.CustomerId == key));
    }
}
