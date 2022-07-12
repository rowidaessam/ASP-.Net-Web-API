namespace FirstTestWithHelaly.Services
{
    public interface IMovieServices
    {
        Task<IEnumerable<Movie>> GetAll(byte genreid =0); 
        Task<Movie> GetAllByID(int id);
        Task<Movie> Add(Movie movie);
        Movie Update(Movie movie);
        Movie Delete(Movie movie);

    }
}
