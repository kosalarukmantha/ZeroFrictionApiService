using ZeroFriction.DataAccessLayer.DataAccess;
using ZeroFriction.DataAccessLayer.DataStucture;
using ZeroFriction.DataAccessLayer.Model;
using ZeroFriction.DataAccessLayer.StandardData;
using ZeroFriction.DataAccessLayer.StandardData.Databases;
using ZeroFriction.Model.Models;
using ZeroFriction.Model.Request;
using ZeroFriction.Repository.IRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System.Net;

namespace ZeroFriction.Repository.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly string _scriptFile = "Invoice";
        private CosmosClient _cosmosClient;
        private Database _database;
        private Container _container;
        private CosmosDatabase _cosmosDatabase ;
        private string _databaseId;
        private string _containerId;
        private string _endpointUri;
        private string _primaryKey;
        public InvoiceRepository(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _cosmosDatabase = new CosmosDatabase(_loggerFactory);
            this._databaseId = _cosmosDatabase.GetDatabaseAsync();
            this._containerId = _cosmosDatabase.GetContainerAsync();
            this._endpointUri = _cosmosDatabase.GetEndPointAsync();
            this._primaryKey = _cosmosDatabase.GetPrimaryKeyAsync();
            
        }
        public async Task<string> SaveInvoice(InvoiceDto dto)
        {
            this._cosmosClient = new CosmosClient(_endpointUri, _primaryKey, new CosmosClientOptions() { ApplicationName = "ZeroFrictionApi" });
            await this.CreateDatabaseAsync();
            await this.CreateContainerAsync();
            return await Task.FromResult( await AddInvoiceToContainerAsync(dto));
        }

        public async Task<string> UpdateInvoice(InvoiceDto dto)
        {
            this._cosmosClient = new CosmosClient(_endpointUri, _primaryKey, new CosmosClientOptions() { ApplicationName = "ZeroFrictionApi" });
            await this.CreateDatabaseAsync();
            await this.CreateContainerAsync();
            return await Task.FromResult(await ReplaceFamilyItemAsync(dto));
            
        }

        // <CreateDatabaseAsync>
        /// <summary>
        /// Create the database if it does not exist
        /// </summary>
        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            this._database = await this._cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseId);
        }
        // </CreateDatabaseAsync>

        // <CreateContainerAsync>
        /// <summary>
        /// Create the container if it does not exist. 
        /// </summary>
        /// <returns></returns>
        private async Task CreateContainerAsync()
        {
            // Create a new container
            this._container = await this._database.CreateContainerIfNotExistsAsync(_containerId, "/partitionKey");
        }

        private async Task<string> AddInvoiceToContainerAsync(InvoiceDto dto)
        {

            try
            {
                // Read the item to see if it exists.  
                ItemResponse<InvoiceDto> invoice = await this._container.ReadItemAsync<InvoiceDto>(dto.id.ToString(), new PartitionKey(dto.partitionKey));
                return await Task.FromResult(" Item in database with id: already exists");
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item 
                ItemResponse<InvoiceDto> invoice = await this._container.CreateItemAsync<InvoiceDto>(dto, new PartitionKey(dto.partitionKey));
                return await Task.FromResult(" Created item in database with id");
            }

           
        }

        private async Task<string> ReplaceFamilyItemAsync(InvoiceDto dto)
        {
            try
            {
                ItemResponse<InvoiceDto> invoice = await this._container.ReadItemAsync<InvoiceDto>(dto.id.ToString(), new PartitionKey(dto.partitionKey));
                await this._container.ReplaceItemAsync<InvoiceDto>(dto, dto.InvoiceID.ToString(), new PartitionKey(dto.partitionKey));
                return await Task.FromResult(" Item in database with id: " + invoice.Resource.id + " Updated");

            }
            catch (Exception ex)
            {
                return await Task.FromResult(" Item in database with id: " + dto.InvoiceID + "Not Updated");
            }
        }
    }
}
