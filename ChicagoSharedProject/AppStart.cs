using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Autofac;
using TabsAdmin.Mobile.Shared.Models;
using TabsAdmin.Mobile.Shared.Interfaces;
using TabsAdmin.Mobile.Shared.Managers;
using TabsAdmin.Mobile.Shared.WebServices;

namespace TabsAdmin.Mobile.Shared
{
    public class AppStart
    {

        #region Constants, Enums, and Variables

        #endregion

        #region Properties

        /// <summary>
        /// Gets or set the AutoFac Container
        /// </summary>
        public static IContainer AutoFacContainer { get; set; }

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public static void Init()
        {
            try
            {

                var builder = new ContainerBuilder();

                builder.RegisterInstance(new UsersService()).As<Interfaces.Users.IUserFactory>();
                builder.RegisterType<Managers.Users.UsersFactory>();

                builder.RegisterInstance(new BusinessService()).As<Interfaces.Businesses.IBusinessFactory>();
                builder.RegisterType<Managers.Businesses.BusinessFactory>();

                builder.RegisterInstance(new IndividualsService()).As<Interfaces.Individuals.IIndividualFactory>();
                builder.RegisterType<Managers.Individuals.IndividualFactory>();

                builder.RegisterInstance(new VerificationCodeService()).As<IVerificationCode>();
                builder.RegisterType<VerificationCodeFactory>();

                builder.RegisterInstance(new ToastersService()).As<Interfaces.Individuals.IToastersFactory>();
                builder.RegisterType<Managers.Individuals.ToastersFactory>();

                builder.RegisterInstance(new AddressService()).As<Interfaces.Businesses.IAddressFactory>();
                builder.RegisterType<Managers.Businesses.AddressFactory>();

                builder.RegisterInstance(new BusinessTypesService()).As<Interfaces.Businesses.IBusinessTypesFactory>();
                builder.RegisterType<Managers.Businesses.BusinessTypesFactory>();

                builder.RegisterInstance(new SMSMessageService()).As<Interfaces.Individuals.ISMSMessageFactory>();
                builder.RegisterType<Managers.Individuals.SMSMessageFactory>();

                builder.RegisterInstance(new NotificationRegisterService()).As<INotificationRegisterFactory>();
                builder.RegisterType<NotificationRegisterFactory>();

                builder.RegisterInstance(new CheckInService()).As<Interfaces.CheckIns.ICheckInFactory>();
                builder.RegisterType<Managers.CheckIns.CheckInFactory>();

                builder.RegisterInstance(new ReportedSpamCheckInService()).As<Interfaces.Reports.Spams.IReportedSpamCheckInFactory>();
                builder.RegisterType<Managers.Reports.Spams.ReportedSpamCheckInFactory>();

                builder.RegisterInstance(new InappropriateReportCheckInService()).As<Interfaces.Reports.InappropriateReports.IInappropriateReportCheckInFactory>();
                builder.RegisterType<Managers.Reports.InappropriateReports.InappropriateReportCheckInFactory>();

                builder.RegisterInstance(new ReportedUserService()).As<Interfaces.Reports.Users.IReportedUserFactory>();
                builder.RegisterType<Managers.Reports.Users.ReportedUserFactory>();

                AutoFacContainer = builder.Build();

            } catch(Exception ex)
            {
                var a = ex;
            }
        }

        #endregion

    }
}
