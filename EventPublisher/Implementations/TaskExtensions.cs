using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyEventPublisher.Implementations
{
    public static class TaskExtensions
    {
        public static Task[][] Chunk(this List<Task> tasks, int paralelismDegree)
        {
            int count = (int)Math.Ceiling((decimal)tasks.Count / paralelismDegree);

            var result = new Task[count][];

            int index = 0;

            for (int i = 0; i < tasks.Count; i += paralelismDegree)
            {
                var chuckedList = tasks.Skip(i).Take(paralelismDegree).ToArray();

                result[index] = chuckedList;

                index++;

            }
            return result;
        }
    }
}
