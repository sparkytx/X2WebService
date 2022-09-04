using System.ComponentModel;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace X2WebService;

public static class WebServiceExtension
{
    public static ActionResult ReturnWebResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);
        return new BadRequestObjectResult(string.Join(";", result.Errors));
    }

    public static void CleanSwaggerJson(object param)
    {
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(param))
        {
            var value = property.GetValue(param);
            var ptype=property.PropertyType;
            if (ptype==typeof(string) && value!=null && value.Equals("string"))
            {
                property.SetValue(param,null);
            }
            else if (property.Name.Contains("Id") && property.Name.Length>2 && value==(object?) 0)
                property.SetValue(param,null);

         
        }
    }

}