using Microsoft.AspNetCore.Mvc;
using Movie_Application.Models;
using Movie_Application.Repository.Interface;
using Movie_Application.ViewModel;

namespace Movie_Application.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public IActionResult AddComment()
        {
            return PartialView("_AddComment");
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentVM commentVM)
        {
            try
            {
                string loggedInUser = User.Identity.Name;
                Comment comment = new Comment();
                if (ModelState.IsValid)
                {
                    comment.UserName = loggedInUser;
                    comment.Content = commentVM.Content;
                    comment.MovieId = commentVM.MovieId;
                    comment.DatePosted = DateTime.Now;

                    await _commentRepository.AddComment(comment);
                    return RedirectToAction("GetMovieById", "Movie", new { id = comment.MovieId });
                }
                else
                {
                    return Content("problem adding comment");
                }
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }


        }
        [Route("Comment/GetMovieComments/{movieId}")]
        public async Task<IActionResult> GetMovieComments(Guid movieId)
        {
            try
            {
                List<Comment> comments = new List<Comment>();
                comments = await _commentRepository.GetMovieComments(movieId);
                comments = comments.OrderByDescending(c => c.DatePosted).ToList();
                return PartialView("_GetMovieComments", comments);

            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }

        public async Task<IActionResult> Edit(Guid CommentId)
        {
            Comment comment = new Comment();
            CommentVM commentVM = new CommentVM();
            comment = await _commentRepository.GetCommentById(CommentId);
            commentVM.CommentId = comment.CommentId;
            commentVM.MovieId = comment.MovieId;
            commentVM.Content = comment.Content;

            return View(commentVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentVM commentVM)
        {
            Comment existingComment = new Comment();
            existingComment = await _commentRepository.GetCommentById(commentVM.CommentId);
            if (existingComment != null)
            {
                if (existingComment.UserName == User.Identity.Name)
                {
                    existingComment.CommentId = commentVM.CommentId;
                    existingComment.MovieId = commentVM.MovieId;
                    existingComment.UserName = User.Identity.Name;
                    existingComment.DatePosted = DateTime.Now;
                    existingComment.Content = commentVM.Content;
                    await _commentRepository.UpdateComment(existingComment);
                    return RedirectToAction("GetMovieById", "Movie", new { id = commentVM.MovieId });
                }
                else
                {
                    return Content("Not your Comment");
                }
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Delete(Guid CommentId, Guid MovieId)
        {
            await _commentRepository.DeleteComment(CommentId);
            return RedirectToAction("GetMovieById", "Movie", new { id = MovieId });
        }
    }
}
