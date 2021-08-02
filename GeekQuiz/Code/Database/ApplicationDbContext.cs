using GeekQuiz.Code.Database;
using GeekQuiz.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection.Emit;
using System.Web;

namespace GeekQuiz.Code.Database
{

  public class DbContext : System.Data.Entity.DbContext
  {
    public DbSet<TriviaQuestion> TriviaQuestions { get; set; }

    public DbSet<TriviaOption> TriviaOptions { get; set; }

    public DbSet<TriviaAnswer> TriviaAnswers { get; set; }

    // public DbSet<Roles> Roles { get; set; }
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


    public DbContext()
        : base("DefaultConnection")
    {
      System.Data.Entity.Database.SetInitializer(new ApplicationDbInitializer());
    }

    public DbContext(DbSet<TriviaQuestion> questions)
    {
      TriviaQuestions = questions;
    }

    public DbContext(DbSet<TriviaAnswer> answers)
    {
      TriviaAnswers = answers;
    }

    public DbContext(DbSet<TriviaOption> options)
    {
      TriviaOptions = options;
    }
    public static DbContext Create()
    {
      return new DbContext();
    }

  }
}