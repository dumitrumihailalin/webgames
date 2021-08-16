using GeekQuiz.Code.Database;
using System.Data.Entity;

namespace BooksAPI.Code.Database
{

    public class TriviaDbInitializer : CreateDatabaseIfNotExists<TriviaContext>
    {
        protected override void Seed(TriviaContext context)
        {
            base.Seed(context);
        }
    }
}