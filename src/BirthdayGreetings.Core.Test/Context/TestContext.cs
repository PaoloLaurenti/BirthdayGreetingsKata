using System;
using System.Linq.Expressions;
using Common.Logging;
using FakeItEasy;

namespace BirthdayGreetings.Core.Test.Context
{
    internal class TestContext
    {
        private GivenContext _givenContext;
        private WhenContext _whenContext;
        private ThenContext _thenContext;
        private IEmployeeGateway _employeeGateway;
        private IGreetingsChannelGateway _greetingsChannelGateway;

        internal TestContext()
        {
            InitGivenContext();
            InitWhenContext();
            InitThenContext();
        }

        private void InitGivenContext()
        {
            var chosenDate = new DateTime(2014, 4, 21);
            _givenContext = new GivenContext(EmployeesGateway, chosenDate);
        }

        private void InitWhenContext()
        {
            var sut = new SendBirthdayGreetingsCommandHandler(EmployeesGateway, GreetingsChannelGateway, A.Fake<ILog>());
            _whenContext = new WhenContext(sut);
        }

        private void InitThenContext()
        {
            _thenContext = new ThenContext(GreetingsChannelGateway);
        }

        private IEmployeeGateway EmployeesGateway
        {
            get { return _employeeGateway ?? (_employeeGateway = A.Fake<IEmployeeGateway>()); }
        }

        private IGreetingsChannelGateway GreetingsChannelGateway
        {
            get { return _greetingsChannelGateway ?? (_greetingsChannelGateway = A.Fake<IGreetingsChannelGateway>()); }
        }

        internal TestContext Given(Expression<Action<GivenContext>> expression)
        {
            InvokeExpression(expression, _givenContext);
            return this;
        }

        internal TestContext When(Expression<Action<WhenContext>> expression)
        {
            InvokeExpression(expression, _whenContext);
            return this;
        }

        internal TestContext Then(Expression<Action<ThenContext>> expression)
        {
            InvokeExpression(expression, _thenContext);
            return this;
        }

        private static void InvokeExpression<T>(Expression<Action<T>> expression, T actionParameter)
        {
            expression.Compile().Invoke(actionParameter);            
        }
    }
}