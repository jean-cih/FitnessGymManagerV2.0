using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymApplicationV2._0.Helpers
{
    public class MessageHelper
    {
        public static void MessageWindowOk(string stringMessage, string status)
        {
            if (status == "Ошибка")
            {
                Logger.Error(stringMessage);
            }
            else if(status == "Предупреждение")
            {
                Logger.Warning(stringMessage);
            }
            else
            {
                Logger.Info(stringMessage);
            }
                
            MessageBox.Show(
                stringMessage,
                status,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static DialogResult MessageWindowYesNo(string stringMessage)
        {
            Logger.Info(stringMessage);

            return MessageBox.Show(
                stringMessage,
                "Сообщение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);
        }
    }
}
