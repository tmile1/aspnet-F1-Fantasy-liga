using F1_Fantasy_liga.Models;

namespace F1_Fantasy_liga.Repositories
{
    public class UserMockRepository
    {
        private readonly MockDataStore _mockDataStore;

        public UserMockRepository(MockDataStore mockDataStore)
        {
            _mockDataStore = mockDataStore;
        }

        public List<User> GetAll()
        {
            return _mockDataStore.Users;
        }

        public User? GetById(int id)
        {
            return _mockDataStore.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}