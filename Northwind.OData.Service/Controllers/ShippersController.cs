using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ALLinONE.Shared;

namespace Northwind.OData.Service.Controllers;

public class ShippersController : ODataController
{
    protected readonly NorthwindContext db;

    public ShippersController(NorthwindContext db)
    {
        this.db = db;
    }

    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(db.Shippers);
    }

    [EnableQuery]
    public IActionResult Get(int key)
    {
        return Ok(db.Shippers.Where(shipper => shipper.ShipperId == key));
    }
}
