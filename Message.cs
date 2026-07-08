using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymApplicationV2._0
{
    public class Message
    {
        public static void MessageWindowOk(string stringMessage)
        {
            MessageBox.Show(
                stringMessage,
                "Сообщение",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static DialogResult MessageWindowYesNo(string stringMessage)
        {
            return MessageBox.Show(
                stringMessage,
                "Сообщение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);
        }
    }
}
