using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            DataClasses1DataContext lq = new DataClasses1DataContext();
            lq.Table_1.InsertOnSubmit(new Table_1()
            {
                Name = "ADF",
                Type = "",
                TypeSmall = "",
                Location = 0,
                Nums = 0,
                Information = ""
            });
            lq.SubmitChanges();
            var query = from s in lq.Table_1
                        select s;
            var editStudent = lq.Table_1.SingleOrDefault<Table_1>(s => s.ID == 5);
            if(editStudent != null)
            {
                editStudent.Name = "dfv";
            }
            foreach (Table_1 t in query)
            {
                if (t.Name == "c")
                {
                    t.Name = "j";
                }
            }
            lq.SubmitChanges();
            query = from s in lq.Table_1
                    select s;
            foreach (Table_1 t in query)
            {
                Console.WriteLine(t.Name);
            }
            Console.Read();
        }
    }
}
