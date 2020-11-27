using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiService.Api.Validators
{
    public static class ValidationErrors
    {
        public static string AuthorNameRequired = "Author name is required";
        public static string AuthorNameInvalid = "Author name cannot contain only special symbols";
        public static string TitleRequired = "Title is required";
        public static string TitleInvalid = "Title cannot contain only special symbols";
    }
}
