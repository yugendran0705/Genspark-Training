using System.Threading.Tasks;

namespace BankApp.Interfaces
{
    public interface IFaqService
    {
        Task<string> AskQuestionAsync(string question);
    }
}
