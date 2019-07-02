using Nancy;
using System;

namespace NancyIntro
{

    public class CurrentDateTimeModule
      : NancyModule
    {
        public CurrentDateTimeModule()
        {
            Get("/", _ => DateTime.UtcNow);
        }
    }
}
