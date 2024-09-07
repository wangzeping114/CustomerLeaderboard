using CustomerLeaderboard.Api.Models;
using System.Collections.Concurrent;

namespace CustomerLeaderboard.Api.Repositories
{
    public class InMemoryCustomerRepository : ICustomerRepository
    {
        private static readonly ConcurrentDictionary<long, Customer> _customers = new ConcurrentDictionary<long, Customer>();

        public Customer GetCustomer(long customerId)
        {
            _customers.TryGetValue(customerId, out var customer);
            return customer;
        }

        public void AddOrUpdateCustomer(Customer customer)
        {
            _customers[customer.CustomerId] = customer;
        }

        public void RemoveCustomer(long customerId)
        {
            _customers.TryRemove(customerId, out _);
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customers.Values;
        }
    }
}
