using Microsoft.AspNetCore.Mvc;

namespace StalTradeUI.Helpers
{
    public static class ResponseHandler
    {
        public static void HandleResponse(HttpResponseMessage response, Controller controller, string successMessage = null)
        {
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseContent);
            var message = responseData.message.ToString();

            if (response.IsSuccessStatusCode)
            {
                controller.TempData["SuccessMessage"] = successMessage ?? message;
            }
            else
            {
                controller.TempData["ErrorMessage"] = $"{message}. {response.ReasonPhrase}";
            }
        }
    }
}
