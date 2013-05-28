using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpleth
{
    /// <summary>
    /// Represent the Exception of RplethProxy
    /// </summary>
    class RplethException : ApplicationException
    {
        /// <summary>
        /// Constructor call basic ApplicationException
        /// </summary>
        public RplethException() : base() {}

        /// <summary>
        /// Constructor call basic ApplicationException add the message
        /// </summary>
        /// <param name="message"></param>
        public RplethException(string message) : base(message) {}

        /// <summary>
        /// Constructor call basic ApplicationException add the message and inherits from other exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public RplethException(string message, System.Exception inner) : base(message, inner) {}
        protected RplethException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context): base(info, context) {}
    }
}
