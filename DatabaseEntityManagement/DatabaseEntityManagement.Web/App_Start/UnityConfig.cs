using DatabaseEntityManagement.Data.Context;
using DatabaseEntityManagement.Data.Context.QueryContext.Service;
using DatabaseEntityManagement.Data.Repositories.Person;
using DatabaseEntityManagement.Services.Person;
using System;

using Unity;

namespace DatabaseEntityManagement.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // Instances
            container.RegisterInstance(typeof(DatabaseEntityManagementContext), new DatabaseEntityManagementContext());
            container.RegisterInstance(typeof(IUnityContainer), container);

            // Data Services
            container.RegisterType<IQueryContextService, QueryContextService>();

            // Repositories
            container.RegisterType<IPersonRepository, PersonRepository>();

            // Services
            container.RegisterType<IPersonService, PersonService>();

        }
    }
}