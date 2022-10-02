using Core.Features.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            try
            {
                return Ok(await _mediator.Send(new GetCustomerQuery { Id = id, BypassCache = false }));
            }
            catch (Exception ex)
            {
                var response = new ServiceResponse();
                response.Success = false;
                response.Message = ex.Message;
                return BadRequest(response); 
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                return Ok(await _mediator.Send(new GetCustomerListQuery { BypassCache = false }));
            }
            catch (Exception ex)
            {
                var response = new ServiceResponse();
                response.Success = false;
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }
    }
}
