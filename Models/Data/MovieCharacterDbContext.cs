using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieChatacterAPI.Models.Domain;


namespace MovieChatacterAPI.Models
{
    public class MovieCharacterDbContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Franchise> Franchises { get; set; }

        public MovieCharacterDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Franchise>().HasData(new Franchise { Id = 1, Name = "Lord of the Rings", Description = "Destroy the ring" });
            modelBuilder.Entity<Franchise>().HasData(new Franchise { Id = 2, Name = "Harry Potter", Description = "Defeat Voldermort" });
            modelBuilder.Entity<Franchise>().HasData(new Franchise { Id = 3, Name = "Marvel", Description = "Superheroes" });

            modelBuilder.Entity<Movie>().HasData(new Movie { 
                Id = 1, 
                MovieTitle = "The Fellowship of the ring",
                Genre = "Fantasy", 
                ReleaseYear = 2001, 
                Director = "Peter Jackson",
                FranchiseId = 1,
                Picture = "https://imdb.to/3hrm05a",
                Trailer = "https://imdb.to/3K7qL01"
            });

            modelBuilder.Entity<Movie>().HasData(new Movie
            {
                Id = 2,
                MovieTitle = "The Prisoner of Azkaban",
                Genre = "Fantasy",
                ReleaseYear = 2004,
                FranchiseId = 2,
                Director = "Alfonso Cuaron",
                Picture = "https://imdb.to/3vmnzcN",
                Trailer = "https://imdb.to/3srrcfn"
            });

            modelBuilder.Entity<Movie>().HasData(new Movie
            {
                Id = 3,
                MovieTitle = "The Avengers",
                Genre = "Fantasy",
                ReleaseYear = 2012,
                Director = "Joss Whedon",
                FranchiseId = 3,
                Picture = "https://imdb.to/3tdEV96",
                Trailer = "https://imdb.to/3BXO21A"
            });

            modelBuilder.Entity<Character>().HasData(new Character
            {
                Id = 1,
                FullName = "Elijah Wood",
                Alias = "Frodo",
                Gender = "Male",
                Picture = "https://imdb.to/3sppziu"
            });

            modelBuilder.Entity<Character>().HasData(new Character
            {
                Id = 2,
                FullName = "Emma Watson",
                Alias = "Hermoine",
                Gender = "Female",
                Picture = "https://imdb.to/344JN7W"
            });

            modelBuilder.Entity<Character>().HasData(new Character
            {
                Id = 3,
                FullName = "Chris Evans",
                Alias = "Cpt. America",
                Gender = "Male",
                Picture = "https://imdb.to/36EbkheW"
            });

            modelBuilder.Entity<Movie>()
                .HasMany(p => p.Characters)
                .WithMany(m => m.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieCharacter",
                    r => r.HasOne<Character>().WithMany().HasForeignKey("CharacterId"),
                    l => l.HasOne<Movie>().WithMany().HasForeignKey("MovieId"),
                    je =>
                    {
                        je.HasKey("MovieId", "CharacterId");
                        je.HasData(
                            new { MovieId = 1, CharacterId = 1 },
                            new { MovieId = 2, CharacterId = 2 },
                            new { MovieId = 3, CharacterId = 3 }
                        );
                    });
        }
    }
}
