using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using StarterProject.Api.Data;

namespace StarterProject.Api.Tests.Features
{
    public class BaseTest
    {
        public DataContext Context;
        public ThreadLocal<DbContextOptions<DataContext>> Options;

        [SetUp]
        public void SetUp()
        {
            Options = new ThreadLocal<DbContextOptions<DataContext>>
            {
                Value = new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options
            };

            Context = new DataContext(Options.Value);
        }

        private readonly ThreadLocal<int> _nextId = new ThreadLocal<int> { Value = 100 };
        public int NextId => ++_nextId.Value;

        public void FreshContext(Action<DataContext> action)
        {
            using (var context = new DataContext(Options.Value))
            {
                action(context);
            }
        }
    }
}
