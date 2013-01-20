using System;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Models;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Base
{
    public abstract class SaveContentCommandBase<TIn, TOut> : CommandBase, ICommand<TIn, TOut>
        where TIn : ISaveContentHistory
        where TOut : ISaveContentResponse

    {
      
    }
}