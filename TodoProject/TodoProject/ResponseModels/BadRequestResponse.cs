using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace TodoProject.ResponseModels
{
    public class BadRequestResponse : ApiResponse
    {
        public Dictionary<string, List<string>> Errors { get; }

        public BadRequestResponse(ModelStateDictionary modelState)
            : base((int)HttpStatusCode.BadRequest)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("Model State must be invalid", nameof(modelState));
            }

            Errors = new Dictionary<string, List<string>>();
            foreach (var error in modelState)
            {
                var value = error.Value.Errors.Select(x => x.ErrorMessage).ToList();
                if (!Errors.ContainsKey(error.Key))
                {
                    Errors.Add(error.Key, value);
                }
            }
        }
    }
}
