using AOPExample.Application.Attributes;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOPExample.Application.Interceptors
{
    public class InterceptorSelectorAsync<TImplementation> : IInterceptorSelector where TImplementation : class
    {
        //Hangi Interceptor'leri kullanmak istediğimizi seçeriz. Bir nevi filtreleme yapmamızı sağlar.
        public IInterceptor[]? SelectInterceptors(Type type, MethodInfo methodInfo, IInterceptor[] interceptors)

        {
            //type => { Name = "MyClass" FullName = "AOP.Infrastructure.MyClass"}
            //typeInfo.Name => CalcAsync

            //Attributebase tipinde bir liste oluşturalım.
            List<AttributeBase> attributeBaseList = new();

            //Çağrılan Metodun bulunduğu Class'a bakıyoruz, AttributeBase tipinde attribute'ler var mı ?
            var classAtributeBaseList = type.GetCustomAttributes<AttributeBase>(true);//.Cast<AttributeBase>();

            if (classAtributeBaseList is not null)
                attributeBaseList.AddRange(classAtributeBaseList);

            //Çağrılan metod'un parametrelerini getir.
            var methodParameterTypes = methodInfo.GetParameters().Select(s => s.ParameterType).ToArray();

            var concreteMethod = typeof(TImplementation).GetMethod(methodInfo.Name, methodParameterTypes);

            if (concreteMethod is not null)
            {
                //Çağrılan Metod'da bulunan AttributeBase tipindeki Attribute'leri getir
                var methodAttributeBaseList = concreteMethod.GetCustomAttributes<AttributeBase>(true);//.Cast<AttributeBase>();

                attributeBaseList.AddRange(methodAttributeBaseList);
            }

            var interceptorList = new List<IInterceptor>();

            foreach (var item in attributeBaseList.OrderBy(o => o.Priority))
            {
                var baseType = typeof(InterceptorBaseAsync<>).MakeGenericType(item.GetType());// InterceptorBaseAsync

                foreach (var _interceptor in interceptors)
                { 
                    //Eğer true ise interceptorList listesine ekleriz.
                    if (((AsyncDeterminationInterceptor)_interceptor).AsyncInterceptor.GetType().IsAssignableTo(baseType))
                        interceptorList.Add(_interceptor);

                    //.AsyncInterceptor=> altta yatan async interceptor'u döndürür
                    //.GetType ile Tip değerini alıp
                    //.IsAssignableTo metodu ile içine parametre olarak verdiğimiz baseType ile temsil edebiliyor muyuz kontrol ederiz.

                }

            }

            return [.. interceptorList];

        }
    }
}
