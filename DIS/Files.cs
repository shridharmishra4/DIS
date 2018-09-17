using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DIS.Models
{
    public class Files
    {
        public ObjectId Id { get; set; }

        //[BsonElement("FileName")]
        public string FileName { get; set; }

        [BsonElement("GeneratedAt")]
        public System.DateTime GeneratedAt { get; set; }

        [BsonElement("DocumentType")]
        public string DocumentType { get; set; }

        [BsonElement("Base64EncodedFile")]
        public string Base64EncodedFile { get; set; }

        public string HashCode { get; set; }

    }

    public class AllFilesDAO
    {

        //[BsonElement("FileName")]
        public string FileName { get; set; }


        //[BsonElement("GeneratedAt")]
        //public System.DateTime GeneratedAt { get; set; }
    }
}