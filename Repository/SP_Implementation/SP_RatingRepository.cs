using Movie_Application.Data;
using Movie_Application.Models;
using Movie_Application.Repository.Interface;
using System.Data;
using System.Data.SqlClient;

namespace Movie_Application.Repository.SP_Implementation
{
    public class SP_RatingRepository : IRatingRepository
    {
        private readonly MovieContext _context;
        private readonly IConfiguration _configuration;
        private string connectionString;
        public SP_RatingRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("MovieConnect");
        }
        public async Task AddRating(Rating rating)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[sp_AddRating]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MovieId", rating.MovieId);
                cmd.Parameters.AddWithValue("@UserId", rating.UserId);
                cmd.Parameters.AddWithValue("@Rate", rating.Rate);
                await conn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<double> GetAverageRating(Guid MovieId)
        {
            double averageRating = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("[dbo].[sp_GetAverageRating]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("MovieId", MovieId);

                    object result = await command.ExecuteScalarAsync();

                    // Check if the result is not null and convert it to int
                    if (result != null && result != DBNull.Value)
                    {
                        averageRating = Convert.ToDouble(result);
                    }
                }
            }
            return averageRating;
        }

        public async Task<int?> GetRatingByUserIdAndMovieId(string UserId, Guid MovieId)
        {
            int rate = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_GetRatingByUserIdAndMovieId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("UserId", UserId);
                    command.Parameters.AddWithValue("MovieId", MovieId);

                    object result = await command.ExecuteScalarAsync();

                    // Check if the result is not null and convert it to int
                    if (result != null && result != DBNull.Value)
                    {
                        rate = Convert.ToInt32(result);
                    }
                }
            }
            return rate;
        }

        public async Task<Rating> GetRatingDataByUserIdAndMovieId(string UserId, Guid MovieId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("[dbo].[sp_GetRatingDataByUserIdAndMovieId]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserId", UserId));
                cmd.Parameters.Add(new SqlParameter("@MovieId", MovieId));
                await conn.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        Rating rating = new Rating()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            MovieId = reader.GetGuid(reader.GetOrdinal("MovieId")),
                            UserId = reader.GetString(reader.GetOrdinal("UserId")),
                            Rate = reader.GetInt32(reader.GetOrdinal("Rate"))
                        };
                        return rating;
                    }
                }
                await conn.CloseAsync();
            }
            return null;
        }

        public async Task UpdateUserRating(Rating rating)
        {
            Rating existingRating = await GetRatingDataByUserIdAndMovieId(rating.UserId, rating.MovieId);
            if (existingRating != null)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[dbo].[sp_UpdateRating]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", existingRating.Id);
                    cmd.Parameters.AddWithValue("@MovieId", rating.MovieId);
                    cmd.Parameters.AddWithValue("@UserId", rating.UserId);
                    cmd.Parameters.AddWithValue("@Rate", rating.Rate);
                    await conn.OpenAsync();

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                }
            }
        }
    }
}
