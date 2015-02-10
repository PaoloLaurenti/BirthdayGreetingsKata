using System;
using System.Linq.Expressions;
using BirthdayGreetings.Core.Employees;
using Common.Logging;
using Common.Logging.Configuration;
using Common.Logging.Simple;
using FakeItEasy;

namespace BirthdayGreetings.Core.Test.Context
{
    internal class TestContext
    {
        private static readonly DateTime ChosenDate = new DateTime(2014, 4, 21);

        private GivenContext _givenContext;
        private WhenContext _whenContext;
        private ThenContext _thenContext;
        private IEmployeeGateway _employeeGateway;
        private readonly MockSimpleHandlerFactory _mockSimpleHandlerFactory;
        private readonly MockSendBirthdayGreetingsByEmailCommandHandler _mockSendBirthdayGreetingsByEmailCommandHandler;

        internal TestContext()
        {
            _mockSendBirthdayGreetingsByEmailCommandHandler = new MockSendBirthdayGreetingsByEmailCommandHandler(GetLogger());
            _mockSimpleHandlerFactory = new MockSimpleHandlerFactory(_mockSendBirthdayGreetingsByEmailCommandHandler);
        }

        private IEmployeeGateway EmployeesGateway
        {
            get { return _employeeGateway ?? (_employeeGateway = A.Fake<IEmployeeGateway>()); }
        }

        private GivenContext GivenContext
        {
            get { return _givenContext ?? (_givenContext = new GivenContext(EmployeesGateway, _mockSendBirthdayGreetingsByEmailCommandHandler, ChosenDate)); }
        }

        private WhenContext WhenContext
        {
            get { return _whenContext ?? (_whenContext = new WhenContext(EmployeesGateway, _mockSimpleHandlerFactory, ChosenDate)); }
        }

        private ThenContext ThenContext
        {
            get { return _thenContext ?? (_thenContext = new ThenContext(GivenContext, WhenContext, _mockSendBirthdayGreetingsByEmailCommandHandler)); }
        }

        internal TestContext Given(Expression<Action<GivenContext>> expression)
        {
            InvokeExpression(expression, GivenContext);
            return this;
        }

        internal TestContext When(Expression<Action<WhenContext>> expression)
        {
            InvokeExpression(expression, WhenContext);
            return this;
        }

        internal TestContext Then(Expression<Action<ThenContext>> expression)
        {
            InvokeExpression(expression, ThenContext);
            return this;
        }

        private static void InvokeExpression<T>(Expression<Action<T>> expression, T actionParameter)
        {
            expression.Compile().Invoke(actionParameter);            
        }

        private static ILog GetLogger()
        {
            var properties = new NameValueCollection();
            properties["showDateTime"] = "true";
            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter(properties);
            return LogManager.GetLogger(typeof(TestContext));
        }
    }
}