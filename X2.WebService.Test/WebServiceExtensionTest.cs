using ComTech.X2.Common;
using NUnit.Framework;
using Shouldly;
using X2WebService;

namespace X2.WebService.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var queryInfo = new GetterInfo();
            queryInfo.Name = "string";
            WebServiceExtension.CleanSwaggerJson(queryInfo);
            queryInfo.Name.ShouldBeNull();
            queryInfo.SourceId.ShouldBeNull();


        }
    }
}