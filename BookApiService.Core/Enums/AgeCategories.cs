using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiService.Core.Enums
{
    public enum AgeCategories
    {
        NotSet = -1,
        Toddlers, // 0-3
        Child, // 3-10
        PreTeen, // 10-15
        Teenager, // 15-17
        Adult // 18+
    }
}
