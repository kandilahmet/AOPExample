using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPExample.Application.Attributes
{
    [AttributeUsage(
           AttributeTargets.Class //Class'larda kullılsın
          | AttributeTargets.Method  //Metod'larda kullılsın
          | AttributeTargets.Assembly //Assembly'de kullılsın
          , AllowMultiple = true//Birden fazla attribute tanımına izin verelim
          , Inherited = true //Kalıtımla devralınmasına izin verelim
          )]
    public abstract class AttributeBase : Attribute
    {
        public int Priority { get; set; }
    }
}
