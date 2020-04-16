using System;
using System.Collections.Generic;
using Kpi.Core.DTO;

namespace Kpi.DataAccess.DataContext
{
    public partial class Sys_Menu : IEntityDTO
    {
    }

    public partial class Kpi_KpiEvaluation : IEntityDTO
    {
    }
    public partial class Sys_KpiPeriodConfig : IEntityDTO
    {
    }
    public partial class Kpi_DiaryCriterionDetail : IEntityDTO
    {
    }

    public partial class View_KpiPointEvaluation : IEntityDTO
    {
    }
    public partial class Org_UserOrg : IEntityDTO
    {
  
    }
    public partial class EmpTransfer : IEntityDTO
    {
    }

    public partial class View_User_EventDiary : IEntityDTO
    {
    }
    public partial class View_EventDiaryConfig : IEntityDTO
    {
    }

    public partial class Sys_EventDiaryConfig : IEntityDTO
    {
    }
    public partial class View_KpiEvaluation_Organization : IEntityDTO
    {
    }

    public partial class Org_Organization : IEntityDTO
    {
    }
    public partial class Kpi_CriterionType : IEntityDTO
    {
    }

    public partial class Kpi_CriterionCatalog : IEntityDTO
    {
    }

    public partial class Notification_Queue : IEntityDTO
    {
    }

    public partial class Notification_Type : IEntityDTO
    {
    }

    public partial class Kpi_Task
    {
        public virtual ICollection<Kpi_Task_File> Kpi_Task_File { get; set; }
    }

    public partial class E_VoucherType : IEntityDTO
    {
    }
}
