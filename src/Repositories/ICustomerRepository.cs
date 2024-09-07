using CustomerLeaderboard.Api.Models;

namespace CustomerLeaderboard.Api.Repositories
{
    public interface ICustomerRepository
    {
        Customer GetCustomer(long customerId);
        void AddOrUpdateCustomer(Customer customer);
        void RemoveCustomer(long customerId);
        IEnumerable<Customer> GetAllCustomers();
    }
}
