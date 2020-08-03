using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using DDDInPractice.Persistence;
using DDDInPractice.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DDDInPractice.UI.WinformApp
{
    static class Program
    {
        public static IContainer Container { get; set; }

        public static string ConnectionString =
            ConfigurationManager.ConnectionStrings["DDDInPractice"].ConnectionString;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            InitAutofac();
            Application.Run(new MainForm());
        }
        
        private static void InitAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModule());
            
            builder.Register(c =>
                {
                    var opt = new DbContextOptionsBuilder<AppDbContext>();
                    opt.UseSqlServer(ConnectionString);

                    return new AppDbContext(opt.Options);
                })
                .As<AppDbContext>()
                .InstancePerLifetimeScope();

            Container = builder.Build();
        }
    }
}