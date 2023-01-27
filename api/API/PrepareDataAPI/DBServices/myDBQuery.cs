using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using System.Transactions;
using Cassandra;
using MySqlConnector;
using PrepareDataAPI.Models;

namespace PrepareDataAPI.DBService
{
    public class myDBQuery
    {
        public myDBConnect _context { get; }
        public cassandraDBConnect _cassandraContext { get; }

        public myDBQuery(myDBConnect context, cassandraDBConnect cassandraContext)
        {
            _context = context;
            _cassandraContext = cassandraContext;
        }
        private async Task<List<Level>> ReadAllAsync(DbDataReader reader)
        {
            var levels = new List<Level>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var level = new Level()
                    {
                        Levelid = reader.GetInt32("levelid"),
                        Levelname = reader.GetString("levelname"),
                        Superlevelid = reader.GetInt32("superlevelid")
                    };
                    levels.Add(level);
                }
            }
            return levels;
        }

        public async Task CreateLevelInfoAsync()
        {
            string levelList = "";
            await _context.Connection.OpenAsync();
            using var txnLevel = await _context.Connection.BeginTransactionAsync();
            using var cmdLevel = _context.Connection.CreateCommand();
            {
                cmdLevel.CommandText = @"select * from level";
                cmdLevel.Transaction = txnLevel;
                var resultLevel = await ReadAllAsync(await cmdLevel.ExecuteReaderAsync());
                foreach (var item in resultLevel)
                {
                    levelList += item.Levelname + ',';
                }
            }
            levelList = levelList.TrimEnd(',');
            await txnLevel.CommitAsync();

            using var txnProc = await _context.Connection.BeginTransactionAsync();
            using var cmdProc = _context.Connection.CreateCommand();
            {
                cmdProc.CommandText = @"call CreateLevelInfo('" + levelList + "')";
                cmdProc.Transaction = txnProc;
                await cmdProc.ExecuteNonQueryAsync();
            }
            await txnProc.CommitAsync();
            await _context.Connection.CloseAsync();
        }

        public async Task<List<dynamic>> GetResourceLevelInfoAsync()
        {
            List<dynamic> resultData = new List<dynamic>();
            try
            {
                await _context.Connection.OpenAsync();
                using var txnLevelInfo = await _context.Connection.BeginTransactionAsync();
                using var cmdLevelInfo = _context.Connection.CreateCommand();
                cmdLevelInfo.CommandText = @"call GetResourceLevelInfo()";
                cmdLevelInfo.Transaction = txnLevelInfo;
                var readerLevelInfo = await cmdLevelInfo.ExecuteReaderAsync();

                while (await readerLevelInfo.ReadAsync())
                {
                    resultData.Add(GetDynamicData(readerLevelInfo));
                }
                await _context.Connection.CloseAsync();
            }
            catch (Exception ex)
            {
                resultData.Add(ex.Source + ": " + ex.Message);
                //throw;
            }         
            return resultData;
        }

        private dynamic GetDynamicData(DbDataReader reader)
        {
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                expandoObject.Add(reader.GetName(i), reader[i]);
            }
            return expandoObject;
        }

        public async Task CreateAzureCostAsync()
        {
            string sql = @"CREATE TABLE azuredata.azurecost(resourceid text PRIMARY KEY, resourcename text, resourcetype text, resourcegroupname text, subscriptionid text, region text, usagedate timestamp, projectname text, projectownereail text, cost decimal";
            string dropSql = @"drop table if exists azuredata.azurecost";
            await _context.Connection.OpenAsync();
            using var txnLevel = await _context.Connection.BeginTransactionAsync();
            using var cmdLevel = _context.Connection.CreateCommand();
            {
                cmdLevel.CommandText = @"select * from level";
                cmdLevel.Transaction = txnLevel;
                var resultLevel = await ReadAllAsync(await cmdLevel.ExecuteReaderAsync());
                foreach (var item in resultLevel)
                {
                    sql += ", " + item.Levelname + " text";
                }
            }
            sql = sql.TrimEnd();
            sql += ')'; ;
            await _context.Connection.CloseAsync();

            using (var session = _cassandraContext.Connection.Connect())
            {
                var dropQuery = new SimpleStatement(dropSql);
                await session.ExecuteAsync(dropQuery);
                var query = new SimpleStatement(sql);
                await session.ExecuteAsync(query);
            }
        }
        /// <summary>
        /// This method is only temporary to showcase in Power BI
        /// </summary>
        /// <returns></returns>
        public async Task<List<dynamic>> SelectAzureCostAsync()
        {
            List<dynamic> resultData = new List<dynamic>();
            try
            {
                string sql = @"select * from azuredata.azurecost";
                using (var session = _cassandraContext.Connection.Connect())
                {
                    var query = new SimpleStatement(sql);
                    var rs = await session.ExecuteAsync(query);
                    foreach (var row in rs)
                    {
                        resultData.Add(GetCassandraData(row));
                    }
                }
            }
            catch (Exception ex)
            {
                resultData.Add(ex.Source + ": " + ex.Message);
                //throw;
            }
            
            return resultData;
        }

        /// <summary>
        /// This method is only temporary to showcase in Power BI
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private dynamic GetCassandraData(Row reader)
        {
            string[] colName = { "resourceid", "bu", "cost", "costcenter", "division", "projectname", "projectowneremail", "region", "resourcegroupname", "resourcename", "resourcetype", "subscriptionid", "usagedate" };
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;
            for (int i = 0; i < reader.Length; i++)
            {
                expandoObject.Add(colName[i], reader[i]);
            }
            return expandoObject;
        }
    }
}
