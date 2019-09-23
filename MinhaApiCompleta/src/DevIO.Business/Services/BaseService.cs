using System;
using System.Threading.Tasks;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Notifications;
using FluentValidation;
using FluentValidation.Results;

namespace DevIO.Business.Services
{
    public abstract class BaseService
    {
        public readonly INotificador _notificador;

        public BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }
        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }

        }
        protected void Notificar(string message)
        {
            _notificador.Handle(new Notificacao(message));
        }

        protected bool ExecutarValidacao<TValidation, TEntity>(TValidation validation, TEntity entity) 
            where TValidation: AbstractValidator<TEntity> 
            where TEntity: Entity
        {
            var validator = validation.Validate(entity);

            if(validator.IsValid) return true;

            Notificar(validator);
            return false;
            
        }
    }
}