using DataAccess.DAO;
using DataAccess.Interfaces;
using DataAccess.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IFCustomerDao _customerDao;
        public CustomersController(IFCustomerDao customerDao)
        {
            _customerDao = customerDao; 
        }

        // Opretter en ny kunde
        [HttpPost]
        public IActionResult CreateCustomer([FromBody] Customer customer)
        {
            int customerId = _customerDao.CreateCustomer(customer);

            return Ok(customerId);
        }

        // Henter en kunde ud fra ID
        [HttpGet("{customerId}")]
        public IActionResult GetCustomerById(int customerId)
        {
            Customer? customer = _customerDao.GetCustomerById(customerId);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }
    }
}