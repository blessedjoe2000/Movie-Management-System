
using System.Collections.Generic;
using MMS.Data.Models;

namespace MMS.Data.Services
{
    public interface IMovieService
    {
        void Initialise();

        //movie management operation

        //get all movies in the database
        IList<Movie> GetAllMovies(string order=null);

        //get a particular movie by id from the database
        Movie GetMovieById(int id);

        //Delete a particular movie by movie id from the database
        bool DeleteMovie(int id);

        //update a particular movie by movie id in the database
        bool UpdateMovie(Movie m);

        //add a new movie to the list of movie in the database
        Movie AddMovie(Movie m);


        //review manamgement operation

        //get a review of a movie
        Review GetReviewById(int id);

        //add a review to a movie
        Review AddReview(Review r);

        //delete a review of a movie
        bool DeleteReview(int id);


        //user management
        User GetUserByEmail(string email);
        User Authenticate(string email, string password);
        User Register(string username, string email, string password, Role role);
    }




}