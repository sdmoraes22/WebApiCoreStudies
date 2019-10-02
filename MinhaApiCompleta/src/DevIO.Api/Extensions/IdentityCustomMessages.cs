using Microsoft.AspNetCore.Identity;

namespace DevIO.Api.Extensions
{
    public class IdentityCustomMessages : IdentityErrorDescriber
    {
        public override IdentityError ConcurrencyFailure(){return new IdentityError{Code = nameof(ConcurrencyFailure), Description = "Falha de ocorrência otimista, o objeto foi modificado"};}
        public override IdentityError DefaultError(){return new IdentityError{Code = nameof(DefaultError), Description = $"Ocorreu um erro desconhecido"};}
        public override IdentityError DuplicateEmail(string email){return new IdentityError{Code = nameof(DuplicateEmail), Description = $"O email '{email}' já está sendo utilizado"};}
        public override IdentityError DuplicateRoleName(string role){return new IdentityError{Code = nameof(DuplicateRoleName), Description = $"A permissão '{role}' já está sendo utilizada"};}
        public override IdentityError DuplicateUserName(string userName){return new IdentityError{Code = nameof(DuplicateUserName), Description = $"O Login '{userName}' já está sendo utilizado"};}
        public override IdentityError InvalidEmail(string email){return new IdentityError{Code = nameof(InvalidEmail), Description = $"Email '{email}' é inválido"};}
        public override IdentityError InvalidRoleName(string role){return new IdentityError{Code = nameof(InvalidRoleName), Description = $"A permissão '{role}' é inválida"};}
        public override IdentityError InvalidUserName(string userName){return new IdentityError{Code = nameof(InvalidUserName), Description = $"O Login '{userName}' é inválido por conter apenas números ou dígitos"};}
        public override IdentityError PasswordRequiresDigit(){return new IdentityError{Code = nameof(PasswordRequiresDigit), Description = "Senhas devem conter ao menos um dígito [0-9]"};}
        public override IdentityError PasswordRequiresNonAlphanumeric(){return new IdentityError{Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Senhas devem conter ao menos um caractere não alfanumérico"};}
        public override IdentityError PasswordTooShort(int length){return new IdentityError{Code = nameof(PasswordTooShort), Description = $"Senhas devem conter ao menos '{length}' caracteres."};}
        public override IdentityError UserAlreadyInRole(string role){return new IdentityError{Code = nameof(UserAlreadyInRole), Description = $"O usuário já possui a permissão '{role}'."};}
        public override IdentityError UserLockoutNotEnabled(){return new IdentityError{Code = nameof(UserLockoutNotEnabled), Description = "Lockout não está habilitado para este usuário."};}
        public override IdentityError UserNotInRole(string role){return new IdentityError{Code = nameof(UserNotInRole), Description = $"O usuário não possui a permissão '{role}'."};}
        public override IdentityError PasswordRequiresUpper(){return new IdentityError{Code = nameof(PasswordRequiresLower), Description = "Senhas devem conter ao menos um caractere em caixa alta ['A'-'Z']"};}
        public override IdentityError PasswordRequiresLower(){return new IdentityError{Code = nameof(PasswordRequiresLower), Description = "Senhas devem conter ao menos um caractere em caixa baixa ['a'-'z']."};}
    }
}