using Common.Utilities.CustomAttributes;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using dotnetstarter.organisations.common.DataObjects;

namespace dotnetstarter.organisations.api.Application.Queries
{
    public class RoleQueries : IRoleQueries
    {
        private string _connectionString = string.Empty;

        public RoleQueries(IConfiguration configuration)
        {
            _connectionString = configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:ConnectionStrings:OrganisationsConnection"];
            _connectionString = !string.IsNullOrWhiteSpace(_connectionString) ? $"{_connectionString};Connection Timeout=30" : throw new ArgumentNullException(nameof(_connectionString));
        }

        private async Task<List<T>> ExecuteQuery<T>(string sqlQuery)
        {
            try
            {
                var res = new List<T>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var result = await connection.QueryAsync<T>(sqlQuery);

                    res = result.AsList();

                    connection.Close();

                }
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DTORole>> GetAllRolesAsync()
        {
            try
            {
                var res = new List<DTORole>();

                var table = DatatableAttributeHelper.BuildFromType(typeof(DTORole));

                var queryStr = "SELECT ";

                table.columnDefinitions.ForEach(c =>
                {
                    queryStr += $" {c.dbName} as '{c.data}',";
                });

                queryStr = $"{queryStr.TrimEnd(',')} FROM dbo.Roles ";

                res = await ExecuteQuery<DTORole>(queryStr);


                return res;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
