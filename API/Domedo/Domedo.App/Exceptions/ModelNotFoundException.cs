using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domedo.App.Exceptions
{
    public class ModelNotFoundException : Exception
    {
        public ModelNotFoundException() : base("Please the requested model could not be found")
        {

        }

        public ModelNotFoundException(string id, string modelType) : base($"{modelType} with id, '{id}' not found")
        {

        }

        public ModelNotFoundException(string message) : base(message)
        {

        }
    }
}
