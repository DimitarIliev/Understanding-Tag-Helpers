using Cities.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cities.Infrastructure.TagHelpers
{
    [HtmlTargetElement("select", Attributes = "model-for")]
    public class SelectOptionTagHelper: TagHelper
    {
        private IRepository _repository;

        public SelectOptionTagHelper(IRepository repository)
        {
            _repository = repository;
        }

        public ModelExpression ModelFor { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.Append((await output.GetChildContentAsync(false)).GetContent());

            object selected;
            context.Items.TryGetValue(typeof(SelectTagHelper), out selected);
            IEnumerable<string> selectedValues = (selected as IEnumerable<string>) ?? Enumerable.Empty<string>();

            PropertyInfo property = typeof(City).GetTypeInfo().GetDeclaredProperty(ModelFor.Name);

            foreach (var country in _repository.Cities.Select(x => property.GetValue(x)).Distinct())
            {
                if (selectedValues.Any(x => x.ToString().Equals(country.ToString(), StringComparison.OrdinalIgnoreCase)))
                    output.Content.AppendHtml($"<option selected>{country}</option>");
                else
                    output.Content.AppendHtml($"<option>{country}</option>");
            }
        }
    }
}
