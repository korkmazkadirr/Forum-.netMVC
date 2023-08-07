using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Models
{
    public class LeadEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Adress { get; set; }

        public Gender Gender { get; set; }

        public DateTime Birthdate { get; set; }

        public City City { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }

    public enum Gender
    {
        Woman,
        Man
    }

    public enum City
    {
        Artvin,
        Giresun,
        Trabzon,
        Ordu,
        Düzce,
        Sakarya,
        Diğer
    }
}

