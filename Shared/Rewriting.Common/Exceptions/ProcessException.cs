using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Common.Exceptions
{
    public class ProcessException : Exception
    {
        /// <summary>
        ///Error code
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Error name
        /// </summary>
        public string Name { get; }

        public ProcessException()
        {
        }

        public ProcessException(string message) : base(message)
        {
        }

        public ProcessException(Exception inner) : base(inner.Message, inner)
        {
        }

        public ProcessException(string message, Exception inner) : base(message, inner)
        {
        }

        public ProcessException(string code, string message) : base(message)
        {
            Code = code;
        }

        public ProcessException(string code, string message, Exception inner) : base(message, inner)
        {
            Code = code;
        }
    }
}
