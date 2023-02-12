using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentMVP
{
    public interface ISport
    {
        Players Calculate(List<string> matchData);
    }

}
