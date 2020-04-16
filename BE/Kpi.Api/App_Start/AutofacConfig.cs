using Autofac;
using Autofac.Integration.WebApi;
using Kpi.DataAccess.Repo.Base;
using Kpi.DataAccess.Repo.EFRepos.EVoucher;
using Kpi.DataAccess.Repo.EFRepos.File;
using Kpi.DataAccess.Repo.EFRepos.Kpi;
using Kpi.DataAccess.Repo.EFRepos.Orgs;
using Kpi.DataAccess.Repo.EFRepos.Reports;
using Kpi.DataAccess.Repo.EFRepos.Sys;
using Kpi.Service.ClientServices;
using Kpi.Service.DataServices.EVoucher;
using Kpi.Service.DataServices.Kpi;
using Kpi.Service.DataServices.Org;
using Kpi.Service.DataServices.Orgs;
using Kpi.Service.DataServices.Reports;
using Kpi.Service.DataServices.SysServices;
using Kpi.Service.FileServices;
using System.Reflection;
using System.Web.Http;

namespace Kpi.Api.App_Start
{
    public class AutofacConfig
    {
        public static IContainer Container;

        public static void Register()
        {
            Initialize(GlobalConfiguration.Configuration);
        }

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            //Register your Web API controllers.  
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            RegisterDependency(builder);
            //Set the dependency resolver to be Autofac.  
            Container = builder.Build();
            return Container;
        }

        /// <summary>
        /// Denpendency user register
        /// </summary>
        private static void RegisterDependency(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerDependency();
            builder.RegisterType<MenuRepository>().As<IMenuRepository>().InstancePerDependency();
            builder.RegisterType<MenuService>().As<IMenuService>().InstancePerDependency();
            builder.RegisterType<KpiEvaluationRepository>().As<IKpiEvaluationRepository>().InstancePerRequest();
            builder.RegisterType<KpiService>().As<IKpiService>().InstancePerRequest();
            builder.RegisterType<DiaryCriterionDetailRepository>().As<IDiaryCriterionDetailRepository>().InstancePerRequest();
            builder.RegisterType<DiaryCriterionDetailService>().As<IDiaryCriterionDetailService>().InstancePerRequest();
            builder.RegisterType<EventDiaryRepository>().As<IEventDiaryRepository>().InstancePerRequest();
            builder.RegisterType<EventDiaryService>().As<IEventDiaryService>().InstancePerRequest();
            builder.RegisterType<OrgUserRepository>().As<IOrgUserRepository>().InstancePerDependency();
            builder.RegisterType<UserOrgService>().As<IUserOrgService>().InstancePerDependency();
            builder.RegisterType<CriterionCatalogService>().As<ICriterionCatalogService>().InstancePerDependency();
            builder.RegisterType<CriterionCatalogRepository>().As<ICriterionCatalogRepository>().InstancePerDependency();
            builder.RegisterType<EventDiaryConfigService>().As<IEventDiaryConfigService>().InstancePerDependency();
            builder.RegisterType<EventDiaryConfigRepository>().As<IEvenDiaryConfigRepository>().InstancePerDependency();
            builder.RegisterType<ViewEventDiaryConfigService>().As<IViewEventDiaryConfigService>().InstancePerDependency();
            builder.RegisterType<ViewEventDiaryConfigRepository>().As<IViewEventDiaryConfigRepository>().InstancePerDependency();
            builder.RegisterType<ViewUserEventDiaryService>().As<IViewUserEventDiaryService>().InstancePerDependency();
            builder.RegisterType<ViewUserEventDiaryRepository>().As<IViewUserEventDiaryRepository>().InstancePerDependency();
            builder.RegisterType<KpiPeriodConfigService>().As<IKpiPeriodConfigService>().InstancePerDependency();
            builder.RegisterType<KpiPeriodConfigRepository>().As<IKpiPeriodConfigRepository>().InstancePerDependency();
            builder.RegisterType<View_KpiEvaluation_OrganizationService>().As<IView_KpiEvaluation_OrganizationService>().InstancePerDependency();
            builder.RegisterType<View_KpiEvaluation_OrganizationRepository>().As<IView_KpiEvaluation_OrganizationRepository>().InstancePerDependency();
            builder.RegisterType<View_Statistics_ReportsService>().As<IView_Statistics_ReportsService>().InstancePerDependency();
            builder.RegisterType<View_Statistics_ReportsRepository>().As<IView_Statistics_ReportsRepository>().InstancePerDependency();
            builder.RegisterType<HttpClientLoginAdApiService>().As<IClientLoginAdService>().InstancePerDependency();
            builder.RegisterType<EmpTransferService>().As<IEmpTransferService>().InstancePerDependency();
            builder.RegisterType<EmpTransferRepository>().As<IEmpTransferRepository>().InstancePerDependency();
            builder.RegisterType<EmploymentHistoryService>().As<IEmploymentHistoryService>().InstancePerDependency();
            builder.RegisterType<EmploymentHistoryRepository>().As<IEmploymentHistoryRepository>().InstancePerDependency();
            builder.RegisterType<OrgService>().As<IOrgService>().InstancePerDependency();
            builder.RegisterType<OrgRepository>().As<IOrgRepository>().InstancePerDependency();
            builder.RegisterType<KpiCriterionTypeService>().As<IKpiCriterionTypeService>().InstancePerDependency();
            builder.RegisterType<KpiCriterionTypeRepository>().As<IKpiCriterionTypeRepository>().InstancePerDependency(); 
            builder.RegisterType<NotificationService>().As<INotificationService>().InstancePerDependency();
            builder.RegisterType<NotificationRepository>().As<INotificationRepository>().InstancePerDependency();
            builder.RegisterType<FileService>().As<IFileService>().InstancePerDependency();
            builder.RegisterType<FileRepository>().As<IFileRepository>().InstancePerDependency(); 
            builder.RegisterType<EvoucherTypeService>().As<IEvoucherTypeService>().InstancePerDependency();
            builder.RegisterType<EvoucherTypeRepository>().As<IEvoucherTypeRepository>().InstancePerDependency();
            builder.RegisterType<EvoucherBudgetService>().As<IEvoucherBudgetService>().InstancePerDependency();
            builder.RegisterType<EvoucherBudgetRepository>().As<IEvoucherBudgetRepository>().InstancePerDependency();
            builder.RegisterType<View_EVoucherUserService>().As<IView_EVoucherUserService>().InstancePerDependency();
            builder.RegisterType<View_EVoucherUserRepository>().As<IView_EVoucherUserRepository>().InstancePerDependency();
            builder.RegisterType<EvoucherBudgetDistributeService>().As<IEvoucherBudgetDistributeService>().InstancePerDependency();
            builder.RegisterType<EvoucherBudgetDistributeRepository>().As<IEvoucherBudgetDistributeRepository>().InstancePerDependency();
            builder.RegisterType<EVoucherCodeLineService>().As<IEVoucherCodeLineService>().InstancePerRequest();
            builder.RegisterType<EvoucherCodeLineRepository>().As<IEvoucherCodeLineRepository>().InstancePerRequest();
        }
    }
}