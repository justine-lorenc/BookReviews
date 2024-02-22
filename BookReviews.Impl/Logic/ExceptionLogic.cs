using AutoMapper;
using BookReviews.Impl.Logic.Interfaces;
using BookReviews.Impl.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BookReviews.Impl.Logic
{
    public class ExceptionLogic : IExceptionLogic
    {
        IMapper _mapper;
        IExceptionRepository _exceptionRepository;

        public ExceptionLogic(IMapper mapper, IExceptionRepository exceptionRepository)
        {
            _mapper = mapper;
            _exceptionRepository = exceptionRepository;
        }

        public async Task<Guid> LogException(System.Exception exception, string message, Dictionary<string, string> arguments = null, int userId = 0,
            [CallerMemberName] string method = null, [CallerFilePath] string filePath = null)
        {
            try
            {
                Entities.Exception exceptionRecord = _mapper.Map<Entities.Exception>(exception);
                exceptionRecord.Message = !String.IsNullOrWhiteSpace(message) ? message : exception.Message;
                exceptionRecord.Method = FormatMethodName(method, filePath);
                exceptionRecord.Arguments = _mapper.Map<string>(arguments);
                exceptionRecord.UserId = userId == 0 ? GetCurrentUserId() : userId;

                Guid exceptionId = await _exceptionRepository.InsertException(exceptionRecord);

                return exceptionId;
            }
            catch (Exception ex)
            {
                // swallow
                return Guid.NewGuid();
            }
        }

        public async Task<Guid> LogException(string error, string message = null, Dictionary<string, string> arguments = null, int userId = 0,
            [CallerMemberName] string method = null, [CallerFilePath] string filePath = null)
        {
            try
            {
                Entities.Exception exceptionRecord = new Entities.Exception()
                {
                    Error = error,
                    Message = message,
                    Method = FormatMethodName(method, filePath),
                    Arguments = _mapper.Map<string>(arguments),
                    UserId = userId == 0 ? GetCurrentUserId() : userId
                };

                Guid exceptionId = await _exceptionRepository.InsertException(exceptionRecord);

                return exceptionId;
            }
            catch (Exception ex)
            {
                // swallow
                return Guid.NewGuid();
            }
        }

        private int GetCurrentUserId()
        {
            int userId = 0;

            try
            {
                ClaimsPrincipal user = HttpContext.Current?.User as ClaimsPrincipal;

                if (user == null || user.Claims.Count() == 0)
                    return 0;

                string id = user.Claims.Where(x => x.Type == "UserId").Select(x => x.Value).FirstOrDefault();

                Int32.TryParse(id, out userId);
            }
            catch { }

            return userId;
        }

        private string FormatMethodName(string method, string filePath)
        {
            string methodName = String.Empty;

            try
            {
                if (!String.IsNullOrWhiteSpace(method))
                    methodName = method;

                if (!String.IsNullOrWhiteSpace(filePath))
                {
                    string className = filePath.Split('\\').LastOrDefault();

                    if (!String.IsNullOrWhiteSpace(className))
                        methodName = $"{className.Replace(".cs", "")}.{methodName}";
                }
            }
            catch { }

            return methodName;
        }
    }
}
