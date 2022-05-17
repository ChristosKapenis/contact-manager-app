using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ClosedXML.Excel;

namespace WPFUI
{
    public class XLDataAccess
    {
        public static void SaveXL(List<Contact> contacts, string path)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Contacts");

                for (int i = 0; i < contacts.Count; i++)
                {
                    worksheet.Cell($"A{i+1}").Value = contacts[i].Name;
                    worksheet.Cell($"B{i+1}").SetValue(contacts[i].CellphoneNumber);
                    worksheet.Cell($"C{i+1}").SetValue(contacts[i].TelephoneNumber);
                }

                workbook.SaveAs(path);
            }
        }

        public static List<Contact> LoadExcel(string path)
        {
            List<Contact> contacts = new List<Contact>();

            var wb = new XLWorkbook(path);
            var ws = wb.Worksheet(1);

            var firstRowUsed = ws.FirstRowUsed();
            var contactRow = firstRowUsed.RowUsed();

            string name, cellphone, telephone;

            //NOTE: What if there's an empty cell seperating two rows that contain info to be imported. The last row and the ones after that are ignored.
            //TODO: Find a better way to recognize when the actual last row is reached. (start from the ClosedXML docs)
            while (!contactRow.Cell(1).IsEmpty())
            {
                name = contactRow.Cell(1).GetString();
                cellphone = contactRow.Cell(2).GetString();
                telephone = contactRow.Cell(3).GetString();
                
                //One cell might contain multiple telephone numbers separated by a '-' character.
                //In this case a duplicate contact is created for each separate telephone number.
                //This isn't the best solution, but it works for now.
                //TODO: Add multiple numbers support for each contact.
                string[] s = telephone.Trim().Split("-");

                for (int i = 0; i < s.Length; i++)
                {
                    contacts.Add(new Contact { Name = name, CellphoneNumber = cellphone, TelephoneNumber = s[i] });
                }

                contactRow = contactRow.RowBelow();
            }

            return contacts;
        }
    }
}
