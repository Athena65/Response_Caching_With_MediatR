using Core.Abstractions;
using Core.Models;
using MediatR;

namespace Core.Features.Customers.Queries
{
    public class GetCustomerListQuery : IRequest<List<Customer>>, ICacheableMediatrQuery
    {
        public bool BypassCache { get; set; }

        public string CacheKey => $"CustomerList";

        public TimeSpan? SlidingExpiration { get; set; }    
    }

    internal class GetCustomerListQueryHandler:IRequestHandler<GetCustomerListQuery,List<Customer>>
    {
        private readonly ICustomerService _customerService;

        public GetCustomerListQueryHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public async Task<List<Customer>> Handle(GetCustomerListQuery request,  CancellationToken cancellationToken)
        {
            return _customerService.GetCustomerList().ToList();
        }
    }
}
