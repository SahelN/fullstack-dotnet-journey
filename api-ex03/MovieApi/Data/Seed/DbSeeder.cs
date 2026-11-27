using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Data.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(MovieContext context)
    {
        // Idempotency: if data already exists, do nothing
        if (await context.Movies.AnyAsync())
            return;

        var genres = CreateGenres();
        var actors = CreateActors();
        var movies = CreateMovies(genres, actors);

        // EF follows the navigation properties and saves Genre, Actor,
        // MovieDetails, Review and ActorMovie automatically
        context.Movies.AddRange(movies);
        await context.SaveChangesAsync();
    }

    private static Dictionary<string, Genre> CreateGenres() => new()
    {
        ["Crime"] = new Genre { Name = "Crime" },
        ["Comedy"] = new Genre { Name = "Comedy" },
        ["Sci-Fi"] = new Genre { Name = "Sci-Fi" },
        ["Western"] = new Genre { Name = "Western" },
        ["Action"] = new Genre { Name = "Action" }
    };

    private static Dictionary<string, Actor> CreateActors() => new()
    {
        ["Harvey Keitel"] = new Actor { Name = "Harvey Keitel", BirthYear = 1939 },
        ["Tim Roth"] = new Actor { Name = "Tim Roth", BirthYear = 1961 },
        ["Steve Buscemi"] = new Actor { Name = "Steve Buscemi", BirthYear = 1957 },
        ["Samuel L. Jackson"] = new Actor { Name = "Samuel L. Jackson", BirthYear = 1948 },
        ["John Travolta"] = new Actor { Name = "John Travolta", BirthYear = 1954 },
        ["Uma Thurman"] = new Actor { Name = "Uma Thurman", BirthYear = 1970 },
        ["Jamie Foxx"] = new Actor { Name = "Jamie Foxx", BirthYear = 1967 },
        ["Christoph Waltz"] = new Actor { Name = "Christoph Waltz", BirthYear = 1956 },
        ["Leonardo DiCaprio"] = new Actor { Name = "Leonardo DiCaprio", BirthYear = 1974 },
        ["Joseph Gordon-Levitt"] = new Actor { Name = "Joseph Gordon-Levitt", BirthYear = 1981 },
        ["Graham Chapman"] = new Actor { Name = "Graham Chapman", BirthYear = 1941 },
        ["John Cleese"] = new Actor { Name = "John Cleese", BirthYear = 1939 },
        ["Eric Idle"] = new Actor { Name = "Eric Idle", BirthYear = 1943 },
        ["Michelle Yeoh"] = new Actor { Name = "Michelle Yeoh", BirthYear = 1962 },
        ["Ke Huy Quan"] = new Actor { Name = "Ke Huy Quan", BirthYear = 1971 },
        ["Tom Cruise"] = new Actor { Name = "Tom Cruise", BirthYear = 1962 }
    };

    private static List<Movie> CreateMovies(
        Dictionary<string, Genre> genres,
        Dictionary<string, Actor> actors) =>
    [
        new Movie
        {
            Title = "Reservoir Dogs",
            Year = 1992,
            Duration = 99,
            Genre = genres["Crime"],
            MovieDetails = new MovieDetails
            {
                Synopsis = "After a jewelry heist goes wrong, the surviving criminals gather in a warehouse and suspect that one of them is a police informant.",
                Language = "English",
                Budget = 1_200_000m
            },
            MovieActors =
            [
                new MovieActor { Actor = actors["Harvey Keitel"], Role = "Mr. White" },
                new MovieActor { Actor = actors["Tim Roth"], Role = "Mr. Orange" },
                new MovieActor { Actor = actors["Steve Buscemi"], Role = "Mr. Pink" }
            ],
            Reviews =
            [
                new Review { ReviewerName = "Anna Berg", Comment = "Tense dialogue and a brilliant non-linear story.", Rating = 5 },
                new Review { ReviewerName = "Johan Lind", Comment = "Strong acting, but very violent.", Rating = 4 }
            ]
        },
        new Movie
        {
            Title = "Pulp Fiction",
            Year = 1994,
            Duration = 154,
            Genre = genres["Crime"],
            MovieDetails = new MovieDetails
            {
                Synopsis = "Several stories about hitmen, a boxer and a gangster's wife intertwine in Los Angeles.",
                Language = "English",
                Budget = 8_500_000m
            },
            MovieActors =
            [
                new MovieActor { Actor = actors["Samuel L. Jackson"], Role = "Jules Winnfield" },
                new MovieActor { Actor = actors["John Travolta"], Role = "Vincent Vega" },
                new MovieActor { Actor = actors["Uma Thurman"], Role = "Mia Wallace" },
                new MovieActor { Actor = actors["Tim Roth"], Role = "Pumpkin" },
                new MovieActor { Actor = actors["Harvey Keitel"], Role = "Winston Wolfe" }
            ],
            Reviews =
            [
                new Review { ReviewerName = "Sara Holm", Comment = "A classic with unforgettable scenes.", Rating = 5 },
                new Review { ReviewerName = "Erik Nyström", Comment = "Fun, but a bit too long for me.", Rating = 3 }
            ]
        },
        new Movie
        {
            Title = "Django Unchained",
            Year = 2012,
            Duration = 165,
            Genre = genres["Western"],
            MovieDetails = new MovieDetails
            {
                Synopsis = "A freed slave teams up with a German bounty hunter to rescue his wife from a brutal plantation owner.",
                Language = "English",
                Budget = 100_000_000m
            },
            MovieActors =
            [
                new MovieActor { Actor = actors["Jamie Foxx"], Role = "Django" },
                new MovieActor { Actor = actors["Christoph Waltz"], Role = "Dr. King Schultz" },
                new MovieActor { Actor = actors["Leonardo DiCaprio"], Role = "Calvin Candie" },
                new MovieActor { Actor = actors["Samuel L. Jackson"], Role = "Stephen" }
            ],
            Reviews =
            [
                new Review { ReviewerName = "Maria Ek", Comment = "Christoph Waltz steals every scene.", Rating = 5 }
            ]
        },
        new Movie
        {
            Title = "Inception",
            Year = 2010,
            Duration = 148,
            Genre = genres["Sci-Fi"],
            MovieDetails = new MovieDetails
            {
                Synopsis = "A thief who steals secrets from people's dreams is given a final job: to plant an idea instead.",
                Language = "English",
                Budget = 160_000_000m
            },
            MovieActors =
            [
                new MovieActor { Actor = actors["Leonardo DiCaprio"], Role = "Dom Cobb" },
                new MovieActor { Actor = actors["Joseph Gordon-Levitt"], Role = "Arthur" }
            ],
            Reviews =
            [
                new Review { ReviewerName = "Anna Berg", Comment = "Smart and visually stunning.", Rating = 5 },
                new Review { ReviewerName = "Peter Sand", Comment = "Confusing the first time, better the second.", Rating = 4 },
                new Review { ReviewerName = "Lisa Ström", Comment = "Great music and action.", Rating = 4 }
            ]
        },
        new Movie
        {
            Title = "Monty Python's Life of Brian",
            Year = 1979,
            Duration = 94,
            Genre = genres["Comedy"],
            MovieDetails = new MovieDetails
            {
                Synopsis = "Brian is born on the same night as Jesus, next door, and is mistaken for the Messiah for the rest of his life.",
                Language = "English",
                Budget = 4_000_000m
            },
            MovieActors =
            [
                new MovieActor { Actor = actors["Graham Chapman"], Role = "Brian Cohen" },
                new MovieActor { Actor = actors["John Cleese"], Role = "Reg" },
                new MovieActor { Actor = actors["Eric Idle"], Role = "Mr. Cheeky" }
            ],
            Reviews =
            [
                new Review { ReviewerName = "Johan Lind", Comment = "Still one of the funniest films ever made.", Rating = 5 },
                new Review { ReviewerName = "Karin Vik", Comment = "Not my kind of humor.", Rating = 2 }
            ]
        },
        new Movie
        {
            Title = "Everything Everywhere All at Once",
            Year = 2022,
            Duration = 139,
            Genre = genres["Sci-Fi"],
            MovieDetails = new MovieDetails
            {
                Synopsis = "A laundromat owner discovers she can connect with versions of herself in parallel universes to save the world.",
                Language = "English",
                Budget = 14_300_000m
            },
            MovieActors =
            [
                new MovieActor { Actor = actors["Michelle Yeoh"], Role = "Evelyn Wang" },
                new MovieActor { Actor = actors["Ke Huy Quan"], Role = "Waymond Wang" }
            ],
            Reviews =
            [
                new Review { ReviewerName = "Sara Holm", Comment = "Crazy, emotional and completely original.", Rating = 5 }
            ]
        },
        new Movie
        {
            Title = "Top Gun: Maverick",
            Year = 2022,
            Duration = 131,
            Genre = genres["Action"],
            MovieDetails = new MovieDetails
            {
                Synopsis = "After thirty years of service, Maverick returns to train a group of young pilots for a dangerous mission.",
                Language = "English",
                Budget = 170_000_000m
            },
            MovieActors =
            [
                new MovieActor { Actor = actors["Tom Cruise"], Role = "Pete Mitchell" }
            ],
            Reviews = []   // movie without reviews – useful for testing empty lists
        }
    ];
}