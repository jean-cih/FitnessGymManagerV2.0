using GymApplicationV2._0.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GymApplicationV2._0.Helpers
{
    internal static class FontHelper
    {
        internal static void ApplyFontSettings(Form form, string[] notChangeableTexts = null)
        {
            foreach (Control control in GetAllControls(form))
            {
                ApplyFontToControl(control, notChangeableTexts);
            }
        }

        internal static void ApplyFontToControl(Control control, string[] notChangeableTexts)
        {
            // Кнопки
            if (control is JeanModernButton button)
            {
                int fontSize = Math.Min(DataConfig.sizeFontButtons, 12);
                button.Font = new Font(button.Font.FontFamily, fontSize, button.Font.Style);
            }
            // Метки 
            else if (control is Label label && (notChangeableTexts == null || !notChangeableTexts.Contains(label.Text)))
            {
                if (label.Tag?.ToString() == "content")
                {
                    label.Font = new Font(label.Font.FontFamily, DataConfig.sizeFontText, label.Font.Style);
                }
                else
                {
                    label.Font = new Font(label.Font.FontFamily, DataConfig.sizeFontCaptions, label.Font.Style);
                }
            }
            else if (control is jeanSoftTextBox || control is JeanTextBox)
            {
                int fontSize = Math.Min(DataConfig.sizeFontText, 12);
                var textControl = (Control)control;
                textControl.Font = new Font(textControl.Font.FontFamily, fontSize, textControl.Font.Style);
            }
            // DataGridView
            else if (control is DataGridView dataGrid)
            {
                dataGrid.Font = new Font(dataGrid.Font.FontFamily, DataConfig.sizeFontTables, dataGrid.Font.Style);
            }
            // ComboBox
            else if (control is ComboBox comboBox)
            {
                comboBox.Font = new Font(comboBox.Font.FontFamily, DataConfig.sizeFontText, comboBox.Font.Style);
            }
            // RadioButton
            else if (control is RadioButton radioButton)
            {
                int fontSize = Math.Min(DataConfig.sizeFontText, 12);
                radioButton.Font = new Font(radioButton.Font.FontFamily, fontSize, radioButton.Font.Style);
            }
            // CheckBox
            else if (control is CheckBox checkBox)
            {
                int fontSize = Math.Min(DataConfig.sizeFontText, 12);
                checkBox.Font = new Font(checkBox.Font.FontFamily, fontSize, checkBox.Font.Style);
            }
        }


        internal static IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                yield return control;

                if (control.HasChildren)
                {
                    foreach (Control child in GetAllControls(control))
                    {
                        yield return child;
                    }
                }
            }
        }

        internal static void UpdateAllOpenForms(string[] notChangeableTexts)
        {
            foreach (Form form in Application.OpenForms)
            {
                ApplyFontSettings(form, notChangeableTexts);
            }
        }
    }
}