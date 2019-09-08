using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Sample.Services
{
    public interface IMemberService
    {
        int GetMemberBalance(int memberId);
    }

    public class MemberService : IMemberService
    {
        public int GetMemberBalance(int memberId)
        {
            return 100;
        }
    }
}
