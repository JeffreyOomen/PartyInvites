using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using PartyInvites.Domain.Abstract;
using PartyInvites.Domain.Concrete;

namespace PartyInvites.WebUI.Infrastructure {
    public class NinjectDependencyResolver : IDependencyResolver{
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam) {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType) {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType) {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings() {
            //Singleton lifeCycle so just one instance will be used for every request
            kernel.Bind<IGuestRepository>().To<EFGuestRepository>().InSingletonScope();
        }
    }
}