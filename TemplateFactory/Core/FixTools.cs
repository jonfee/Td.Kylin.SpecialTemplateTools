using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateFactory.Core
{
    /// <summary>
    /// 不动点算子工具类
    /// </summary>
    public static class FixTools
    {
        static Func<T, TResult> Fix<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> g)
        {
            return (x) => g(Fix(g))(x);
        }
    }
}
