using System.ComponentModel;
using FluentResults;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

public class BadRequestObjectResultErrors : BadRequestObjectResult
{
    public BadRequestObjectResultErrors(object? error) : base(Parse(error))
    {
        
    }

    private static object? Parse(object? error)
    {
        if (error is List<IError> errors)
            return string.Join(';', errors.Select(e=>e.Message));
        return error;
    }

    public BadRequestObjectResultErrors(ModelStateDictionary modelState) : base(modelState)
    {
    }
}