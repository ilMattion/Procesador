using AutoFixture;

namespace Procesador.Tests
{
    public abstract class Tests
    {
        protected Fixture fixture;

        public Tests()
        {
            fixture = new Fixture();
        }
    }
}
