using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApiService.Api.Routing
{
    /// <summary>
    /// Routing Convention sets base route prefix for all controllers.
    /// </summary>
    public class RoutingConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _centralPrefix;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="attributeModelName">Prefix name - like "Base Route Prefix"</param>
        /// <param name="attributeModelTemplate">Prefix template - like "api/bookservice/"</param>
        public RoutingConvention(string attributeModelName, string attributeModelTemplate)
        {
            _centralPrefix = new AttributeRouteModel
            {
                Name = attributeModelName,
                Template = attributeModelTemplate
            };
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                    {
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix,
                            selectorModel.AttributeRouteModel);
                    }
                }

                var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
                if (unmatchedSelectors.Any())
                {
                    foreach (var selectorModel in unmatchedSelectors)
                    {
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix,
                            new AttributeRouteModel(new RouteAttribute("[controller]")));
                    }
                }
            }
        }
    }
}
