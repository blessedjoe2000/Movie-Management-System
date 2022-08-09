using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using MMS.Data.Models;
using MMS.Data.Repositories;
using MMS.Data.Security;

namespace MMS.Data.Services
{
    // create IMovieService implementation called MovieServiceDb
    // using the provided Entityframework Repository (MovieDbContext)
    public class MovieServiceDb : IMovieService
    {
        private readonly MovieDbContext db;

        public MovieServiceDb()
        {

            db = new MovieDbContext();
        }

        public void Initialise()
        {
            db.Initialise();
        }

        // ------------------ Movie Related Operations ------------------------

        // retrieve list of Movies
        public IList<Movie> GetAllMovies(string order=null)
        {
            // return the collection as a list
            return db.Movies.ToList();
        }

        // Retrive movie by Id 
        public Movie GetMovieById(int id)
        {
            return db.Movies
                     .Include(m => m.Reviews) //attach review to a movie
                     .FirstOrDefault(m => m.Id == id);//select the movie by id
        }

        // Delete the movie identified by Id returning true 
        //if deleted and false if not found
        public bool DeleteMovie(int id)
        {
            var mov = GetMovieById(id); //find the movie by it's id
            if (mov == null)
            {
                return false;
            }
            db.Movies.Remove(mov); //remove the move from the database
            db.SaveChanges();//// write the new changes to the database
            return true;
        }

        // Update the movie with the details in m 
        public bool UpdateMovie(Movie m)
        {
            // verify the movie exists
            var mov = GetMovieById(m.Id); //find a movie by it's id
            if (mov == null)
            {
                return false;
            }

            // update the details of the movie retrieved and save
            mov.Title = m.Title;
            mov.Director = m.Director;
            mov.Year = m.Year;
            mov.Duration = m.Duration;
            mov.Budget = m.Budget;
            mov.PosterUrl = m.PosterUrl;
            mov.Genre = m.Genre;
            mov.Cast = m.Cast;
            mov.Plot = m.Plot;

            db.SaveChanges(); //write the new changes to the database
            return true;
        }

        public Movie AddMovie(Movie m)
        {
            //check if movie id already exist 
            var movie = GetMovieById(m.Id); //find movie by id if it already exist
            if (movie != null)
            {
                return null; //i.e id already exist and cannot add movie
            }

            //if movie do not exist, add movie
            var mov = new Movie
            {
                Title = m.Title,
                Director = m.Director,
                Year = m.Year,
                Duration = m.Duration,
                Budget = m.Budget,
                PosterUrl = m.PosterUrl,
                Genre = m.Genre,
                Cast = m.Cast,
                Plot = m.Plot
            };
            db.Movies.Add(mov); //add the new movie to the list of movies in the database
            db.SaveChanges(); //write the new changes to the database
            return mov; //return newly added movie
        }

        
        public IList<Movie> GetMoviesQuery(Func<Movie, bool> q)
        {
            return db.Movies
                     .Include(m => m.Reviews)
                     .Where(q).ToList();
        }


        //***************Review management******************
    
        //retrieve review by id
        public Review GetReviewById(int id)
        {
            var review = db.Reviews
                           .Include(r => r.Movie) //include the movie attached to the ticket
                           .FirstOrDefault(r => r.Id ==id);
            return review;
        }

        //add review to movie
        public Review AddReview(Review r)
        {
            //check if movie exist for review
            //if not return null
            var movie = GetMovieById(r.Id); //find movie by id associated with the review
            if (movie == null)
            {
                return null; //if movie do not exist then review cannot exist
            }

            //if movie exist, add review to the movie
            var review = new Review
            {
                MovieId = r.Id,
                Name = r.Name,
                CreatedOn = DateTime.Now,
                Comment = r.Comment,
                Rating = r.Rating
            };
            movie.Reviews.Add(review); //add review to the existing movie
            db.SaveChanges(); //write the new changes to the database
            return review;
        }

        //delete review from movie
        public bool DeleteReview(int id)
        {
            //retrieve review by id
            //if review do not exist return false
            var review = GetReviewById(id); //find review by it's id
            if (review == null)
            {
                return false;
            }

            //remove review from student
            var delete = review.Movie.Reviews.Remove(review); // remove review associated with movie
            db.SaveChanges(); //write the new changes to the database
            return true;
        }

        // return all reviews and the movie generating the review
        public IList<Review> GetReviewsQuery(Func<Review,bool> q)
        {
            return db.Reviews
                     .Include(m => m.Movie)
                     .Where(q).ToList();
        }


        //**************User Related Operations**********************

        //retrieve user by email 
        public User GetUserByEmail(string email)
        {
            return db.Users.FirstOrDefault(u => u.Email == email);
        }

        //Authenticate user
        public User Authenticate(string email, string password)
        {
            // retrieve the user based on the Email address (assumes Email is unique)
            var user = GetUserByEmail(email);

            // Verify the user exists and Hashed User password matches the password provided
            // return user if authenticated otherwise null
            return (user != null && Hasher.ValidateHash(user.Password, password)) ? user : null;
        }
        
        // Register a new user
        public User Register(string username, string email, string password, Role role)
        {
            // check that the user does not already exist (unique user name)
            var exists = GetUserByEmail(email);
            if (exists != null)
            {
                return null;
            }

            //create new user
            var user =  new User{
                Username = username,
                Email = email,
                Password = Hasher.CalculateHash(password),
                Role = role
            };

            db.Users.Add(user); //adding new user to the database
            db.SaveChanges(); //updating the  database
            return user;
        }

    }
}