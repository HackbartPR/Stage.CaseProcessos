using Microsoft.AspNetCore.Mvc;
using Stage.Domain.Config;
using Stage.Domain.Notifications;
using System.Net;

namespace Stage.Application.Extensions
{
    public static class MontarResponse<T> where T : class
    {
        public static IActionResult Ok(T value, INotificationContext notificationContext)
        {
            var notifications = notificationContext.Notifications();
            return new ObjectResult(null)
            {
                StatusCode = (int) HttpStatusCode.OK,
                Value = new BaseResponse<T>(value, notifications, true)
            };
        }

        public static IActionResult OkPaged(T value)
        {
            return new ObjectResult(null)
            {
                StatusCode = (int)HttpStatusCode.OK,
                Value = value
            };
        }

        public static IActionResult Failure(string errorMessage, INotificationContext notificationContext)
        {
            var notifications = notificationContext.Notifications();
            return new ObjectResult(null)
            {
                Value = new BaseResponse<string>(errorMessage, notifications, false)
            };
        }
    }
}
