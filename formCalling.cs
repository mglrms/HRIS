using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRISCDBS
{
    public static class FormManager
{
    public static T OpenForm<T>(T formInstance, Form parent = null) where T : Form, new()
    {
        if (formInstance == null || formInstance.IsDisposed)
        {
            formInstance = new T();

            if (parent != null && parent.IsMdiContainer)
            {
                formInstance.MdiParent = parent;
                formInstance.Dock = DockStyle.Fill;
            }

            formInstance.Show();
        }
        else
        {
            formInstance.BringToFront();
            formInstance.Activate();
        }

        return formInstance;
    }
}

}


