using System;
using System.Linq.Expressions;
using ComTech.X2.Common;
using NUnit.Framework;
using Shouldly;

namespace X2.WebService.Test;

[TestFixture]
public class QueryParameterInfoTest
{
    [Test]
    public void DateTypeTest()
    {
        var queryItem = new QueryParameterType("'12/7/1941'");
        queryItem.Type.ShouldBe(QueryParameterTypeValue.DateOnly);
    }
    [Test]
    public void IntTypeTest()
    {
        var queryItem = new QueryParameterType("1234");
        queryItem.Type.ShouldBe(QueryParameterTypeValue.Int);
    }
    [Test]
    public void DoubleTypeTest()
    {
        var queryItem = new QueryParameterType("1234.56");
        queryItem.Type.ShouldBe(QueryParameterTypeValue.Double);
    }
    [Test]
    public void DoubleEmptyTypeTest()
    {
        var queryItem = new QueryParameterType("1234.");
        queryItem.Type.ShouldBe(QueryParameterTypeValue.Double);
    }
    [Test]
    public void StringTypeTest()
    {
        var queryItem = new QueryParameterType("'abcd'");
        queryItem.Type.ShouldBe(QueryParameterTypeValue.String);
    }
    [Test]
    public void BooleanTypeTest()
    {
        var queryItem = new QueryParameterType("true");
        queryItem.Type.ShouldBe(QueryParameterTypeValue.Boolean);
    }

}