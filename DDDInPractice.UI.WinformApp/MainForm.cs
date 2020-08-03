using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using DDDInPractice.Persistence;
using DDDInPractice.Persistence.Features;
using DDDInPractice.Persistence.Infrastructure;
using Microsoft.Extensions.Caching.Memory;

namespace DDDInPractice.UI.WinformApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (var scope = Program.Container.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();

                await mediator.SendAsync(new TestCommand());
            }
        }
    }
}