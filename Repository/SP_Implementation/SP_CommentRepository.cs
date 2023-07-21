using Movie_Application.Data;
using Movie_Application.Models;
using Movie_Application.Repository.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Movie_Application.Repository.SP_Implementation
{
    public class SP_CommentRepository : ICommentRepository
    {
        private readonly MovieContext _context;
        private readonly IConfiguration _configuration;
        private string connectionString;
        public SP_CommentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("MovieConnect");
        }

        public async Task AddComment(Comment comment)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[sp_AddComment]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MovieId", comment.MovieId);
                cmd.Parameters.AddWithValue("@UserName", comment.UserName);
                cmd.Parameters.AddWithValue("@Content", comment.Content);
                cmd.Parameters.AddWithValue("@DatePosted", comment.DatePosted);
                await conn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

            }
        }

        public async Task<List<Comment>> GetMovieComments(Guid movieId)
        {
            List<Comment> comments = new List<Comment>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[sp_GetMovieComments]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MovieId", movieId);
                conn.Open();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {

                        Comment comment = new Comment()
                        {
                            CommentId = reader.GetGuid(reader.GetOrdinal("CommentId")),
                            MovieId = reader.GetGuid(reader.GetOrdinal("MovieId")),
                            UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? null : reader.GetString(reader.GetOrdinal("UserName")),
                            Content = reader.IsDBNull(reader.GetOrdinal("Content")) ? null : reader.GetString(reader.GetOrdinal("Content")),
                            DatePosted = reader.GetDateTime(reader.GetOrdinal("DatePosted"))
                        };
                        comments.Add(comment);
                    }
                }
                await conn.CloseAsync();
            }
            return comments;
        }

        public async Task<Comment> GetCommentById(Guid CommentId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[sp_GetCommentById]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CommentId", CommentId));
                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        Comment comment = new Comment()
                        {
                            CommentId = reader.GetGuid(reader.GetOrdinal("CommentId")),
                            MovieId = reader.GetGuid(reader.GetOrdinal("MovieId")),
                            UserName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? null : reader.GetString(reader.GetOrdinal("UserName")),
                            Content = reader.IsDBNull(reader.GetOrdinal("Content")) ? null : reader.GetString(reader.GetOrdinal("Content")),
                            DatePosted = reader.GetDateTime(reader.GetOrdinal("DatePosted"))

                        };
                        return comment;
                    }
                }
                await conn.CloseAsync();
            }
            return null;
        }

        public async Task UpdateComment(Comment comment)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[sp_UpdateComment]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CommentId", comment.CommentId);
                cmd.Parameters.AddWithValue("@MovieId", comment.MovieId);
                cmd.Parameters.AddWithValue("@UserName", comment.UserName);
                cmd.Parameters.AddWithValue("@Content", comment.Content);
                cmd.Parameters.AddWithValue("@DatePosted", comment.DatePosted);

                await conn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteComment(Guid commentId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[sp_DeleteComment]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("CommentId", commentId);

                await con.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
            }






        }
    }
}
