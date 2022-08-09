using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace MMS.Data.Models
{
    public class Review
    {     
        public int Id { get; set; }      

        // name of reviewer
        public string Name { get; set; }   

        // date review was made        
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        // reviewer comments
        public string Comment { get; set; }

        // value between 1-10
        [Range(1, 10)]
        public int Rating { get; set; }
    
        // EF Dependant Relationship Review belongs to a Movie
        public int MovieId { get; set; }

        // Navigation property
        [JsonIgnore]
        public Movie Movie { get; set; }
 
    }
}