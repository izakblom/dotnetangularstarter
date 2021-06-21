using Common.DataObjects;
using Common.Utilities.CustomAttributes;
using Common.Utilities.Datatable;
using Dapper;
using dotnetstarter.organisations.common.DataObjects;
using dotnetstarter.organisations.common.DataObjects.DataTable;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.api.Application.Queries
{
    public class UserQueries : DatatableQuery, IUserQueries
    {
        private string _connectionString = string.Empty;
        private readonly IUserRepository _userRepository;

        public UserQueries(IConfiguration configuration, IUserRepository userRepository) : base()
        {
            _connectionString = configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:ConnectionStrings:OrganisationsConnection"];
            _connectionString = !string.IsNullOrWhiteSpace(_connectionString) ? $"{_connectionString};Connection Timeout=30" : throw new ArgumentNullException(nameof(_connectionString));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<List<DTODropdownOption>> GetCoEUsersForDropDown()
        {
            try
            {
                var result = await _userRepository.GetCoEUsersForDropDown();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<DTODropdownOption>> GetCoEUsersByCostCentreIdForDropDown(int costCentreId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var res = await connection.QueryAsync<DTODropdownOption>(
                        $"SELECT dbo.Users.Id AS 'Key' ,CONCAT(dbo.Users.FirstName + ' ',dbo.Users.LastName) AS 'VALUE'  FROM dbo.CostCentreCoeUsers LEFT JOIN dbo.Users  on dbo.Users.Id = dbo.CostCentreCoeUsers.CoeUserId WHERE dbo.CostCentreCoeUsers.CostCentreId = {costCentreId}"
                    );

                    connection.Close();

                    return res.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<DTODataTableResult<DTOUser>> GetUsersByFilter(DTOUsersDataTableFilter filter)
        {
            var res = new DTODataTableResult<DTOUser>();


            //res.totalRecordCount = await GetUsersCount();


            var queryStructure = DatatableAttributeHelper.BuildFromType(typeof(DTOUser));

            res.totalRecordCount = await GetUsersTotalCountByFilter(filter, queryStructure);

            foreach (var col in queryStructure.columnDefinitions)
                res.columns.Add(new DTODataTableColumnDefinition { orderable = col.orderable, propertyName = col.propertyName, title = col.title, visible = col.visible, searchable = col.searchable, searchOptionsURL = col.searchOptionsUrl });

            //var cols = new List<string>();

            var queryStr = "SELECT ";

            queryStructure.columnDefinitions.ForEach(c =>
            {
                queryStr += $" {c.dbName} as '{c.data}',";
            });

            queryStr = $"{queryStr.TrimEnd(',')} FROM dbo.Users ";




            queryStr += GetFilterClause(filter.filter, queryStructure);


            //group by 

            var groupBy = "Group By ";
            queryStructure.columnDefinitions.Where(c => !c.dbName.ToLower().Contains("count(")).ToList().ForEach(c =>
            {
                groupBy += $" {c.dbName},";
            });

            queryStr += groupBy.TrimEnd(',');


            //var orderBy = "";
            var orderBy = "";

            if (!String.IsNullOrEmpty(filter.sortColumnName))
            {
                var colName = queryStructure.columnDefinitions.Find(col => { return col.propertyName.Equals(filter.sortColumnName); }).dbName;
                orderBy = $" ORDER BY {colName} {filter.sortDirection} ";
            }
            else
            {
                orderBy = $" ORDER BY {queryStructure.columnDefinitions.ElementAt(0).dbName}";
            }



            queryStr += orderBy;



            queryStr += $" OFFSET {filter.skip} ROWS FETCH NEXT {filter.take} ROWS ONLY";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var result = await connection.QueryAsync<DTOUser>(queryStr);

                res.data = result.AsList();

                connection.Close();

            }



            //res.filteredRecordCount = await GetUsersTotalCountByFilter(WithStatusFilter, queryStructure);


            return res;
        }

        public async Task<int> GetUsersCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var res = await connection.ExecuteScalarAsync<int>(
                   $"SELECT COUNT(Id) AS Count FROM dbo.Users"
                    );

                connection.Close();

                return res;
            }
        }

        public async Task<int> GetUsersTotalCountByFilter(DTOUsersDataTableFilter filter, DTODatatablesStructure queryStructure)
        {
            var queryStr = "SELECT COUNT(DISTINCT(dbo.Users.Id)) ";

            queryStr = $"{queryStr.TrimEnd(',')} FROM dbo.Users ";

            

            queryStr += GetFilterClause(filter.filter, queryStructure);


            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var q = $@"{queryStr}";

                var res = await connection.ExecuteScalarAsync<int>(q);

                connection.Close();

                return res;
            }
        }


    }
}
