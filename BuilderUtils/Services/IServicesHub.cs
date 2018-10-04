using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderUtils.Services
{
    public interface IServicesHub
    {
        void CreateOutputHub();
        void CreateOutputHub(bool verbose, string stateId, string conditionalVariable, string path);
        void InsertExtrasEventTrack();
        void InsertChatbaseRequests();
    }
}
