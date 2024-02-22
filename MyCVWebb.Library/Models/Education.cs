using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCVWebb.Library.Models
{
    public class Education
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string SchoolName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Year { get; set; }
        public string Description { get; set; }
    }

}
