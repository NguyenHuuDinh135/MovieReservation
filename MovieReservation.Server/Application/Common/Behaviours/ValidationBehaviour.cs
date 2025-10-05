using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReservation.Server.Application.Common.Behaviours
{
   public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
   {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validator = new InlineValidator<TRequest>();
            var result = await validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
            throw new ValidationException(result.Errors);
            }
            return await next();
        }
   }
}