using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Domedo.API.Config.Swagger
{
    public static class ApiParameterDescriptionExtensions
    {
        public static bool IsRequired(this ApiParameterDescription parameterDescription)
        {
            if (parameterDescription.RouteInfo?.IsOptional == false)
                return true;
            if (parameterDescription.ModelMetadata.IsBindingRequired)
                return true;
            if (parameterDescription.ModelMetadata.IsRequired && DefaultValue(parameterDescription) == null)
                return true;

            return false;
        }

        public static object DefaultValue(this ApiParameterDescription parameterDescription)
        {
            switch (parameterDescription.ModelMetadata.MetadataKind)
            {
                case ModelMetadataKind.Property:
                    {
                        var container = Activator.CreateInstance(parameterDescription.ModelMetadata.ContainerType);
                        var val = parameterDescription.ModelMetadata.PropertyGetter(container);

                        // check that value is not default(T)
                        if (parameterDescription.Type.IsValueType && Equals(val, Activator.CreateInstance(parameterDescription.Type)))
                        {
                            return null;
                        }

                        return val;
                    }
                case ModelMetadataKind.Parameter
                    when parameterDescription.ParameterDescriptor is ControllerParameterDescriptor descriptor:
                    {
                        return descriptor.ParameterInfo.HasDefaultValue
                            ? descriptor.ParameterInfo.DefaultValue
                            : null;
                    }
                default:
                    return null;
            }
        }
    }
}