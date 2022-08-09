
using System;
using System.Linq;
using Xunit;

using MMS.Data.Models;
using MMS.Data.Services;

namespace MMS.Test
{
    public class TestMovieService
    {
        private readonly IMovieService svc;
              
        public TestMovieService()
        {
            // create and initialise MovieServiceDb service
            svc = new MovieServiceDb();

            // to ensure data source is empty before each test
            svc.Initialise();
                 
        }

        // add tests here - test should ensure your service implementation works
        [Fact]
        public void Movie_GetTwoMoviesCreated_ShouldReturnTwo() 
        {
            //arrange
            Movie mov = new Movie();
            mov.Title = "The Expendables";
            mov.Director = "Sylvester Stallone";
            mov.Year = 2010;
            mov.Duration = 103;
            mov.Budget = 80000000;
            mov.PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/tDynwDj3pvexrEQ8wb0uy8EdcGQ.jpg";
            mov.Genre = Genre.Action;
            mov.Cast = "Sylvester Stallone";
            mov.Plot = "Good movie";


            Movie mov2 = new Movie();
            mov2.Title = "Cherry";
            mov2.Director = "Anthony Russo";
            mov2.Year = 2021;
            mov2.Duration = 140;
            mov2.Budget = 40000000;
            mov2.PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/pwDvkDyaHEU9V7cApQhbcSJMG1w.jpg";
            mov2.Genre = Genre.Thriller;
            mov2.Cast = "Tom Holland";
            mov2.Plot = "Good movie";

            var m = svc.AddMovie(mov);
            var m2 = svc.AddMovie(mov2);

            //act
            var movie = svc.GetAllMovies();
            var count = movie.Count();

            //assert
            Assert.Equal(2, count);
            Assert.NotNull(m);
            Assert.NotNull(m2);
        }

        [Fact]
        public void Movie_GetAllMoviesWhenNone_ShouldReturnNone()
        {
            //arrange

            //act
            var mov = svc.GetAllMovies();
            var count = mov.Count();

            //assert
            Assert.Equal(0, count);

        }

        [Fact]
        public void Moive_AddMovieWhenUnique_ShouldSetAllProperties()
        {
            //arrange
            Movie mov = new Movie();
            mov.Title = "The Expendables";
            mov.Director = "Sylvester Stallone";
            mov.Year = 2010;
            mov.Duration = 103;
            mov.Budget = 80000000;
            mov.PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/tDynwDj3pvexrEQ8wb0uy8EdcGQ.jpg";
            mov.Genre = Genre.Action;
            mov.Cast = "Sylvester Stallone";
            mov.Cast = "Jason Statham";
            mov.Plot = "Good movie";

            var mo = svc.AddMovie(mov);

            //act
            var m = svc.GetMovieById(mo.Id);

            //assert
            Assert.NotNull(m);
            Assert.Equal(m.Id, m.Id);
            Assert.Equal("The Expendables", m.Title);
            Assert.Equal("Sylvester Stallone", m.Director);
            Assert.Equal(2010, m.Year);
            Assert.Equal(103, m.Duration);
            Assert.Equal(80000000, m.Budget);
            Assert.Equal("https://www.themoviedb.org/t/p/w600_and_h900_bestv2/tDynwDj3pvexrEQ8wb0uy8EdcGQ.jpg", m.PosterUrl);
            Assert.Equal(Genre.Action, m.Genre);
            Assert.Equal("Jason Statham", m.Cast);
            Assert.Equal("Good movie", m.Plot);

        }

        [Fact]
        public void Movie_GetMovieThatExist_ShouldReturnMovie()
        {
            //arrange
            Movie mov = new Movie();
            mov.Title = "The Expendables";
            mov.Director = "Sylvester Stallone";
            mov.Year = 2010;
            mov.Duration = 103;
            mov.Budget = 80000000;
            mov.PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/tDynwDj3pvexrEQ8wb0uy8EdcGQ.jpg";
            mov.Genre = Genre.Action;
            mov.Cast = "Sylvester Stallone";
            mov.Plot = "Good movie";

            //act
            var m = svc.AddMovie(mov);
            var movie = svc.GetMovieById(m.Id);

            //assert
            Assert.NotNull(m);
            Assert.Equal(m.Id, movie.Id);

        }

        [Fact]
        public void Movie_UpdateWhenExist_ShouldSetReturnTrue()
        {
            //arrange

            Movie mov = new Movie();
            mov.Title = "Cherry";
            mov.Director = "Anthony Russo";
            mov.Year = 2021;
            mov.Duration = 140;
            mov.Budget = 40000000;
            mov.PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/pwDvkDyaHEU9V7cApQhbcSJMG1w.jpg";
            mov.Genre = Genre.Thriller;
            mov.Cast = "Tom Holland";
            mov.Plot = "Good movie";

            var m = svc.AddMovie(mov);
            //act

            m.Title = "The Expendables";
            m.Director = "Sylvester Stallone";
            m.Year = 2010;
            m.Duration = 103;
            m.Budget = 80000000;
            m.PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/tDynwDj3pvexrEQ8wb0uy8EdcGQ.jpg";
            m.Genre = Genre.Action;
            m.Cast = "Sylvester Stallone";
            m.Cast = "Jason Statham";
            m.Plot = "Good movie";
            
            var updated = svc.UpdateMovie(m);

            //assert
            Assert.NotNull(m);
            Assert.True(updated);
        }

        [Fact]
        public void Movie_DeleteMovieThatExist_shouldReturnTrue()
        {
            //arrange
            Movie m = new Movie();
            m.Title = "The Expendables";
            m.Director = "Sylvester Stallone";
            m.Year = 2010;
            m.Duration = 103;
            m.Budget = 80000000;
            m.PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/tDynwDj3pvexrEQ8wb0uy8EdcGQ.jpg";
            m.Genre = Genre.Action;
            m.Cast = "Sylvester Stallone";
            m.Cast = "Jason Statham";
            m.Plot = "Good movie";
            
            var movie = svc.AddMovie(m);

            //act
            var deleted = svc.DeleteMovie(movie.Id);
            var mov = svc.GetMovieById(movie.Id);

            //assert
            Assert.True(deleted);
            Assert.Null(mov);
        }

        [Fact]
        public void Movie_DeletdMovieThatExist_ShouldReduceMovieCountByOne()
        {
            //arrange
            Movie mov = new Movie();
            mov.Title = "The Expendables";
            mov.Director = "Sylvester Stallone";
            mov.Year = 2010;
            mov.Duration = 103;
            mov.Budget = 80000000;
            mov.PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/tDynwDj3pvexrEQ8wb0uy8EdcGQ.jpg";
            mov.Genre = Genre.Action;
            mov.Cast = "Sylvester Stallone";
            mov.Plot = "Good movie";


            Movie mov2 = new Movie();
            mov2.Title = "Cherry";
            mov2.Director = "Anthony Russo";
            mov2.Year = 2021;
            mov2.Duration = 140;
            mov2.Budget = 40000000;
            mov2.PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/pwDvkDyaHEU9V7cApQhbcSJMG1w.jpg";
            mov2.Genre = Genre.Thriller;
            mov2.Cast = "Tom Holland";
            mov2.Plot = "Good movie";

            var m = svc.AddMovie(mov);
            var m2 = svc.AddMovie(mov2);

            //act
            var movie = svc.DeleteMovie(m2.Id);
            var movi = svc.GetAllMovies();
            var count = movi.Count();

            //assert
            Assert.Equal(1, count);
        }

        [Fact]
        public void Movie_DeleteMovieThatDoNotExist_ShouldNotChangeMovieCount()
        {
            //arrange
            Movie mov = new Movie();
            mov.Title = "The Expendables";
            mov.Director = "Sylvester Stallone";
            mov.Year = 2010;
            mov.Duration = 103;
            mov.Budget = 80000000;
            mov.PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/tDynwDj3pvexrEQ8wb0uy8EdcGQ.jpg";
            mov.Genre = Genre.Action;
            mov.Cast = "Sylvester Stallone";
            mov.Plot = "Good movie";

            var m = svc.AddMovie(mov);

            //act
            var delete = svc.DeleteMovie(0);
            var movie = svc.GetAllMovies();
            var count = movie.Count();

            //assert
            Assert.NotNull(m);
            Assert.Equal(1, count);
            Assert.False(delete);
        }

        //************Review Test***********************

        [Fact]
        public void Review_AddReviewToExistingMovie_ShouldUpdateReview()
        {
            //arrange
            Movie mov = new Movie();
            mov.Title = "Cherry";
            mov.Director = "Anthony Russo";
            mov.Year = 2021;
            mov.Duration = 140;
            mov.Budget = 40000000;
            mov.PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/pwDvkDyaHEU9V7cApQhbcSJMG1w.jpg";
            mov.Genre = Genre.Thriller;
            mov.Cast = "Tom Holland";
            mov.Plot = "Good movie";

            var m = svc.AddMovie(mov);

            //act
            Review rev = new Review();
            rev.Id = m.Id;
            rev.Name = "rating";
            rev.Comment = "good movie";
            rev.Rating = 4;

            var review = svc.AddReview(rev);

            //assert
            Assert.NotNull(review);
        }

        [Fact]
        public void Review_GetReviewWhenExist_ShouldReturnReview()
        {
            //arrange
            Movie mov = new Movie();
            mov.Title = "Cherry";
            mov.Director = "Anthony Russo";
            mov.Year = 2021;
            mov.Duration = 140;
            mov.Budget = 40000000;
            mov.PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/pwDvkDyaHEU9V7cApQhbcSJMG1w.jpg";
            mov.Genre = Genre.Thriller;
            mov.Cast = "Tom Holland";
            mov.Plot = "Good movie";

            var m = svc.AddMovie(mov);

            //act
            Review rev = new Review();
            rev.Id = m.Id;
            rev.Name = "rating";
            rev.Comment = "good movie";
            rev.Rating = 3;

            var review = svc.AddReview(rev);
            var r = svc.GetReviewById(review.Id);   

            //assert
            Assert.NotNull(r);         
        }

        [Fact]
        public void Review_DeleteReviewWhenExist_ShouldReturnTrue()
        {
            //arrange
            Movie mov = new Movie();
            mov.Title = "Cherry";
            mov.Director = "Anthony Russo";
            mov.Year = 2021;
            mov.Duration = 140;
            mov.Budget = 40000000;
            mov.PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/pwDvkDyaHEU9V7cApQhbcSJMG1w.jpg";
            mov.Genre = Genre.Thriller;
            mov.Cast = "Tom Holland";
            mov.Plot = "Good movie";

            var m = svc.AddMovie(mov);

            //act
            Review rev = new Review();
            rev.Id = m.Id;
            rev.Name = "rating";
            rev.Comment = "good movie";
            rev.Rating = 3;

            var review = svc.AddReview(rev);
            var deleted = svc.DeleteReview(review.Id);
            var r = svc.GetReviewById(review.Id);

            //assert
            Assert.True(deleted);
            Assert.Null(r);
        }

        [Fact]
        public void Review_DeleteReviewThatDoNotExit_ShouldNotChangeReview()
        {
            //arrange
            Movie mov = new Movie();
            mov.Title = "Cherry";
            mov.Director = "Anthony Russo";
            mov.Year = 2021;
            mov.Duration = 140;
            mov.Budget = 40000000;
            mov.PosterUrl = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2/pwDvkDyaHEU9V7cApQhbcSJMG1w.jpg";
            mov.Genre = Genre.Thriller;
            mov.Cast = "Tom Holland";
            mov.Plot = "Good movie";

            var m = svc.AddMovie(mov);

            //act
            Review rev = new Review();
            rev.Id = m.Id;
            rev.Name = "rating";
            rev.Comment = "good movie";
            rev.Rating = 3;

            var review = svc.AddReview(rev);
            var deleted = svc.DeleteReview(0);

            //
            Assert.NotNull(review);
            Assert.False(deleted);
        }


        //**************User Test****************

        [Fact] // --- Register Valid User test
        public void User_Register_WhenValid_ShouldReturnUser()
        {
            //arrange
            var reg = svc.Register("kenny", "kenney@gmail.com", "kenny", Role.guest);

            //act
            var user = svc.GetUserByEmail(reg.Email);

            //assert
            Assert.NotNull(reg);
            Assert.NotNull(user);
        }

        [Fact] // --- Register Duplicate Test
        public void User_Register_WhenDuplicateEmail_ShouldReturnNull()
        {
            //arrange
            var reg1 = svc.Register("kenny", "kenney@gmail.com", "kenny", Role.guest);

            //act
            var reg2 = svc.Register("kenny", "kenney@gmail.com", "kenny", Role.admin);

            //assert
            Assert.NotNull(reg1);
            Assert.Null(reg2);
        }

        [Fact] // --- Authenticate Invalid Test
        public void User_Authenticate_WhenInvalidCredentials_ShouldReturnNull()
        {
            //arrange
            var reg = svc.Register("kenny", "kenney@gmail.com", "kenny", Role.guest);

            //act
            var user = svc.Authenticate("kenney@gmail.com", "kenneys");

            //assert
            Assert.Null(user);
        }

        [Fact] // --- Authenticate Valid Test
        public void User_Authenticate_WhenValidCredentials_ShouldReturnUser()
        {
            //arrange
            var reg = svc.Register("kenny", "kenney@gmail.com", "kenny", Role.guest);

            //act
            var user = svc.Authenticate("kenney@gmail.com", "kenny");

            //assert
            Assert.NotNull(user);
        }

    }
}
