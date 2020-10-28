using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TsvToHtmlTable
{
    public class Sub
    {
        public void Method()
        {
            Console.WriteLine("Sub.Method()");
            var list = new List<int> { 1, 84, 95, 95, 40, 6 };
            this.show(this.getByQueryStatement(list));
            this.show(this.getByMethodStatement(list));
        }
        private IEnumerable<int> getByQueryStatement(List<int> list)
        {
            return from x in list
                    where x % 2 == 0
                    orderby x
                    select x * 3;
        }
        private IEnumerable<int> getByMethodStatement(List<int> list)
        {
            return list
                    .Where(x => x % 2 == 0)
                    .OrderBy(x => x)
                    .Select(x => x * 3);
        }
        private void show(IEnumerable<int> query)
        {
            Console.WriteLine("----------");
            foreach(var item in query)
            {
                Console.WriteLine(item);
            }
        }
    }
}
