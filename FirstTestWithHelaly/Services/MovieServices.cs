namespace FirstTestWithHelaly.Services
{
    public class MovieServices : IMovieServices
    {
        private readonly ApplicationDbContext _context;

        public MovieServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> Add(Movie movie)
        {
            await _context.AddAsync(movie);
            _context.SaveChanges();
            return movie;
        }

        public Movie Delete(Movie movie)
        {
            _context.Remove(movie);
            _context.SaveChanges();
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAll(byte genreid = 0)
        {
           return await _context.Movies.Include(m => m.Genre)
                .Where(m=>m.GenreId==genreid || genreid == 0)
                .OrderByDescending(m => m.Rate)
                .ToListAsync();
        }

        public async Task<Movie> GetAllByID(int id)
        {
           return await _context.Movies.Include(c => c.Genre).SingleOrDefaultAsync(d => d.Id == id);
        }

        public Movie Update(Movie movie)
        {
            _context.Update(movie);
            _context.SaveChanges();
            return movie;
        }
    }
}
