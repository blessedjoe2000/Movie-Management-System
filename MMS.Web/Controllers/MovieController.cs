using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MMS.Data.Models;
using MMS.Data.Services;

namespace MMS.Web.Controllers
{
    
    public class MovieController : BaseController
    {
        private IMovieService svc;
        public MovieController()
        {
            //initialising svc to a new MovieServiceDb every time it compiles
            svc = new MovieServiceDb();
        }

        //GET /movie/index
        public IActionResult Index()
        {
            var movies = svc.GetAllMovies(); //using svc to get all movies in the database
            return View(movies); //display a list of movies retrieved
        }

        //Get /movie/details/{id}
        public IActionResult Details(int id)
        {
            var movie = svc.GetMovieById(id); //retrieve movie by id

            if(movie == null)// check if movie is null
            {
                Alert("Movie not found", AlertType.warning); //display an info to alert user
                return RedirectToAction("Index"); //redirect to index page
            }

            return View(movie); //if movie is found, dispaly movie
        }

        //GET /movie/create
        // [Authorize(Roles="admin")] //I couldn't activate authorize by admin because my register page didn't work perfectly
        public IActionResult Create()
        {
            var movie = new Movie(); //display blank form to create a student
            return View(movie);
        }

        //POST /movie/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize(Roles="admin")]
        public IActionResult Create(Movie movie)
        {
            if (ModelState.IsValid) //if the blank movie was created
            {
                var addMovie = svc.AddMovie(movie); //add properties to the blank movie created
                addMovie.Title = movie.Title;
                addMovie.Director = movie.Director;
                addMovie.Year = movie.Year;
                addMovie.Duration = movie.Duration;
                addMovie.Budget = movie.Budget;
                addMovie.PosterUrl = movie.PosterUrl;
                addMovie.Genre = movie.Genre;
                addMovie.Cast = movie.Cast;
                addMovie.Plot = movie.Plot;
                
                Alert("Movie created successfully", AlertType.success); //displays info to user
                return RedirectToAction("Index"); //redirect ti index page
            }

            return View(movie); //redisplay the newly created movie
        }


        //GET /movie/edit/{id}
        // [Authorize(Roles="admin")]
        public IActionResult Edit(int id)
        {
            var movie = svc.GetMovieById(id); //using svc to retrieve movie from the database

            if (movie==null) //if movie is not found
            {
                Alert("Movie not found", AlertType.warning); //dispalys info to user
                return RedirectToAction("Index"); //redirect to index page
            }

            return View(movie); //if movie is found, display the movie for edit
        }
        
        //POST /movie/edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize(Roles="admin")]
        public IActionResult Edit(int id, Movie movie)
        {
            if (ModelState.IsValid)
            {
                svc.UpdateMovie(movie); //pass the movie found to svc to update to the databse

                Alert("Movie details saved", AlertType.success); //displays info to user
                return RedirectToAction("Index"); //redirect to index page
            }

            return View(movie); //displays updated movie
        }

        //GET /movie/delete/{id}
        // [Authorize(Roles="admin")]
        public IActionResult Delete(int id)
        {
            var movie = svc.GetMovieById(id); //using svc to retrieve movie from database 

            if (movie==null) //if movie is not found
            {
                Alert("Movie not found", AlertType.warning); //displays info to user
                return RedirectToAction("Index"); //rediect to index page
            }

            return View(movie); //if movie is found, return movie for deletion
        }

        //POST /movie/delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize(Roles="admin")]
        public IActionResult DeleteConfirm(int id)
        {
            svc.DeleteMovie(id); //delete the movie found with id

            Alert("Movie deleted successfully", AlertType.success); //displays info to user
            return RedirectToAction("Index"); //redirect to index page
        }
        

        //GET /movie/createreview/{id}
        public IActionResult CreateReview(int id)
        {
            var movie = svc.GetMovieById(id); //retrieve a movie with it's id

            if (movie==null)
            {
                Alert("Movie not found", AlertType.warning); //displays info to user
                return RedirectToAction("Index"); //redirect to index page
            }

            var r = new Review
            {
                MovieId = id
            }; //if movie is found, create review to the movie via reviewCreateViewModel

            return View("CreateReview", r);
        }

        //POST /movie/createreview
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateReview(Review r)
        {
            var movie = svc.GetMovieById(r.MovieId); //retrieve a movie
            
            if (movie==null)
            {
                Alert("Movie not found", AlertType.warning); //displays info to user
                return RedirectToAction("Index"); //redirect to index
            }

            svc.AddReview(r); //add review created
            Alert("Review added successfully", AlertType.success); //displays info to user
            return RedirectToAction("Details", new {Id=r.MovieId});
            
        }

        //GET /movie/deletereview
        // [Authorize(Roles="admin")]
        public IActionResult DeleteReview(int id)
        {
            var review = svc.GetReviewById(id); //using svc to retrieve review from database 

            if (review==null) //if review is not found
            {
                Alert("review not found", AlertType.warning); //displays info to user
                return RedirectToAction("Index"); //rediect to details page
            }

            return View("DeleteReview", review);
        }

        //POST /movie/deletereview/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize(Roles="admin")]
        public IActionResult DeleteReview(Review r)
        {
            svc.DeleteReview(r.Id);
            Alert("Review deleted successfully", AlertType.success);
            
            return RedirectToAction("Index");
                    
        }
        
    }
}