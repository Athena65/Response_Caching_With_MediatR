using Core.Abstractions;
using Core.Models;
using MediatR;

namespace Core.Features.Customers.Queries
{
    public class GetCustomerQuery : IRequest<Customer>, ICacheableMediatrQuery
    {
        public int Id { get; set; } 
        public bool BypassCache { get; set; }

        public string CacheKey => $"Customer-{Id}";

        public TimeSpan? SlidingExpiration { get; set; }    
    }
    internal class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Customer>
    {
        private readonly ICustomerService _customerService;

        public GetCustomerQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public async Task<Customer> Handle(GetCustomerQuery request,CancellationToken cancellationToken)
        {
            return _customerService.GetCustomer(request.Id);
            
        }
    }
}
