using Services.Interfaces.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Testing.Services.Input
{
    internal class ChangeRequestServiceTests
    {
        private IChangeRequestService _changeRequestService;

        //1) Generate Change Request Log

        //a) missing entries from persisted are added (from DTOs)

        //b) missing entries from input are removed  (from DTOs)

        //c) existing entries with different fields are updated (from DTOs)

        //d) all operations above in combination (from DTOs)




        //2) Commit Change Request


        //3) Deny Change Request

    }
}
