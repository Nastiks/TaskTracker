using System;
using System.Collections.Generic;
using System.Text;

namespace WebTasks.Shared
{
    public static class Routes
    {
        private const string root = "/api";

        public static class V1
        {
            private const string version = "/v1";
            public const string Tasks = root + version + "/tasks";
            public const string Projects = root + version + "/projects";
        }
    }
}
