using AspNetTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;   

namespace AspNetTask.Services
{
    public interface IDapperService
    {
        Task<bool> UpdateAthlete(int specificTestId, int playerId, string result,int testId);

        Task<IEnumerable<Test>> GetAllTests();

        Task<List<Athlete>> GetAthletes();

        Task<IEnumerable<SpecificTest>> GetTestDetails(int testId);

        Task<bool> CreateTest(TestType testType,DateTime DateAM);

        Task<bool> AddAthlete(int TestId, int playerId, string result);

        Task<bool> DeleteTest(int TestId);

        Task<bool> DeleteAthleteFromTest(int SpecificTestId);

        Task<TestType> GetTestType(int TestId);

        Task<bool> DoesAthleteExist(int specificId);
    }
}
