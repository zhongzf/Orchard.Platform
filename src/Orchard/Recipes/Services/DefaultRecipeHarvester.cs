using Orchard.Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orchard.Recipes.Services
{
    public class DefaultRecipeHarvester : IRecipeHarvester
    {
        public IEnumerable<Recipe> HarvestRecipes(string extensionId)
        {
            return new Recipe[] { };
        }
    }
}
