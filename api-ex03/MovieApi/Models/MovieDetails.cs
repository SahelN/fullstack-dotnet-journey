using Microsoft.EntityFrameworkCore;

namespace MovieApi.Models
{
    public class MovieDetails
    {
        public int Id { get; set; }
        public string Synopsis { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;

        [Precision(18, 2)]
        public decimal Budget { get; set; }
        public int MovieId { get; set; } // FK
        public Movie Movie { get; set; } = null!;   // navigation
    }
}
