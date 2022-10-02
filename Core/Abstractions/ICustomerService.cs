using Core.Models;

namespace Core.Abstractions
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetCustomerList();
        Customer GetCustomer(int id);   
    }
}
