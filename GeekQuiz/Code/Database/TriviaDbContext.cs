using BooksAPI.Code.Database;
using GeekQuiz.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace GeekQuiz.Code.Database
{
    // Sa nu numesti tot DbContext pentru ca o sa ai conflicte
    // Numeste contextul dupa ceea ce face
    public class TriviaContext : IdentityDbContext<ApplicationUser>
    {
        public TriviaContext() : base("DefaultConnection")
        {
            System.Data.Entity.Database.SetInitializer(new TriviaDbInitializer());
        }

        public static TriviaContext Create()
        {
            return new TriviaContext();
        }

        public DbSet<TriviaQuestion> TriviaQuestions { get; set; }

        public DbSet<TriviaOption> TriviaOptions { get; set; }

        public DbSet<TriviaAnswer> TriviaAnswers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            //modelBuilder.Entity<TriviaOption>()
            //         .HasKey(o => new { o.QuestionId, o.Id });

        }
    }
}