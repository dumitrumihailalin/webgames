using GeekQuiz.Code.Database;
using GeekQuiz.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace GeekQuiz.Controllers
{
    public class TriviaController : ApiController
    {
        private async Task<TriviaQuestion> NextQuestionAsync(string userId)
        {
            using (TriviaContext db = new TriviaContext())
            {
                var lastQuestionId = await db.TriviaAnswers
                                             .Where(a => a.UserId == userId)
                                             .GroupBy(a => a.QuestionId)
                                             .Select(g => new { QuestionId = g.Key, Count = g.Count() })
                                             .OrderByDescending(q => new { q.Count, QuestionId = q.QuestionId })
                                             .Select(q => q.QuestionId)
                                             .FirstOrDefaultAsync();

                var questionsCount = await db.TriviaQuestions.CountAsync();

                var nextQuestionId = (lastQuestionId % questionsCount) + 1;
                return await db.TriviaQuestions.FindAsync(CancellationToken.None, nextQuestionId);
            }
        }

        // GET api/Trivia
        [ResponseType(typeof(TriviaQuestion))]
        public async Task<IHttpActionResult> Get()
        {
            var userId = User.Identity.Name;

            TriviaQuestion nextQuestion = await this.NextQuestionAsync(userId);

            if (nextQuestion == null)
            {
                return this.NotFound();
            }

            return this.Ok(nextQuestion);
        }

        private async Task<bool> StoreAsync(TriviaAnswer answer)
        {
            using (TriviaContext db = new TriviaContext())
            {
                db.TriviaAnswers.Add(answer);

                await db.SaveChangesAsync();
                var selectedOption = await db.TriviaOptions.FirstOrDefaultAsync(o => o.Id == answer.OptionId
                    && o.QuestionId == answer.QuestionId);

                return selectedOption.IsCorrect;
            }
        }

        // POST api/Trivia
        [ResponseType(typeof(TriviaAnswer))]
        public async Task<IHttpActionResult> Post(TriviaAnswer answer)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            answer.UserId = User.Identity.Name;
            using (TriviaContext db = new TriviaContext())
            {
                var isCorrect = await this.StoreAsync(answer);
                return this.Ok<bool>(isCorrect);
            }
        }
    }
}
