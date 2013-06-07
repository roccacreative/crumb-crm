[assembly: WebActivator.PreApplicationStartMethod(typeof(CrumbCRM.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(CrumbCRM.Web.App_Start.NinjectWebCommon), "Stop")]

namespace CrumbCRM.Web.App_Start
{
    using System;
    using System.Web;
    using CrumbCRM.Data;
    using CrumbCRM.Data.Entity.Entities;
    using CrumbCRM.Services;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using CrumbCRM.Services.Services;
    using System.Web.Security;
    using CrumbCRM.Web.Modules;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            // data
            kernel.Bind<IInvoiceRepository>().To<InvoiceEntities>();
            kernel.Bind<IQuoteRepository>().To<QuoteEntities>();
            kernel.Bind<IContactRepository>().To<ContactEntities>();
            kernel.Bind<ITaskRepository>().To<TaskEntities>();
            kernel.Bind<ILeadRepository>().To<LeadEntities>();
            kernel.Bind<INoteRepository>().To<NoteEntities>();
            kernel.Bind<IActivityRepository>().To<ActivityEntities>();
            kernel.Bind<ISaleRepository>().To<SaleEntities>();
            kernel.Bind<ICampaignRepository>().To<CampaignEntities>();
            kernel.Bind<ITagRepository>().To<TagEntities>();

            // services
            kernel.Bind<IInvoiceService>().To<InvoiceService>();
            kernel.Bind<IQuoteService>().To<QuoteService>();
            kernel.Bind<IContactService>().To<ContactService>();
            kernel.Bind<ITaskService>().To<TaskService>();
            kernel.Bind<ILeadService>().To<LeadService>();
            kernel.Bind<INoteService>().To<NoteService>();
            kernel.Bind<IActivityService>().To<ActivityService>();
            kernel.Bind<ISaleService>().To<SaleService>();
            kernel.Bind<ICampaignService>().To<CampaignService>();
            kernel.Bind<ITagService>().To<TagService>();

            kernel.Bind<IMembershipService>().To<MembershipService>();
            kernel.Bind<IMembershipRepository>().To<MembershipEntities>();

            kernel.Bind<IRoleService>().To<RoleService>();
            kernel.Bind<IRoleRepository>().To<RoleEntities>();

            kernel.Bind<MembershipProvider>().ToMethod(ctx => Membership.Provider);
            kernel.Bind<RoleProvider>().ToMethod(ctx => Roles.Provider);
            kernel.Bind<IHttpModule>().To<ProviderInitializationHttpModule>(); 

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }        
    }
}
