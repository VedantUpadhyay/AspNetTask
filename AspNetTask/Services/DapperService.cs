using AspNetTask.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace AspNetTask.Services
{
    public class DapperService : IDapperService
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString = string.Empty;

        public DapperService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = this.configuration.GetConnectionString("DefaultConnection");  
        }

        public async Task<bool> AddAthlete(int TestId, int playerId, string result)
        {
            string procName = "AddAthlete";

            using var sqlconn = new SqlConnection(connectionString);

            return (await sqlconn.ExecuteAsync(procName, new { 
                testId = TestId,
                playerId = playerId,
                result = result
            },commandType: System.Data.CommandType.StoredProcedure)) > 0;

            
        }

        public async Task<bool> CreateTest(TestType testType, DateTime DateAM)
        {

            using var sqlConnection = new SqlConnection(connectionString);
            string procName = "AddTest";

            int rows = await sqlConnection.ExecuteAsync(
                    procName,
                    new
                    {
                        testType = testType.ToString(),
                        dateAM = DateAM
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

            return rows > 0;
        }

        public async Task<bool> DeleteAthleteFromTest(int SpecificTestId)
        {
            string sqlCommand = "delete from SpecificTest where SpecificTestId = @Id";

            using var sqlConn = new SqlConnection(connectionString);
            int rows = await sqlConn.ExecuteAsync(sqlCommand, new
            {
                Id = SpecificTestId
            });

            return rows > 0;
        }

        public async Task<bool> DeleteTest(int TestId)
        {
            string procName = "deleteTest";

            using var sqlConn = new SqlConnection(connectionString);

            int rows = await sqlConn.ExecuteAsync(procName,
                new
                {
                    testId = TestId
                }
                , commandType: System.Data.CommandType.StoredProcedure);

            return rows > 0;
        }

        public async Task<bool> DoesAthleteExist(int specificId)
        {
            string sqlCmd = "select count(*) from SpecificTest where SpecificTestId = @Id";

            using var sqlConn = new SqlConnection(connectionString);
            int count = await sqlConn.ExecuteScalarAsync<int>(sqlCmd, new
            {
                Id = specificId
            });

            return count > 0;
           
        }

        public async Task<IEnumerable<Test>> GetAllTests()
        {
            using var sqlConn = new SqlConnection(connectionString);

            string procName = "GetAllTest";

            return await sqlConn.QueryAsync<Test>(procName, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<List<Athlete>> GetAthletes()
        {
            string sql = "select * from Athlete";

            using var sqlconn = new SqlConnection(connectionString);

            return (await sqlconn.QueryAsync<Athlete>(sql)).ToList();
        }

        public async Task<IEnumerable<SpecificTest>> GetTestDetails(int testId)
        {
            string procName = "GetTestDetails";

            using var sqlConnection = new SqlConnection(connectionString);
            IEnumerable<SpecificTest> resultSet;
            try
            {
                 resultSet = await sqlConnection.QueryAsync<SpecificTest>(procName, new { testId = testId }, commandType: System.Data.CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {   
                throw;
            }

            return resultSet;

            
        }

        public async Task<TestType> GetTestType(int TestId)
        {
            string sqlCmd = "select TestType from Test where TestId = @Id";

            using var sqlConn = new SqlConnection(connectionString);

            string Type = await sqlConn.ExecuteScalarAsync<string>(sqlCmd, new
            {
                Id = TestId
            });
            return (TestType)Enum.Parse(typeof(TestType), Type);
        }

        public async Task<bool> UpdateAthlete(int specificTestId, int playerId, string result, int testId)
        {
            string procName = "UpdateAthlete";

            using var sqlConn = new SqlConnection(connectionString);



            return (await sqlConn.ExecuteAsync(procName,new
            {
                specificTestId = specificTestId,
                playerId = playerId,
                result = result,
                testId = testId
            }, commandType: System.Data.CommandType.StoredProcedure)) > 0;

            
        }
    }
}
