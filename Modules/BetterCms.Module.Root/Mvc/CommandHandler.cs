// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandHandler.cs" company="Devbridge Group LLC">
//
// Copyright (C) 2015,2016 Devbridge Group LLC
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
//
// Website: https://www.bettercms.com
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Text;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Exceptions.Service;
using BetterCms.Module.Root.Content.Resources;

using Common.Logging;

using BetterModules.Core.Exceptions;
using BetterModules.Core.Exceptions.DataTier;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Mvc
{
    /// <summary>
    /// Contains logic to handle command execution in the controller context.
    /// </summary>
    public static class CommandHandler
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>False if exception was caught, and True otherwise.</returns>
        public static bool ExecuteCommand(this ICommand command)
        {
            try
            {
                command.Execute();
                return true;
            }
            catch (ConfirmationRequestException ex)
            {
                HandleConfirmationRequestException(ex, command);
            }
            catch (ValidationException ex)
            {
                HandleValidationException(ex, command);
            }
            catch (ConcurrentDataException ex)
            {
                HandleConcurrentDataException(ex, command);
            }
            catch (EntityNotFoundException ex)
            {
                HandleEntityNotFoundException(ex, command);
            }
            catch (SecurityException ex)
            {
                HandleSecurityException(ex, command);
            }
            catch (CoreException ex)
            {
                HandleCoreException(ex, command);
            }
            catch (Exception ex)
            {
                HandleException(ex, command);
            }

            return false;
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <typeparam name="TRequest">The type of the command request.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="request">The request.</param>
        /// <returns>False if exception was caught, and True otherwise.</returns>
        public static bool ExecuteCommand<TRequest>(this ICommandIn<TRequest> command, TRequest request)
        {
            try
            {
                command.Execute(request);
                return true;
            }
            catch (ConfirmationRequestException ex)
            {
                HandleConfirmationRequestException(ex, command, request);
            }
            catch (ValidationException ex)
            {
                HandleValidationException(ex, command, request);
            }
            catch (ConcurrentDataException ex)
            {
                HandleConcurrentDataException(ex, command, request);
            }
            catch (EntityNotFoundException ex)
            {
                HandleEntityNotFoundException(ex, command, request);
            }
            catch (SecurityException ex)
            {
                HandleSecurityException(ex, command, request);
            }
            catch (CoreException ex)
            {
                HandleCoreException(ex, command, request);
            }
            catch (Exception ex)
            {
                HandleException(ex, command, request);
            }

            return false;
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <typeparam name="TResponse">The type of the command response.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>Command result or default(TResponse) value if exception occurred.</returns>
        public static TResponse ExecuteCommand<TResponse>(this ICommandOut<TResponse> command)
        {
            try
            {
                return command.Execute();
            }
            catch (ConfirmationRequestException ex)
            {
                HandleConfirmationRequestException(ex, command);
            }
            catch (ValidationException ex)
            {
                HandleValidationException(ex, command);
            }
            catch (ConcurrentDataException ex)
            {
                HandleConcurrentDataException(ex, command);
            }
            catch (EntityNotFoundException ex)
            {
                HandleEntityNotFoundException(ex, command);
            }
            catch (SecurityException ex)
            {
                HandleSecurityException(ex, command);
            }
            catch (CoreException ex)
            {
                HandleCoreException(ex, command);
            }
            catch (Exception ex)
            {
                HandleException(ex, command);
            }

            return default(TResponse);
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <typeparam name="TRequest">The type of the command request.</typeparam>
        /// <typeparam name="TResponse">The type of the command response.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="request">The request.</param>
        /// <returns>Command result or default(TResponse) value if exception occurred.</returns>
        public static TResponse ExecuteCommand<TRequest, TResponse>(this ICommand<TRequest, TResponse> command, TRequest request)
        {
            try
            {
                return command.Execute(request);
            }
            catch (ConfirmationRequestException ex)
            {
                HandleConfirmationRequestException(ex, command, request);
            }
            catch (ValidationException ex)
            {
                HandleValidationException(ex, command, request);
            }
            catch (ConcurrentDataException ex)
            {
                HandleConcurrentDataException(ex, command, request);
            }
            catch (EntityNotFoundException ex)
            {
                HandleEntityNotFoundException(ex, command, request);
            }
            catch (SecurityException ex)
            {
                HandleSecurityException(ex, command, request);
            }
            catch (CoreException ex)
            {
                HandleCoreException(ex, command, request);
            }
            catch (Exception ex)
            {
                HandleException(ex, command, request);
            }

            return default(TResponse);
        }

        /// <summary>
        /// Handles the validation exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="command">The command.</param>
        /// <param name="request">The request.</param>
        private static void HandleValidationException(ValidationException ex, ICommandBase command, object request = null)
        {
            Log.Trace(FormatCommandExceptionMessage(command, request), ex);

            string message = null;
            if (ex.Resource != null)
            {
                message = ex.Resource();
            }

            if (string.IsNullOrEmpty(message))
            {
                message = RootGlobalization.Message_InternalServerErrorPleaseRetry;
            }

            if (command.Context != null)
            {
                command.Context.Messages.AddError(message);
            }
        }

        /// <summary>
        /// Handles the user confirmation request exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="command">The command.</param>
        /// <param name="request">The request.</param>
        private static void HandleConfirmationRequestException(ConfirmationRequestException ex, ICommandBase command, object request = null)
        {
            Log.Trace(FormatCommandExceptionMessage(command, request), ex);

            throw ex;
        }

        /// <summary>
        /// Handles the concurrent data exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="command">The command.</param>
        /// <param name="request">The request.</param>
        private static void HandleConcurrentDataException(ConcurrentDataException ex, ICommandBase command, object request = null)
        {
            Log.Trace(FormatCommandExceptionMessage(command, request), ex);

            string message = RootGlobalization.Message_ConcurentDataException;

            if (command.Context != null)
            {
                command.Context.Messages.AddError(message);
            }
        }

        /// <summary>
        /// Handles the entity not found exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="command">The command.</param>
        /// <param name="request">The request.</param>
        private static void HandleEntityNotFoundException(EntityNotFoundException ex, ICommandBase command, object request = null)
        {
            Log.Trace(FormatCommandExceptionMessage(command, request), ex);

            string message = RootGlobalization.Message_EntityNotFoundException;

            if (command.Context != null)
            {
                command.Context.Messages.AddError(message);
            }
        }

        /// <summary>
        /// Handles the security exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="command">The command.</param>
        /// <param name="request">The request.</param>
        private static void HandleSecurityException(SecurityException ex, ICommandBase command, object request = null)
        {
            Log.Error(FormatCommandExceptionMessage(command, request), ex);
            if (command.Context != null)
            {
                command.Context.Messages.AddError(RootGlobalization.Message_AccessForbidden);
            }
        }

        /// <summary>
        /// Handles the CMS exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="command">The command.</param>
        /// <param name="request">The request.</param>
        private static void HandleCoreException(CoreException ex, ICommandBase command, object request = null)
        {
            Log.Error(FormatCommandExceptionMessage(command, request), ex);
            if (command.Context != null)
            {
                command.Context.Messages.AddError(RootGlobalization.Message_InternalServerErrorPleaseRetry);
            }
        }

        /// <summary>
        /// Handles the unknown exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="command">The command.</param>
        /// <param name="request">The request.</param>
        private static void HandleException(Exception ex, ICommandBase command, object request = null)
        {
            Log.Fatal(FormatCommandExceptionMessage(command, request), ex);
            if (command.Context != null)
            {
                command.Context.Messages.AddError(RootGlobalization.Message_InternalServerErrorPleaseRetry);
            }
        }

        /// <summary>
        /// Creates command exception message.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="request">The request.</param>
        /// <returns>Command execution failure message.</returns>
        private static string FormatCommandExceptionMessage(object command, object request = null)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Failed to execute command {0}. ", command != null ? command.GetType().Name : "NULL");

            if (request != null)
            {
                sb.AppendFormat("Request data: {0}. ", request);
            }

            return sb.ToString();
        }
    }
}