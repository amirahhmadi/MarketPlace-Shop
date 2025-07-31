using GameOnline.Common.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.ViewComponents
{
    public class ResultComponents : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            OperationResult<string> result = null;

            if (TempData["Result"] != null)
            {
                result = JsonConvert.DeserializeObject<OperationResult<string>>(TempData["Result"].ToString());
            } 
            
            if (TempData["ResultParent"] != null)
            {
                result = JsonConvert.DeserializeObject<OperationResult<string>>(TempData["ResultParent"].ToString());
            }

            return await Task.FromResult(View("Result", result));
        }
    }
}