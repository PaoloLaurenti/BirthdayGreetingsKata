using System;
using System.Linq.Expressions;
using BirthdayGreetings.Core.Employees;
using BirthdayGreetings.Core.Greetings;
using BirthdayGreetings.Core.Test.Extension;
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
        private IGreetingsGateway _greetingsGateway;

        internal TestContext()
        {
            InitGivenContext();
            InitWhenContext();
            InitThenContext();
        }

        private IEmployeeGateway EmployeesGateway
        {
            get { return _employeeGateway ?? (_employeeGateway = A.Fake<IEmployeeGateway>()); }
        }

        private IGreetingsGateway GreetingsGateway
        {
            get { return _greetingsGateway ?? (_greetingsGateway = CreateFakeGreetingsChannelGateway()); }
        }

        private IGreetingsGateway CreateFakeGreetingsChannelGateway()
        {
            var fakeGreetingsChannelGateway = A.Fake<IGreetingsGateway>();
            fakeGreetingsChannelGateway.ConfigureToNotifyGreetingsSent(greetings => _thenContext.NotifyGreetingsSent(greetings));
            return fakeGreetingsChannelGateway;
        }

        private void InitGivenContext()
        {
            _givenContext = new GivenContext(EmployeesGateway, GreetingsGateway, ChosenDate);
        }

        private void InitWhenContext()
        {
            _whenContext = new WhenContext(EmployeesGateway, GreetingsGateway, ChosenDate);
        }

        private void InitThenContext()
        {
            _thenContext = new ThenContext(GreetingsGateway, _givenContext, _whenContext);
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