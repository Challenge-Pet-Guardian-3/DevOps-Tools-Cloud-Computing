using PetGuardian.Application.DTOs;
using PetGuardian.Application.Repositories;
using PetGuardian.Application.Services.Interfaces;

namespace PetGuardian.Application.Services.Implementations;

public sealed class UsuarioPetService(
    IUsuarioPetRepository usuarioPetRepository,
    IUsuarioRepository usuarioRepository,
    IPetRepository petRepository,
    ITarefaRepository tarefaRepository,
    IAtendimentoRepository atendimentoRepository) : IUsuarioPetService
{
    public IReadOnlyList<UsuarioPetResponse> GetAll() =>
        usuarioPetRepository.GetAll().Select(UsuarioPetResponse.FromDomain).ToList();

    public IReadOnlyList<UsuarioPetResponse> GetByUsuarioId(Guid usuarioId) =>
        usuarioPetRepository.GetByUsuarioId(usuarioId).Select(UsuarioPetResponse.FromDomain).ToList();

    public IReadOnlyList<UsuarioPetResponse> GetByPetId(Guid petId) =>
        usuarioPetRepository.GetByPetId(petId).Select(UsuarioPetResponse.FromDomain).ToList();

    public UsuarioPetResponse Create(UsuarioPetRequest request)
    {
        ValidarUsuarioEPet(request.UsuarioId, request.PetId);
        ValidarRegrasResponsavelPrincipalParaNovoVinculo(request.PetId, request.ResponPrinc);

        if (usuarioPetRepository.Exists(request.UsuarioId, request.PetId))
            throw new InvalidOperationException("Este usuário já está vinculado a este pet.");

        var vinculo = request.ToDomain();
        usuarioPetRepository.Add(vinculo);
        return UsuarioPetResponse.FromDomain(vinculo);
    }

    public UsuarioPetResponse InviteByUsuario(UsuarioPetInviteByUsuarioRequest request)
    {
        ValidarAdministradorPrincipal(request.AdminUsuarioId, request.PetId);

        if (!usuarioRepository.ExistsById(request.UsuarioConvidadoId))
            throw new InvalidOperationException("Usuário convidado não encontrado.");

        if (usuarioPetRepository.Exists(request.UsuarioConvidadoId, request.PetId))
            throw new InvalidOperationException("Usuário convidado já está vinculado a este pet.");

        var vinculo = new Domain.Entities.UsuarioPet(
            request.UsuarioConvidadoId,
            request.PetId,
            responPrinc: false);

        usuarioPetRepository.Add(vinculo);
        return UsuarioPetResponse.FromDomain(vinculo);
    }

    public UsuarioPetResponse InviteByEmail(UsuarioPetInviteByEmailRequest request)
    {
        ValidarAdministradorPrincipal(request.AdminUsuarioId, request.PetId);

        var usuarioConvidado = usuarioRepository.GetByEmail(request.Email)
            ?? throw new InvalidOperationException("Usuário convidado não encontrado para o e-mail informado.");

        if (usuarioPetRepository.Exists(usuarioConvidado.Id, request.PetId))
            throw new InvalidOperationException("Usuário convidado já está vinculado a este pet.");

        var vinculo = new Domain.Entities.UsuarioPet(
            usuarioConvidado.Id,
            request.PetId,
            responPrinc: false);

        usuarioPetRepository.Add(vinculo);
        return UsuarioPetResponse.FromDomain(vinculo);
    }

    public RedeCuidadoResponse GetRedeCuidadoByUsuarioId(Guid usuarioId)
    {
        if (!usuarioRepository.ExistsById(usuarioId))
            throw new InvalidOperationException("Usuário não encontrado.");

        var vinculosDoUsuario = usuarioPetRepository.GetByUsuarioId(usuarioId);
        var petIds = vinculosDoUsuario.Select(v => v.PetId).Distinct().ToList();

        var petsDaRede = new List<RedeCuidadoPetResponse>();
        var coCuidadorIds = new HashSet<Guid>();

        foreach (var petId in petIds)
        {
            var pet = petRepository.GetById(petId);
            if (pet is null)
                continue;

            var tarefas = tarefaRepository.GetByPetId(petId)
                .Select(t => new RedeCuidadoTarefaResponse(
                    t.Id,
                    t.Titulo,
                    t.Prazo,
                    t.Conclusao,
                    t.UsuarioId,
                    t.StatusId,
                    t.PontosTarefa))
                .ToList();

            var atendimentos = atendimentoRepository.GetByPetId(petId)
                .Select(a => new RedeCuidadoAtendimentoResponse(
                    a.Id,
                    a.Data,
                    a.Anotacoes,
                    a.Valor,
                    a.StatusId,
                    a.VeterinarioId))
                .ToList();

            petsDaRede.Add(new RedeCuidadoPetResponse(
                pet.Id,
                pet.Nome,
                tarefas,
                atendimentos));

            var vinculosDoPet = usuarioPetRepository.GetByPetId(petId);
            foreach (var vinculo in vinculosDoPet)
            {
                if (vinculo.UsuarioId != usuarioId)
                    coCuidadorIds.Add(vinculo.UsuarioId);
            }
        }

        var coCuidadores = coCuidadorIds
            .Select(id => usuarioRepository.GetById(id))
            .Where(u => u is not null)
            .Select(u => new RedeCuidadoCoCuidadorResponse(u!.Id, u.Nome, u.Email))
            .OrderBy(u => u.Nome)
            .ToList();

        return new RedeCuidadoResponse(usuarioId, petsDaRede, coCuidadores);
    }

    public bool Delete(Guid usuarioId, Guid petId)
    {
        var vinculo = usuarioPetRepository.GetByUsuarioAndPet(usuarioId, petId);
        if (vinculo is null)
            return false;

        if (vinculo.ResponPrinc)
        {
            var totalResponsaveis = usuarioPetRepository.GetByPetId(petId)
                .Count(v => v.ResponPrinc);

            if (totalResponsaveis <= 1)
                throw new InvalidOperationException(
                    "Não é permitido desvincular o responsável principal quando ele é o único administrador do pet.");
        }

        return usuarioPetRepository.Delete(usuarioId, petId);
    }

    private void ValidarUsuarioEPet(Guid usuarioId, Guid petId)
    {
        if (!usuarioRepository.ExistsById(usuarioId))
            throw new InvalidOperationException("Usuário não encontrado.");

        if (!petRepository.ExistsById(petId))
            throw new InvalidOperationException("Pet não encontrado.");
    }

    private void ValidarAdministradorPrincipal(Guid adminUsuarioId, Guid petId)
    {
        ValidarUsuarioEPet(adminUsuarioId, petId);

        var vinculoAdmin = usuarioPetRepository.GetByUsuarioAndPet(adminUsuarioId, petId);
        if (vinculoAdmin is null)
            throw new InvalidOperationException("Administrador não está vinculado ao pet.");

        if (!vinculoAdmin.ResponPrinc)
            throw new InvalidOperationException(
                "Apenas o responsável principal do pet pode convidar novos co-cuidadores.");
    }

    private void ValidarRegrasResponsavelPrincipalParaNovoVinculo(Guid petId, bool novoVinculoEhPrincipal)
    {
        var vinculosDoPet = usuarioPetRepository.GetByPetId(petId);
        var possuiPrincipal = vinculosDoPet.Any(v => v.ResponPrinc);

        if (!possuiPrincipal && !novoVinculoEhPrincipal)
            throw new InvalidOperationException("Todo pet precisa ter um responsável principal.");

        if (novoVinculoEhPrincipal && possuiPrincipal)
            throw new InvalidOperationException("Este pet já possui responsável principal.");
    }
}
