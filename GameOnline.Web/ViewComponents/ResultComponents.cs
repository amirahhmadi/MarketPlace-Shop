using GameOnline.Common.Core;
using GameOnline.Core.ExtenstionMethods;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameOnline.Web.ViewComponents
{
    public class ResultComponents : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            OperationResult<string> result = null;

            if (TempData[TempDataName.Result] != null)
            {
                result = JsonConvert.DeserializeObject<OperationResult<string>>(TempData[TempDataName.Result].ToString());
            } 
            
            if (TempData[TempDataName.ResultParent] != null)
            {
                result = JsonConvert.DeserializeObject<OperationResult<string>>(TempData[TempDataName.ResultParent].ToString());
            }

            return await Task.FromResult(View("Result", result));
        }
    }
}