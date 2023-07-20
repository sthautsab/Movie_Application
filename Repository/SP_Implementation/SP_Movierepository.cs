using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Movie_Application.Data;
using Movie_Application.Models;
using Movie_Application.Repository.Interface;
using System.Data;

namespace Movie_Application.Repository.SP_Implementation
{
    public class SP_Movierepository : IMovieRepository
    {
        private readonly MovieContext _context;
        private readonly IConfiguration _configuration;
        private string connectionString;
        public SP_Movierepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("MovieConnect");
        }

        public async Task<bool> AddMovie(Movie movie)
        {
            //SqlParameter Name = new SqlParameter("@Name", model.Name);
            //SqlParameter Genre = new SqlParameter("@Genre", model.Genre);
            //SqlParameter Director = new SqlParameter("@Director", model.Director != null ? model.Director : DBNull.Value);
            //SqlParameter Description = new SqlParameter("@Description", model.Description != null ? model.Description : DBNull.Value);
            //SqlParameter PhotoPath = new SqlParameter("@PhotoPath", model.PhotoPath != null ? model.PhotoPath : DBNull.Value);
            //SqlParameter AverageRating = new SqlParameter("@AverageRating", model.AverageRating != null ? model.AverageRating : 0);
            //int rowsAffected = await _context.Database.ExecuteSqlRawAsync("sp_AddMovie @Name, @Genre, @Director, @Description, @PhotoPath, @AverageRating", Name, Genre, Director, Description, PhotoPath, AverageRating);
            //if (rowsAffected > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[sp_AddMovie]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", movie.Name);
                cmd.Parameters.AddWithValue("@Genre", movie.Genre);
                cmd.Parameters.AddWithValue("@Director", movie.Director != null ? movie.Director : DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", movie.Description != null ? movie.Description : DBNull.Value);
                cmd.Parameters.AddWithValue("@PhotoPath", movie.PhotoPath != null ? movie.PhotoPath : DBNull.Value);
                cmd.Parameters.AddWithValue("@AverageRating", movie.AverageRating != null ? movie.AverageRating : DBNull.Value);
                await conn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



        public async Task<Movie> GetMovieById(Guid id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[sp_GetMovieById]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                conn.Open();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        Movie movie = new Movie()
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Genre = reader.IsDBNull(reader.GetOrdinal("Genre")) ? null : reader.GetString(reader.GetOrdinal("Genre")),
                            Director = reader.IsDBNull(reader.GetOrdinal("Director")) ? null : reader.GetString(reader.GetOrdinal("Director")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                            PhotoPath = reader.IsDBNull(reader.GetOrdinal("PhotoPath")) ? null : reader.GetString(reader.GetOrdinal("PhotoPath")),
                            AverageRating = reader.IsDBNull(reader.GetOrdinal("AverageRating")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("AverageRating"))
                        };
                        return movie;
                    }
                }
                await conn.CloseAsync();
            }
            return null;
        }

        public List<Movie> GetMovies()
        {
            List<Movie> movies = new List<Movie>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[sp_GetMovies]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string genre = reader.GetString("Genre");
                        Movie movie = new Movie()
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Genre = reader.IsDBNull(reader.GetOrdinal("Genre")) ? null : reader.GetString(reader.GetOrdinal("Genre")),
                            Director = reader.IsDBNull(reader.GetOrdinal("Director")) ? null : reader.GetString(reader.GetOrdinal("Director")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                            PhotoPath = reader.IsDBNull(reader.GetOrdinal("PhotoPath")) ? null : reader.GetString(reader.GetOrdinal("PhotoPath")),
                            AverageRating = reader.IsDBNull(reader.GetOrdinal("AverageRating")) ? 0.0 : reader.GetDouble(reader.GetOrdinal("AverageRating"))
                        };
                        movies.Add(movie);
                    }
                }
                conn.Close();
            }
            return movies;
        }

        public async Task<bool> UpdateMovie(Movie updatedMovie)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[sp_UpdateMovie]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Id", updatedMovie.Id);
                cmd.Parameters.AddWithValue("@Name", updatedMovie.Name);
                cmd.Parameters.AddWithValue("@Genre", updatedMovie.Genre);
                cmd.Parameters.AddWithValue("@Director", updatedMovie.Director != null ? updatedMovie.Director : DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", updatedMovie.Description != null ? updatedMovie.Description : DBNull.Value);
                cmd.Parameters.AddWithValue("@PhotoPath", updatedMovie.PhotoPath != null ? updatedMovie.PhotoPath : DBNull.Value);
                cmd.Parameters.AddWithValue("@AverageRating", updatedMovie.AverageRating != null ? updatedMovie.AverageRating : DBNull.Value);

                await conn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DeleteMovie(Guid id)
        {
            using (SqlConnection con = new SqlConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Id", id);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
