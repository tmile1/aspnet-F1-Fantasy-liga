using F1_Fantasy_liga.Models;

namespace F1_Fantasy_liga.Repositories
{
    public class FantasyTeamMockRepository
    {
        private readonly MockDataStore _mockDataStore;

        public FantasyTeamMockRepository(MockDataStore mockDataStore)
        {
            _mockDataStore = mockDataStore;
        }

        public List<FantasyTeam> GetAll()
        {
            return _mockDataStore.FantasyTeams;
        }

        public FantasyTeam? GetById(int id)
        {
            return _mockDataStore.FantasyTeams.FirstOrDefault(t => t.Id == id);
        }
    }
}