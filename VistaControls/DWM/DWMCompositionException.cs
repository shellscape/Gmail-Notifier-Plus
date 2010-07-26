/*****************************************************
 *            Vista Controls for .NET 2.0
 * 
 * http://www.codeplex.com/vistacontrols
 * 
 * @author: Lorenz Cuno Klopfenstein
 * Licensed under Microsoft Community License (Ms-CL)
 * 
 *****************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace VistaControls.Dwm
{
    [Serializable]
    class DwmCompositionException : Exception
    {
        public DwmCompositionException(string m)
            : base(m) {
        }

        public DwmCompositionException(string m, Exception innerException)
            : base(m, innerException) {
        }

        public DwmCompositionException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }
    }
}
