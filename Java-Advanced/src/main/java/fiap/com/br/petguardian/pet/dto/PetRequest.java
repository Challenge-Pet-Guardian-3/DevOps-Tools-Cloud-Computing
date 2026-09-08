package fiap.com.br.petguardian.pet.dto;

import fiap.com.br.petguardian.pet.Pet;
import fiap.com.br.petguardian.pet.PetPorte;
import fiap.com.br.petguardian.pet.raca.Raca;
import fiap.com.br.petguardian.validation.EnumValidation;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PastOrPresent;

import java.time.LocalDate;

public record PetRequest(
        @NotBlank
        String nome,

        @NotNull
        @PastOrPresent(message = "Data de nascimento não pode estar no futuro.")
        LocalDate dataNasc,

        @NotBlank
        String raca,

        @NotNull
        @EnumValidation(enumClass = PetPorte.class)
        String porte,

        @NotNull
        Character sexo,

        @NotNull
        Boolean castrado,

        @NotNull
        Long usuarioId
) {
    public Pet toEntity(Raca racaObj) {
        return Pet.builder()
                .nome(nome)
                .dataNasc(dataNasc)
                .raca(racaObj)
                .porte(PetPorte.valueOf(porte))
                .sexo(sexo)
                .castrado(castrado)
                .build();
    }

    public Pet aplicarEm(Pet pet, Raca racaObj) {
        pet.setNome(nome);
        pet.setDataNasc(dataNasc);
        pet.setRaca(racaObj);
        pet.setPorte(PetPorte.valueOf(porte));
        pet.setSexo(sexo);
        pet.setCastrado(castrado);
        return pet;
    }
}
