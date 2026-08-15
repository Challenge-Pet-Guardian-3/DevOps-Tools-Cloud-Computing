using System.Net.Http.Json;
using System.Text.Json.Serialization;
using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Application.Services.Implementations;

public sealed class EnderecoService(
    IRepository<Endereco> enderecoRepository,
    IRepository<Bairro> bairroRepository,
    IRepository<Cidade> cidadeRepository,
    IRepository<Estado> estadoRepository) : IEnderecoService
{
    private static readonly HttpClient HttpClient = new();

    public IReadOnlyList<EnderecoResponse> GetAll() =>
        enderecoRepository.GetAll().Select(EnderecoResponse.FromDomain).ToList();

    public EnderecoResponse? GetById(Guid id)
    {
        var e = enderecoRepository.GetById(id);
        return e is null ? null : EnderecoResponse.FromDomain(e);
    }

    public EnderecoResponse Create(EnderecoRequest request)
    {
        var resolved = ResolveAddressFromCep(request.Cep);
        var endereco = FindOrCreateByCepAndNumero(request.Cep, request.Numero, resolved.Rua, resolved.Bairro.Id);
        return EnderecoResponse.FromDomain(endereco);
    }

    public bool Delete(Guid id) => enderecoRepository.Delete(id);

    private Endereco FindOrCreateByCepAndNumero(string cep, string numero, string rua, Guid bairroId)
    {
        var cepLimpo = cep.Trim().Replace("-", "");
        var numeroLimpo = numero.Trim();

        var endereco = enderecoRepository.GetAll().FirstOrDefault(e =>
            e.Cep == cepLimpo && e.Numero == numeroLimpo && e.BairroId == bairroId);

        if (endereco == null)
        {
            endereco = new Endereco(cepLimpo, rua, numeroLimpo, bairroId);
            enderecoRepository.Add(endereco);
        }

        return endereco;
    }

    private ResolvedAddress ResolveAddressFromCep(string cep)
    {
        var cepLimpo = cep.Trim().Replace("-", "");
        var url = $"https://viacep.com.br/ws/{cepLimpo}/json/";

        try
        {
            var response = HttpClient.GetFromJsonAsync<ViaCepResponse>(url).GetAwaiter().GetResult();

            if (response == null || response.Erro == true)
                throw new InvalidOperationException($"CEP {cepLimpo} não encontrado.");

            var estadoNome = response.Estado ?? response.Uf ?? throw new InvalidOperationException("Estado não informado na resposta do CEP.");
            var cidadeNome = response.Localidade ?? throw new InvalidOperationException("Cidade não informada na resposta do CEP.");
            var bairroNome = response.Bairro ?? throw new InvalidOperationException("Bairro não informado na resposta do CEP.");
            var ruaNome = response.Logradouro ?? "";

            var estado = FindOrCreateEstado(estadoNome);
            var cidade = FindOrCreateCidade(cidadeNome, estado.Id);
            var bairro = FindOrCreateBairro(bairroNome, cityId: cidade.Id);

            return new ResolvedAddress(ruaNome, bairro);
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            throw new InvalidOperationException("Erro ao consultar o serviço de CEP.", ex);
        }
    }

    private Estado FindOrCreateEstado(string nomeEstado)
    {
        var nomeNormalizado = nomeEstado.Trim();
        var estado = estadoRepository.GetAll().FirstOrDefault(e =>
            e.NomeEstado.Equals(nomeNormalizado, StringComparison.OrdinalIgnoreCase));

        if (estado == null)
        {
            estado = new Estado(nomeNormalizado);
            estadoRepository.Add(estado);
        }

        return estado;
    }

    private Cidade FindOrCreateCidade(string nomeCidade, Guid estadoId)
    {
        var nomeNormalizado = nomeCidade.Trim();
        var cidade = cidadeRepository.GetAll().FirstOrDefault(c =>
            c.NomeCidade.Equals(nomeNormalizado, StringComparison.OrdinalIgnoreCase) && c.EstadoId == estadoId);

        if (cidade == null)
        {
            cidade = new Cidade(nomeNormalizado, estadoId);
            cidadeRepository.Add(cidade);
        }

        return cidade;
    }

    private Bairro FindOrCreateBairro(string nomeBairro, Guid cityId)
    {
        var nomeNormalizado = nomeBairro.Trim();
        var bairro = bairroRepository.GetAll().FirstOrDefault(b =>
            b.NomeBairro.Equals(nomeNormalizado, StringComparison.OrdinalIgnoreCase) && b.CidadeId == cityId);

        if (bairro == null)
        {
            bairro = new Bairro(nomeNormalizado, cityId);
            bairroRepository.Add(bairro);
        }

        return bairro;
    }

    private record ViaCepResponse(
        [property: JsonPropertyName("logradouro")] string? Logradouro,
        [property: JsonPropertyName("bairro")] string? Bairro,
        [property: JsonPropertyName("localidade")] string? Localidade,
        [property: JsonPropertyName("uf")] string? Uf,
        [property: JsonPropertyName("estado")] string? Estado,
        [property: JsonPropertyName("erro")] bool? Erro
    );

    private record ResolvedAddress(string Rua, Bairro Bairro);
}
