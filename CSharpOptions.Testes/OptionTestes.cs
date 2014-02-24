using FluentAssertions;
using NUnit.Framework;

namespace CSharpOptions.Testes
{
    [TestFixture]
    public class OptionTestes
    {
        [Test]
        public void OptionNoneTeste()
        {
            var pessoa = new Pessoa
            {
                Nome = "Robb Stark"
            };

            var optionPessoa = Option.Create(pessoa);

            var logradouro = optionPessoa
                                    .Map(p => p.Endereco)
                                    .Map(e => e.Cidade)
                                    .GetOrElse("Não informado");

            logradouro.Should().Be("Não informado");
        }

        [Test]
        public void OptionSomeTeste()
        {
            var pessoa = new Pessoa
            {
                Nome = "Robb Stark",
                Endereco = new Endereco
                {
                    Cidade = "Winterfell"
                }
            };

            var optionPessoa = Option.Create(pessoa);

            var logradouro = optionPessoa
                                    .Map(p => p.Endereco)
                                    .Map(e => e.Cidade)
                                    .GetOrElse("Não informado");

            logradouro.Should().Be("Winterfell");
        }

        [Test]
        public void FromValue()
        {
            var option = Option.From(1);

            option.Should().Be(Option.From(1));
        }

        [Test]
        public void ToOption()
        {
            var option = 1.ToOption();

            option.Should().Be(Option.From(1));
        }

        [Test]
        public void FromNull()
        {
            var option = Option.From<string>(null);

            option.Should().Be(Option.None<string>());
        }
    }

    public class Pessoa
    {
        public string Nome { get; set; }

        public int Idade { get; set; }

        public Endereco Endereco { get; set; }
    }

    public class Endereco
    {
        public string Bairro { get; set; }

        public string Cidade { get; set; }
    }
}
