using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.CMS.Common.Interface
{
    public interface IEnvironment
    {
        string GetDbConnStr();

        string GetImgurClientID();

        string GetImgurClientSecret();
    }
}
