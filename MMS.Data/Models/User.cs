using System;
using System.ComponentModel.DataAnnotations;

namespace MMS.Data.Models
{
    public enum Role { admin, guest }

    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public Role Role { get; set; }

    }
}