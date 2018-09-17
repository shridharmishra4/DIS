using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace DIS.Models
{
    public class DataAccess
    {

        public  IConfiguration _configuration;
        MongoClient _client;
        IMongoDatabase _db;
        public IMongoCollection<Files> collection;

        public DataAccess(IConfiguration configuration)
        {

            _configuration = configuration;
            //_client = new MongoClient("mongodb://localhost:27017");
            _client = new MongoClient(_configuration.GetConnectionString("MongoClient"));
            _db = _client.GetDatabase(_configuration.GetConnectionString("DbName"));
            collection = _db.GetCollection<Files>(_configuration.GetConnectionString("CollectionName"));

        }
        public IEnumerable<Files> GetAllFiles()

        {
            return collection.Find(FilterDefinition<Files>.Empty).Project<Files>(
            "{FileName:1}").ToList();

            //return _db.GetCollection<Files>("Files").Find(_ => true).ToList();

        }

        public Files GetFile(string FileName)
        {
            //var res = Query<Files>.EQ();
            return collection.Find(p => p.FileName == FileName).FirstOrDefault();
        }
        public Files Create(Files p)
        {
            collection.InsertOne(p);
            return p;
        }
        //public void Update(ObjectId id, Files p)
        //{
        //    p.Id = id;
        //    var res = Query<Files>.EQ(pd => pd.Id, id);
        //    var operation = Update<Files>.Replace(p);
        //    _db.GetCollection<Files>("Files").Update(res, operation);
        //}
        //public void Remove(ObjectId id)
        //{
        //    var res = Query<Files>.EQ(e => e.Id, id);
        //    var operation = _db.GetCollection<Files>("Files").Remove(res);
        //}
    }
}